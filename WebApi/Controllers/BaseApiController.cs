using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        public HttpResponseMessage Found(object obj)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        public HttpResponseMessage Found()
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage DoesNotExist()
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage Conflict(string message)
        {
            return Request.CreateResponse(HttpStatusCode.Conflict, new { error = message });
        }

        public HttpResponseMessage BadApiRequest(string message)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, new { error = message });
        }
    }
}