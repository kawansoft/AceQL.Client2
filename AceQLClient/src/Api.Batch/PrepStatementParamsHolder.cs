/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2022,  KawanSoft SAS
 * (http://www.kawansoft.com). All rights reserved.                                
 *                                                                               
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this filePath except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api.Batch
{
    /// <summary>
    /// Class PrepStatementParamsHolder. Allows to store all parameters of a Prepared Statement
    /// </summary>
    internal class PrepStatementParamsHolder
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
