using System;
using System.Web;

namespace PracticaProfesional2025
{
    public class Global : HttpApplication
    {
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            // Guardamos los detalles del error en sesión
            HttpContext.Current.Session["ErrorMessage"] = ex.Message;
            HttpContext.Current.Session["ErrorException"] = ex.ToString(); // stack trace completo

            // Limpia el error del servidor
            Server.ClearError();

            // Redirige a la página de error
            Response.Redirect("~/Error.aspx");
        }
    }
}
