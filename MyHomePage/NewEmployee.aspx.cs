using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Printing;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewEmployee : System.Web.UI.Page
{
    private PrintServer server;
    private PrintQueueCollection printersCollection;
    private List<String> printersOnServer = new List<String>();
    private List<String> floors = new List<String>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack )
        {
            new Thread( new ThreadStart( metrics ) ).Start();

            string client = System.Net.Dns.GetHostEntry( Request.ServerVariables["remote_addr"] ).HostName;
            litUser.Text = client;
            hlOEM.NavigateUrl = @"\\" + client + @"\C$\Windows\OEM";
            hlBIOS.NavigateUrl = @"\\dr3docs1\Windows8\BIOS-Update";
            hlFirmware.NavigateUrl = @"\\dr3docs1\Windows8\BIOS-Update\2-BIOS-Settings-OP7010-Basic.exe";
            hlSoftware.NavigateUrl = @"\\iwmdocs\iwm\CIWMB-INFOTECH\Software2";
            hlLync.NavigateUrl = @"\\iwmdocs\iwm\CIWMB-INFOTECH\HelpCenter\RKOEN\Website\OfficeShortcuts\Lync 2013.lnk";
            hlWord.NavigateUrl = @"\\iwmdocs\iwm\CIWMB-INFOTECH\HelpCenter\RKOEN\Website\OfficeShortcuts\Word 2013.lnk";
            hlOutlook.NavigateUrl = @"\\iwmdocs\iwm\CIWMB-INFOTECH\HelpCenter\RKOEN\Website\OfficeShortcuts\Outlook 2013.lnk";
        }

        server = new PrintServer( @"\\DR3PRINT" );
        printersCollection = server.GetPrintQueues();

        foreach ( PrintQueue printer in printersCollection )
        {
            if ( printer.Name.Contains( "HP" ) || printer.Name.Contains( "NPI9EA6A6" ) || printer.Name.Contains( "AUDITS" ) || printer.Name.Contains( "Cert" ) ||
                 printer.Name.Contains( "MICR" ) || printer.Name.Contains( "KEN-SCOTT" ) || printer.Name.Contains( "COASTAL" ) || printer.Name.Contains( "Tire" ) )
            {
                continue;
            }
            printersOnServer.Add( printer.Name );
        }

        floors = stripFloorNumbers( printersOnServer );
        ddFloor.DataSource = floors;

        if ( !IsPostBack )
        {
            ddFloor.DataBind();
            ddFloor_SelectedIndexChanged( ddFloor, null );
        }
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
                        using ( SqlCommand command = new SqlCommand( "INSERT INTO NewEmployeeInstructions VALUES(@ComputerName, @Time)", connection ) )
                        {
                            command.Parameters.Add( new SqlParameter( "ComputerName", client.Split( '.' )[0] ) );
                            command.Parameters.Add( new SqlParameter( "Time", DateTime.Now ) );

                            command.ExecuteNonQuery();
                        }
                    }
                    catch 
                    { }
                }
                catch 
                {
                }
            }
        }
        catch { }
    }

    /// <summary>
    /// Takes all the floor numbers and adds them to the floor drop down list.
    /// Remove duplicates and sorts the list.
    /// </summary>
    /// <param name="printersOnServer"></param>
    /// <returns></returns>
    private List<string> stripFloorNumbers(List<string> printersOnServer)
    {
        List<String> temp = new List<String>();
        foreach ( String printer in printersOnServer )
        {
            temp.Add(printer.Substring(0, printer.IndexOf('-')));
        }

        temp.Sort();
        temp = temp.Distinct().ToList<String>();

        return temp;
    }

    protected void ddFloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddPrinter.Items.Clear();
        string floor = ddFloor.SelectedValue.ToString();
        DropDownList list = (DropDownList)sender;
        string value = (string)list.SelectedValue;
        foreach ( string printer in printersOnServer )
        {
            if ( floor.Contains( "K" ) )
            {
                if ( printer.Substring( 0, 3 ).Contains( floor ) )
                {
                    ddPrinter.Items.Add( printer );
                }
            }
            else
            {
                if ( printer.Substring( 0, 2 ).Contains( floor ) && !printer.Substring( 0, 3 ).Contains( "K" ) )
                {
                    ddPrinter.Items.Add( printer );
                }
            }
        }
    }
    protected void btnInstallPrinter_Click(object sender, EventArgs e)
    {
        string client = System.Net.Dns.GetHostEntry( Request.ServerVariables["remote_addr"] ).HostName;
        Printer.installPrinter( client, ddPrinter.SelectedValue.ToString() );
    }
}