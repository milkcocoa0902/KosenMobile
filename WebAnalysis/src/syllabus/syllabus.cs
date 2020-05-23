using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace WebAnalysis.Syllabus{
		public class Syllabus{
				static HttpClient client_;
				static String url_{get{
					return "https://syllabus.kosen-k.go.jp/Pages/PublicSubjects";
					}
				}
				static String detailUrl_{get{
					return "https://syllabus.kosen-k.go.jp/Pages/PublicSyllabus";
					}
				}

				static HtmlParser parser_;
				public System.Collections.Concurrent.BlockingCollection<Subject> strage_{get; private set;}
				public System.Collections.Concurrent.BlockingCollection<query> query_{get; private set;}

				Subject model_;
				Int64 maxId_;

				public Syllabus(){
						client_ = new HttpClient();
						parser_ = new HtmlParser();
						strage_ = new System.Collections.Concurrent.BlockingCollection<Subject>();
						model_ = new Subject();
						query_ = new System.Collections.Concurrent.BlockingCollection<query>();
				}

				public void TEST(){
					query q = new query();
					q.school_id = "23";
					q.department_id = "11";
					q.subject_code = "72012";
					q.year="2020";
					Console.WriteLine(q.Serialize());
				}

				public async Task Build(){
						List<query> courseList = new List<query>();
						foreach(var id in new []{"11", "12", "13", "14", "15"}){
							courseList.Add(new query(){
										school_id = "23",
										department_id = id,
										year = "2020"
										});
						}
					
						var courseListLoop = Parallel.ForEach(courseList, (_course)=>{
							var client = new HttpClient();

							Console.WriteLine(string.Join("?", new []{url_, _course.Serialize()}));
							var res = client.GetAsync(string.Join("?", new []{url_, _course.Serialize()})).Result;
						
							res.EnsureSuccessStatusCode();
							var parser = new HtmlParser();

							var subject = ( parser.ParseDocumentAsync(res.Content.ReadAsStringAsync().Result)).Result
											.GetElementsByClassName("subject-item");
							foreach(var sbj in subject){
								var detail = sbj.GetElementsByClassName("mcc-show")[0].Attributes["href"].Value;
								var req = detail.Split("?")[1].Split("&");
								var q = new query();
								foreach(var elm in req){
									var key = elm.Split("=")[0];
									var val = elm.Split("=")[1];
									switch(key){
									case "school_id":
											q.school_id = val;
											break;
									case "department_id":
											q.department_id = val;
											break;
									case "subject_code":
											q.subject_code = val;
											break;
									case "year":
											q.year = val;
											break;
									}
								}
								query_.Add(q);
							}
						});

							while(!courseListLoop.IsCompleted);
							Console.WriteLine("Complete to Tasking,  Add to Queue");

							var syllabusGetLoop = Parallel.ForEach(query_, (q)=>{
								var client = new HttpClient();
								Console.WriteLine(string.Join("?", new []{detailUrl_, q.Serialize()}));
								var res = client.GetAsync(string.Join("?", new []{detailUrl_, q.Serialize()})).Result;
								res.EnsureSuccessStatusCode();
								var parser = new HtmlParser();

								Subject model = new Subject();
								model.id_ = q.subject_code;

								model.course_ = q.subject_code[1].ToString();
								model.grade_ = q.subject_code[2] - '0';

								model.title_ = (parser.ParseDocumentAsync(res.Content.ReadAsStringAsync().Result)).Result
													.GetElementsByClassName("mcc-title-bar")[0]
													.QuerySelectorAll("h1")
													.First()
													.InnerHtml;
								
								model.assesment_ = new List<Assessment>();
								var assessmentTable = (parser.ParseDocumentAsync(res.Content.ReadAsStringAsync().Result)).Result
															.GetElementById("MainContent_SubjectSyllabus_wariaiTable")
															.QuerySelectorAll("tr");
								var assesmentName = assessmentTable[0].QuerySelectorAll("th")
																		.Select(n =>{
																			return n.InnerHtml;
																		}).ToList();
																		
								var assesmentVal = assessmentTable[1].QuerySelectorAll("td")
																		.Select(n =>{
																			return n.QuerySelectorAll("b").First().InnerHtml;
																		}).ToList();
								
								for(var i = 0;i < assesmentName.Count() - 1;i++){
									model.assesment_.Add(new Assessment(){
										name_ = assesmentName[i],
										value_ = int.Parse(assesmentVal[i])
									});
								}

								strage_.Add(model);
							});

							while(!syllabusGetLoop.IsCompleted);
							Console.WriteLine("Complete to Tasking,  Add to Queue");


							//foreach(var m in strage_){
							//	Console.WriteLine("title:{0}, id:{1}", m.title_, m.id_);
						//	}

							var serializer = new XmlSerializer(typeof(List<Subject>));
							System.IO.StreamWriter sw = new System.IO.StreamWriter("Syllabus.xml", false, new System.Text.UTF8Encoding());
							serializer.Serialize(sw, strage_.ToList());
							sw.Close();

						

					// 各学科のURLを取得
					// 各学科のページから科目URLを取得
					// 科目URLから科目情報を取得
					// xmlとして保存
				}

	//			private async Task<System.Collections.Concurrent.BlockingCollection<Model>>  GetInformation(string _url){
		//		}
		};
}
