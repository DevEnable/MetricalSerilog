using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWebsite.DAL;

namespace MyWebsite
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DbConnection connection = ConnectionFactory.GetConnection();
            MyDataAccess dal = new MyDataAccess(connection);

            dal.GetShipper("speed");
        }
    }
}