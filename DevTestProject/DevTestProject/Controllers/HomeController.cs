using DevTestProject.ViewModel;
using System;
using System.Web.Mvc;

namespace DevTestProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string errorMsg = String.Empty;
            HomeVm model = new HomeVm();
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            model.ErrorMsg = errorMsg;
            return View("Index", model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}