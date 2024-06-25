using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableHouse
{
    class DBadapter
    {
        private string _dbName = "house";
        private List<string> tables = new List<string>();
        private SQLiteConnection qLiteConnection = null;

        /// <summary>
        /// Список таблиц в базе данных
        /// </summary>
        public string[] Tables { get {
                return tables.ToArray();
            } }

        public DBadapter() { }
        /// <summary>
        /// Создание экземпляра с параметрами
        /// </summary>
        /// <param name="name">имя БД</param>
        public DBadapter(string name) { 
            _dbName = name;
        }
     
        public void Init()
        {
            CreateDB(_dbName);
            string connectionString = "Data Source=" + _dbName + ".sqlite;Version=3;";
            qLiteConnection = new SQLiteConnection(connectionString);
        }
        /// <summary>
        /// Создание таблиц по шаблону
        /// </summary>
        /// <param name="pairs">шаблон название - столбцы</param>
        public void CreateTables(Dictionary<string, string[]> pairs)
        {
            foreach(var pair in pairs)
            {
                CreateTableDB(qLiteConnection, pair.Key, pair.Value);
            }
           
        }

        private static void CreateDB(string name)
        {
            if (!File.Exists(name + ".sqlite")) SQLiteConnection.CreateFile(name + ".sqlite");
        }

        private static void CreateTableDB(SQLiteConnection m_dbConnection, string tableName, string[] columns)
        {


            string sql = "CREATE TABLE IF NOT EXISTS " + tableName + " (id INTEGER PRIMARY KEY AUTOINCREMENT,";     //name varchar(20), score int)";
            for (int i = 0; i < columns.Length; i++)
            {
                if (i != columns.Length - 1) sql += columns[i] + ", ";
                else sql += columns[i];
            }
            sql += ", AddDate DATE, SK TEXT)";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

        }
    }
}
