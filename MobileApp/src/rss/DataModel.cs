using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Database.Sqlite;
using Android.Database;
using Android.Runtime;
using System.Threading.Tasks;

namespace KosenMobile.src.rss {
  public class DataModel: SQLiteOpenHelper {
    public class Model {
      public string id_;
      public string date_;
      public string title_;
      public string detail_;
      public string hash_;
    }

    static Model HEADER;

    Context context_;
    private static int databaseVersion_ => 1;
    public static string databaseName_ => "rss.db";
    public  string databasePath_;
    private static string tableName_ => "rss";

    private List<Model> data_;
    public IReadOnlyList<Model> dataRef_;


    public DataModel(Context _context) : base(_context, System.IO.Path.Combine(_context.DataDir.Path, DataModel.databaseName_), null, databaseVersion_)  {
      context_ = _context;
      databasePath_ = System.IO.Path.Combine(context_.DataDir.Path, DataModel.databaseName_);
      data_ = new List<Model>();
      dataRef_ = data_;
      HEADER = new Model() {
        id_ = "id",
        date_ = "date",
        title_ = "title",
        detail_ = "detail",
        hash_ = "hash"
      };
    }

    public void adddata() {
      
      data_.Add(new Model() {date_ = "2020-03-01", detail_ = "", hash_ = "", id_ = "", title_ = "aefwefwe" });
    }

    public override void OnCreate(SQLiteDatabase db) {
      //throw new NotImplementedException();
    }

    public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
      //throw new NotImplementedException();
    }

    public override void OnDowngrade(SQLiteDatabase db, int oldVersion, int newVersion) {
      base.OnDowngrade(db, oldVersion, newVersion);
    }

    public override void OnOpen(SQLiteDatabase db) {
      base.OnOpen(db);
    }

    public override void OnConfigure(SQLiteDatabase db) {
      base.OnConfigure(db);
    }

    public void write() {
      var db = this.WritableDatabase;
    }

    public void read() {
      var db = this.ReadableDatabase;


      var cursor = db.Query(tableName_,
        new string[] {
          HEADER.id_,
        HEADER.date_,
        HEADER.title_,
        HEADER.detail_,
        HEADER.hash_},
        null,
        null,
        null,
        null,
        "date DESC");

      
      cursor.MoveToFirst();
      for(var i = 0;i < cursor.Count;i++) {
        data_.Add(new Model() {
          id_ = cursor.GetString(0),
          date_ = cursor.GetString(1),
          title_ = cursor.GetString(2),
          detail_ = cursor.GetString(3),
          hash_ = cursor.GetString(4)
        });
        cursor.MoveToNext();
      }
      cursor.Close();
      return;
    }
  }
}
