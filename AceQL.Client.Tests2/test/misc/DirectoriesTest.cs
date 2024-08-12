using AceQL.Client.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AceQL.Client.Test.Metadata.misc
{
    public class DirectoriesTest
    {
        public static void DisplayDirectories()
        {
            //string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.aceql";
            //StorageFolder localFolder = ApplicationData.Current.TemporaryFolder;
            //string folderPathTemp = localFolder.Path + ".aceql";

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.aceql";

            FileInfo fileInfo = new FileInfo(folderPath);
            if (!fileInfo.Exists)
            {
                _ = Directory.CreateDirectory(folderPath);
            }

            var tmp = Path.GetTempPath();

            Console.WriteLine("folderPath   : " + folderPath);
            Console.WriteLine();

            Console.WriteLine("AceQLConnection.GetAceQLLocalFolder(): " + AceQLConnection.GetAceQLLocalFolder());
            Console.ReadLine();
        }
    }
}
