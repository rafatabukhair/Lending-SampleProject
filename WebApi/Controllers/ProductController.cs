using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using BusinessEntities;
using Data.Repositories;

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
        public IHttpActionResult Create(Guid productId, [FromBody] Product product)
        {
            product.Id = productId.ToString();

            var dedupFields = ConfigurationManager.AppSettings["ProductDeduplicationFields"]
                ?.Split(',')
                .Select(f => f.Trim())
                .ToList();

            if (dedupFields != null && dedupFields.Count > 0)
            {
                var isDuplicate = _repo.GetAll().Any(existing =>
                    dedupFields.All(field =>
                        GetFieldValue(existing, field) == GetFieldValue(product, field)
                    ));

                if (isDuplicate)
                    return Conflict();
            }

            _repo.Create(product);
            return Ok(product);
        }

        [HttpPost]
        [Route("{productId:guid}/update")]
        public IHttpActionResult Update(Guid productId, [FromBody] Product product)
        {
            product.Id = productId.ToString();
            _repo.Update(product);
            return Ok(product);
        }

        [HttpDelete]
        [Route("{productId:guid}/delete")]
        public IHttpActionResult Delete(string productId)
        {
            _repo.Delete(productId);
            return Ok();
        }

        [HttpGet]
        [Route("{productId:guid}")]
        public IHttpActionResult Get(string productId)
        {
            return Ok(_repo.GetById(productId));
        }

        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetAll(string category = null, string name = null)
        {
            var products = _repo.GetAll(p =>
                (string.IsNullOrEmpty(category) || p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(name) || p.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
            );

            return Ok(products);
        }

        private string GetFieldValue(Product product, string fieldName)
        {
            var prop = typeof(Product).GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            var value = prop?.GetValue(product);
            return value?.ToString()?.Trim().ToLowerInvariant();
        }
    }
}