using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redisdump
{
    
    public class ApplicationArguments
    {
        private string _file = "";
        public string File
        {
            get
            {
                return _file;
            }
            set
            {
                _file = value;
            }
        }

        private string _host = "localhost";
        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
            }
        }

        private int _port = 6379;
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        private string _password = "";
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        private int _dbID = 0;
        public int DbID
        {
            get
            {
                return _dbID;
            }
            set
            {
                _dbID = value;
            }
        }

        private bool _backup = false;
        public bool Backup
        {
            get
            {
                return _backup;
            }
            set
            {
                _backup = value;
            }
        }

        private bool _restore = false;
        public bool Restore
        {
            get
            {
                return _restore;
            }
            set
            {
                _restore = value;
            }
        }
    }
}
