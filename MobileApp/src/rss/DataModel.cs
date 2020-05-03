using System;
using System.Collections.Generic;

namespace KosenMobile.src.rss {
  public class DataModel {
    public class Model {
      public string id_;
      public string date_;
      public string title_;
      public string defail_;
      public string hash_;

    }

    private List<Model> data_;
    public IReadOnlyList<Model> dataRef_;


    public DataModel() {
      data_ = new List<Model>();
      dataRef_ = data_;
    }

    public void adddata() {
      
      data_.Add(new Model() {date_ = "2020-03-01", defail_ = "", hash_ = "", id_ = "", title_ = "aefwefwe" });
    }
  }
}
