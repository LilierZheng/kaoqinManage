using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace TLO_KQGL.Utilities
{
    public static class Common
    {
        ///<summary>	计算请假的小时数</summary>	
        ///<param name="dtStart">休假开始日期/时间</param>	
        ///<param name="dtEnd">休假结束日期/时间</param>	
        public static int GetLeaveDay(DateTime dtStart, DateTime dtEnd)
        {

            DateTime dtFirstDayGoToWork = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 9, 0, 0);//请假第一天的上班时间
            DateTime dtFirstDayGoOffWork = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 18, 0, 0);//请假第一天的下班时间

            DateTime dtLastDayGoToWork = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 9, 0, 0);//请假最后一天的上班时间
            DateTime dtLastDayGoOffWork = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 18, 0, 0);//请假最后一天的下班时间

            DateTime dtFirstDayRestStart = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 12, 0, 0);//请假第一天的午休开始时间
            DateTime dtFirstDayRestEnd = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 13, 0, 0);//请假第一天的午休结束时间

            DateTime dtLastDayRestStart = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 12, 0, 0);//请假最后一天的午休开始时间
            DateTime dtLastDayRestEnd = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 13, 0, 0);//请假最后一天的午休结束时间

            //如果开始请假时间早于上班时间或者结束请假时间晚于下班时间，者需要重置时间
            //if (!IsWorkDay(dtStart) && !IsWorkDay(dtEnd))
            //    return 0;
            //if (dtStart >= dtFirstDayGoOffWork && dtEnd <= dtLastDayGoToWork && (dtEnd - dtStart).TotalDays < 1)
            //    return 0;
            //if (dtStart >= dtFirstDayGoOffWork && !IsWorkDay(dtEnd) && (dtEnd - dtStart).TotalDays < 1)
            //    return 0;

            if (dtStart < dtFirstDayGoToWork)//早于上班时间
                dtStart = dtFirstDayGoToWork;
            if (dtStart >= dtFirstDayGoOffWork)//晚于下班时间
            {
                while (dtStart < dtEnd)
                {
                    dtStart = new DateTime(dtStart.AddDays(1).Year, dtStart.AddDays(1).Month, dtStart.AddDays(1).Day, 9, 0, 0);
                    //if (IsWorkDay(dtStart))
                    //{
                    //    dtFirstDayGoToWork = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 8, 30, 0);//请假第一天的上班时间
                    //    dtFirstDayGoOffWork = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 17, 30, 0);//请假第一天的下班时间
                    //    dtFirstDayRestStart = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 11, 30, 0);//请假第一天的午休开始时间
                    //    dtFirstDayRestEnd = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 12, 30, 0);//请假第一天的午休结束时间

                    //    break;
                    //}
                }
            }

            if (dtEnd > dtLastDayGoOffWork)//晚于下班时间
                dtEnd = dtLastDayGoOffWork;
            if (dtEnd <= dtLastDayGoToWork)//早于上班时间
            {
                while (dtEnd > dtStart)
                {
                    dtEnd = new DateTime(dtEnd.AddDays(-1).Year, dtEnd.AddDays(-1).Month, dtEnd.AddDays(-1).Day, 18, 0, 0);
                    //if (IsWorkDay(dtEnd))//
                    //{
                    //    dtLastDayGoToWork = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 8, 30, 0);//请假最后一天的上班时间
                    //    dtLastDayGoOffWork = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 17, 30, 0);//请假最后一天的下班时间
                    //    dtLastDayRestStart = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 11, 30, 0);//请假最后一天的午休开始时间
                    //    dtLastDayRestEnd = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 12, 30, 0);//请假最后一天的午休结束时间
                    //    break;
                    //}
                }
            }

            //计算请假第一天和最后一天的小时合计数并换算成分钟数           
            double iSumMinute = dtFirstDayGoOffWork.Subtract(dtStart).TotalMinutes + dtEnd.Subtract(dtLastDayGoToWork).TotalMinutes;//计算获得剩余的分钟数

            if (dtStart > dtFirstDayRestStart && dtStart < dtFirstDayRestEnd)
            {//开始休假时间正好是在午休时间内的，需要扣除掉
                iSumMinute = iSumMinute - dtFirstDayRestEnd.Subtract(dtStart).Minutes;
            }
            if (dtStart < dtFirstDayRestStart)
            {//如果是在午休前开始休假的就自动减去午休的60分钟
                iSumMinute = iSumMinute - 60;
            }
            if (dtEnd > dtLastDayRestStart && dtEnd < dtLastDayRestEnd)
            {//如果结束休假是在午休时间内的，例如“请假截止日是1月31日 12:00分”的话那休假时间计算只到 11:30分为止。
                iSumMinute = iSumMinute - dtEnd.Subtract(dtLastDayRestStart).Minutes;
            }
            if (dtEnd > dtLastDayRestEnd)
            {//如果是在午休后结束请假的就自动减去午休的60分钟
                iSumMinute = iSumMinute - 60;
            }


            int leaveday = 0;//实际请假的天数
            double countday = 0;//获取两个日期间的总天数

            DateTime tempDate = dtStart;//临时参数
            while (tempDate < dtEnd)
            {
                countday++;
                tempDate = new DateTime(tempDate.AddDays(1).Year, tempDate.AddDays(1).Month, tempDate.AddDays(1).Day, 0, 0, 0);
            }
            //循环用来扣除双休日、法定假日 和 添加调休上班
            for (int i = 0; i < countday; i++)
            {
                DateTime tempdt = dtStart.Date.AddDays(i);
                leaveday++;
                //if (IsWorkDay(tempdt))
                //    leaveday++;
            }

            //去掉请假第一天和请假的最后一天，其余时间全部已8小时计算。
            //SumMinute/60： 独立计算 请假第一天和请假最后一天总归请了多少小时的假
            double doubleSumHours = ((leaveday - 2) * 8) + iSumMinute / 60;
            int intSumHours = Convert.ToInt32(doubleSumHours);

            if (doubleSumHours > intSumHours)//如果请假时间不足1小时话自动算作1小时
                intSumHours++;

            return intSumHours;
        }

        /// <summary>

        /// 关闭Excel进程

        /// </summary>



        [DllImport("User32.dll", CharSet = CharSet.Auto)]

        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        {

            try
            {

                IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口 

                int k = 0;

                GetWindowThreadProcessId(t, out k);   //得到本进程唯一标志k

                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用

                p.Kill();     //关闭进程k

            }

            catch (System.Exception ex)
            {

                throw ex;

            }

        }

    }
}