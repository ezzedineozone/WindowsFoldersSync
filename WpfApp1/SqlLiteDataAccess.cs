using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class SqlLiteDataAccess
    {
        public static List<SaveSlot> GetSaveSlots()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<SaveSlot>("select * from SaveSlot", new DynamicParameters());
                return output.ToList();
            }

        }
        public static void AddSaveSlot(SaveSlot slot)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into SaveSlot (Source, Destination, 'Order') values (@Source, @Destination, @Order)", slot);

            }

        }
        public static void DeleteTable()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from SaveSlot");
            }
        }
        private static string LoadConnectionString(string id="default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
