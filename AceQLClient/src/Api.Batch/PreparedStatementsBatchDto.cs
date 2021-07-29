using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api.Batch
{
    /// <summary>
    /// Class PreparedStatementsBatchDto.
    /// </summary>
    public class PreparedStatementsBatchDto
    {
        private readonly List<PrepStatementParamsHolder> prepStatementParamsHolderList1 = new List<PrepStatementParamsHolder>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PreparedStatementsBatchDto"/> class.
        /// </summary>
        /// <param name="prepStatementParamsHolderList">The prep statement parameters holder list.</param>
        public PreparedStatementsBatchDto(List<PrepStatementParamsHolder> prepStatementParamsHolderList)
        {
            this.prepStatementParamsHolderList1 = prepStatementParamsHolderList;
        }

        /// <summary>
        /// Gets or sets the prep statement parameters holder list. THE PROPERTY MUST BE IN LOWERCASE FIRST CHAR FOR HOST TO DESERIALIZE.
        /// </summary>
        /// <value>The prep statement parameters holder list.</value>
        public List<PrepStatementParamsHolder> prepStatementParamsHolderList { get => prepStatementParamsHolderList1; set => prepStatementParamsHolderList1 = value; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "PreparedStatementsBatchDto [prepStatementParamsHolderList=" + String.Join(", ", prepStatementParamsHolderList1) + "]";
        }
    }
}
