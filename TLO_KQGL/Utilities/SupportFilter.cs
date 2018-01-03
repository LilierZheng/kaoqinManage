using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;

namespace TLO_KQGL.Utilities
{
    public class SupportFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //通过url获取token
            var content = actionContext.Request.Properties["MS_HttpContext"] as HttpContextBase;
            var token = content.Request.QueryString["Token"];
           // actionContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            if (!string.IsNullOrEmpty(token))
            {
                //解密用户ticket,并校验用户名密码是否匹配
                if (ValidateTicket(token))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }
        }

        //校验用户名密码（对Session匹配，或数据库数据匹配）
        private bool ValidateTicket(string encryptToken)
        {
            if (string.IsNullOrEmpty(encryptToken))
            {
                return false;
            }
            else
            {
                return true;
            }
            //解密Ticket
            var strTicket = FormsAuthentication.Decrypt(encryptToken).UserData;

            //从Ticket里面获取用户名和密码
            var index = strTicket.IndexOf("&");
            string userName = strTicket.Substring(0, index);
            string password = strTicket.Substring(index + 1);
            //取得session，不通过说明用户退出，或者session已经过期
            var token = HttpContext.Current.Session[userName];
            if (token == null)
            {
                //todo
               return false;
            }
            //对比session中的令牌
            if (token.ToString() == encryptToken)
            {
                return true;
            }

            return false;

        }

    }
}