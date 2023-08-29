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
        const string EXT_RR5_RAW = "RR5.RAW";

        const string EXT_RR5_LZ = "RR5.LZ";

        public bool Unpack(String fileDirectory, string elfPath, string arcPath, int tocAddress)
        {
            try {
                // elf file
                using (FileStream elfFileStream = new FileStream(elfPath, FileMode.Open, FileAccess.Read))
                // arc file
                using (FileStream arcFileStream = new FileStream(arcPath, FileMode.Open, FileAccess.Read))
                {
                    elfFileStream.Seek((long)tocAddress, SeekOrigin.Begin);

                    int i = 0;
                    while (true)
                    {
                        TOC toc = new TOC();
                        bool result = toc.Unpack(elfFileStream);
                        if (result)
                        {
                            break;
                        }

                        string extention = EXT_RR5_RAW;
                        if (toc.compressedSize < toc.uncompressedSize)
                        {
                            extention = EXT_RR5_LZ; // Needs decompress by LZSS(RRV Format).
                        }

                        string fileName = string.Format("{0:D8}.{1}", i, extention);
                        string fullpath = $@"{fileDirectory}\{fileName}";
                        using (FileStream destFileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] destBytes = new byte[toc.compressedSize];
                            arcFileStream.Seek(toc.blockOffset * 0x800, SeekOrigin.Begin);
                            arcFileStream.Read(destBytes, 0x00, toc.compressedSize);
                            destFileStream.Write(destBytes, 0x00, toc.compressedSize);
                        }

                        i++;
                    }
                }
            }
            catch
            {
                return true;
            }
            return false;
        }

        public bool Pack(List<string> filePaths, string elfPath, string arcPath, int tocAddress)
        {
            try
            {
                // elf file
                using (FileStream elfFileStream = new FileStream(elfPath, FileMode.Open, FileAccess.ReadWrite))
                // arc file
                using (FileStream arcFileStream = new FileStream(arcPath, FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = new byte[1];

                    // Goto TOC Address
                    elfFileStream.Seek(tocAddress, SeekOrigin.Begin);
                    foreach (string srcFilePath in filePaths)
                    {
                        TOC toc = new TOC();
                        using (FileStream srcFileStream = new FileStream(srcFilePath, FileMode.Open, FileAccess.Read))
                        {
                            if (arcFileStream.Position > 1)
                            {
                                toc.blockOffset = (int)arcFileStream.Position / 0x800;
                            }

                            // Write TOC
                            toc.Pack(srcFileStream, elfFileStream);

                            bytes = new byte[toc.compressedSize];
                            srcFileStream.Read(bytes, 0x00, toc.compressedSize);
                        }
                        arcFileStream.Write(bytes, 0x00, toc.compressedSize);

                        // padding
                        bytes = new byte[1];
                        bytes[0] = 0x00;
                        int padding = (toc.blockSize * 0x800) - (toc.compressedSize);
                        for (int i = 0; i < padding; i++)
                        {
                            arcFileStream.Write(bytes, 0x00, bytes.Length);
                        }
                    }

                    // check teminator of TOC
                    bytes = new byte[4];
                    elfFileStream.Read(bytes, 0x00, bytes.Length);
                    int terminator = BitConverter.ToInt32(bytes, 0x00);
                    if (terminator != 0xCC0000)
                    {
                        return true;
                    }

                }
            }
            catch
            {
                return true;
            }

            return false;
        }

    }

}
