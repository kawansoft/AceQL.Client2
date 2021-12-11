using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Test.Util
{
    public class TypesUtil
    {
        public static void TestCSharpTypes()
        {
            bool myBool = true;
            short myShort = 1;
            int myInt = 42;
            String myString = "myString";
            float myFloat = 42;
            double myDouble = 42.42;
            long myLong = 42L;
            DateTime myDateTime = DateTime.Now;

            AceQLConsole.WriteLine("bool.GetType().Name    : " + myBool.GetType().Name);
            AceQLConsole.WriteLine("short.GetType().Name   : " + myShort.GetType().Name);
            AceQLConsole.WriteLine("int.GetType().Name     : " + myInt.GetType().Name);
            AceQLConsole.WriteLine("String.GetType().Name  : " + myString.GetType().Name);
            AceQLConsole.WriteLine("float.GetType().Name   : " + myFloat.GetType().Name);
            AceQLConsole.WriteLine("double.GetType().Name  : " + myDouble.GetType().Name);
            AceQLConsole.WriteLine("long.GetType().Name    : " + myLong.GetType().Name);
            AceQLConsole.WriteLine("DateTime.GetType().Name: " + myDateTime.GetType().Name + " " + myDateTime.ToFileTime());
            AceQLConsole.WriteLine();
            AceQLConsole.WriteLine("Press enter to continue....");
            Console.ReadLine();
        }

    }
}
