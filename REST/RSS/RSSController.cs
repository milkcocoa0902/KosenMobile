using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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
        [HttpGet]
        public ActionResult<List<Table>> GetAction()=>data_;

        [HttpGet("{id:int}", Name="aaa")]
        public ActionResult<List<Table>> CheckForUpdate(int id){
            var response = new List<Table>();
            Console.WriteLine("ID:{0}", id);
            Console.WriteLine("data_.Count:{0}", data_.Count);
            Console.WriteLine(data_[0].title_);
            for(var i = id; i < data_.Count; i++){
                response.Add(data_[i]);
            }

Console.WriteLine("response.count:{0}", response.Count);
            return response;
        }
       
    }
    
}
