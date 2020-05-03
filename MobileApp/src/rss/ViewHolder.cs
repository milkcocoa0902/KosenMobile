using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Runtime;


namespace KosenMobile.src.rss {
  public class Holder : RecyclerView.ViewHolder {
    public TextView created_;
    public TextView content_;
    public string id_ = "";
    public string hash_ = "";
    public string detail_ = "";


    public Holder(View itemView) : base(itemView) {
      created_ = itemView.FindViewById<TextView>(Resource.Id.postedDate);
      content_ = itemView.FindViewById<TextView>(Resource.Id.postedTitle);
    }

    public Holder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
    }
  }
}
