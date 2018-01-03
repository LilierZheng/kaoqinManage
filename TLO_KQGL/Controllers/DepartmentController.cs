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
using System.Web.Security;
using TLO_KQGL.Utilities;
using System.Web.Providers.Entities;
using Newtonsoft.Json;
using TLO_KQGL.BusinessLayer;

namespace TLO_KQGL.Controllers
{
    public class DepartmentController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        public IHttpActionResult GetDepartmentTemp()
        {
            DepartmentBll bll=new DepartmentBll ();
            var ret = bll.GetList().ToList();
            if (ret.Count() > 0)
            {
                return Json<List<Department>>(ret);
            }
            return null;
        }
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}