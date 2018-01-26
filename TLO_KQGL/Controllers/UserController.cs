using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;
using System.Text;
using System.Web.Security;
using System.Web;
using NPOI.Util;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Diagnostics;
using System.IO;
using TLO_KQGL.BusinessLayer;
using TLO_KQGL.ViewModels;
//using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using TLO_KQGL.Utilities;
using System.Web.Hosting;

namespace TLO_KQGL.Controllers
{
    public class UserController : ApiController
    {
        private TLO_KQGLDAL db = new TLO_KQGLDAL();
        private EmployeeBll bll = new EmployeeBll();
        private const int OLDOFFICEVESION = -4143;
        private const int NEWOFFICEVESION = 56;
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userNo">用户名</param>
        /// <param name="pwd">用户密码</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Login([FromUri] string userNo, [FromUri] string pwd)
        {
            StringBuilder sb = new StringBuilder();
            var users = (from p in db.User
                         where p.userNo == userNo && p.password == pwd
                         select p).Include("role").FirstOrDefault();
            var emps = (from p in db.Employees where p.Emp_No == userNo select p).Include("Dep").FirstOrDefault();
            var classes = (from p in db.ClassType where p.DeptID == emps.Dep.ID select p).FirstOrDefault();
            if (users == null)
            {
                sb.Append("{\"status\":\"").Append("falied").Append("\",\"Message\":\"用户名或密码错误!\"}");
                return Json(sb.ToString());
            }
            FormsAuthenticationTicket token = new FormsAuthenticationTicket(0, userNo, DateTime.Now,
                            DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", userNo, pwd),
                            FormsAuthentication.FormsCookiePath);
            var Token = FormsAuthentication.Encrypt(token).ToString();
            //将身份信息保存在session中，验证当前请求是否是有效请求
            HttpContext.Current.Session[userNo] = Token;
            sb.Clear();
            sb.Append("{\"status\":\"").Append("success").Append("\",\"Message\":\"登录成功\"");
            sb.Append(",\"Token\":\"").Append(Token).Append("\",\"Lontitude\":\"")
                .Append(emps.Dep.lontitude).Append("\",\"Latitude\":\"")
                .Append(emps.Dep.latitude).Append("\",\"DeptName\":\"")
                .Append(emps.Dep.DeptName).Append("\",\"DeptId\":").Append("\"").Append(emps.Dep.ID).Append("\"")
                .Append(",\"EmpId\":").Append("\"").Append(emps.ID).Append("\"");

            if (classes != null)
            {
                sb.Append(",\"SignOn\":\"").Append(classes.OnWorkTime)
                    .Append("\",\"SignOff\":\"").Append(classes.OffWorkTime).Append("\"")
                  .Append(",\"ClassId\":\"").Append(classes.ID).Append("\",\"BeginSleepTime\":\"").Append(classes.BeginSleepTime)
                  .Append("\",\"EndSleepTime\":\"").Append(classes.EndSleepTime).Append("\",\"WorkEtraTime\":\"").Append(classes.WorkEtraTime).Append("\"");
            }
            foreach (var item in users.role)
            {
                if (item.RoleName == "pro_Leader")
                {
                    sb.Append(",\"Role\":\"").Append("pro_Leader").Append("\"");
                }
                if (item.RoleName == "manager")
                {
                    sb.Append(",\"Role\":\"").Append("manager").Append("\"");
                }
            }

            sb.Append("}");
            return Json(sb.ToString());
        }
        [HttpGet]
        public HttpResponseMessage GetExcel()
        {
            HttpResponseMessage result = null;
            //导出excel
            string time = DateTime.Now.ToString("yyyyMM");
            //string filePath = @"C:\Users\liuxiaoli\Desktop\TRIO勤务表_201801_周焕英.xls";
           // string filePath = @"..\App_Data\TRIO勤务表_201801_周焕英.xls";
            string filePath = HostingEnvironment.MapPath("~/App_Data/TRIO勤务表模板.xls");
            GetAllExcel(filePath);
            result = Request.CreateErrorResponse(HttpStatusCode.OK, "无法找到数据");
            return result;
        }

        //将数据写入已存在Excel
        public void WriteToExcel(string filepath)
        {
            //保存excel文件的格式
            int FormatNum;
            //excel版本号
            string Version;
            //1.创建Applicaton对象
            Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
            //2.得到workbook对象，打开已有的文件
            Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks.Open(filepath);
            //3.指定要操作的Sheet
            Microsoft.Office.Interop.Excel.Worksheet xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];
            //4.向相应对位置写入相应的数据
            string id1 = "8D5DAACE-1F3F-493B-BE7D-1B5420E4714B";
            Guid _id = Guid.Parse(id1);
            List<AttendanceViewModel> ls = bll.GetAttendanceForExcel(_id).ToList();
            if (ls.Count == 0) return;
            double getHours = 0.00;
            string LeaveHours = "";
            string signon = "";
            string signoff = "";
            string sbhour = "";
            string sbminute = "";
            string xbhour = "";
            string xbminute = "";
            string week = "";
            DateTime sbsj = System.DateTime.Now;
            DateTime xbsj = System.DateTime.Now;
            DateTime nyr = System.DateTime.Now;
            double hours = 0;
            double workExtraHours = 0;
            int idate = System.DateTime.DaysInMonth(System.DateTime.Now.Year, System.DateTime.Now.Month); //本月有多少天
            //int RestDay = 0;
            for (int i = 7; i < idate + 7; i++)
            {
                if (i == ls.Count + 7) return;
                string date = System.DateTime.Now.Month + "/" + (i - 6).ToString();
                DateTime dtTemp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, i - 6);
                string tempDate = dtTemp.ToString("yyyy-MM-dd");
                xSheet.Cells[i, 1] = date; //日期
                string[] Day = new string[] { "日", "一", "二", "三", "四", "五", "六" };
                week = Day[Convert.ToInt32(dtTemp.DayOfWeek.ToString("d"))].ToString();
                xSheet.Cells[i, 2] = week; //星期
                for (int j = 0; j < ls.Count; j++)
                {
                    //if (week == "六" || week == "日" && string.IsNullOrEmpty(ls[j].SignOn))
                    //{
                    //    RestDay += 1;
                    //    break;
                    //}
                    if (!string.IsNullOrEmpty(ls[j].SignOn))
                    {
                        string tempSignOn = DateTime.Parse(ls[j].SignOn).ToString("yyyy-MM-dd");
                        if (tempDate == tempSignOn)
                        {
                            signon = ls[j].SignOn.ToString();
                            sbsj = Convert.ToDateTime(signon);
                            signoff = ls[j].SignOff.ToString();
                            xbsj = Convert.ToDateTime(signoff);
                            sbhour = signon.Substring(11, 2);
                            sbminute = signon.Substring(14, 2);
                            xbhour = signoff.Substring(11, 2);
                            xbminute = signoff.Substring(14, 2);
                            xSheet.Cells[i, 3] = sbhour; //（时）上班时刻 
                            xSheet.Cells[i, 5] = sbminute; //（分）上班时刻
                            xSheet.Cells[i, 6] = xbhour; //（时）下班时刻 
                            xSheet.Cells[i, 8] = xbminute; //（分）下班时刻
                            System.TimeSpan sbsc = xbsj - sbsj;
                            getHours = sbsc.TotalHours;
                            xSheet.Cells[i, 9] = getHours - 1; //工作时间
                            hours += getHours;
                            int Late = Convert.ToInt16(ls[j].Late);
                            int LeaveEary = Convert.ToInt16(ls[j].LeaveEary);
                            xSheet.Cells[i, 13] = Late; //迟到
                            if (Late == 0)
                            {
                                xSheet.Cells[i, 13] = "";
                            }
                            xSheet.Cells[i, 14] = LeaveEary; //早退
                            if (LeaveEary == 0)
                            {
                                xSheet.Cells[i, 14] = "";
                            }
                            //休日出勤
                            if (week == "六" || week == "日")
                            {
                                if (4 < getHours && getHours < 8)
                                {
                                    xSheet.Cells[i, 16] = 0.5;
                                }
                                if (getHours >= 8)
                                {
                                    xSheet.Cells[i, 16] = 1;
                                }
                                else
                                    xSheet.Cells[i, 16] = "";
                            }
                            else
                            {
                                xSheet.Cells[i, 16] = "";
                            }
                            if (!string.IsNullOrEmpty(ls[j].WorkOverTime))
                            {
                                workExtraHours += double.Parse(ls[j].WorkOverTime);
                            }
                        }
                    }
                    string _tr = tempDate.Replace("-", "");

                    if (_tr == ls[j].AttenNo && !string.IsNullOrEmpty(ls[j].LeaveId))
                    {
                        LeaveHours = ls[j].LeaveHours;
                        //请假
                        if (Convert.ToDouble(LeaveHours) < 4)
                        {
                            xSheet.Cells[i, 15] = 0.5;
                        }
                        if (4 < Convert.ToDouble(LeaveHours) && Convert.ToDouble(LeaveHours) <= 8)
                        {
                            xSheet.Cells[i, 15] = 1;
                        }
                        else
                            xSheet.Cells[i, 15] = "";
                        // 处理办法（年假、病假、调休、事假、婚假、产假、丧假）
                        string LeaveTypeName = ls[j].LeaveTypeName;
                        if (Convert.ToDouble(LeaveHours) < 4)
                        {
                            if (LeaveTypeName == "年休")
                            {
                                xSheet.Cells[i, 17] = 0.5;
                            }
                            if (LeaveTypeName == "病假")
                            {
                                xSheet.Cells[i, 18] = 0.5;
                            }
                            if (LeaveTypeName == "调休")
                            {
                                xSheet.Cells[i, 19] = 0.5;
                            }
                            if (LeaveTypeName == "事假")
                            {
                                xSheet.Cells[i, 20] = 0.5;
                            }
                            if (LeaveTypeName == "婚假")
                            {
                                xSheet.Cells[i, 21] = 0.5;
                            }
                            if (LeaveTypeName == "产假")
                            {
                                xSheet.Cells[i, 22] = 0.5;
                            }
                            if (LeaveTypeName == "丧假")
                            {
                                xSheet.Cells[i, 23] = 0.5;
                            }
                        }
                        if (4 < Convert.ToDouble(LeaveHours) && Convert.ToDouble(LeaveHours) <= 8)
                        {
                            if (LeaveTypeName == "年休")
                            {
                                xSheet.Cells[i, 17] = 1;
                            }
                            if (LeaveTypeName == "病假")
                            {
                                xSheet.Cells[i, 18] = 1;
                            }
                            if (LeaveTypeName == "调休")
                            {
                                xSheet.Cells[i, 19] = 1;
                            }
                            if (LeaveTypeName == "事假")
                            {
                                xSheet.Cells[i, 20] = 1;
                            }
                            if (LeaveTypeName == "婚假")
                            {
                                xSheet.Cells[i, 21] = 1;
                            }
                            if (LeaveTypeName == "产假")
                            {
                                xSheet.Cells[i, 22] = 1;
                            }
                            if (LeaveTypeName == "丧假")
                            {
                                xSheet.Cells[i, 23] = 1;
                            }
                        }
                    }
                }
                xSheet.Cells[38, 9] = hours; //公司勤务规则合计
                xSheet.Cells[40, 9] = hours; //实际工作时间
                xSheet.Cells[41, 9] = workExtraHours;//平日加班时间
            }
            int year = System.DateTime.Now.Year;
            int month = System.DateTime.Now.Month;
            int days = System.DateTime.DaysInMonth(year, month);
            xSheet.Cells[40, 3] = days - 8; //工作天数
            xSheet.Cells[41, 3] = days - 8; //勤务天数
            //获取你使用的excel 的版本号
            Version = xApp.Version;
            //使用Excel 97-2003
            if (Convert.ToDouble(Version) < 12)
            {
                FormatNum = OLDOFFICEVESION;
            }
            //使用excel 2007或者更新
            else
            {
                FormatNum = NEWOFFICEVESION;
            }
            //5.保存WorkBook
            xBook.SaveAs(filepath, FormatNum);
            //Excel从内存中退出
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xBook);
            xApp.Quit();
            Common.Kill(xApp);//调用方法关闭进程
            GC.Collect();
        }

        //将数据写入已存在Excel
        public void GetAllExcel(string filepath)
        {
            //保存excel文件的格式
            int FormatNum;
            //excel版本号
            string Version;
            //1.创建Applicaton对象
            Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
            //2.得到workbook对象，打开已有的文件
            Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks.Open(filepath);
            //3.指定要操作的Sheet
            Microsoft.Office.Interop.Excel.Worksheet xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];

            xApp.DisplayAlerts = false;
            //4.向相应对位置写入相应的数据
            Dictionary<string, List<AttendanceViewModel>> dic = bll.GetAllAttendanceForExcel();
            if (dic.Count == 0) return;
            double ysbsc = 0.00;
            double jbsc =0.00;
            string LeaveHours = "";
            string signon = "";
            string signoff = "";
            string sbhour = "";
            string sbminute = "";
            string xbhour = "";
            string xbminute = "";
            string week = "";
            DateTime sbsj = System.DateTime.Now;
            DateTime xbsj = System.DateTime.Now;
            DateTime nyr = System.DateTime.Now;
            int idate = System.DateTime.DaysInMonth(System.DateTime.Now.Year, System.DateTime.Now.Month); //本月有多少天
            foreach (var item in dic)
            {
                //string filePathNew = "C:\\Users\\liuxiaoli\\Desktop\\TRIO勤务表_" + DateTime.Now.Year + DateTime.Now.Month + a + ".xls";
                string filePathNew = HostingEnvironment.MapPath("~/App_Data/TRIO勤务表_" + DateTime.Now.Year + DateTime.Now.Month + item.Key + ".xls");
                string filePath = HostingEnvironment.MapPath("~/App_Data/TRIO勤务表_" + DateTime.Now.Year + DateTime.Now.Month );
                string fileName = "";
                xBook.SaveCopyAs(filePathNew);
                //2.得到workbook对象，打开已有的文件
                Microsoft.Office.Interop.Excel.Workbook xBookNew = xApp.Workbooks.Open(filePathNew);
                //3.指定要操作的Sheet
                Microsoft.Office.Interop.Excel.Worksheet xSheetNew = (Microsoft.Office.Interop.Excel.Worksheet)xBookNew.Sheets[1];
                xSheetNew.Cells[1, 9] = DateTime.Now.Year + "年" + DateTime.Now.Month + "月";
                double hours = 0;//工作时间累计
                double workExtraHours = 0;
                double getHours = 0.00;
                double days = 0;

                for (int i = 7; i < idate + 7; i++)
                {
                    string date = System.DateTime.Now.Month + "/" + (i - 6).ToString();
                    DateTime dtTemp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, i - 6);
                    string tempDate = dtTemp.ToString("yyyy-MM-dd");
                    xSheetNew.Cells[i, 1] = date; //日期
                    string[] Day = new string[] { "日", "一", "二", "三", "四", "五", "六" };
                    week = Day[Convert.ToInt32(dtTemp.DayOfWeek.ToString("d"))].ToString();
                    xSheetNew.Cells[i, 2] = week; //星期
                    //if (i == dic[item.Key].Count + 7) return;
                    if (dic[item.Key].Count > 0)
                    {
                        for (int j = 0; j < dic[item.Key].Count; j++)
                        {
                            fileName = dic[item.Key][j].EmpName;
                            xSheetNew.Cells[3, 1] = dic[item.Key][j].EmpNo;
                            xSheetNew.Cells[3, 3] = dic[item.Key][j].EmpName;
                            xSheetNew.Cells[3, 29] = dic[item.Key][j].OnWorkTime.Substring(0, 2);
                            xSheetNew.Cells[3, 31] = dic[item.Key][j].OnWorkTime.Substring(2, 2);
                            xSheetNew.Cells[4, 29] = dic[item.Key][j].OffWorkTime.Substring(0, 2);
                            xSheetNew.Cells[4, 31] = dic[item.Key][j].OffWorkTime.Substring(2, 2);
                            string beginsleephour = dic[item.Key][j].BeginSleepTime.Substring(0, 2);
                            string beginsleepminute = dic[item.Key][j].BeginSleepTime.Substring(2, 2);
                            string beginsleeptime = beginsleephour + ":" + beginsleepminute;
                            string endsleephour = dic[item.Key][j].EndSleepTime.Substring(0, 2);
                            string endsleepminute = dic[item.Key][j].EndSleepTime.Substring(2, 2);
                            string endsleeptime = endsleephour + ":" + endsleepminute;
                            double sleepHours= (Convert.ToDateTime(endsleeptime) - Convert.ToDateTime(beginsleeptime)).Duration().TotalHours;
                            string WorkExtraTime = dic[item.Key][j].WorkEtraTime.Substring(0, 2) + ":" + dic[item.Key][j].WorkEtraTime.Substring(2, 2);
                            DateTime dtWorkExtraTime = DateTime.Parse(tempDate + " " + WorkExtraTime);
                            xSheetNew.Cells[3, 32] = sleepHours;
                            if (!string.IsNullOrEmpty(dic[item.Key][j].SignOn))
                            {
                                string tempSignOn = DateTime.Parse(dic[item.Key][j].SignOn).ToString("yyyy-MM-dd");
                                if (tempDate == tempSignOn)
                                {
                                    signon = dic[item.Key][j].SignOn;
                                    //sbsj = Convert.ToDateTime(signon);
                                    signoff = dic[item.Key][j].SignOff;
                                    xbsj = Convert.ToDateTime(signoff);
                                    if(signon.Substring(11,2) == "08" )
                                    {
                                        sbsj = Convert.ToDateTime(signon.Substring(0, 11) + "09" + ":" + "00");
                                    }
                                    else
                                    {
                                        sbsj = Convert.ToDateTime(signon);
                                    }
                                    DateTime yxbsj = Convert.ToDateTime(signon.Substring(0,11) + "18" + ":" + "00");
                                    ysbsc = (yxbsj - sbsj).TotalHours -sleepHours;
                                    if (xbsj < dtWorkExtraTime)
                                    {
                                        jbsc = 0;
                                    }
                                    else
                                    {
                                        jbsc = (dtWorkExtraTime - xbsj).Duration().TotalHours;
                                    }
                                    sbhour = signon.Substring(11, 2);
                                    sbminute = signon.Substring(14, 2);
                                    xbhour = signoff.Substring(11, 2);
                                    xbminute = signoff.Substring(14, 2);
                                    xSheetNew.Cells[i, 3] = sbhour; //（时）上班时刻 
                                    xSheetNew.Cells[i, 5] = sbminute; //（分）上班时刻
                                    //System.TimeSpan sbsc = xbsj - sbsj;
                                    if (xbsj == sbsj)
                                    {
                                        getHours = 0;
                                        xSheetNew.Cells[i, 9] = 0;
                                     
                                    }
                                    else
                                    {
                                        xSheetNew.Cells[i, 6] = xbhour; //（时）下班时刻 
                                        xSheetNew.Cells[i, 8] = xbminute; //（分）下班时刻
                                        xSheetNew.Cells[i, 9] = ysbsc + jbsc; //工作时间
                                        getHours = ysbsc + jbsc;
                                    }
                                  
                                    hours += getHours;
                                    int Late = Convert.ToInt16(dic[item.Key][j].Late);
                                    int LeaveEary = Convert.ToInt16(dic[item.Key][j].LeaveEary);
                                    xSheetNew.Cells[i, 13] = Late; //迟到
                                    if (Late == 0)
                                    {
                                        xSheetNew.Cells[i, 13] = "";
                                    }
                                    xSheetNew.Cells[i, 14] = LeaveEary; //早退
                                    if (LeaveEary == 0)
                                    {
                                        xSheetNew.Cells[i, 14] = "";
                                    }
                                    //休日出勤
                                    if (week == "六" || week == "日")
                                    {
                                        if (4 < getHours && getHours < 8)
                                        {
                                            xSheetNew.Cells[i, 16] = 0.5;
                                        }
                                        if (getHours >= 8)
                                        {
                                            xSheetNew.Cells[i, 16] = 1;
                                        }
                                        else
                                            xSheetNew.Cells[i, 16] = "";
                                    }
                                    else
                                    {
                                        xSheetNew.Cells[i, 16] = "";
                                    }
                                    if (!string.IsNullOrEmpty(dic[item.Key][j].WorkOverTime))
                                    {
                                        workExtraHours += double.Parse(dic[item.Key][j].WorkOverTime);
                                    }
                                }
                            }
                            string _tr = tempDate.Replace("-", "");

                            if (_tr == dic[item.Key][j].AttenNo && !string.IsNullOrEmpty(dic[item.Key][j].LeaveId))
                            {
                                LeaveHours = dic[item.Key][j].LeaveHours;
                                //请假
                                if (Convert.ToDouble(LeaveHours) < 4)
                                {
                                    xSheetNew.Cells[i, 15] = 0.5;
                                }
                                if (4 < Convert.ToDouble(LeaveHours) && Convert.ToDouble(LeaveHours) <= 8)
                                {
                                    xSheetNew.Cells[i, 15] = 1;
                                }
                                else
                                    xSheetNew.Cells[i, 15] = "";
                                // 处理办法（年假、病假、调休、事假、婚假、产假、丧假）
                                string LeaveTypeName = dic[item.Key][j].LeaveTypeName;
                                if (Convert.ToDouble(LeaveHours) < 4)
                                {
                                    if (LeaveTypeName == "年休")
                                    {
                                        xSheetNew.Cells[i, 17] = 0.5;
                                    }
                                    if (LeaveTypeName == "病假")
                                    {
                                        xSheetNew.Cells[i, 18] = 0.5;
                                    }
                                    if (LeaveTypeName == "调休")
                                    {
                                        xSheetNew.Cells[i, 19] = 0.5;
                                    }
                                    if (LeaveTypeName == "事假")
                                    {
                                        xSheetNew.Cells[i, 20] = 0.5;
                                    }
                                    if (LeaveTypeName == "婚假")
                                    {
                                        xSheetNew.Cells[i, 21] = 0.5;
                                    }
                                    if (LeaveTypeName == "产假")
                                    {
                                        xSheetNew.Cells[i, 22] = 0.5;
                                    }
                                    if (LeaveTypeName == "丧假")
                                    {
                                        xSheetNew.Cells[i, 23] = 0.5;
                                    }
                                }
                                if (4 < Convert.ToDouble(LeaveHours) && Convert.ToDouble(LeaveHours) <= 8)
                                {
                                    if (LeaveTypeName == "年休")
                                    {
                                        xSheetNew.Cells[i, 17] = 1;
                                    }
                                    if (LeaveTypeName == "病假")
                                    {
                                        xSheetNew.Cells[i, 18] = 1;
                                    }
                                    if (LeaveTypeName == "调休")
                                    {
                                        xSheetNew.Cells[i, 19] = 1;
                                    }
                                    if (LeaveTypeName == "事假")
                                    {
                                        xSheetNew.Cells[i, 20] = 1;
                                    }
                                    if (LeaveTypeName == "婚假")
                                    {
                                        xSheetNew.Cells[i, 21] = 1;
                                    }
                                    if (LeaveTypeName == "产假")
                                    {
                                        xSheetNew.Cells[i, 22] = 1;
                                    }
                                    if (LeaveTypeName == "丧假")
                                    {
                                        xSheetNew.Cells[i, 23] = 1;
                                    }
                                }
                            }
                        }
                    }
                    xSheetNew.Cells[38, 9] = hours; //公司勤务规则合计
                    xSheetNew.Cells[40, 9] = hours; //实际工作时间
                    xSheetNew.Cells[41, 9] = workExtraHours;//平日加班时间
                }
                int year = System.DateTime.Now.Year;
                int month = System.DateTime.Now.Month;
                
                if (hours % 8 == 0)
                {
                    days = hours / 8;
                }
                if (hours % 8 <= 4)
                {
                    days = Math.Floor(hours / 8) + 0.5;
                }
                if (hours % 8 > 4)
                {
                    days = Math.Floor(hours / 8) + 1;
                }
                xSheetNew.Cells[40, 3] = days; //工作天数
                xSheetNew.Cells[41, 3] = days; //勤务天数
                //获取你使用的excel 的版本号
                Version = xApp.Version;
                //使用Excel 97-2003
                if (Convert.ToDouble(Version) < 12)
                {
                    FormatNum = OLDOFFICEVESION;
                }
                //使用excel 2007或者更新
                else
                {
                    FormatNum = NEWOFFICEVESION;
                }
                //5.保存WorkBook
                xBookNew.SaveAs(filePath+fileName+".xls", FormatNum);
                xBookNew.Close();
                //Excel从内存中退出
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xSheetNew);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xBookNew);

            }
            xBook.Close();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xBook);

            xApp.Quit();
            Common.Kill(xApp);//调用方法关闭进程
            GC.Collect();

        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<User> GetUser()
        {
            return db.User;
        }

        // GET api/User/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT api/User/5 修改用户
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.ID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/User 增加用户
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.ID }, user);
        }

        // DELETE api/User/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.User.Count(e => e.ID == id) > 0;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userno">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost][SupportFilter]
        public HttpResponseMessage AlterPassword([FromUri]string userno, [FromUri] string password, [FromUri]string Token)
        {
            if (string.IsNullOrEmpty(userno) || string.IsNullOrEmpty(password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "用户名或修改密码为空");
            }
            //获取前台传回的数据
            var user = db.User.Where(p=>p.userNo==userno).ToList();
            if (user.Count == 0) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "该用户没有找到，请联系管理员！");
            TLO_KQGL.Models.User userModel = user[0];
            db.Entry(userModel).State = EntityState.Modified;
            userModel.password = password;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.OK, "修改成功");
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFilePath()
        {
            StringBuilder sb = new StringBuilder();
            string filePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/Apk");
            string filePath1 = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + @"/App_Data/Apk/";
            var files = Directory.GetFiles(filePath, "*.apk");
            string Newfile = "";
            string NewfileName = "";
            if (files.Count() > 0)
            {
               foreach (var file in files)
               {
                   Newfile = file;
                   //NewfileName = Newfile.Substring(file.LastIndexOf(@"\"),file.Length-file.IndexOf(".")+1);
                   NewfileName = Newfile.Substring(file.LastIndexOf("V"), file.Length - file.IndexOf("."));
               }
               Newfile = Newfile.Replace(@"F:\KQGLPublish\TLO_KQGL\App_Data\Apk\", filePath1);
            }
            sb.Append("{\"apkUrl\":\"").Append(Newfile).Append("\",\"fileName\":\"").Append(NewfileName).Append("\"}");
            //sb.Append("{");
            //sb.Append("apkurl:");
            //sb.Append(Newfile);
            //sb.Append(",");
            //sb.Append("fileName:");
            //sb.Append(NewfileName);
            //sb.Append("}");
            return Json(sb.ToString());
            //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Newfile);
        }

        public class obj
        {
            public data data { get; set; }
        }

        public class data
        {
            public string apkUrl { get; set; }
        }
    }
}