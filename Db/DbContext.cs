using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace WeenieIconBuilder.Db
{
    public class DbContext
    {
        private MySqlConnection mysql;
        public bool usingSQLite { get; private set; }

        // Wether or not we successfully opened a database connection
        public bool Connected { get; private set; }

        public DbContext() {
            Connected = InitMySQL();
        }

        public void Close()
        {
            mysql.Close();  
        }

        private bool InitMySQL()
        {
            string host = WeenieIconBuilderSettings.Default.mysql_host;
            string user = WeenieIconBuilderSettings.Default.mysql_user;
            string pass = WeenieIconBuilderSettings.Default.mysql_pass;
            string dbname = WeenieIconBuilderSettings.Default.mysql_dbname;
            string port = WeenieIconBuilderSettings.Default.mysql_port;

            string connect = $"server={host};port={port};database={dbname};uid={user};pwd={pass}";
            try
            {
                mysql = new MySqlConnection(connect);
                mysql.Open();
            }
            catch (Exception ex)
            {
                mysql.Close();
                return false;
            }

            return true;
        }

        public DbDataReader GetReader(string query, Dictionary<string, object> parameters = null)
        {
            var command = mysql.CreateCommand();
            command.CommandText = query;

            if (parameters != null)
                foreach (var p in parameters)
                    command.Parameters.AddWithValue(p.Key, p.Value);

            var reader = command.ExecuteReader();

            return reader;
        }
    }
}
