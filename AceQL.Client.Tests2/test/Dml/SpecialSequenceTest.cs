using AceQL.Client.Api;
using AceQL.Client.Test.Connection;
using AceQL.Client.Test.Dml;
using AceQL.Client.Test.Util;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AceQL.Client.test.Dml
{
    /// <summary>
    /// Class SpecialSequenceTest.
    /// Do a full sequence of INSERT / SELECT / UPDATE / SELECT with special characters (Russian, ...)
    /// </summary>
    public class SpecialSequenceTest
    {
        private AceQLConnection connection;

        public static void TheMain(string[] args)
        {
            try
            {
                DoIt().Wait();

                AceQLConsole.WriteLine();
                AceQLConsole.WriteLine("Press enter to continue....");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                AceQLConsole.WriteLine(exception.ToString());
                AceQLConsole.WriteLine(exception.StackTrace);
                AceQLConsole.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }

        static async Task DoIt()
        {

            var netCoreVer = System.Environment.Version; // 3.0.0
            AceQLConsole.WriteLine(netCoreVer + "");

            using (AceQLConnection connection = await ConnectionCreator.ConnectionCreateAsync().ConfigureAwait(false))
            {
                SpecialSequenceTest specialSequenceTest = new SpecialSequenceTest(connection);
                await specialSequenceTest.TestSequence();
            }

        }

        public SpecialSequenceTest(AceQLConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Do a full sequence of INSERT / SELECT / UPDATE / SELECT and test at each
        /// action that attended values are OK with Xunit.
        /// </summary>
        public async Task TestSequence()
        {
            // Purge all
            SqlDeleteTest sqlDeleteTest = new SqlDeleteTest(connection);
            await sqlDeleteTest.DeleteCustomerAll();
            AceQLConsole.WriteLine("Delete witht DeleteCustomerAll() done to clear all for test.");

            SqlInsertTest sqlInsertTest;
            for (int i = 0; i < 100; i++)
            {
                sqlInsertTest = new SqlInsertTest(connection);
                await sqlInsertTest.InsertCustomer(i);
            }

            SqlSelectTest sqlSelectTest = new SqlSelectTest(connection);
            await sqlSelectTest.SelectCustomerExecute();

        }

    }
}
