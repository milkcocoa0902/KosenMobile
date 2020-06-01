
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace KosenMobile.syllabus {
  [Activity(Label = "SyllabusActivity")]
  public class SyllabusActivity : AppCompatActivity {

    syllabus.DataManager sylabusDataManager_;
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      // Create your application here
      SetContentView(Resource.Layout.syllabus_main_content);
      sylabusDataManager_ = new syllabus.DataManager(ApplicationContext);
    }
  }
}
