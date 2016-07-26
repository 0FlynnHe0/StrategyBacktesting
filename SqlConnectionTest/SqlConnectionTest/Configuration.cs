using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnectionTest
{
    class Configuration
    {
        /// <summary>
        /// 记录50etf的表名称。
        /// </summary>
        public static string tableOf50ETF = "MarketData_510050_SH";
        /// <summary>
        /// 数据库的名称。
        /// </summary>
        public static string dataBaseName = "WindFullMarket" + "200701";
        /// <summary>
        /// 记录50etf的表名称。
        /// </summary>
        public static string connectionString = "server=(local);database=WindFullMarket" + "200701" + ";Integrated Security=true;";
        /// <summary>
        /// 保存交易日信息的表的名称。
        /// </summary>
        public static string tradeDaysTableName = "myTradeDays";
        /// <summary>
        /// 提供远程数据库的sql连接字符串信息。
        /// </summary>
        public static string connectionString218 = "server=192.168.1.170;uid =reader;pwd=reader;";
        /// <summary>
        /// 给定期权标的的名称。
        /// </summary>
        public static string underlyingAsset = "510050.SH";
        /// <summary>
        /// 无风险收益率。
        /// </summary>
        public static double RiskFreeReturn = 0.05;
    }
}
