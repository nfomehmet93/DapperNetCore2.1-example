using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestFullServices.Data;
using RestFullServices.Services;

namespace RestFullServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        public ValuesController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<Product> Get()
        {
            Product model = new Product();
            model.Name = "example";
            model.ISBN = "435345345";
            model.CreateonUtc = DateTime.UtcNow;
            _appRepository.Add(model);
            model.Id = 7;
            model.Name = "example1";
            model.UpdateonUtc= DateTime.UtcNow;
            _appRepository.Update(model);
            _appRepository.Delete(model);
            var prodList = _appRepository.ExecuteQueryToCollection<ExampleProduct>(@"select Id,Name from Product where Id>@prm ", new {prm=2 });
            var prod = _appRepository.GetItemFromFieldList<Product>("Id", 2);
            var prod1 = _appRepository.GetItemFromFieldList<Product>("Id", "2");
            var prod2 = _appRepository.GetItemFromField<Product>("Name", "test1");
            var prod3 = _appRepository.GetItemFromField<Product>("Id", 3);
            var product=_appRepository.GetItem<Product>(3);
            return model;
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
