using System;
using WebAnalysis.RSS;

namespace WebAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
						var rss = new WebAnalysis.RSS.RSS();
            Console.WriteLine("Hello World!");
						//rss.Build().Wait();
            rss.Update().Wait();
            rss.Update().Wait();
				}
    }
}
