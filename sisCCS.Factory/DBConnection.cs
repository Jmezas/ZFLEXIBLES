using System;
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Factory
{
    public class DBConnection
    {
        protected System.Data.Common.DbConnection GetConnection(DataBase Database)
        {
            switch (Database)
            {
                case DataBase.SqlServer:
                    return new SqlConnection(ConfigurationManager.ConnectionStrings["SQLLocalConnection"].ConnectionString);
                case DataBase.Oracle:
                    return null;
                case DataBase.MySQL:
                    return new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString);
            }
            throw new Exception("No se ha especificado una base de datos correcta.");
        }

        protected System.Data.Common.DbConnection GetServerConnection(DataBase Database)
        {
            switch (Database)
            {
                case DataBase.SqlServer:
                    return new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServerConnection"].ConnectionString);
                case DataBase.Oracle:
                    return null;
                case DataBase.MySQL:
                    return new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString);
            }
            throw new Exception("No se ha especificado una base de datos correcta.");
        }
    }

    public enum DataBase
    {
        SqlServer = 1,
        Oracle = 2,
        MySQL = 3
    }
}
