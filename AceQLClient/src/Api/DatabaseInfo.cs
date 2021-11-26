using AceQL.Client.Api.Metadata.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api
{
	/// 
	/// <summary>
	/// A simple shortcut class that contains main remote database and JDBC info.
	/// 
	/// @author Nicolas de Pomereu
	/// @since 8.1
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

		/// <summary>
		/// Package protected Constructor </summary>
		/// <param name="databaseInfoDto"> The DatabaseInfo container </param>

		internal DatabaseInfo(DatabaseInfoDto databaseInfoDto)
		{
			DatabaseInfo theDatabaseInfo = databaseInfoDto.DatabaseInfo;
			datatabaseMajorVersion = theDatabaseInfo.DatatabaseMajorVersion;
			databaseMinorVersion = theDatabaseInfo.DatabaseMinorVersion;
			databaseProductName = theDatabaseInfo.DatabaseProductName;
			databaseProductVersion = theDatabaseInfo.DatabaseProductVersion;
			driverMajorVersion = theDatabaseInfo.DriverMajorVersion;
			driverMinorVersion = theDatabaseInfo.DriverMinorVersion;
			driverName = theDatabaseInfo.DriverName;
			driverVersion = theDatabaseInfo.DriverVersion;
		}
		
		/// <summary>
		/// Gets the database major version </summary>
		/// <returns> the database major version  </returns>
		public virtual int DatatabaseMajorVersion
		{
			get
			{
				return datatabaseMajorVersion;
			}
		}

		/// <summary>
		/// Gets the database minor version </summary>
		/// <returns> the database minor version  </returns>
		public virtual int DatabaseMinorVersion
		{
			get
			{
				return databaseMinorVersion;
			}
		}

		/// <summary>
		/// Gets the database product name </summary>
		/// <returns> the database product name </returns>
		public virtual string DatabaseProductName
		{
			get
			{
				return databaseProductName;
			}
		}

		/// <summary>
		/// Gets the database product version </summary>
		/// <returns> the database product version </returns>
		public virtual string DatabaseProductVersion
		{
			get
			{
				return databaseProductVersion;
			}
		}

		/// <summary>
		/// Gets the driver major version </summary>
		/// <returns> the driver major version  </returns>
		public virtual int DriverMajorVersion
		{
			get
			{
				return driverMajorVersion;
			}
		}

		/// <summary>
		/// Gets the driver minor version </summary>
		/// <returns> the driver minor version  </returns>
		public virtual int DriverMinorVersion
		{
			get
			{
				return driverMinorVersion;
			}
		}

		/// <summary>
		/// Gets the driver name </summary>
		/// <returns> the driver name </returns>
		public virtual string DriverName
		{
			get
			{
				return driverName;
			}
		}

		/// <summary>
		/// Gets the driver version </summary>
		/// <returns> the driver version  </returns>
		public virtual string DriverVersion
		{
			get
			{
				return driverVersion;
			}
		}

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
		{
			return "DatabaseInfo [datatabaseMajorVersion=" + datatabaseMajorVersion + ", databaseMinorVersion=" + databaseMinorVersion + ", databaseProductName=" + databaseProductName + ", databaseProductVersion=" + databaseProductVersion + ", driverMajorVersion=" + driverMajorVersion + ", driverMinorVersion=" + driverMinorVersion + ", driverName=" + driverName + ", driverVersion=" + driverVersion + "]";
		}

	}

}
