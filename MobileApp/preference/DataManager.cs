using System;

using Android;
using Android.Content;
using System.Xml;
using System.Xml.Linq;

namespace KosenMobile.preference {
  public class DataManager : IDisposable {
    string preferencePath_;
    string preferenceFileName_ = "preference.xml";
    Context context_;
    public DataModel dataModel_ { get; private set; }

    public DataManager(Context _context) {
      context_ = _context;
      dataModel_ = new DataModel(_context);
      if(!System.IO.File.Exists(dataModel_.preferencePath_)) {
        Restore();
      } else {
        Load();
      }
    }

    public void Restore() {
      dataModel_.Write(new DataModel.Model() {
        checkForUpdate_ = false,
        cache_ = false,
        useInternalBrowser_ = true
      });
    }

    public void Load() {
      dataModel_.Read();
    }

    public void Store() {
      dataModel_.write();
    }

    public void Store(DataModel.Model _model) {
      dataModel_.Write(_model);
    }


    public void Dispose() {
      Store();
      GC.SuppressFinalize(this);
    }
  }
}
