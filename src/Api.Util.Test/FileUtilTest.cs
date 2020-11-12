

using System;
using System.IO;
using System.IO.Compression;

namespace AceQL.Client.Api.Util.Test
{
    class FileUtilTest
    {
        public FileUtilTest()
        {
            // Comments
        }

        public String GetUserFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        public void CopyHttpStreamToFile(String path, Stream input)
        {
            if (input != null)
            {
                //if (aceQLHttpApi.GzipResult)
                bool GzipResult = true;
                if (GzipResult)
                {
                    using (GZipStream decompressionStream = new GZipStream(input, CompressionMode.Decompress))
                    {
                        using (var stream = System.IO.File.OpenRead(path))
                        {
                            decompressionStream.CopyTo(stream);
                        }
                    }
                }
                else
                {
                    using (var stream = System.IO.File.OpenRead(path))
                    {
                        input.CopyTo(stream);
                    }
                }
            }
        }
    }
}
