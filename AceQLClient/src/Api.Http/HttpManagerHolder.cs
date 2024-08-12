using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AceQL.Client.src.Api.Http
{

    internal class HttpManagerHolder
    {
        private string proxyUri;
        private ICredentials proxyCredentials;
        private int timeout;
        private bool enableDefaultSystemAuthentication;
        private Dictionary<string, string> requestHeaders;

        private int maxRetries = 0;
        private int retryIntervalMs = 0;

        public string ProxyUri { get => proxyUri; set => proxyUri = value; }
        public ICredentials ProxyCredentials { get => proxyCredentials; set => proxyCredentials = value; }
        public int Timeout { get => timeout; set => timeout = value; }
        public bool EnableDefaultSystemAuthentication { get => enableDefaultSystemAuthentication; set => enableDefaultSystemAuthentication = value; }
        public Dictionary<string, string> RequestHeaders { get => requestHeaders; set => requestHeaders = value; }
        public int MaxRetries { get => maxRetries; set => maxRetries = value; }
        public int RetryIntervalMs { get => retryIntervalMs; set => retryIntervalMs = value; }
    }
}
