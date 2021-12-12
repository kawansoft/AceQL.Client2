using AceQL.Client.Api.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AceQL.Client.Test.Util
{
    public class TypesDisplayUtil
    {
        public static void TestCSharpTypes()
        {
            /*
            bool    : Boolean
            short   : Int16
            int     : Int32
            String  : String
            string  : String
            float   : Single
            double  : Double
            long    : Int64
            DateTime: DateTime 132837806962201774
            */

            bool myBool = true;
            short myShort = 1;
            int myInt = 42;
            String myString = "myString";
            string mystring = "mystring";
            float myFloat = 42;
            double myDouble = 42.42;
            long myLong = 42L;
            DateTime myDateTime = DateTime.Now;

            AceQLConsole.WriteLine("bool    : " + myBool.GetType().Name);
            AceQLConsole.WriteLine("short   : " + myShort.GetType().Name);
            AceQLConsole.WriteLine("int     : " + myInt.GetType().Name);
            AceQLConsole.WriteLine("String  : " + myString.GetType().Name);
            AceQLConsole.WriteLine("string  : " + mystring.GetType().Name);
            AceQLConsole.WriteLine("float   : " + myFloat.GetType().Name);
            AceQLConsole.WriteLine("double  : " + myDouble.GetType().Name);
            AceQLConsole.WriteLine("long    : " + myLong.GetType().Name);
            AceQLConsole.WriteLine("DateTime Old: " + myDateTime.GetType().Name + " " + ConvertToTimestampOld(myDateTime));
            AceQLConsole.WriteLine("DateTime New: " + myDateTime.GetType().Name + " " + ConvertToTimestamp(myDateTime).ToString());
            AceQLConsole.WriteLine();
            AceQLConsole.WriteLine("Press enter to continue....");
            Console.ReadLine();
        }

        internal static String ConvertToTimestampOld(DateTime dateTime)
        {
            double theDouble = (TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
            String theTimeString = theDouble.ToString(CultureInfo.InvariantCulture);

            // Remove "." or ',' depending on Locale:
            theTimeString = StringUtils.SubstringBefore(theTimeString, ",");
            theTimeString = StringUtils.SubstringBefore(theTimeString, ".");
            return theTimeString;
        }

        internal static long ConvertToTimestamp(DateTime value)
        {
            DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan elapsedTime = TimeZoneInfo.ConvertTime(value, TimeZoneInfo.Utc) - Epoch;
            long theTimestamp = (long)elapsedTime.TotalMilliseconds;
            return theTimestamp;
        }

    }
}
