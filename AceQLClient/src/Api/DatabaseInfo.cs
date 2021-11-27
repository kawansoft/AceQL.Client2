using AceQL.Client.Api.Metadata.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api
{
	/// 
	/// <summary>
	/// A simple shortcut class that contains main remote database and JDBC info.
	/// </summary>
	public class DatabaseInfo
	{
		private int datatabaseMajorVersion;
		private int databaseMinorVersion;
		private string databaseProductName;
		private string databaseProductVersion;
		private int driverMajorVersion;
		private int driverMinorVersion;
		private string driverName;
		private string driverVersion;

        internal DatabaseInfo(DatabaseInfoDto databaseInfoDto)
        {
            this.datatabaseMajorVersion = databaseInfoDto.DatatabaseMajorVersion;
            this.databaseMinorVersion = databaseInfoDto.DatabaseMinorVersion;
            this.databaseProductName = databaseInfoDto.DatabaseProductName;
            this.databaseProductVersion = databaseInfoDto.DatabaseProductVersion;
            this.driverMajorVersion = databaseInfoDto.DriverMajorVersion;
            this.driverMinorVersion = databaseInfoDto.DriverMinorVersion;
            this.driverName = databaseInfoDto.DriverName;
            this.driverVersion = databaseInfoDto.DriverVersion;
        }

        public int DatatabaseMajorVersion { get => datatabaseMajorVersion; }
		public int DatabaseMinorVersion { get => databaseMinorVersion;}
		public string DatabaseProductName { get => databaseProductName;  }
        public string DatabaseProductVersion { get => databaseProductVersion;  }
        public int DriverMajorVersion { get => driverMajorVersion; }
        public int DriverMinorVersion { get => driverMinorVersion; }
        public string DriverName { get => driverName;  }
        public string DriverVersion { get => driverVersion;}

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
		{
			return "DatabaseInfo [datatabaseMajorVersion=" + DatatabaseMajorVersion + ", databaseMinorVersion=" + DatabaseMinorVersion + ", databaseProductName=" + DatabaseProductName + ", databaseProductVersion=" + DatabaseProductVersion + ", driverMajorVersion=" + DriverMajorVersion + ", driverMinorVersion=" + DriverMinorVersion + ", driverName=" + DriverName + ", driverVersion=" + DriverVersion + "]";
		}

	}

}
