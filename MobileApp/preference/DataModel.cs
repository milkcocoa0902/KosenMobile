using System;

using Android;
using Android.Content;

namespace KosenMobile.preference {
  public class DataModel {
    public class Model {
      public bool checkForUpdate_;
      public bool cache_;
      public bool useInternalBrowser_;
    }


    public string preferencePath_;
    public Model pref_ { private set;  get; }
    string preferenceFileName_ = "preference.xml";
    Context context_;

    public DataModel(Context _context) {
      context_ = _context;
      preferencePath_ = System.IO.Path.Join(context_.DataDir.Path, preferenceFileName_);
    }

    public void write() {
      System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(DataModel.Model));
      System.IO.StreamWriter sw = new System.IO.StreamWriter(preferencePath_, false, new System.Text.UTF8Encoding(false));
      serializer.Serialize(sw, pref_);
      sw.Close();
    }

    public void Write(Model _model) {
      pref_ = _model;
      write();
    }

    public void Read() {
      System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(DataModel.Model));
      System.IO.StreamReader sr = new System.IO.StreamReader(preferencePath_, new System.Text.UTF8Encoding(false));
      pref_ = (DataModel.Model)serializer.Deserialize(sr);
      sr.Close();
    }

  }
}
