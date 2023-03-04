using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kanbardoo.WebAPI.Controllers;
public class FallbackController : Controller
{
    public ActionResult Index()
    {
        return PhysicalFile(
            Path.Combine(Directory.GetCurrentDirectory(), "index.html"),
            "text/HTML");
    }
}
