using System;
using System.Collections.Generic;

namespace AceQL.Client.Tests2.tests
{
    class ConnectionOptions
    {
        private string proxies;
        private string auth;
        private bool passwordIsSessionId;
        private bool gzipResult;
        private int timeout = 0;
        private string requestHeaders;

        public ConnectionOptions(string proxies, string auth, bool passwordIsSessionId, bool gzipResult, int timeout, string requestHeaders)
        {
            this.proxies = proxies;
            this.auth = auth;
            this.passwordIsSessionId = passwordIsSessionId;
            this.gzipResult = gzipResult;
            this.timeout = timeout;
            this.requestHeaders = requestHeaders;
        }

        public string Proxies { get => proxies; }
        public string Auth { get => auth; }
        public bool PasswordIsSessionId { get => passwordIsSessionId; }
        public bool GzipResult { get => gzipResult;  }
        public int Timeout { get => timeout;  }
        public string RequestHeaders { get => requestHeaders; }
    }
}
