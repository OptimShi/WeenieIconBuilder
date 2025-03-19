using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Xml.Linq;
using WeenieIconBuilder.Enums;

namespace WeenieIconBuilder.Db
{
    public class DbManager
    {
        //private SQLiteConnection sqlite;
        public DbContext db;
        public string Version = "";
        public Dictionary<int, dbWeenie> Weenies = new Dictionary<int, dbWeenie>();

        public bool Connect()
        {
            db = new DbContext();
            if (db.Connected)
            {
                GetVersion();
                LoadAllWeenies();
                return true;
            }

            return false;
        }

        public void Disconnect()
        {
            db.Close();
        }

        public void Dispose()
        {
            this.Disconnect();
        }

        private void GetVersion()
        {
            using (var reader = db.GetReader("SELECT * FROM `version` limit 1"))
            {
                while (reader.Read())
                {
                    string version_base = reader.GetString(reader.GetOrdinal("base_Version"));
                    string version_patch = reader.GetString(reader.GetOrdinal("patch_Version"));
                    var mod = reader.GetDateTime(reader.GetOrdinal("last_Modified"));

                    string dbType;
                    if (db.usingSQLite)
                        dbType = "SQLite";
                    else
                        dbType = "MySQL";
                    Version = $"{dbType} - ace_world Base: {version_base}, Patch: {version_patch}, Date: {mod.ToString()}";
                }
            }
        }


        public void LoadAllWeenies()
        {
            string sql = "SELECT * FROM `weenie` where `class_Id` order by `class_Id` ASC;";
            
            using (var reader = db.GetReader(sql))
            {
                while (reader.Read())
                {
                    dbWeenie weenie = new dbWeenie();
                    weenie.WCID = reader.GetInt32(reader.GetOrdinal("class_Id"));
                    Weenies.Add(weenie.WCID, weenie);
                }
            }

            foreach(var weenie in Weenies)
            {
                weenie.Value.DIDs = _GetDIDs(weenie.Key);
                weenie.Value.Ints = _GetInts(weenie.Key);
                weenie.Value.Bools = _GetBools(weenie.Key);
            }
        }

        private Dictionary<PropertyBool, bool> _GetBools(int wcid)
        {
            Dictionary<PropertyBool, bool> results = new Dictionary<PropertyBool, bool>();

            string sql = "SELECT `type`, `value` FROM `weenie_properties_bool` WHERE `object_Id` = @wcid order by `type`";
            Dictionary<string, object> dbParams = new Dictionary<string, object>();
            dbParams.Add("@wcid", wcid);
            using (var reader = db.GetReader(sql, dbParams))
            {
                while (reader.Read())
                {
                    int key = reader.GetInt32(reader.GetOrdinal("type"));
                    int value = reader.GetInt32(reader.GetOrdinal("value"));
                    results.Add((PropertyBool)key, value == 1);
                }
            }

            return results;
        }

        private Dictionary<PropertyFloat, float> _GetFloats(int wcid)
        {
            Dictionary<PropertyFloat, float> results = new Dictionary<PropertyFloat, float>();
            var sql = $"SELECT `type`, `value` FROM `weenie_properties_float` WHERE `object_Id` = @wcid order by `type`";
            Dictionary<string, object> dbParams = new Dictionary<string, object> { { "@wcid", wcid } };
            using (var reader = db.GetReader(sql, dbParams))
            {
                while (reader.Read())
                {
                    int key = reader.GetInt32(reader.GetOrdinal("type"));
                    float value = reader.GetFloat(reader.GetOrdinal("value"));
                    results.Add((PropertyFloat)key, value);
                }
            }

            return results;
        }

        private Dictionary<PropertyInt, int> _GetInts(int wcid)
        {
            Dictionary<PropertyInt, int> results = new Dictionary<PropertyInt, int>();
            var sql = $"SELECT `type`, `value` FROM `weenie_properties_int` WHERE `object_Id` = @wcid order by `type`";
            Dictionary<string, object> dbParams = new Dictionary<string, object> { { "@wcid", wcid } };
            using (var reader = db.GetReader(sql, dbParams))
            {
                while (reader.Read())
                {
                    int key = reader.GetInt32(reader.GetOrdinal("type"));
                    int value = reader.GetInt32(reader.GetOrdinal("value"));
                    results.Add((PropertyInt)key, value);
                }
            }

            return results;
        }

        private Dictionary<PropertyIID, int> _GetIIDs(int wcid)
        {
            Dictionary<PropertyIID, int> results = new Dictionary<PropertyIID, int>();
            var sql = $"SELECT `type`, `value` FROM `weenie_properties_i_i_d` WHERE `object_Id` = @wcid order by `type`";
            Dictionary<string, object> dbParams = new Dictionary<string, object> { { "@wcid", wcid } };
            using (var reader = db.GetReader(sql, dbParams))
            {
                while (reader.Read())
                {
                    int key = reader.GetInt32(reader.GetOrdinal("type"));
                    int value = reader.GetInt32(reader.GetOrdinal("value"));
                    results.Add((PropertyIID)key, value);
                }
            }

            return results;
        }

        private Dictionary<PropertyDID, int> _GetDIDs(int wcid)
        {
            Dictionary<PropertyDID, int> results = new Dictionary<PropertyDID, int>();
            var sql = $"SELECT `type`, `value` FROM `weenie_properties_d_i_d` WHERE `object_Id` = @wcid order by `type`";
            Dictionary<string, object> dbParams = new Dictionary<string, object> { { "@wcid", wcid } };
            using (var reader = db.GetReader(sql, dbParams))
            {
                while (reader.Read())
                {
                    int key = reader.GetInt32(reader.GetOrdinal("type"));
                    int value = reader.GetInt32(reader.GetOrdinal("value"));
                    results.Add((PropertyDID)key, value);
                }
            }

            return results;
        }
    }
}
