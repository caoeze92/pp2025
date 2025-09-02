using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (Session["Rol"] != null && Session["Rol"].ToString() == "admin")
                {
                    menuAdmin.Visible = true;
                }
                else
                {
                    menuAdmin.Visible = false;
                }
            }
        }
    }
}