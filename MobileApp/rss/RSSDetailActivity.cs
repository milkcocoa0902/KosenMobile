
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;

namespace KosenMobile.rss {
  [Activity(Label = "RSSDetailActivity")]
  public class RSSDetailActivity : Activity {
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.rss_detail_activity);

      var webView = FindViewById<WebView>(Resource.Id.webView);
      webView.LoadUrl(Intent.GetStringExtra("detail"));

      // Create your application here
    }
  }
}
