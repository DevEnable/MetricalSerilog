using System;
using System.Data.Common;
using MyWebsite.DAL;

namespace MyWebsite
{
    public partial class _Default : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DbConnection connection = ConnectionFactory.GetConnection();
            MyDataAccess dal = new MyDataAccess(connection);

            dal.GetShipper("speed");
        }
    }
}