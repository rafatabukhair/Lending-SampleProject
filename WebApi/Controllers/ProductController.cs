using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Users;
using Data.Repositories;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : ApiController
    {
        private readonly IInMemoryRepository<Product> _repo;

        public ProductController(IInMemoryRepository<Product> repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route("{productId:guid}/create")]
        public IHttpActionResult Create(Product product)
        {
            _repo.Create(product);
            return Ok(product);
        }

        [HttpPost]
        [Route("{productId:guid}/update")]
        public IHttpActionResult Update(Product product)
        {
            _repo.Update(product);
            return Ok(product);
        }

        [HttpDelete]
        [Route("{productId:guid}/delete")]
        public IHttpActionResult Delete(string id)
        {
            _repo.Delete(id);
            return Ok();
        }

        [HttpGet]
        [Route("{productId:guid}")]
        public IHttpActionResult Get(string id)
        {
            return Ok(_repo.GetById(id));
        }

        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetAll()
        {
            return Ok(_repo.GetAll());
        }
    }
}