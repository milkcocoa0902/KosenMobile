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


namespace KosenMobile.rss {
  public class Adapter : RecyclerView.Adapter {
    IReadOnlyList<DataModel.Model> model_;
    List<DataModel.Model> rows_;
    Activity activity_;
    public Adapter(Activity _activity, IReadOnlyList<DataModel.Model> _model) {
      activity_ = _activity;
      model_ = _model;
    }

    public Adapter(Activity _activity) {
      activity_ = _activity;
    }

    public override int ItemCount => model_.Count;
    public Action<DataModel.Model> onRowClicked;

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
      ((Holder)(holder)).created_.Text = DateTime.Parse(model_[position].date_).ToString("yyyy-MM-dd");
      ((Holder)(holder)).content_.Text = model_[position].title_;
      ((Holder)(holder)).id_ = model_[position].id_;
      ((Holder)(holder)).hash_ = model_[position].hash_;
      ((Holder)(holder)).detail_ = model_[position].detail_;
      ((Holder)(holder)).content_.Click += (sender, e) => {
        var intent = new Android.Content.Intent(activity_.ApplicationContext, typeof(rss.RSSDetailActivity)) ;
        intent.PutExtra("detail", ((Holder)holder).detail_);
        activity_.StartActivity(intent);
      };
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


