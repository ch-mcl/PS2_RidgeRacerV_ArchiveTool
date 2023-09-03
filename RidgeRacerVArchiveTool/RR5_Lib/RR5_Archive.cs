using System;
using System.Collections.Generic;
using System.IO;

namespace RidgeRacerVArchiveTool.RR5_Lib
{
    class TOC
    {
        public int blockOffset = 0x00;

        public int blockSize = 0xCC0000;

        public int compressedSize = 0x00;

        public int uncompressedSize = 0x00;

        public bool Unpack(FileStream fileStream)
        {
            byte[] bytes = new byte[4];
            fileStream.Read(bytes, 0x00, bytes.Length);
            blockOffset = BitConverter.ToInt32(bytes, 0x00);
            if (blockOffset == 0xCC0000)
            {
                return true;
            }
            fileStream.Read(bytes, 0x00, bytes.Length);
            blockSize = BitConverter.ToInt32(bytes, 0x00);
            fileStream.Read(bytes, 0x00, bytes.Length);
            compressedSize = BitConverter.ToInt32(bytes, 0x00);
            fileStream.Read(bytes, 0x00, bytes.Length);
            uncompressedSize = BitConverter.ToInt32(bytes, 0x00);

            return false;
        }

        public void Pack(FileStream srcFileStream, FileStream elfFileStream)
        {
            // compressed Size
            int srcLength = (int)srcFileStream.Length;
            compressedSize = srcLength;

            // block Size
            int calcedBlockSize = srcLength / 0x800;
            if (srcLength % 0x800 != 0x00)
            {
                calcedBlockSize += 1;
            }
            blockSize = calcedBlockSize;

            // uncompressed Size
            uncompressedSize = srcLength;
            byte[] bytes = new byte[4];

            srcFileStream.Seek(0x06, SeekOrigin.Begin);
            srcFileStream.Read(bytes, 0x00, bytes.Length);
            int lzLength = BitConverter.ToInt32(bytes, 0x00);
            if ( (lzLength > 1) && srcFileStream.Length == (lzLength += 0x0A) )
            {
                srcFileStream.Seek(0x02, SeekOrigin.Begin);
                srcFileStream.Read(bytes, 0x00, bytes.Length);
                int _uncompressedSize = BitConverter.ToInt32(bytes, 0x00);

                uncompressedSize = _uncompressedSize;
            }
            srcFileStream.Seek(0x00, SeekOrigin.Begin);

            bytes = BitConverter.GetBytes(blockOffset);
            elfFileStream.Write(bytes, 0x00, bytes.Length);
            bytes = BitConverter.GetBytes(blockSize);
            elfFileStream.Write(bytes, 0x00, bytes.Length);
            bytes = BitConverter.GetBytes(compressedSize);
            elfFileStream.Write(bytes, 0x00, bytes.Length);
            bytes = BitConverter.GetBytes(uncompressedSize);
            elfFileStream.Write(bytes, 0x00, bytes.Length);

            return;
        }

    }

    class RR5_Archive
    {
        public const string EXT_RR5_RAW = "RR5.RAW";

        public const string EXT_RR5_LZ = "RR5.LZ";

    }

}
