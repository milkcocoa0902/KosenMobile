using System;
using System.Linq;
using System.Collections.Generic;

namespace WebAnalysis.Syllabus{
	public class Assessment{
		public string name_{get; set;}  // 評価名
		public int value_{get; set;} // 評価割合
	}

	public class Model{
		public string title_{get; set;} // 科目名
		public string id_{get; set;}  // 科目番号
		public string course_{get; set;} // 学科
		public int grade_{get; set;}  // 学年
		public int credit_{get; set;}  // 単位数
		public List<Assessment> assesment_{get; set;} // 評価基準
	}

	public class query{
		public string school_id;
		public string department_id;
		public string subject_code;
		public string year;

		public string Serialize(){
			var ret = new List<string>();
			if(school_id != null) ret.Add(string.Join("=", new []{"school_id", school_id}));
			if(department_id != null) ret.Add(string.Join("=", new []{"department_id", department_id}));
			if(subject_code != null) ret.Add(string.Join("=", new []{"subject_code", subject_code}));
			if(year != null) ret.Add(string.Join("=", new []{"year", year}));

			return string.Join("&", ret);
		}
	}
}
