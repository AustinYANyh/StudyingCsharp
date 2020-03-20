using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace testSqlite
{
    class Program
    {
        static SQLiteConnection sqliteConnection = new SQLiteConnection();

        void CreateNewDB()
        {
            SQLiteConnection.CreateFile("MySQliteDB.sqlite");
        }

        void ConnectDB()
        {
            sqliteConnection = new SQLiteConnection("Data Source = MySQliteDB.sqlite;Version=3;");
            sqliteConnection.Open();
        }

        void CreateTable()
        {
            string sql = "create table Test (name varchar(20),point int)";
            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);
            command.ExecuteNonQuery();
        }

        void InsertData()
        {
            string sql = "insert into Test (name, point) values ('Me', 3000)";
            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);
            command.ExecuteNonQuery();
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.CreateNewDB();
            p.ConnectDB();
            p.CreateTable();
            p.InsertData();

            string sql = "select * from Test order by point desc";
            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["point"]);
            Console.ReadLine();
        }

    }
}
