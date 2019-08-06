using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Angel.Api;
using Angel.DbAccess;
using Angel.Queries;
using Angel.Result;
using Microsoft.AspNetCore.Mvc;

namespace Angel.Controllers
{

    public class tbUsers
    {
        public string fstruserId { get; set; }
    }

    public class tbUsersQuery: QueryBase
    {
        public string LoginCode { get; set; }
        public string UserName { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseApiController
    {
        private readonly ISqlQuery _sqlQuery;
        public ValuesController(ISqlQuery sqlQuery)
        {
            _sqlQuery = sqlQuery;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var page = new Paged<tbUsers>();
            var data = _sqlQuery.QueryPage<tbUsers>("select * ", "from tbusers where  fstrLogCode=@LoginCode and fstrUserName=@UserName", " order by fstrUserName desc", new tbUsersQuery { Param= new { LoginCode = "admin", UserName = "admin" } });
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
