using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyHomePage
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) 
        {
            if(!IsPostBack)
                new Thread( new ThreadStart( metrics ) ).Start();
        }

        private void metrics()
        {
            string client = System.Net.Dns.GetHostEntry( Request.ServerVariables["remote_addr"] ).HostName;
            using ( SqlConnection connection = new SqlConnection( "Data Source=W8-RKOEN;Initial Catalog=HomePageReporting;User ID=sa;Password=p@ssw0rd" ) )
            {
                try
                {
                    connection.Open();
                    try
                    {
                        using ( SqlCommand command = new SqlCommand( "INSERT INTO MyHomePage VALUES(@ComputerName, @Time)", connection ) )
                        {
                            command.Parameters.Add( new SqlParameter( "ComputerName", client.Split( '.' )[0] ) );
                            command.Parameters.Add( new SqlParameter( "Time", DateTime.Now ) );

                            command.ExecuteNonQuery();
                        }
                    }
                    catch ( Exception ex )
                    { Response.Write( "Error in SQL Command metrics." ); }
                }
                catch ( Exception ex )
                {
                    Response.Write("Error in SQL Connect metrics.")
                }
            }
        }
    }
}