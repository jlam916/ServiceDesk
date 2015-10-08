using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Printer_Migration_Status_Page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    private void Bind()
    {
        SqlDataSource SqlDataSource1 = new SqlDataSource();
        this.Page.Controls.Add(SqlDataSource1);
        SqlDataSource1.ConnectionString = "Data Source=W8-RKOEN;Initial Catalog=PrinterMigration;User ID=sa;Password=p@ssw0rd";
        SqlDataSource1.SelectCommand = "SELECT * FROM Main Order By Floor";
        GridView1.DataSource = SqlDataSource1;
        GridView1.DataBind();
    }
}