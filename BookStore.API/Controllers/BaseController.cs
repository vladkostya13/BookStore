using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Authorize]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}