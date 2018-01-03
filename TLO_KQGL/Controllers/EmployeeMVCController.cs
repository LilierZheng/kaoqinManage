using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLO_KQGL.BusinessLayer;

namespace TLO_KQGL.Controllers
{
    public class EmployeeMVCController : Controller
    {
        //
        // GET: /EmployeeMVC/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            DepartmentBll bll=new DepartmentBll ();
            var dept = bll.GetList();
            return View();
        }
	}
}