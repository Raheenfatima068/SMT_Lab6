
using System;
using System.Web;
using System.Web.Routing;

namespace SMT_Lab6
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Session startup
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Application-level error handler
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Session ended
        }

        protected void Application_End(object sender, EventArgs e)
        {
            // Application shutdown
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(
                "GridViewInsertUpdateDeleteRoute",
                "GridViewInsertUpdateDelete",
                "~/GridViewInsertUpdateDelete.aspx"
            );
        }
    }
}
