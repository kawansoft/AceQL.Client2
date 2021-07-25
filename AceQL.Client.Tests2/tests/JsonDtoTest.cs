using AceQL.Client.Api.Metadata.Dto;
using AceQL.Client.Tests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Tests2.tests
{
    public class JsonDtoTest
    {
        public static void TheMain(string[] args)
        {

            try
            {
                DoTest();
                AceQLConsole.WriteLine();
                AceQLConsole.WriteLine("Press enter to close....");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                AceQLConsole.WriteLine(exception.ToString());
                AceQLConsole.WriteLine(exception.StackTrace);
                AceQLConsole.WriteLine("Press enter to close...");
                Console.ReadLine();
            }
        }

        private static void DoTest()
        {
            /*
            List<String> tableNames = new List<string>();
            tableNames.Add("table1");
            tableNames.Add("table2");
            tableNames.Add("table3");

            TableNamesDto tableNamesDto = new TableNamesDto(tableNames);

            String jsonString = JsonConvert.SerializeObject(tableNamesDto);
            Console.WriteLine("jsonString:");
            Console.WriteLine(jsonString);

            tableNames = null;
            tableNamesDto = null;

            TableNamesDto tableNamesDto2 = JsonConvert.DeserializeObject<TableNamesDto>(jsonString);
            Console.WriteLine();
            Console.WriteLine("tableNamesDto2.ToString():");
            Console.WriteLine(tableNamesDto2);
            */
        }
    }
}
