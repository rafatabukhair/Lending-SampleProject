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
    [RoutePrefix("orders")]
    public class OrderController : ApiController
    {
        private readonly IInMemoryRepository<Order> _repo;

        public OrderController(IInMemoryRepository<Order> repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route("{orderId:guid}/create")]
        public IHttpActionResult Create(Guid orderId, [FromBody] Order order)
        {
            order.Id = orderId.ToString();

            var dedupFields = ConfigurationManager.AppSettings["OrderDeduplicationFields"]
                ?.Split(',')
                .Select(f => f.Trim())
                .ToList();

            if (dedupFields != null && dedupFields.Count > 0)
            {
                var isDuplicate = _repo.GetAll().Any(existing =>
                    dedupFields.All(field =>
                        GetFieldValue(existing, field) == GetFieldValue(order, field)
                    ));

                if (isDuplicate)
                    return Conflict();
            }

            _repo.Create(order);
            return Ok(order);
        }

        [HttpPost]
        [Route("{orderId:guid}/update")]
        public IHttpActionResult Update(Guid orderId, [FromBody] Order order)
        {
            order.Id = orderId.ToString();
            _repo.Update(order);
            return Ok(order);
        }

        [HttpDelete]
        [Route("{orderId:guid}/delete")]
        public IHttpActionResult Delete(string orderId)
        {
            _repo.Delete(orderId);
            return Ok();
        }

        [HttpGet]
        [Route("{orderId:guid}")]
        public IHttpActionResult Get(string orderId)
        {
            return Ok(_repo.GetById(orderId));
        }

        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetAll(decimal? minTotal = null, string productId = null, DateTime? fromDate = null)
        {
            var orders = _repo.GetAll(o =>
                (!minTotal.HasValue || o.Total >= minTotal.Value) &&
                (string.IsNullOrEmpty(productId) || o.OrderItems.Any(i => i.ProductId == productId)) &&
                (!fromDate.HasValue || o.OrderDate > fromDate.Value)
            );

            return Ok(orders);
        }

        // Utility: Get a string field value via reflection
        private string GetFieldValue(Order order, string fieldName)
        {
            var prop = typeof(Order).GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            var value = prop?.GetValue(order);
            return value?.ToString()?.Trim().ToLowerInvariant();
        }
    }
}