using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace sisCCS.UserLayer.Models
{
    public class SessionFilter : ActionFilterAttribute
    {
        Authentication GrifosoftAuthenticartion = new Authentication();

        public int ViewId { get; set; }

        public override void OnActionExecuting(ActionExecutingContext FilterContext)
        {
            HttpCookie SessionCookie = HttpContext.Current.Request.Cookies["SessionCookie"];
            if (SessionCookie == null)
            {
                FilterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller", "Seguridad" },
                        { "Action", "Login" }
                    }
                );
            }
            else
            {
                if (HttpContext.Current.Session["Usuario"] == null)
                    GrifosoftAuthenticartion.RestartSession();
                if (ViewId != 0)
                {
                    if (!GrifosoftAuthenticartion.IsValidView(ViewId))
                    {
                        FilterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Seguridad" },
                                { "Action", "Principal" }
                            }
                        );
                    }
                }
            }
        }
    }
}