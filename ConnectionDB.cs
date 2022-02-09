using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPS
{
    class ConnectionDB
    {
        public string getConnection()
        {
            string con = "datasource = 192.168.1.88; database = invoice; port = 3306; username = root; password =; SslMode = none";
            return con;
        }
    }
}
