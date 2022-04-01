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
                return NotFound(new { status = 404, result = repository.Get(), message = "Data Tidak Ditemukan" });
            }
            return Ok(new { status = 200, result = repository.Get(), message = "Data Ditemukan" });
        }
        [HttpGet("{key}")]
        public ActionResult Get(Key key)
        {
            if (repository.Get(key) == null)
            {
                return NotFound(new { status = 404, result = new List<Entity>(), message = "Data Tidak Ditemukan" });
            }
            return Ok(new { status = 200, result = repository.Get(key), message = "Data Ditemukan" });
        }

        [HttpPost]
        public virtual ActionResult Post(Entity entity)
        {
            repository.Insert(entity);
            return Ok(new { status = 200, message = "Data Berhasil Ditambahkan" });
        }
        [HttpPut]
        public ActionResult Update(Entity entity)
        {
            repository.Update(entity);
            return Ok(new { status = 200, message = "Data Berhasil Diupdate" });
        }
        [HttpDelete("{key}")]
        public ActionResult Delete(Key key)
        {
            if (repository.Get(key) == null)
            {
                return NotFound(new { status = 404, message = "Data Tidak Ditemukan" });
            }
            repository.Delete(key);
            return Ok(new { status = 200, message = "Data Berhasil Dihapus" });

        }
    }
}
