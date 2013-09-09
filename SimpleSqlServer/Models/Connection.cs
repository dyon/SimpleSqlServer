using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleSqlServer.Models
{
    public class ConnectionInfo
    {
        public string Server { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }
    }
}
