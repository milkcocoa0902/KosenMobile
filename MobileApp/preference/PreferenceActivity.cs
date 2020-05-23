
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

namespace KosenMobile.preference {
  [Activity(Label = "PreferenceActivity")]
  public class PreferenceActivity : Activity {
    preference.DataManager dataManager_;
    Switch useInternalBrowserSwitch_;
    Switch rssCacheSwitch_;
    Switch rssAutoUpdateSwitch_;
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.preference_activity);

      dataManager_ = new preference.DataManager(ApplicationContext);
      useInternalBrowserSwitch_ = FindViewById<Switch>(Resource.Id.useInternalBrowserSwitch);
      useInternalBrowserSwitch_.Checked = dataManager_.dataModel_.pref_.useInternalBrowser_;

      rssCacheSwitch_ = FindViewById<Switch>(Resource.Id.rssCacheSwitch);
      rssCacheSwitch_.Checked = dataManager_.dataModel_.pref_.cache_;

      rssAutoUpdateSwitch_ = FindViewById<Switch>(Resource.Id.rssAutoUpdateSwitch);
      rssAutoUpdateSwitch_.Checked = dataManager_.dataModel_.pref_.checkForUpdate_;

      // Create your application here
    }

    protected override void OnStop() {
      dataManager_.Store(new DataModel.Model() {
        useInternalBrowser_ = useInternalBrowserSwitch_.Checked,
        cache_ = rssCacheSwitch_.Checked,
        checkForUpdate_ = rssAutoUpdateSwitch_.Checked
      }); ;
      base.OnStop();
    }

    public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e) {
      switch(keyCode) {
      case Keycode.Back:
        Finish();
        return true;
      default:
        return base.OnKeyDown(keyCode, e);
      }
    }
  }
}
