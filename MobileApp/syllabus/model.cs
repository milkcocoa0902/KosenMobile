using System;
using System.Linq;
using System.Collections.Generic;
using Android.Content;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace KosenMobile.syllabus {

	public class DataModel {
    public enum Course {
      G = 0,
      M = 1,
      E = 7,
      I = 3,
      C = 4,
      A = 5
    };

		public class Assessment {
			[System.Xml.Serialization.XmlElement("Name")]
			public string name_ { get; set; }  // 評価名

			[System.Xml.Serialization.XmlElement("Value")]
			public int value_ { get; set; } // 評価割合
		}

		public class Subject {

			[System.Xml.Serialization.XmlElement("Title")]
			public string title_ { get; set; } // 科目名

			[System.Xml.Serialization.XmlElement("Id")]
			public string id_ { get; set; }  // 科目番号

			[System.Xml.Serialization.XmlElement("Course")]
			public string course_ { get; set; } // 学科

			[System.Xml.Serialization.XmlElement("Grade")]
			public int grade_ { get; set; }  // 学年

			[System.Xml.Serialization.XmlElement("Credit")]
			public int credit_ { get; set; }  // 単位数

			[System.Xml.Serialization.XmlElement("Assessment")]
			public List<Assessment> assesment_ { get; set; } // 評価基準
		}


		Context context_;
		private static int databaseVersion_ => 1;
		public static string databaseName_ => "syllabus.xml";
		public string databasePath_;

		private List<Subject> data_;
		public IReadOnlyList<Subject> dataRef_;


		public DataModel(Context _context) {
			context_ = _context;
			databasePath_ = System.IO.Path.Combine(context_.GetExternalFilesDir(null).Path, DataModel.databaseName_);
			data_ = new List<Subject>();
			dataRef_ = data_;
		}

    public void read() {
			data_ = (List<Subject>)((new XmlSerializer(typeof(List<Subject>))).Deserialize(System.Xml.XmlReader.Create(new StreamReader(databasePath_, Encoding.UTF8))));
			dataRef_ = data_;
		}
	}
}
