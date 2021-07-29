using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api.Batch
{
    /// <summary>
    /// Class PrepStatementParamsHolder. Allows to store all parameters of a Prepared Statement
    /// </summary>
    public class PrepStatementParamsHolder
    {
        /** All the PreparedStatement parameters and their values */
        private readonly Dictionary<string, string> statementParameters1;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrepStatementParamsHolder"/> class.
        /// </summary>
        /// <param name="statementParameters">The statement parameters.</param>
        public PrepStatementParamsHolder(Dictionary<string, string> statementParameters)
        {
            this.statementParameters1 = statementParameters;
        }

        /// <summary>
        /// Gets the statement parameters. THE PROPERTY MUST BE IN LOWERCASE FIRST CHAR FOR HOST TO DESERIALIZE.
        /// </summary>
        /// <value>The statement parameters.</value>
        public Dictionary<string, string> statementParameters { get => statementParameters1; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("PrepStatementParamsHolder [statementParameters= ");
            foreach (KeyValuePair<string, string> kvp in statementParameters)
            {
                sb.Append(" " + kvp.Key + ", " + kvp.Value);
            }
            sb.Append("]");
            return sb.ToString();
        }

    }
}
