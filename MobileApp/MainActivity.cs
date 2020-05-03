using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;

namespace KosenMobile {
  [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
  public class MainActivity : AppCompatActivity {
    src.rss.Adapter adapter_;
    src.rss.DataModel dataModel_;
    src.rss.DataManager dataManager_;

    LinearLayoutManager manager_;

    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      Xamarin.Essentials.Platform.Init(this, savedInstanceState);
      SetContentView(Resource.Layout.activity_main);

      Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
      toolbar.Title = "";
      SetSupportActionBar(toolbar);

      dataManager_ = new src.rss.DataManager(Application.Context);




      var recycler = FindViewById<RecyclerView>(Resource.Id.timeLine);

      //dateTime_ = DateTime.Today;
      //FindViewById<TextView>(Resource.Id.year).Text = dateTime_.Year.ToString("D4") + "年";
      //FindViewById<TextView>(Resource.Id.month).Text = dateTime_.Month.ToString("D2") + "月";
      //FindViewById<TextView>(Resource.Id.day).Text = dateTime_.Day.ToString("D2") + "日";

      /// TODO:
      /// onCreate()で重い処理を実行することは
      /// 起動時間的にも懸念事項となるので将来的に
      /// は別のところで実行をする．
      adapter_ = new src.rss.Adapter(this, dataManager_.dataModel_.dataRef_);
      manager_ = new LinearLayoutManager(this);
      recycler.HasFixedSize = false;
      recycler.SetLayoutManager(manager_);
      recycler.SetAdapter(adapter_);



      FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
      fab.Click += (sender, e) => {
        dataModel_.adddata();
        adapter_.NotifyDataSetChanged();
      };

    }

    public override bool OnCreateOptionsMenu(IMenu menu) {
      MenuInflater.Inflate(Resource.Menu.menu_main, menu);
      return true;
    }

    public override bool OnOptionsItemSelected(IMenuItem item) {
      int id = item.ItemId;
      if(id == Resource.Id.action_settings) {
        return true;
      }

      return base.OnOptionsItemSelected(item);
    }

    private void FabOnClick(object sender, EventArgs eventArgs) {
      View view = (View)sender;
      Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
          .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
    }
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
      Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
  }
}

