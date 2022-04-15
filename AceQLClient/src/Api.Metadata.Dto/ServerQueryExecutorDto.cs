using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api.Metadata.Dto
{
    /// <summary>
    /// Class ServerQueryExecutorDto.
    /// </summary>
    public class ServerQueryExecutorDto
    {
		private string serverQueryExecutorClassName1;
		private List<string> parameterTypes1;
		private List<string> parameterValues1;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="serverQueryExecutorClassName"> </param>
		/// <param name="parameterTypes"> </param>
		/// <param name="parameterValues"> </param>
		public ServerQueryExecutorDto(string serverQueryExecutorClassName, List<string> parameterTypes, List<string> parameterValues)
		{
			this.serverQueryExecutorClassName1 = serverQueryExecutorClassName;
			this.parameterTypes1 = parameterTypes;
			this.parameterValues1 = parameterValues;
		}

		///THE PROPERTY MUST BE IN LOWERCASE FIRST CHAR FOR HOST TO DESERIALIZE.
		/// <returns> the serverQueryExecutorClassName </returns>
		public virtual string serverQueryExecutorClassName
		{
			get
			{
				return serverQueryExecutorClassName1;
			}
		}

		///THE PROPERTY MUST BE IN LOWERCASE FIRST CHAR FOR HOST TO DESERIALIZE.
		/// <returns> the parameterTypes </returns>
		public virtual List<string> parameterTypes
		{
			get
			{
				return parameterTypes1;
			}
		}

		///THE PROPERTY MUST BE IN LOWERCASE FIRST CHAR FOR HOST TO DESERIALIZE.
		/// <returns> the parameterValues </returns>
		public virtual List<string> parameterValues
		{
			get
			{
				return parameterValues1;
			}
		}

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
		{
			return "ServerQueryExecutorDto [serverQueryExecutorClassName=" + serverQueryExecutorClassName + ", parameterTypes=" + parameterTypes + ", parameterValues=" + parameterValues + "]";
		}



	}
}
