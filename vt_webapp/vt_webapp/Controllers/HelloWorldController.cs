using System.Web;
using System.Web.Mvc;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        public ActionResult index()
        {
            // Controller verwendet index.cshtml
            return View();
        }

        // 
        // GET: /Hubber/bla

        public string bla()
        {
            return "<h1> Welcome to the Hubber <h1>";
        }

        // 
        // GET: /Hubber/temperature/ 

        public string welcome(string name, int numTimes = 1)
        {
            // HtmlEncode schützt Anwendung vor böswilligen Eingaben(JavaScript)
            // BEACHTE: Die URL ist nicht case-sensitive
            // Controller sucht in URL nach name=<> & numTimes=<>
            // "?" Dient dabei als Lerrzeichen
            return HttpUtility.HtmlEncode("Hello" + name + ", numTimes: " + numTimes);
            //return HttpUtility.HtmlEncode()

            // Parameterübergabe über Route z.B.localhost:XXX/HelloWorld/Welcome/Scott/3
            // oder so localhost:xxx/HelloWorld/Welcome/1?name möglich, wobei ersteres
            // typischer in ASP.NET
        }

        public ActionResult Welcome2(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello" + name;
            ViewBag.NumTimes = numTimes;
            return View();
        }
    }
}