using System;
using System.Collection.Generic;

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
		public List<Assesment> assesment_{get; set;} // 評価基準
	}
}
