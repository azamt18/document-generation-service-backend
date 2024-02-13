using API.Controllers.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Net;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Web.Http.Results;

namespace API.Controllers.Base;

[ApiController]
[TypeFilter(typeof(DefaultRequestLog), Order = 1)]
public abstract class BaseController: ControllerBase
{
}