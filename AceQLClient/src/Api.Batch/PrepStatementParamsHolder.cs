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
        Dictionary<string, string> statementParameters1;

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

        //public override bool Equals(object obj)
        //{
        //    return obj is PrepStatementParamsHolder holder &&
        //           EqualityComparer<Dictionary<string, string>>.Default.Equals(statementParameters, holder.statementParameters);
        //}

        //public override int GetHashCode()
        //{
        //    return -38088490 + EqualityComparer<Dictionary<string, string>>.Default.GetHashCode(statementParameters);
        //}

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {

            String theString = "PrepStatementParamsHolder [statementParameters= ";
            foreach (KeyValuePair<string, string> kvp in statementParameters)
            {
                theString += " " + kvp.Key + ", " + kvp.Value;
            }
            theString += "]";
            return theString;
        }

    }
}
