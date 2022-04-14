using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace API.Base
{
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity,Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public ActionResult<Entity> Get()
        {
            if (repository.Get().Count() == 0)
            {
                return NotFound(repository.Get());
            }
            return Ok(repository.Get());
        }
        [HttpGet("{key}")]
        public ActionResult Get(Key key)
        {
            if (repository.Get(key) == null)
            {
                return NotFound(repository.Get(key));
            }
            return Ok(repository.Get(key));
        }

        [HttpPost]
        public virtual ActionResult Post(Entity entity)
        {
            return Ok(repository.Insert(entity));
        }
        [HttpPut]
        public ActionResult Update(Entity entity)
        {
            return Ok(repository.Update(entity));
        }
        [HttpDelete("{key}")]
        public ActionResult Delete(Key key)
        {
            if (repository.Get(key) == null)
            {
                return NotFound(0);
            }
            return Ok(repository.Delete(key));
        }
    }
}
