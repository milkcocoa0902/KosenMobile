using System;
namespace WebAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
						RSS rss = new RSS();
            Console.WriteLine("Hello World!");
						rss.test().Wait();
				}
    }
}
