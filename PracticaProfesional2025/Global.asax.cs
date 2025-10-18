using System;
using System.Web;

namespace PracticaProfesional2025
{
    public class Global : HttpApplication
    {

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            Application["ErrorMessage"] = ex.Message;
            Application["ErrorException"] = ex.ToString();

            Server.ClearError();
            Response.Redirect("~/Error.aspx");
        }
    }
}
