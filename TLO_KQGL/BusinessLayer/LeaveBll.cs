using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.DBAccessLayer;
using TLO_KQGL.ViewModels;
using TLO_KQGL.Models;

namespace TLO_KQGL.BusinessLayer
{
    public class LeaveBll
    {
        private LeaveDal dal = new LeaveDal();

        public LeaveViewModel GetLeaveById(string id)
        {
            return dal.GetLeaveById(id);
        }
        /// <summary>
        /// 获取所有假条
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LeaveViewModel> GetLeave()
        {
            return dal.GetLeave();
        }
        /// <summary>
        /// 根据条件获取假条
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="isCheck">是否审核</param>
        /// <returns></returns>
        public IEnumerable<LeaveViewModel> GetLeaveBy(string beginDate, string endDate, bool isCheck)
        {
            return dal.GetLeaveBy(beginDate, endDate, isCheck);
        }
        /// <summary>
        /// 根据员工id获取假条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<LeaveViewModel> GetLeaveByEmpId(Guid id)
        {
            return dal.GetLeaveByEmpId(id);
        }
        /// <summary>
        /// 审核指定假条
        /// </summary>
        /// <param name="id">假条id</param>
        /// <param name="IsPass">是否同意</param>
        /// <returns></returns>
        public int AuditLeave(LeaveViewModel lea, int days)
        {
            return dal.AuditLeave(lea, days);
        }

        public IEnumerable<DictionaryViewModel> GetLeaveDic()
        {
            return dal.GetLeaveDic();
        }
    }
}
