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
		private readonly int datatabaseMajorVersion;
		private readonly int databaseMinorVersion;
		private readonly string databaseProductName;
		private readonly string databaseProductVersion;
		private readonly int driverMajorVersion;
		private readonly int driverMinorVersion;
		private readonly string driverName;
		private readonly string driverVersion;

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

        /// <summary>
        /// Gets the database major version.
        /// </summary>
        public int DatatabaseMajorVersion { get => datatabaseMajorVersion; }

        /// <summary>
        /// Gets the database minor version.
        /// </summary>
        public int DatabaseMinorVersion { get => databaseMinorVersion;}

        /// <summary>
        /// Gets the database product name.
        /// </summary>
		public string DatabaseProductName { get => databaseProductName;  }

        /// <summary>
        ///  Gets the database product version.
        /// </summary>
        public string DatabaseProductVersion { get => databaseProductVersion;  }

        /// <summary>
        /// Gets the driver major version.
        /// </summary>
        public int DriverMajorVersion { get => driverMajorVersion; }

        /// <summary>
        /// Gets the driver minor version.
        /// </summary>
        public int DriverMinorVersion { get => driverMinorVersion; }


        /// <summary>
        /// Gets the driver major version.
        /// </summary>
        public string DriverName { get => driverName;  }

        /// <summary>
        /// Gets the driver minor version.
        /// </summary>
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
