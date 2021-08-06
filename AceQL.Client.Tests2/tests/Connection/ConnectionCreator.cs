using AceQL.Client.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Tests.Test.Connection
{
    /// <summary>
    /// Class ConnectionCreator.
    /// </summary>
    public class ConnectionCreator
    {
        /// <summary>
        /// RemoteConnection Quick Start client example.
        /// Creates a Connection to a remote database and open it.
        /// </summary>
        /// <returns>The connection to the remote database</returns>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public static async Task<AceQLConnection> ConnectionCreateAsync()
        {
            string connectionString = ConnectionStringCurrent.Build();

            AceQLConnection theConnection = new AceQLConnection(connectionString);

            // Opens the connection with the remote database.
            // On the server side, a JDBC connection is extracted from the connection 
            // pool created by the server at startup. The connection will remain ours 
            // during the session.
            await theConnection.OpenAsync();

            return theConnection;
        }
    }
}
