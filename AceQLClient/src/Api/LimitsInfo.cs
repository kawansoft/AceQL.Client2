using AceQL.Client.Api.Metadata.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api
{
	/// <summary>
	/// A simple shortcut class that contains main remote database limits info:
	/// maxRows: the maximum number of SQL rows that may be returned to the
	/// client by the AceQL server.
	/// maxBlobLength: the maximum length allowed for a Blob upload.
	/// 
	/// </summary>
	public class LimitsInfo
	{

		private long maxRows = 0;
		private long maxBlobLength = 0;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="limitsInfoDto">	the Limits DTO </param>
		internal LimitsInfo(LimitsInfoDto limitsInfoDto)
		{
			this.maxRows = limitsInfoDto.MaxRows;
			this.maxBlobLength = limitsInfoDto.MaxBlobLength;
		}

		/// <summary>
		/// Gets the maximum number of SQL rows that may be returned to the client by the
		/// AceQL server.
		/// </summary>
		/// <returns> the maximum number of SQL rows that may be returned to the client by
		///         the AceQL server. </returns>
		public virtual long MaxRows
		{
			get
			{
				return maxRows;
			}
		}

		/// <summary>
		/// Gets the maximum length allowed for a Blob upload. </summary>
		/// <returns> the maximum length allowed for a Blob upload. </returns>
		public virtual long MaxBlobLength
		{
			get
			{
				return maxBlobLength;
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return "LimitsInfo [maxRows=" + maxRows + ", maxBlobLength=" + maxBlobLength + "]";
		}
	}

}
