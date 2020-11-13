

using System;
using System.IO;
using System.IO.Compression;

namespace AceQL.Client.Api.Util
{
    /// <summary>
    /// Class FileUtil2.
    /// </summary>
    class FileUtil2
    {
      
        /// <summary>
        /// Gets the user folder path.
        /// </summary>
        /// <returns>String.</returns>
        public static String GetUserFolderPath()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.aceql";

            FileInfo fileInfo = new FileInfo(folderPath);
            if (!fileInfo.Exists)
            {
                _ = Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }

        /// <summary>
        /// Copies the HTTP stream to filePath.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="input">The input.</param>
        /// <param name="GzipResult">if set to <c>true</c>, stream will be unzipped before copy to filePath.</param>
        public static void CopyHttpStreamToFile(String path, Stream input, bool GzipResult)
        {
            if (input != null)
            {
                if (GzipResult)
                {
                    using (GZipStream decompressionStream = new GZipStream(input, CompressionMode.Decompress))
                    {
                        using (var stream = System.IO.File.OpenWrite(path))
                        {
                            decompressionStream.CopyTo(stream);
                        }
                    }
                }
                else
                {
                    using (var stream = System.IO.File.OpenWrite(path))
                    {
                        input.CopyTo(stream);
                    }
                }
            }
        }

        /// <summary>
        /// Generates a unique File on the system for the downloaded result set content.
        /// </summary>
        /// <returns>A unique File on the system.</returns>
        public static String GetUniqueResultSetFile()
        {
            String path = GetUserFolderPath() + "\\" + Guid.NewGuid().ToString() + "-result-set.txt";
            return path;
        }
    }
}
