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
        private DepartmentBll deptBll = new DepartmentBll();
        private EmployeeBll bll = new EmployeeBll();
        //
        // GET: /EmployeeMVC/
        public ActionResult Index()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "请选择",
                Value ="0"
            });
            var dic = new Dictionary<int, string>(deptBll.GetDeptDic());
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    list.Add(new SelectListItem()
                    {
                        Text = item.Value,
                        Value = item.Key.ToString()
                    });
                    
                }
            }
            ViewData["Dept"] = list;
            return View();
        }
        public JsonResult GetEmps(string EmpName,string EmpNo,string DeptId,int pageSize,int pageIndex)
        {
            var list= bll.GetList();
            var total = list.Count();
            var rows = list.Skip(pageIndex).Take(pageSize).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add()
        {
            DepartmentBll bll=new DepartmentBll ();
            var dept = bll.GetList();
            return View();
        }
	}
}