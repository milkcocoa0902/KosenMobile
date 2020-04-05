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

				System.Collections.Generic.List<DataBase.Table> listElm_;

				DataBase db_;

				public RSS(){
						client_ = new HttpClient();
						parser_ = new HtmlParser();
						listElm_ = new System.Collections.Generic.List<DataBase.Table>();
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
						GetInformation(url_ + "page/" + _page).Wait();
					});
					
					listElm_.ForEach(item =>{
							Console.WriteLine("date:{0}, title:{1}, detail:{2}, no:{3}", item.date_, item.title_, item.detail_, item.hash_);
						});

						db_.EXECUTE(db_.RequestBuilder(DataBase.REQUEST.INSERT, listElm_));
				}

				public async Task Rebuild(){
					Build();
				}

				public async Task Update(){

				}
				private async Task GetInformation(string _url){
						Console.WriteLine(_url);
					
						var response = await client_.GetAsync(_url);
						response.EnsureSuccessStatusCode();
						var responseBody = await response.Content.ReadAsStringAsync();
						var doc = (await parser_.ParseDocumentAsync(responseBody));
						
						try{
						listElm_.AddRange(doc.GetElementsByClassName("widget_list")[0]
						.QuerySelectorAll("li")
						.Select(n =>{
							var title = n.QuerySelectorAll("a").Last().InnerHtml;
							var detail = n.QuerySelectorAll("a").Last().Attributes["href"].Value;
							var date = n.QuerySelectorAll("time").First().Attributes["datetime"].Value;
							var hash = System.Security.Cryptography.SHA512.Create().ComputeHash(((new System.Text.ASCIIEncoding()).GetBytes(detail))).ToString();

							return new DataBase.Table{title_ = title, 
										detail_ = detail, 
										date_ = date,
										hash_ = hash};
						}).ToList());
						}catch(Exception e){
							Console.WriteLine(e.ToString());
						}
				}
		};
}
