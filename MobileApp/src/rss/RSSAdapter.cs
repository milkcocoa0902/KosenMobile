using System;
using System;

using Android.OS;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Runtime;
using Android.App;
using Android.Graphics;

namespace KosenMobile.src.rss {
  public class Adapter : RecyclerView.Adapter {
    List<DataModel.Model> rows_;
    Activity activity_;
    public Adapter(Activity _activity, List<DataModel.Model> _rows) {
      activity_ = _activity;
      rows_ = _rows;
    }

    public Adapter(Activity _activity) {
      activity_ = _activity;
    }

    public override int ItemCount => rows_.Count;
    public Action<DataModel.Model> onRowClicked;

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
      ((Holder)(holder)).created_.Text = DateTime.Parse(rows_[position].date_).ToString("yyyy-MM-dd");
      ((Holder)(holder)).content_.Text = rows_[position].title_;
      ((Holder)(holder)).id_ = rows_[position].id_;
      ((Holder)(holder)).hash_ = rows_[position].hash_;
      ((Holder)(holder)).detail_ = rows_[position].defail_;
      //((Holder)(holder)).content_.Click += (sender, e) => {}
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
      return new Holder(LayoutInflater
            .From(parent.Context)
            .Inflate(Resource.Layout.rss_summary,
                parent,
                false));
    }
  }
}


