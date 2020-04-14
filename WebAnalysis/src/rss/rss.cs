using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using System.Security.Cryptography;


namespace WebAnalysis.RSS{
		class RSS{
				static HttpClient client_;
				static String url_{get{
					return "https://www.toyota-ct.ac.jp/information/";
					}
					}
				static HtmlParser parser_;

				System.Collections.Concurrent.BlockingCollection<DataBase.Table> listElm_;

				DataBase db_;

				public RSS(){
						client_ = new HttpClient();
						parser_ = new HtmlParser();
						listElm_ = new System.Collections.Concurrent.BlockingCollection<DataBase.Table>();
						db_ = new DataBase();
				}

				public async Task Build(){
					var res = await client_.GetAsync(url_);
					res.EnsureSuccessStatusCode();

					var last = (await parser_.ParseDocumentAsync(await res.Content.ReadAsStringAsync()))
								.GetElementsByClassName("pagenavi")[0]
								.GetElementsByClassName("page-numbers")[4].InnerHtml;
					System.Console.WriteLine("last:{0}", last);

					Parallel.For(1, int.Parse(last) + 1, (int _page)=>{
						Console.WriteLine("page:{0}", _page);
						Parallel.ForEach(GetInformation(url_ + "page/" + _page).Result, (v)=>{listElm_.Add(v);});
					});
					
					listElm_.ToList().ForEach(item =>{
							Console.WriteLine("date:{0}, title:{1}, detail:{2}, no:{3}", item.date_, item.title_, item.detail_, item.hash_);
						});

					db_.EXECUTE(db_.RequestBuilder(DataBase.REQUEST.INSERT, listElm_));
				}

				public async Task Rebuild(){
					Build();
				}

				public void test(){
				}

				public async Task Update(){
						db_.Select(null, (_data)=>{
										while(_data.Read() == true){
												var nExist = listElm_
														.Where(elm => elm.hash_.Equals(_data["hash"]))
														.Count() == 0;
												if(nExist){
													listElm_.Add(new DataBase.Table{title_ = (string)_data["title"],
														detail_ = (string)_data["detail"],
														date_ = (string)_data["date"],
														hash_ = (string)_data["hash"]});
													Console.WriteLine("Add");
												}
										}
								});
				}

				private async Task<System.Collections.Concurrent.BlockingCollection<DataBase.Table>>  GetInformation(string _url){
						Console.WriteLine(_url);
					
						var elm = new System.Collections.Concurrent.BlockingCollection<DataBase.Table>();
						var response = client_.GetAsync(_url).Result;
						response.EnsureSuccessStatusCode();
						var responseBody = response.Content.ReadAsStringAsync().Result;
						var doc = parser_.ParseDocumentAsync(responseBody).Result;
						
						try{
							Parallel.ForEach(doc.GetElementsByClassName("widget_list")[0]
							.QuerySelectorAll("li")
							.Select(n =>{
								var title = n.QuerySelectorAll("a").Last().InnerHtml;
								var detail = n.QuerySelectorAll("a").Last().Attributes["href"].Value;
								var date = n.QuerySelectorAll("time").First().Attributes["datetime"].Value;
								var hashByte = System.Security.Cryptography.SHA512.Create().ComputeHash(((new System.Text.ASCIIEncoding()).GetBytes(detail)));
								
								string hash;
								var builder = new System.Text.StringBuilder();
								foreach(var b in hashByte){
										builder.Append(b.ToString("x2"));
								}
								hash = builder.ToString();

								return new DataBase.Table{title_ = title, 
											detail_ = detail, 
											date_ = date,
											hash_ = hash};
							}).ToList(), (v)=>{
								elm.Add(v);
							});
						}catch(Exception e){
							Console.WriteLine(e.ToString());
						}

						return elm;
				}
		};
}
