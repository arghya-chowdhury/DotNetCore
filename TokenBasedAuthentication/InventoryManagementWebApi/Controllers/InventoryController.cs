using System.Collections.Generic;
using System.Linq;
using InventoryManagementWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        InMemoryDataContext _dataContext;
        public InventoryController(InMemoryDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Article>> Get()
        {
            return new OkObjectResult(_dataContext.ArticleRepo.Values.ToArray());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Article> Get(int id)
        {
            if(!_dataContext.ArticleRepo.ContainsKey(id))
            {
                return new BadRequestResult();
            }
            return new OkObjectResult(_dataContext.ArticleRepo[id]);
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] Article value)
        {
            if(value==null)
                return new BadRequestResult();
            if(!_dataContext.ArticleRepo.Keys.Any())
            {
                value.Id = 1;
            }
            else
            {
                value.Id = _dataContext.ArticleRepo.Keys.Max() + 1;
            }
            _dataContext.ArticleRepo.Add(value.Id, value);
            return new OkResult();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Article value)
        {
            if (!_dataContext.ArticleRepo.ContainsKey(id) || value == null)
            {
                return new BadRequestResult();
            }
            _dataContext.ArticleRepo[id]=value;
            return new OkResult();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!_dataContext.ArticleRepo.ContainsKey(id))
            {
                return new BadRequestResult();
            }
            _dataContext.ArticleRepo.Remove(id);
            return new OkResult();
        }
    }
}
