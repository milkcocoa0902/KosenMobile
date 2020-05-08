using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WebAnalysis.RSS;

namespace REST.RSS
{
    [ApiController]
    [Route("update")]
    public class RSSController : ControllerBase
    {
        static List<Table> data_ = new List<Table>(){
            new Table(){
                date_ = @"2020-03-05",
                title_ = @"dummy",
                detail_ = @"just a dummy",
                hash_ = @"0"}
            };

        public static WebAnalysis.RSS.RSS RSSDataBase = new WebAnalysis.RSS.RSS();
        [HttpGet]
        public ActionResult<List<Table>> GetAction()=>data_;

        [HttpGet("{id:int}", Name="aaa")]
        public ActionResult<List<Table>> CheckForUpdate(int id){
            var response = new List<Table>();
            Console.WriteLine("ID:{0}", id);
            Console.WriteLine("data_.Count:{0}", data_.Count);
            Console.WriteLine(data_[0].title_);
            response = RSSDataBase.strage_
						.Where(_s => _s.id_ > id)
                        .Select(_s =>{
								return new Table{
                                    id_ = (int)_s.id_,
                                    title_ = _s.title_, 
                                    detail_ = _s.detail_, 
                                    date_ = _s.date_,
                                    hash_ = _s.hash_};
                        }).ToList();

            Console.WriteLine("response.count:{0}", response.Count);
            return response;
        }
       
    }
    
}
