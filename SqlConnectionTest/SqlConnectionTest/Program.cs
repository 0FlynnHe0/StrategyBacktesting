using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DataApplication myData = new DataApplication(Configuration.dataBaseName, Configuration.connectionString);
            myData.GetETFPreClosePrice(Configuration.tableOf50ETF);


        }
    }
}
