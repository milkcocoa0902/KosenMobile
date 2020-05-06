using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
namespace REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
    
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDone { get; set; }
    }

    [ApiController]
    [Route("api")]
    public class ToController : ControllerBase
    {
        // Todoアイテムの初期データ。本来はデータベースなどから取得する。
        private static List<TodoItem> items = new List<TodoItem>() {
            new TodoItem() { Id = 1, Name = @"犬の散歩", IsDone = false },
            new TodoItem() { Id = 2, Name = @"買い物", IsDone = true },
            new TodoItem() { Id = 3, Name = @"本棚の修理", IsDone = false },
        };

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
            => items;

        [HttpGet("{id}", Name = "Todo")]
        public ActionResult<TodoItem> GetById(int id)
        {
            var item = items.Find(i => i.Id.Equals(id));
            if (item == null)
                return NotFound();
            return item;
        }

        [HttpPost]
        public ActionResult<TodoItem> GetJson([FromBody] TodoItem json){
            Console.WriteLine(json);
            //var weatherForecast = JsonSerializer.Deserialize<TodoItem>(json);
            return json;
        }
    }
    
}
