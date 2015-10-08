using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class App_Assets_Forms_TeleworkUpgradeChecklist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack )
        {
            new Thread( new ThreadStart( metrics ) ).Start();
            Bind();
        }
    }

    private void Bind()
    {
        SqlDataSource SqlDataSource1 = new SqlDataSource();
        SqlDataSource1.ID = "SqlDataSource1";
        this.Page.Controls.Add( SqlDataSource1 );
        SqlDataSource1.ConnectionString = "Data Source=W8-RKOEN;Initial Catalog=Telework;User ID=sa;Password=p@ssw0rd";
        SqlDataSource1.SelectCommand = "SELECT * FROM Main Order By LNAME, FName";
        GridView1.DataSource = SqlDataSource1;
        GridView1.DataBind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int Computer = 0, Monitor = 0, SurgeProtector = 0, PowerCord = 0, Headset = 0, Mouse = 0, StandardKeyboard = 0, Speakers = 0, NetworkCables = 0, WifiAdapter = 0, NaturalErgoKeyboard = 0;

        foreach ( ListItem cb in CheckBoxList1.Items )
        {
            switch ( cb.Text )
            {
                case "Computer":
                    if ( cb.Selected )
                    {
                        Computer = 1;
                    }
                    break;

                case "Monitor":
                    if ( cb.Selected )
                    {
                        Monitor = 1;
                    }
                    break;

                case "Surge Protector":
                    if ( cb.Selected )
                    {
                        SurgeProtector = 1;
                    }
                    break;

                case "Power Cords (2)":
                    if ( cb.Selected )
                    {
                        PowerCord = 1;
                    }
                    break;

                case "Lync Headset":
                    if ( cb.Selected )
                    {
                        Headset = 1;
                    }
                    break;

                case "Mouse":
                    if ( cb.Selected )
                    {
                        Mouse = 1;
                    }
                    break;

                case "Standard Keyboard":
                    if ( cb.Selected )
                    {
                        StandardKeyboard = 1;
                    }
                    break;

                case "Audio Speakers":
                    if ( cb.Selected )
                    {
                        Speakers = 1;
                    }
                    break;

                case "Network Cables":
                    if ( cb.Selected )
                    {
                        NetworkCables = 1;
                    }
                    break;

                case "Natural Ergo Keyboard":
                    if ( cb.Selected )
                    {
                        NaturalErgoKeyboard = 1;
                    }
                    break;

                case "Wifi Adapter":
                    if ( cb.Selected )
                    {
                        WifiAdapter = 1;
                    }
                    break;
            }
        }

        using ( SqlConnection connection = new SqlConnection( "Data Source=W8-RKOEN;Initial Catalog=Telework;User ID=sa;Password=p@ssw0rd" ) )
        {
            try
            {
                connection.Open();
                try
                {
                    using ( SqlCommand command = new SqlCommand( "INSERT INTO Main VALUES(@TicketNumber, @FName, @LName, @ComputerName," +
                                                                 "@Computer, @Monitor, @SurgeProtector, @PowerCord, @Headset, @Mouse, @StandardKeyboard,  @NaturalErgoKeyboard, " +
                                                                 "@Speakers, @NetworkCables, @WifiAdapter)", connection ) )
                    {
                        command.Parameters.Add( new SqlParameter( "TicketNumber", Convert.ToInt32( TxtTNumber.Text ) ) );
                        command.Parameters.Add( new SqlParameter( "FName", txtFName.Text ) );
                        command.Parameters.Add( new SqlParameter( "LName", txtLName.Text ) );
                        command.Parameters.Add( new SqlParameter( "ComputerName", generateComputerName() ) );
                        command.Parameters.Add( new SqlParameter( "Computer", Computer ) );
                        command.Parameters.Add( new SqlParameter( "Monitor", Monitor ) );
                        command.Parameters.Add( new SqlParameter( "SurgeProtector", SurgeProtector ) );
                        command.Parameters.Add( new SqlParameter( "PowerCord", PowerCord ) );
                        command.Parameters.Add( new SqlParameter( "Headset", Headset ) );
                        command.Parameters.Add( new SqlParameter( "Mouse", Mouse ) );
                        command.Parameters.Add( new SqlParameter( "StandardKeyboard", StandardKeyboard ) );
                        command.Parameters.Add( new SqlParameter( "NaturalErgoKeyboard", NaturalErgoKeyboard ) );
                        command.Parameters.Add( new SqlParameter( "Speakers", Speakers ) );
                        command.Parameters.Add( new SqlParameter( "NetworkCables", NetworkCables ) );
                        command.Parameters.Add( new SqlParameter( "WifiAdapter", WifiAdapter ) );

                        command.ExecuteNonQuery();
                    }
                }
                catch ( Exception )
                { }
            }
            catch ( Exception )
            {
            }
        }

        ScriptManager.RegisterClientScriptBlock( this.Page, this.Page.GetType(), "alert", "alert('Telework Information Successfully Submitted');", true );
        Response.Redirect( Request.RawUrl );
    }

    private string generateComputerName()
    {
        string name;
        string fInitial = txtFName.Text.ElementAt( 0 ).ToString();
        int LNameSize = txtLName.Text.Length;
        string prefix = "HOME-";

        if ( LNameSize <= 7 )
        {
            name = prefix + fInitial + txtLName.Text;
        }
        else
        {
            name = prefix + fInitial + txtLName.Text.Substring( 0, 7 );
        }

        return name;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        //Session["ViewState"] = null;
        //Response.Redirect( "http://w8-rkoen/App_Assets/Forms/TeleworkUpgradeChecklist.aspx" );
        foreach ( ListItem cb in CheckBoxList1.Items )
        {
            cb.Selected = false;
        }
        txtFName.Text = "";
        txtLName.Text = "";
        TxtTNumber.Text = "";
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = GridView1.SelectedRow;
        txtTicketNoPopUp.Text = row.Cells[1].Text;
        txtFNamePopUp.Text = row.Cells[2].Text;
        txtLNamePopUp.Text = row.Cells[3].Text;
        CheckBox chk;
        foreach ( ListItem cb in CheckBoxList2.Items )
        {
            switch ( cb.Text )
            {
                case "Computer":
                    chk = row.Cells[5].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;
                    break;

                case "Monitor":
                    chk = row.Cells[6].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;

                    break;

                case "Surge Protector":

                    chk = row.Cells[7].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;

                    break;

                case "Power Cords (2)":

                    chk = row.Cells[8].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;

                    break;

                case "Lync Headset":

                    chk = row.Cells[9].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;

                    break;

                case "Mouse":

                    chk = row.Cells[10].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;

                    break;

                case "Standard Keyboard":

                    chk = row.Cells[11].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;

                    break;

                case "Audio Speakers":

                    chk = row.Cells[12].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;

                    break;

                case "Network Cables":

                    chk = row.Cells[13].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;

                    break;

                case "Natural Ergo Keyboard":

                    chk = row.Cells[14].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;

                    break;

                case "Wifi Adapter":

                    chk = row.Cells[15].Controls[0] as CheckBox;
                    cb.Selected = chk.Checked;

                    break;
            }
        }
        mpe.Show();
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string fName, lName;
        GridViewRow row = GridView1.Rows[e.RowIndex];
        fName = row.Cells[2].Text;
        lName = row.Cells[3].Text;
        string SQLCmd = "DELETE FROM Main WHERE fname='" + fName + "' AND lname='" + lName + "';";
        using ( SqlConnection connection = new SqlConnection( "Data Source=W8-RKOEN;Initial Catalog=Telework;User ID=sa;Password=p@ssw0rd" ) )
        {
            try
            {
                connection.Open();
                using ( SqlCommand command = new SqlCommand( SQLCmd, connection ) )
                {
                    command.ExecuteNonQuery();
                }
            }
            catch ( Exception ) { }
        }
        Response.Redirect( Request.RawUrl );
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
                        using ( SqlCommand command = new SqlCommand( "INSERT INTO TeleworkChecklist VALUES(@ComputerName, @Time)", connection ) )
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
}