using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WebAnalysis.RSS;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace REST.Syllabus
{
    [ApiController]
    [Route("syllabus")]
    public class SyllabusController : ControllerBase
    {
        static List<Table> data_ = new List<Table>(){
            new Table(){
                date_ = @"2020-03-05",
                title_ = @"dummy",
                detail_ = @"just a dummy",
                hash_ = @"0"}
            };

				static List<Subject> subject_ = (List<Subject>)((new XmlSerializer(typeof(List<Subject>))).Deserialize(System.Xml.XmlReader.Create( new StreamReader("syllabus.xml", Encoding.UTF8))));

        [HttpGet]
        public ActionResult<List<Subject>> GetAction()=>subject_;

        [HttpGet("id/{id}", Name="bbb")]
        public ActionResult<List<Subject>> GetSubject(int id){

						Console.WriteLine("{0}", id);
            return subject_.Where(elm => id.ToString("D5").Equals(elm.id_)).ToList();
        }

				[HttpGet("title/{title}", Name="ccc")]
        public ActionResult<List<Subject>> GetSubject(string title){
					return subject_.Where(elm => title.Equals(elm.title_)).ToList();
			  }
    }
    
}
