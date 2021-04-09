
namespace AceQL.Client.Samples
{
    public class ConnectionInfo
    {
        private string serverUrl;
        private string database;
        private string username;
        private bool password;
        private bool passwordIsSessionId;
        private string proxies;
        private string auth;
        private int timeout = 0;
        private bool gzipResult;
        private string headers;

        public ConnectionInfo(string serverUrl, string database, string username, bool password, bool passwordIsSessionId, string proxies, string auth, int timeout, bool gzipResult, string headers)
        {
            this.serverUrl = serverUrl;
            this.database = database;
            this.username = username;
            this.password = password;
            this.passwordIsSessionId = passwordIsSessionId;
            this.proxies = proxies;
            this.auth = auth;
            this.timeout = timeout;
            this.gzipResult = gzipResult;
            this.headers = headers;
        }

        public string ServerUrl { get => serverUrl;  }
        public string Database { get => database; }
        public string Username { get => username; }
        public bool Password { get => password; }
        public bool PasswordIsSessionId { get => passwordIsSessionId; }
        public string Proxies { get => proxies; }
        public string Auth { get => auth; }
        public int Timeout { get => timeout;  }
        public bool GzipResult { get => gzipResult; }
        public string Headers { get => headers; }
    }
}

