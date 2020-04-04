using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html;
using AngleSharp.Html.Parser;


namespace WebAnalysis.RSS{
		class RSS{
				static HttpClient client_;
				static String url_{get{
					return "https://www.toyota-ct.ac.jp/information/";
					}
					}
				static HtmlParser parser_;
				class list{
					public string date_;
					public string detail_;
					public string title_;
				};

				System.Collections.Generic.List<list> listElm_;

				public RSS(){
						client_ = new HttpClient();
						parser_ = new HtmlParser();
						listElm_ = new System.Collections.Generic.List<list>();
				}

				public async Task Build(){
					var res = await client_.GetAsync(url_);
					res.EnsureSuccessStatusCode();

					var last = (await parser_.ParseDocumentAsync(await res.Content.ReadAsStringAsync()))
								.GetElementsByClassName("pagenavi")[0]
								.GetElementsByClassName("page-numbers")[4].InnerHtml;
					System.Console.WriteLine("last:{0}", last);

					Parallel.For(1, int.Parse(last), (int _, ParallelLoopState __)=>{
						Console.WriteLine("page:{0}", _);
						GetInformation(url_ + "page/" + _).Wait();
					});
					
					listElm_.ForEach(item =>{
							Console.WriteLine("date:{0}, title:{1}, detail:{2}", item.date_, item.title_, item.detail_);
						});
				}

				public async Task Rebuild(){
					Build();
				}

				public async Task Update(){

				}
				private async Task GetInformation(string url){
					
						var response = await client_.GetAsync(url);
						response.EnsureSuccessStatusCode();
						var responseBody = await response.Content.ReadAsStringAsync();
						var doc = (await parser_.ParseDocumentAsync(responseBody));
						
						listElm_.AddRange(doc.GetElementsByClassName("widget_list")[0]
						.QuerySelectorAll("li")
						.Select(n =>{
							var title = n.QuerySelectorAll("a").Last().InnerHtml;
							var detail = n.QuerySelectorAll("a").Last().Attributes["href"].Value;
							var date = n.QuerySelectorAll("time").First().Attributes["datetime"].Value;


							return new list{title_ = title, 
										detail_ = detail, 
										date_ = date
										};
						}).ToList());

						//listElm_.ForEach(item =>{
						//	Console.WriteLine("date:{0}, title:{1}, detail:{2}", item.date_, item.title_, item.detail_);
						//});
				}
		};
}
