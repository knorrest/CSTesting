using Microsoft.AspNetCore.Mvc;
using SynonymsAPI.DTOs;
using SynonymsAPI.Interfaces;

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

        // GET: api/<SynonymsController>
        [HttpGet]
        public async Task<WordSynonymsDto> Get(string word)
        {
            return await _synonymsService.GetAsync(word);
        }

        //// GET api/<SynonymsController>/5
        //[HttpGet("{id}")]
        //public Task<string> Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<SynonymsController>
        [HttpPost]
        public async Task<bool> Post(string word, string synonym)
        {
            return await _synonymsService.AddAsync(word, synonym);
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
