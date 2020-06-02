using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using System.Security.Cryptography;


namespace WebAnalysis.RSS{
		public class RSS{
				static HttpClient client_;
				static String url_{get{
					return "https://www.toyota-ct.ac.jp/information/";
					}
					}
				static HtmlParser parser_;

				public System.Collections.Concurrent.BlockingCollection<DataBase.Table> strage_{get; private set;}

				DataBase db_;
				Int64 maxId_;

				public RSS(){
						client_ = new HttpClient();
						parser_ = new HtmlParser();
						strage_ = new System.Collections.Concurrent.BlockingCollection<DataBase.Table>();
						db_ = new DataBase();

						db_.Select(null, (_data)=>{
										while(_data.Read() == true){
											var a = _data["id"];
											//Console.WriteLine(a.Type());
											maxId_ = Math.Max(maxId_, (Int64)_data["id"]);
											strage_.Add(new DataBase.Table{
												id_ = (Int64)_data["id"],
												title_ = (string)_data["title"],
												detail_ = (string)_data["detail"],
												date_ = (string)_data["date"],
												hash_ = (string)_data["hash"]});
												}});
						Console.WriteLine("load from db");
				}

				public async Task Build(){
					var publish = new System.Collections.Concurrent.BlockingCollection<DataBase.Table>();
					var newElm = new System.Collections.Concurrent.BlockingCollection<DataBase.Table>();
					var res = await client_.GetAsync(url_);
					res.EnsureSuccessStatusCode();

					var last = (await parser_.ParseDocumentAsync(await res.Content.ReadAsStringAsync()))
								.GetElementsByClassName("pagenavi")[0]
								.GetElementsByClassName("page-numbers")[4].InnerHtml;
					System.Console.WriteLine("last:{0}", last);

					Parallel.For(1, int.Parse(last) + 1, (int _page)=>{
						Console.WriteLine("page:{0}", _page);
						Parallel.ForEach(GetInformation(url_ + "page/" + _page).Result, 
										(_elm)=>{
												var tryAdd = false;
												do{
														tryAdd = publish.TryAdd(_elm, 500);
												}while(!tryAdd);
										});
					});

					Parallel.ForEach(publish.ToList(), (_elm)=>{
						var contain = strage_
						.Where(_s => _s.hash_ == _elm.hash_)
						.Count();

						if(contain == 0){
							Console.WriteLine("element {0} does not stored!!", _elm.hash_);
							_elm.id_ = (++maxId_);

							var tryAdd = false;
							do{
									tryAdd = strage_.TryAdd(_elm, 500);
							}while(!tryAdd);

							do{
									tryAdd = newElm.TryAdd(_elm, 500);
							}while(!tryAdd);
						}
					});

					db_.Insert(newElm);
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
								var tryAdd = false;
								do{
										tryAdd = elm.TryAdd(v, 500);
								}while(!tryAdd);
							});
						}catch(Exception e){
							Console.WriteLine(e.ToString());
						}

						return elm;
				}
		};
}
