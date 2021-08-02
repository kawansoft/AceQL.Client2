/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2020,  KawanSoft SAS
 * (http://www.kawansoft.com). All rights reserved.                                
 *                                                                               
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this filePath except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. 
 */

using System;
using System.IO;
using System.IO.Compression;

namespace AceQL.Client.Api.Util
{
    /// <summary>
    /// Class FileUtil2.
    /// </summary>
    static class FileUtil2
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
                        using (var stream = File.OpenWrite(path))
                        {
                            decompressionStream.CopyTo(stream);
                        }
                    }
                }
                else
                {
                    using (var stream = File.OpenWrite(path))
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

        /// <summary>
        /// Generates a unique File on the system for the downloaded result set content.
        /// </summary>
        /// <returns>A unique File on the system.</returns>
        public static String GetUniqueBatchFile()
        {
            String path = GetUserFolderPath() + "\\" + Guid.NewGuid().ToString() + "-batch-file.txt";
            return path;
        }
    }
}
