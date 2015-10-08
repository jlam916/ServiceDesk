using System;
using System.Data.SqlClient;
using System.Threading;

public partial class App_Assets_Configurations_TeleworkConfigurations : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new Thread( new ThreadStart( metrics ) ).Start();
    }

    private void metrics()
    {
        try
        {
            string client = System.Net.Dns.GetHostEntry( Request.ServerVariables["remote_addr"] ).HostName;
            using ( SqlConnection connection = new SqlConnection( "Data Source=W8-RKOEN;Initial Catalog=HomePageReporting;User ID=sa;Password=p@ssw0rd" ) )
            {
                try
                {
                    connection.Open();
                    try
                    {
                        using ( SqlCommand command = new SqlCommand( "INSERT INTO TeleworkInstructions VALUES(@ComputerName, @Time)", connection ) )
                        {
                            command.Parameters.Add( new SqlParameter( "ComputerName", client.Split( '.' )[0] ) );
                            command.Parameters.Add( new SqlParameter( "Time", DateTime.Now ) );

                            command.ExecuteNonQuery();
                        }
                    }
                    catch ( Exception ex )
                    { }
                }
                catch ( Exception ex )
                {
                }
            }
        }
        catch { }
    }
}