using System;
using System.Collections.Generic;
using System.Data;
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
    public class NewsController : ApiController
    {
        private TLO_KQGLDAL db = new TLO_KQGLDAL();
        [HttpGet]
        public IHttpActionResult GetNews()
        {
            var News = (from p in db.news select p).ToList();
            if (News.Count > 0)
            {
                return Json<List<News>>(News,GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings,Encoding.UTF8);
            }
            return null;
        }

        /// <summary>
        /// 根据消息id获取消息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        [SupportFilter]
        public IHttpActionResult GetNew(Guid id, string Token)
        {
            var New = db.news.Where(b => b.ID == id).ToList();
            if (New.Count > 0)
            {
                return Json<List<News>>(New,GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings,Encoding.UTF8);
            } 
            return null;
        }
    }
}