using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Test.Util
{
    public class SHA1
	{
		public static string GetChecksum(string filename)
		{
			using (var hasher = System.Security.Cryptography.HashAlgorithm.Create("SHA1"))
			{
				using (var stream = System.IO.File.OpenRead(filename))
				{
					var hash = hasher.ComputeHash(stream);
					return BitConverter.ToString(hash).Replace("-", "");
				}
			}
		}

	}
}
