using System.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;
using TLO_KQGL.BusinessLayer;
using System.Web.Security;
using TLO_KQGL.Utilities;
using System.Web.Providers.Entities;
using Newtonsoft.Json;
using System.Text;
using TLO_KQGL.ViewModels;

namespace TLO_KQGL.Controllers
{
    public class SuggestController : ApiController
    {
        private TLO_KQGLDAL db = new TLO_KQGLDAL();
        /// <summary>
        /// 新增意见反馈
        /// </summary>
        /// <param name="suggest"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public HttpResponseMessage AddSuggest([FromBody] SuggestModel suggest, [FromUri] string Token)
        {
            Suggest addsuggest = new Suggest();
            addsuggest.ID = Guid.NewGuid();
            addsuggest.Content = suggest.Content; //意见反馈内容  
            addsuggest.CreateUser = suggest.CreateUser; //创建者
            addsuggest.CreateDate = Convert.ToDateTime(DateTime.Now); //创建时间
            try
            {
                db.suggest.Add(addsuggest);
                db.SaveChanges();
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.OK, "新增意见反馈成功！");
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public class SuggestModel
        {
            public string Content { get; set; } //意见反馈内容
            public string CreateUser { get; set; } //创建者

        }
    }
}
