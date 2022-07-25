using Microsoft.AspNetCore.Mvc;
using SynonymsAPI.Interfaces;
using SynonymsDB;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SynonymsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SynonymsController : ControllerBase
    {
        private readonly ISynonymsService _synonymsService;
        public SynonymsController(ISynonymsService synonymsService)
        {
            _synonymsService = synonymsService;
        }

        [HttpGet]
        public ActionResult<Message> Get()
        {
            var words = _synonymsService.Get();
            return new Message()
            {
                Data = words,
                IsValid = true
            };
        }

        [HttpGet("search")]
        public ActionResult<Message> Get([FromQuery] string word)
        {
            var words = _synonymsService.SearchByWord(word);
            return new Message()
            {
                Data = words,
                IsValid = true
            };
        }

       

        //// GET api/<SynonymsController>/5
        //[HttpGet("{id}")]
        //public Task<string> Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<SynonymsController>
        [HttpPost]
        public ActionResult<bool> Post(WordDto word)
        {
            return _synonymsService.Add(word.WordString, word.Synonyms.ToList());
        }

        // PUT api/<SynonymsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SynonymsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
