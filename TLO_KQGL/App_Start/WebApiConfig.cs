using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using TLO_KQGL.Utilities;

namespace TLO_KQGL
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //默认返回 json  
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "json", "application/json"));
            //返回格式选择 datatype 可以替换为任何参数   
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "xml", "application/xml"));

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        
            // 取消注释下面的代码行可对具有 IQueryable 或 IQueryable<T> 返回类型的操作启用查询支持。
            // 若要避免处理意外查询或恶意查询，请使用 QueryableAttribute 上的验证设置来验证传入查询。
            // 有关详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=279712。
            //config.EnableQuerySupport();

            // 若要在应用程序中禁用跟踪，请注释掉或删除以下代码行
            // 有关详细信息，请参阅: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
        public class SessionRouteHandler : HttpControllerHandler, IRequiresSessionState
        {
            public SessionRouteHandler(RouteData routeData)
                : base(routeData)
            {

            }
        }

        public class SessionControllerRouteHandler : HttpControllerRouteHandler
        {
            protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
            {

                return new SessionRouteHandler(requestContext.RouteData);

            }

        }
    }
}
