using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;

namespace KosenMobile {
  [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
  public class RSSActivity : AppCompatActivity, SwipeRefreshLayout.IOnRefreshListener, NavigationView.IOnNavigationItemSelectedListener {
    rss.Adapter adapter_;
    rss.DataModel dataModel_;
    rss.DataManager dataManager_;
    SwipeRefreshLayout swipe_;
    LinearLayoutManager manager_;
    ActionBarDrawerToggle drawerToggle_;
    DrawerLayout drawer_;
    util.NavigationDrawerUtility drawerUtility_;
    NavigationView navigationView_;


    preference.DataManager preferenceManager;

    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      Xamarin.Essentials.Platform.Init(this, savedInstanceState);
      SetContentView(Resource.Layout.rss_activity);

      Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
      toolbar.Title = "";
      SetSupportActionBar(toolbar);




      swipe_ = FindViewById<SwipeRefreshLayout>(Resource.Id.mainContent);
      swipe_.SetOnRefreshListener(this);
      swipe_.SetColorSchemeColors(new int[] { Application.Context.GetColor(Resource.Color.red),
        Application.Context.GetColor(Resource.Color.blue),
        Application.Context.GetColor(Resource.Color.limegreen)
      });

      preferenceManager = new preference.DataManager(ApplicationContext);


      dataManager_ = new rss.DataManager(Application.Context);
      var recycler = FindViewById<RecyclerView>(Resource.Id.timeLine);

      /// TODO:
      /// onCreate()で重い処理を実行することは
      /// 起動時間的にも懸念事項となるので将来的に
      /// は別のところで実行をする．
      adapter_ = new rss.Adapter(this, dataManager_.dataModel_.dataRef_);
      manager_ = new LinearLayoutManager(this);
      recycler.HasFixedSize = false;
      recycler.SetLayoutManager(manager_);
      recycler.SetAdapter(adapter_);



      FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
      fab.Click += (sender, e) => {
      };

      drawer_ = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
      drawerUtility_ = new util.NavigationDrawerUtility(this, drawer_, toolbar, Resource.String.app_name, Resource.String.app_name);
      drawerUtility_.SyncState();
      drawer_.AddDrawerListener(drawerUtility_);

      navigationView_ = FindViewById<NavigationView>(Resource.Id.navigationView);
      navigationView_.SetNavigationItemSelectedListener(this);


    }

    public override bool OnCreateOptionsMenu(IMenu menu) {
      MenuInflater.Inflate(Resource.Menu.menu_main, menu);
      return true;
    }

    public override bool OnOptionsItemSelected(IMenuItem item) {
      int id = item.ItemId;
      if(id == Resource.Id.action_settings) {
        StartActivity(new Android.Content.Intent(ApplicationContext, typeof(preference.PreferenceActivity)));
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

    void SwipeRefreshLayout.IOnRefreshListener.OnRefresh() {
      //new Handler().PostDelayed(() => {
      //  swipe_.Refreshing = false;
      //}, 3000);

      new Handler().Post(async () => {
        await dataManager_.Update();
        adapter_.NotifyDataSetChanged();
        swipe_.Refreshing = false;
      });
    }

    public bool OnNavigationItemSelected(IMenuItem menuItem) {
      switch(menuItem.ItemId) {
      case Resource.Id.action_settings:
        StartActivity(new Android.Content.Intent(ApplicationContext, typeof(preference.PreferenceActivity)));
        drawer_.CloseDrawer((int)GravityFlags.Start);
        break;
      case Resource.Id.action_syllabus:
        StartActivity(new Android.Content.Intent(ApplicationContext, typeof(syllabus.SyllabusActivity)));
        drawer_.CloseDrawer((int)GravityFlags.Start);
        break;
      default:
        break;
      }
      return true;
    }
  }
}

