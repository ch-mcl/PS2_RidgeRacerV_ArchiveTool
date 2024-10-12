using System;
using System.Collections.Generic;
using System.IO;

namespace RRV_Lib
{
    class Header
    {
        /// <summary>
        /// Dictionary Exponent.
        /// size of dictionary = 2 ^ this value.
        /// </summary>
        public byte dictExponent;
        public byte unk0x01;
        /// <summary>
        /// length of uncompressed size.
        /// </summary>
        public uint unCompressedSize;
        /// <summary>
        /// length of compresed size.
        /// </summary>
        public uint compresedSize;

        public void Parse (FileStream fileStream) 
        {
            dictExponent = (byte)fileStream.ReadByte();
            unk0x01 = (byte)fileStream.ReadByte();
            byte[] bytes = new byte[4];
            fileStream.Read(bytes, 0x00, bytes.Length);
            unCompressedSize = BitConverter.ToUInt32(bytes, 0x00);
            fileStream.Read(bytes, 0x00, bytes.Length);
            compresedSize = BitConverter.ToUInt32(bytes, 0x00);

            return;
        }
    }

    /// <summary>
    /// Ridge Racer V LZSS Compression
    /// </summary>
    class RRV_LZSS
    {

        /// <summary>
        /// Decompressing file by Ridge Racer V compression format
        /// </summary>
        /// <returns>false: sucess. true: fail.</returns>
        public bool Decompress(string path)
        {
            string fileExtension = Path.GetExtension(path);

            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appDirectory = Path.GetDirectoryName(appPath);

            // D&D file
            FileStream srcFileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            
            
            Header header = new Header();
            header.Parse(srcFileStream);

            if (header.unCompressedSize < 1
               | header.compresedSize > header.unCompressedSize
               | header.unk0x01 != 0x08
                )
            {
                return true;
            }

            // DecompressedFile
            string fileName = Path.GetFileName(path);
            string fileDirectory = Path.GetDirectoryName(path);
            FileStream destFileStream = new FileStream($@"{fileDirectory}\{fileName}_decomp.bin", FileMode.Create, FileAccess.ReadWrite);

            byte[] flags = new byte[8];
            uint srcFileMaxPos = header.compresedSize + 0x0A;
            long destCurentPos = 0x00;

            ushort destRefOffsetLengthMax = (ushort)Math.Pow(0x2, header.dictExponent);
            ushort destRefOffsetLengthMask = (ushort)((1 << header.dictExponent) - 1);
            ushort lengthMax = (ushort)Math.Pow(2, 16 - header.dictExponent);
            ushort lengthMask = (ushort)~destRefOffsetLengthMask;

            while ((uint)srcFileStream.Position < srcFileMaxPos)
            {
                // Read 8 flags
                srcFileStream.Read(flags, 0x00, 8);
                
                foreach (byte flag in flags)
                {

                    for (int i = 0; i < 8; i++)
                    {
                        if ((uint)srcFileStream.Position >= srcFileMaxPos)
                        {
                            break;
                        }

                        // Compressed
                        if ( ((flag >> i) & 0x01) == 0x00 )
                        {
                            byte[] srcBytes = new byte[2];
                            srcFileStream.Read(srcBytes, 0x00, 2);

                            ushort byte0_1 = (ushort)((srcBytes[0] << 8) | srcBytes[1]);

                            // Length
                            ushort length = (ushort)(byte0_1 & lengthMask);
                            length = (ushort)(length >> header.dictExponent);
                            if (length < 1)
                            {
                                // 0 means max
                                length = lengthMax;
                            }

                            // Destination Reference Offset
                            int destRefOffset = byte0_1 & destRefOffsetLengthMask;
                            if (destRefOffset == 0x00 && destCurentPos >= destRefOffsetLengthMax)
                            {
                                // 0 means max
                                destRefOffset = destRefOffsetLengthMax;
                            }
                            destCurentPos = destFileStream.Position;
                            destFileStream.Seek(-destRefOffset, SeekOrigin.Current);
                            byte[] destBytes = new byte[length];

                            if (length > destRefOffset)
                            {
                                destFileStream.Read(destBytes, 0x00, destRefOffset);
                                int copyIdx = destRefOffset;
                                for (int destIdx = destRefOffset; destIdx < destBytes.Length; destIdx++)
                                {
                                    copyIdx = 0;
                                    if (destRefOffset > 0)
                                    {
                                        copyIdx = destIdx % destRefOffset;
                                    }
                                    byte b = destBytes[copyIdx];
                                    destBytes[destIdx] = b;
                                }

                            } else
                            {
                                destFileStream.Read(destBytes, 0x00, destBytes.Length);
                            }

                            destFileStream.Seek(0x00, SeekOrigin.End);
                            destFileStream.Write(destBytes, 0x00, destBytes.Length);

                        // Uncompressed
                        }
                        else
                        {
                            byte[] destBytes = new byte[1];
                            srcFileStream.Read(destBytes, 0x00, 1);
                            destFileStream.Write(destBytes, 0x00, 1);
                        }
                    }
                }
            }

            // not enghou length
            if (destFileStream.Length < header.unCompressedSize)
            {
                return true;
            }
            srcFileStream.Close();
            destFileStream.Close();

            return false;
        }

    }
}
