using AceQL.Client.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Storage;

namespace AceQL.Client.Test.Metadata.misc
{
    public class DirectoriesTest
    {
        public static void DisplayDirectories()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.aceql";

            StorageFolder localFolder = ApplicationData.Current.TemporaryFolder;
            string folderPathTemp = localFolder.Path + ".aceql";

            FileInfo fileInfo = new FileInfo(folderPathTemp);
            if (!fileInfo.Exists)
            {
                _ = Directory.CreateDirectory(folderPathTemp);
            }

            var tmp = Path.GetTempPath();

            Console.WriteLine("folderPath   : " + folderPath);
            Console.WriteLine("folderPathTemp: " + folderPathTemp);
            Console.WriteLine();

            Console.WriteLine("AceQLConnection.GetAceQLLocalFolder(): " + AceQLConnection.GetAceQLLocalFolder());
            Console.ReadLine();
        }
    }
}
