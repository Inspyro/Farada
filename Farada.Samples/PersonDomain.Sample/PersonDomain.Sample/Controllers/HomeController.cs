using System;
using System.Web.Mvc;
using PersonDomain.Sample.Models;

namespace PersonDomain.Sample.Controllers
{
    public class HomeController : Controller
    {
        private static NodeModel _nodeModel;

        private static NodeModel NodeModel
        {
            get { return _nodeModel ?? (_nodeModel = new NodeModel()); }
        }

        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(NodeModel);
        }
    }
}