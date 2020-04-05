using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace WebAnalysis.RSS{
    class DataBase{
        SQLiteConnectionStringBuilder connectionBuilder_;
        SQLiteConnection connection_;

        string tableName{get{return "rss";}}

        public enum REQUEST{
            CREATE,
            SELECT,
            UPDATE,
            INSERT,
            DELETE
        }


        public class Table{
            public string date_;
            public string detail_;
            public string title_;

            public string hash_;
        };

        public DataBase(){
            connectionBuilder_ = new SQLiteConnectionStringBuilder{DataSource = "WebAnalysis.db"};

            using(var cn = new SQLiteConnection(connectionBuilder_.ToString())){
                cn.Open();

                using(var cmd = new SQLiteCommand(cn)){
                    cmd.CommandText = "create table if not exists rss(id integer not null primary key autoincrement, date text not null, title text not null, detail text not null, hash unique);";
                    Console.WriteLine(cmd.ExecuteScalar());
                }
            }
        }

        public void EXECUTE(System.Collections.Generic.List<string> _query, Action<SQLiteDataReader> _cb = null){
            using (var con = new SQLiteConnection(connectionBuilder_.ToString())){
                con.Open();
                foreach(var q in _query){
                    using(var cmd = new SQLiteCommand(con)){
                        cmd.CommandText = q;
                        if(q.Contains("select")) _cb(cmd.ExecuteReader());
                        else cmd.ExecuteScalar();
                    }
                }
                con.Close();
            }
        }

        public System.Collections.Generic.List<string> RequestBuilder(REQUEST _req, System.Collections.Concurrent.BlockingCollection<Table> _value){
            switch(_req){
                case REQUEST.CREATE:
                    break;
    
                case REQUEST.SELECT:
                    break;
    
                case REQUEST.UPDATE:
                    break;
    
                case REQUEST.INSERT:
                    System.Collections.Generic.List<string> query = new System.Collections.Generic.List<string>();
                    foreach(var v in _value){
                       query.Add("insert into " + tableName + "(date, title, detail, hash) values(" + $"'{v.date_}', '{v.title_}', '{v.detail_}', '{v.hash_}');");
                    }
                    return query;
    
                case REQUEST.DELETE:
                    break;
    
                default:
                    break;

            }
            return new System.Collections.Generic.List<string>();
        }
    }
}