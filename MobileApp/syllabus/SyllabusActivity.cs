
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
    Spinner gradeSpinner_;
    Spinner courseSpinner_;
    Spinner sbjSpinner_;
    List<DataModel.Subject> sbjList_;


    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      // Create your application here
      SetContentView(Resource.Layout.syllabus_main_content);
      sylabusDataManager_ = new syllabus.DataManager(ApplicationContext);

      sbjSpinner_ = FindViewById<Spinner>(Resource.Id.subject_item);
      sbjList_ = sylabusDataManager_.gradeAndCourse(1, DataModel.Course.M);
      sbjSpinner_.Adapter = new ArrayAdapter(this,
        Resource.Layout.support_simple_spinner_dropdown_item,
        sbjList_.Select(sbj => sbj.title_).ToList());
        sbjSpinner_.SetSelection(0);


      gradeSpinner_ = FindViewById<Spinner>(Resource.Id.grade_item);
      gradeSpinner_.Adapter = new ArrayAdapter(this,
        Resource.Layout.support_simple_spinner_dropdown_item,
        new int[]{
          1, 2, 3, 4, 5
      });
      gradeSpinner_.SetSelection(0);

      gradeSpinner_.ItemSelected += (sender, e) => {
        sbjList_ = sylabusDataManager_.gradeAndCourse(gradeSpinner_.SelectedItemPosition + 1, (DataModel.Course)(new[] {1, 7, 3, 4, 5 }[courseSpinner_.SelectedItemPosition]));
        sbjSpinner_.Adapter = new ArrayAdapter(this,
          Resource.Layout.support_simple_spinner_dropdown_item,
          sbjList_.Select(sbj => sbj.title_).ToList());
      };


      courseSpinner_ = FindViewById<Spinner>(Resource.Id.course_item);
      courseSpinner_.Adapter = new ArrayAdapter(this,
        Resource.Layout.support_simple_spinner_dropdown_item,
        new DataModel.Course[]{
          DataModel.Course.M, DataModel.Course.E, DataModel.Course.I, DataModel.Course.C, DataModel.Course.A
      });
      courseSpinner_.SetSelection(0);

      courseSpinner_.ItemSelected += (sender, e) => {
        sbjList_ = sylabusDataManager_.gradeAndCourse(gradeSpinner_.SelectedItemPosition + 1, (DataModel.Course)(new[] { 1, 7, 3, 4, 5 }[courseSpinner_.SelectedItemPosition]));
        sbjSpinner_.Adapter = new ArrayAdapter(this,
          Resource.Layout.support_simple_spinner_dropdown_item,
          sbjList_.Select(sbj => sbj.title_).ToList());
      };
    }
  }
}
