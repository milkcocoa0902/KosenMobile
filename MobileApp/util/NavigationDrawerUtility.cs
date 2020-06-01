using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;

namespace KosenMobile.util {
  public class NavigationDrawerUtility :ActionBarDrawerToggle{
    public NavigationDrawerUtility(Android.App.Activity _activity, DrawerLayout _drawerLayout, Toolbar _toolbar, int _openDrawerContentRes, int _closeDrawerContentRes)
      :base(_activity, _drawerLayout, _toolbar, _openDrawerContentRes, _closeDrawerContentRes){
    }

    public override void OnDrawerOpened(View drawerView) {
      base.OnDrawerOpened(drawerView);
      Android.Util.Log.Debug("drawer", "drawer opend");
    }

    public override void OnDrawerClosed(View drawerView) {
      base.OnDrawerClosed(drawerView);
      Android.Util.Log.Debug("drawer", "drawer closed");
    }

    public override void OnDrawerSlide(View drawerView, float slideOffset) {
      base.OnDrawerSlide(drawerView, slideOffset);
      Android.Util.Log.Debug("drawer", "drawer slided");
    }

    public override void OnDrawerStateChanged(int newState) {
      base.OnDrawerStateChanged(newState);
      Android.Util.Log.Debug("drawer", "drawer state changed");
    }
  }
}
