using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api.Metadata.Dto
{
    internal class ServerQueryExecutorDto
    {
		private readonly string serverQueryExecutorClassName;
		private readonly List<string> parameterTypes;
		private readonly List<string> parameterValues;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="serverQueryExecutorClassName"> </param>
		/// <param name="parameterTypes"> </param>
		/// <param name="parameterValues"> </param>
		public ServerQueryExecutorDto(string serverQueryExecutorClassName, List<string> parameterTypes, List<string> parameterValues)
		{
			this.serverQueryExecutorClassName = serverQueryExecutorClassName;
			this.parameterTypes = parameterTypes;
			this.parameterValues = parameterValues;
		}

		/// <returns> the serverQueryExecutorClassName </returns>
		public string ServerQueryExecutorClassName
		{
			get
			{
				return serverQueryExecutorClassName;
			}
		}

		/// <returns> the parameterTypes </returns>
		public List<string> ParameterTypes
		{
			get
			{
				return parameterTypes;
			}
		}

		/// <returns> the parameterValues </returns>
		public  List<string> ParameterValues
		{
			get
			{
				return parameterValues;
			}
		}

		public override string ToString()
		{
			return "ServerQueryExecutorDto [serverQueryExecutorClassName=" + serverQueryExecutorClassName + ", parameterTypes=" + parameterTypes + ", parameterValues=" + parameterValues + "]";
		}



	}
}
