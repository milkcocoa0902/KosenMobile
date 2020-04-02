using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAnalysis{
		class RSS{
				static HttpClient client_;
				public RSS(){
						client_ = new HttpClient();
						System.Console.WriteLine("aaaaaa");
				}

				public async Task test(){
						var response = await client_.GetAsync("https://www.toyota-ct.ac.jp/information/");
						response.EnsureSuccessStatusCode();
						var responseBody = await response.Content.ReadAsStringAsync();
						System.Console.WriteLine(responseBody);
				}
		};
}
