using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.engine;
using backend.engine.Messages;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IEngine _engine;

        public ValuesController(IEngine engine)
        {
            _engine = engine;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{clientId}")]
        public async Task<ReceiveData> Get(string clientId)
        {
            return await _engine.QueryAsync<ReceiveData>(new QueryData { ClientId = clientId }).ConfigureAwait(false);
        }

        // POST api/values
        [HttpPost("{clientId}")]
        public ActionResult Post(string clientId, [FromBody]string value)
        {
            this._engine.Post(new ReceiveData
            {
                ClientId = clientId,
                TimeStamp = DateTime.UtcNow,
                Data = value
            });

            return NoContent();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
