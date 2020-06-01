using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Android.Content;
using Android.Util;
using System.Linq;

namespace KosenMobile.syllabus {
  public class DataManager {
    HttpClient client_;
    Context context_;
    public DataModel dataModel_;

    public DataManager(Context _context) {
      dataModel_ = new DataModel(_context);
      context_ = _context;
      if(!File.Exists(dataModel_.databasePath_)) {
        CreateNew();
        Log.Debug("RSS.DataManager", "Create New Database");
      } else {
        if(CheckForUpdate()) Update().Wait();
      }

      Load();
      Log.Debug("RSS.DataManager", "Load Database");
    }


    void CreateNew() {
      client_ = new HttpClient();
      using(var request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://me.milkcocoa.info/syllabus.xml")))
      using(var response = client_.SendAsync(request).Result) {

        if(response.StatusCode == HttpStatusCode.OK) {

          var content = response.Content;
          var stream = content.ReadAsStreamAsync().Result;


          var path = dataModel_.databasePath_;
          var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);


          stream.CopyTo(fileStream);
          stream.Close();
          fileStream.Close();
        }
      }

    }

    public async Task Update() {
    }

    bool CheckForUpdate() {
      return false;
    }

    void Load() {
      dataModel_.read();
    }

    List<DataModel.Subject> grade(int _grade) {
      return dataModel_.dataRef_.Where(sbj => sbj.grade_ == _grade).ToList();
    }

    List<DataModel.Subject> course(DataModel.Course _course) {
      return dataModel_.dataRef_.Where(sbj => sbj.course_ == _course.ToString()).ToList();
    }

    List<DataModel.Subject> gradeAndCourse(int _grade, int _course) {
      return grade(_grade).Where(sbj => sbj.course_ == _course.ToString()).ToList();
    }
  }
}
