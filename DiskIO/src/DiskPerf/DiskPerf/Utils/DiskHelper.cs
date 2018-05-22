using System;
using System.IO;
using System.Threading.Tasks;

namespace UW.Cloud.Disk.Utils
{
    /// <summary>
    /// Helper class for Disk operations
    /// </summary>
    class DiskHelper
    {
        const int FILE_FLAG_NO_BUFFERING = 0x20000000;

        private static byte[] block = BuildBlock();

        private const int MB = 1024 * 1024;

        /// <summary>
        /// Generates random data 
        /// </summary>
        /// <returns>byte[] of random bytes</returns>
        private static byte[] BuildBlock()
        {
            byte[] b = new byte[1024];
            Random rand = new Random();
            for (int i = 0; i < 1024; ++i)
            {
                b[i] = (byte)('A' + rand.Next() % 26);
            }

            return b;
        }

        /// <summary>
        /// Writes file to local disk in temp directory. 
        /// Content is statically generated random bytes.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="numOfKBs">Number of KBs to write</param>
        /// <returns>Bytes written</returns>
        public static int DiskWrite(string fileName, int numOfKBs, bool delete = false)
        {
            string baseDir = Path.GetTempPath();
            string filePath = Path.Combine(baseDir, fileName);

            if (File.Exists(filePath))
            {
                if(!delete) return 0;
                Delete(filePath);
            }

            int bytesWritten = 0;
            using (FileStream fileStream = GetFileStreamForWrite(filePath, block.Length))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < numOfKBs; ++i)
                {
                    fileStream.Write(block, 0, block.Length);
                    bytesWritten += block.Length;
                }
            }

            return bytesWritten;
        }

        /// <summary>
        /// Writes file to local disk in temp directory in async fashion. 
        /// Content is statically generated random bytes.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="numOfKBs">Number of KBs to write</param>
        /// <returns>Bytes written</returns>
        public static async Task<int> DiskWriteAsync(string fileName, int numOfKBs)
        {
            string baseDir = Path.GetTempPath();
            string filePath = Path.Combine(baseDir, fileName);

            if (File.Exists(filePath))
            {
                return 0;
            }

            int bytesWritten = 0;
            using (FileStream fileStream = GetFileStreamForWrite(filePath, block.Length))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < numOfKBs; ++i)
                {
                    await fileStream.WriteAsync(block, 0, block.Length).ConfigureAwait(false);
                    bytesWritten += block.Length;
                }
            }

            return bytesWritten;
        }

        /// <summary>
        /// Reads entire file block by block
        /// </summary>
        /// <param name="fileName">Name of the file to read</param>
        /// <param name="blockSize">Size of each block in bytes</param>
        /// <returns>Number of bytes read</returns>
        public static int DiskRead(string fileName, int blockSize)
        {
            string baseDir = Path.GetTempPath();
            string filePath = Path.Combine(baseDir, fileName);
            if (!File.Exists(filePath))
            {
                return 0;
            }

            int bytesRead = 0;
            int total = 0;
            using (FileStream fileStream = GetFileStreamForRead(filePath, blockSize))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[blockSize];

                do
                {
                    bytesRead = fileStream.Read(buffer, 0, blockSize);
                    total += bytesRead;
                }
                while (total < 10 * MB);
            }

            return total;
        }

        /// <summary>
        /// Reads entire file block by block in async fashion.
        /// </summary>
        /// <param name="fileName">Name of the file to read</param>
        /// <param name="blockSize">Size of each block in bytes</param>
        /// <returns>Number of bytes read</returns>
        public static async Task<int> DiskReadAsync(string fileName, int blockSize)
        {
            string baseDir = Path.GetTempPath();
            string filePath = Path.Combine(baseDir, fileName);
            if (!File.Exists(filePath))
            {
                return 0;
            }

            int bytesRead = 0;
            int total = 0;
            using (FileStream fileStream = GetFileStreamForRead(filePath, blockSize))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[blockSize];

                do
                {
                    bytesRead = await fileStream.ReadAsync(buffer, 0, blockSize).ConfigureAwait(false);
                    total += bytesRead;
                }
                while (total < 10 * MB);
            }

            return total;
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="fileName">Name of file to delete</param>
        /// <returns></returns>
        public static string Delete(string fileName)
        {
            string baseDir = Path.GetTempPath();
            string filePath = Path.Combine(baseDir, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return filePath;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns a file stream with 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bufferSize"></param>
        /// <returns>FileStream object</returns>
        private static FileStream GetFileStreamForRead(string filePath, int bufferSize)
        {
            return new FileStream(filePath, FileMode.Open);

            //return new FileStream(filePath, 
            //    FileMode.Open,
            //    FileAccess.Read, 
            //    FileShare.Read, 
            //    bufferSize, 
            //    (FileOptions)FILE_FLAG_NO_BUFFERING | FileOptions.Asynchronous);
        }

        /// <summary>
        /// Returns a file stream with 
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <param name="bufferSize">size of buffer</param>
        /// <returns>FileStream object</returns>
        private static FileStream GetFileStreamForWrite(string filePath, int bufferSize)
        {
            return new FileStream(filePath, FileMode.Create);

            //return new FileStream(filePath,
            //    FileMode.Create,
            //    FileAccess.ReadWrite,
            //    FileShare.ReadWrite,
            //    bufferSize,
            //    (FileOptions)FILE_FLAG_NO_BUFFERING | FileOptions.Asynchronous);
        }

        /// <summary>
        /// Read random 10 MB from the file
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="blockSize">size of blocks during read</param>
        /// <returns>Number of bytes read</returns>
        public static int DiskReadRandom(string fileName, int blockSize)
        {
            string baseDir = Path.GetTempPath();
            string filePath = Path.Combine(baseDir, fileName);
            if (!File.Exists(filePath))
            {
                return 0;
            }

            int bytesRead = 0;
            int total = 0;
            using (FileStream fileStream = GetFileStreamForRead(filePath, blockSize))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[blockSize];
                Random rand = new Random();
                do
                {
                    fileStream.Seek(blockSize * rand.Next(1, 11), SeekOrigin.Current);
                    bytesRead = fileStream.Read(buffer, 0, blockSize);
                    total += bytesRead;
                }
                while (total < 10 * MB);
            }

            return total;
        }

        /// <summary>
        /// Read random 10 MB from the file using async read API
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="blockSize">size of blocks during read</param>
        /// <returns>Number of bytes read</returns>
        public static async Task<int> DiskReadRandomAsync(string fileName, int blockSize)
        {
            string baseDir = Path.GetTempPath();
            string filePath = Path.Combine(baseDir, fileName);
            if (!File.Exists(filePath))
            {
                return 0;
            }

            int bytesRead = 0;
            int total = 0;
            using (FileStream fileStream = GetFileStreamForRead(filePath, blockSize))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[blockSize];
                Random rand = new Random();
                do
                {
                    fileStream.Seek(blockSize * rand.Next(1, 11), SeekOrigin.Current);
                    bytesRead = await fileStream.ReadAsync(buffer, 0, blockSize);
                    total += bytesRead;
                }
                while (total < 10 * MB);
            }

            return total;
        }
    }
}