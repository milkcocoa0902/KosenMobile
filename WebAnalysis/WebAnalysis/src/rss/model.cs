using System;
using System.Data;
using System.Data.SQLite;

namespace WebAnalysis.RSS{
    class DataBase{
        SQLiteConnectionStringBuilder con_;
        public DataBase(){
            con_ = new SQLiteConnectionStringBuilder{DataSource = ":memory:"};

            using(var cn = new SQLiteConnection(con_.ToString())){
                cn.Open();

                using(var cmd = new SQLiteCommand(cn)){
                    cmd.CommandText = "select sqlite_version()";
                    Console.WriteLine(cmd.ExecuteScalar());
                }
            }
        }
    }
}