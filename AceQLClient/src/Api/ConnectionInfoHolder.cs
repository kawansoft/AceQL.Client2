using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AceQL.Client.src.Api
{
    /// <summary>
    /// Class ConnectionInfoHolder.
    /// </summary>
    public class ConnectionInfoHolder
    {
        // Private fields
        private string connectionString;
        private string server;
        private string database;
        private string username;
        private bool isNTLM;
        private string sessionId;
        private string proxyUri;
        private ICredentials proxyCredentials;
        private bool useCredentialCache;
        private int timeout;
        private bool enableDefaultSystemAuthentication;
        private bool gzipResult;
        private bool enableTrace;

        // No Constructor
    
        // Public properties for read/write access
        public string ConnectionString
        {
            get => connectionString;
            set => connectionString = value;
        }

        public string Server
        {
            get => server;
            set => server = value;
        }

        public string Database
        {
            get => database;
            set => database = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

        public bool IsNTLM
        {
            get => isNTLM;
            set => isNTLM = value;
        }

        public string SessionId
        {
            get => sessionId;
            set => sessionId = value;
        }

        public string ProxyUri
        {
            get => proxyUri;
            set => proxyUri = value;
        }

        public ICredentials ProxyCredentials
        {
            get => proxyCredentials;
            set => proxyCredentials = value;
        }

        public bool UseCredentialCache
        {
            get => useCredentialCache;
            set => useCredentialCache = value;
        }

        public int Timeout
        {
            get => timeout;
            set => timeout = value;
        }

        public bool EnableDefaultSystemAuthentication
        {
            get => enableDefaultSystemAuthentication;
            set => enableDefaultSystemAuthentication = value;
        }

        public bool GzipResult
        {
            get => gzipResult;
            set => gzipResult = value;
        }

        public bool EnableTrace
        {
            get => enableTrace;
            set => enableTrace = value;
        }
    }

}
