using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SqlConnectionTest
{
    /// <summary>
    /// 数据库速度提取的函数。
    /// </summary>
    class DataApplication
    {
        public string connectionString;
        public string dataBase;

        /// <summary>
        /// 构造函数。获取数据库以及SQL连接字符串。
        /// </summary>
        /// <param name="dataBase">数据库名称</param>
        /// <param name="connectionString">连接字符串</param>
        public DataApplication(string dataBase, string connectionString)
        {
            this.connectionString = connectionString;
            this.dataBase = dataBase;
        }
        /// <summary>
        /// 读取50etf前收盘数据的函数
        /// </summary>
        /// <param name="tableName">50etf表</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public SortedDictionary<int, double> GetETFPreClosePrice(string tableName, int startDate = 0, int endDate = 0)
        {

            DataTable myDataTable = new DataTable();
            SortedDictionary<int, double> etfClose = new SortedDictionary<int, double>();

            string commandString;

            commandString = "select distinct [cp],[tdate] from " + tableName + " order by [tdate]";

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = commandString;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataTable);

            foreach (DataRow row in myDataTable.Rows)
            {
                etfClose.Add((int)row["Date"], (double)row["PreClose"]);
            }
            return etfClose;
        }

        /// <summary>
        /// 根据给定的表和日期获取数据内容。
        /// </summary>
        /// <param name="tableName">表的名称</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>DataTable格式的数据</returns>
        public DataTable GetDataTable(string tableName, int startDate = 0, int endDate = 0)
        {
            DataTable myDataTable = new DataTable();
            tableName = "[" + dataBase + "].[dbo].[" + tableName + "]";
            string commandString;
            if (startDate == 0)
            {
                commandString = "select * from " + tableName;
            }
            else
            {
                if (endDate == 0)
                {
                    endDate = startDate;
                }
                commandString = "select * from " + tableName + " where [Date]>=" + startDate.ToString() + " and [Date]<=" + endDate.ToString();
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandText = commandString;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(myDataTable);
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return myDataTable;
        }

        /// <summary>
        /// 查询表中数据条目个数的函数。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public int CountNumber(string tableName)
        {
            int count = 0;
            tableName = "[" + dataBase + "].[dbo].[" + tableName + "]";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();//打开数据库  
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "select count(*) from " + tableName;
                        int num = (int)command.ExecuteScalar();
                        count = num;
                    }
                }

            }
            catch (Exception myerror)
            {
                System.Console.WriteLine(myerror.Message);
            }

            return count;
        }

        /// <summary>
        /// 根据股票数据或者期货数据整理得到的链表
        /// </summary>
        /// <param name="data">DataTable格式的股票或者期货数据</param>
        /// <returns>List格式的数据</returns>
        public List<stockFormat> GetStockList(DataTable data)
        {
            List<stockFormat> stockList = new List<stockFormat>();
            int lastTime = 0;
            foreach (DataRow row in data.Rows)
            {
                stockFormat stock = new stockFormat();
                stock.ask = new stockPrice[5];
                stock.bid = new stockPrice[5];
                stock.code = (int)row["Code"];
                stock.date = (int)row["Date"];
                //剔除非交易时间的交易数据。
                int now = (int)row["Time"] + (int)row["Tick"] * 500;
                if (now < 93000000 || (now > 113000000 && now < 130000000) || (now > 150000000) || (int)row["Tick"] >= 2)
                {
                    continue;
                }
                if (now <= lastTime)
                {
                    continue;
                }
                stock.time = now;
                lastTime = now;
                stock.lastPrice = (double)row["LastPrice"];
                for (int i = 1; i <= 5; i++)
                {
                    stock.ask[i - 1] = new stockPrice((double)row["Ask" + i.ToString()], (double)row["Askv" + i.ToString()]);
                    stock.bid[i - 1] = new stockPrice((double)row["Bid" + i.ToString()], (double)row["Bidv" + i.ToString()]);
                }
                stock.preClose = (double)row["PreClose"];
                stockList.Add(stock);
            }
            return stockList;
        }

        /// <summary>
        /// 根据股票数据或者期货数据整理得到的数组
        /// </summary>
        /// <param name="data">DataTable格式的股票或者期货数组</param>
        /// <returns>List格式的数据</returns>
        public stockFormat[] GetStockArray(DataTable data)
        {
            stockFormat[] stockArray = new stockFormat[28802];
            int lastTime = 0;
            foreach (DataRow row in data.Rows)
            {
                stockFormat stock = new stockFormat();
                stock.ask = new stockPrice[5];
                stock.bid = new stockPrice[5];
                stock.code = (int)row["Code"];
                stock.date = (int)row["Date"];
                //剔除非交易时间的交易数据。
                int now = (int)row["Time"] + (int)row["Tick"] * 500;
                if (now < 93000000 || (now > 113000000 && now < 130000000) || (now > 150000000) || (int)row["Tick"] >= 2)
                {
                    continue;
                }
                if (now <= lastTime)
                {
                    continue;
                }
                stock.time = now;
                lastTime = now;
                stock.lastPrice = (double)row["LastPrice"];
                for (int i = 1; i <= 5; i++)
                {
                    stock.ask[i - 1] = new stockPrice((double)row["Ask" + i.ToString()], (double)row["Askv" + i.ToString()]);
                    stock.bid[i - 1] = new stockPrice((double)row["Bid" + i.ToString()], (double)row["Bidv" + i.ToString()]);
                }
                stock.preClose = (double)row["PreClose"];
                stockArray[TradeDays.TimeToIndex(now)] = stock;
            }
            return stockArray;
        }

    }
}
