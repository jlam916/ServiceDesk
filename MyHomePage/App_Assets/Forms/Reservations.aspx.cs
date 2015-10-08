using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;



public partial class App_Assets_Forms_Reservations : System.Web.UI.Page
{
	private ListItemCollection epaRoomNums = new ListItemCollection();
	private ListItemCollection kRoomNums = new ListItemCollection();
	private ListItemCollection epaTrainNums = new ListItemCollection();

    // error when using ConfigManager...
    //private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["presentation"].ConnectionString;
    private static string connectionString = "Data Source=w8-rkoen;Initial Catalog=Presentation;User ID=sa;Password=p@ssw0rd";

	private string userEmail = "";
    private static string[] AD_Users;

	protected void Page_Load(object sender, EventArgs e)
	{
        if ( !IsPostBack )
        {
            fname.Attributes.Add( "readonly", "readonly" );
            lname.Attributes.Add( "readonly", "readonly" );

            email.Focus();
            populate();

            foreach ( ListItem room in epaRoomNums )
            {
                epaRooms.Items.Add( room );
            }
            foreach ( ListItem room in kRoomNums )
            {
                kRooms.Items.Add( room );
            }
            foreach ( ListItem room in epaTrainNums )
            {
                epaTraining.Items.Add( room );
            }
            
            using ( PrincipalContext pc = new PrincipalContext( System.DirectoryServices.AccountManagement.ContextType.Domain, "ITSERVICES" ) )
            {
                using ( UserPrincipal user = new UserPrincipal( pc ) )
                {
                    user.EmailAddress = "*";
                    using ( PrincipalSearcher ps = new PrincipalSearcher() )
                    {
                        ps.QueryFilter = user;
                        ((DirectorySearcher)ps.GetUnderlyingSearcher()).PageSize = 500;
                        PrincipalSearchResult<Principal> psr = ps.FindAll();
                        AD_Users = new string[psr.Count()];

                        int i = 0;
                        foreach ( UserPrincipal u in psr )
                        {
                            AD_Users[i++] = u.EmailAddress.Split('@')[0];
                        }
                    }
                }
            }

           //Debug_FillForm();
        }
	}

	private void Debug_FillForm()
	{
		email.Text = "reese.freeman";
		fname.Text = "Reese";
		lname.Text = "Freeman";
		ticket.Text = "12345";
		startTime.Text = "12:30 PM";
		endTime.Text = "1:30 PM";
		presRadio.SelectedIndex = 1;
		building.SelectedValue = "epa";
		epaRooms.SelectedIndex = 1;
		equipment.SelectedIndex = 0;
		networkReq.SelectedIndex = 1;
		commentBox.Text = "THIS IS" + Environment.NewLine + "A TEST";
	}

	private void populate()
	{
		// Add EPA Rooms
		epaRoomNums.Add( "110" );
		epaRoomNums.Add( "220" );
		epaRoomNums.Add( "230" );
		epaRoomNums.Add( "240" );
		epaRoomNums.Add( "310" );
		epaRoomNums.Add( "320" );
		epaRoomNums.Add( "330" );
		epaRoomNums.Add( "340" );
		epaRoomNums.Add( "350" );
		epaRoomNums.Add( "410" );
		epaRoomNums.Add( "430" );
		epaRoomNums.Add( "440" );
		epaRoomNums.Add( "450" );
		epaRoomNums.Add( "510" );
		epaRoomNums.Add( "520" );
		epaRoomNums.Add( "550" );
		epaRoomNums.Add( "560" );
		epaRoomNums.Add( "570" );
		epaRoomNums.Add( "610" );
		epaRoomNums.Add( "620" );
		epaRoomNums.Add( "710" );
		epaRoomNums.Add( "720" );
		epaRoomNums.Add( "810" );
		epaRoomNums.Add( "910" );
		epaRoomNums.Add( "1010" );
		epaRoomNums.Add( "1110" );
		epaRoomNums.Add( "1210" );
		epaRoomNums.Add( "1310" );
		epaRoomNums.Add( "1410" );
		epaRoomNums.Add( "1510" );
		epaRoomNums.Add( "1520" );
		epaRoomNums.Add( "1530" );
		epaRoomNums.Add( "1540" );
		epaRoomNums.Add( "1610" );
		epaRoomNums.Add( "1630" );
		epaRoomNums.Add( "1710" );
		epaRoomNums.Add( "1810" );
		epaRoomNums.Add( "1910" );
		epaRoomNums.Add( "2010" );
		epaRoomNums.Add( "2020" );
		epaRoomNums.Add( "2110" );
		epaRoomNums.Add( "2210" );
		epaRoomNums.Add( "2310" );
		epaRoomNums.Add( "2320" );
		epaRoomNums.Add( "2410" );
		epaRoomNums.Add( "2420" );
		epaRoomNums.Add( "2510" );
		epaRoomNums.Add( "2520" );
		epaRoomNums.Add( "2530" );
		epaRoomNums.Add( "2540" );
		epaRoomNums.Add( "2550" );

		// Add K St. Rooms
		kRoomNums.Add( "1502" );
        kRoomNums.Add("1702 (Daylight Room)");
		kRoomNums.Add( "1720" );
		kRoomNums.Add( "1902 (Sue Case)" );
		kRoomNums.Add( "1917" );

		// Add EPA Training Rooms
		epaTrainNums.Add( "1 East" );
		epaTrainNums.Add( "1 West" );
		epaTrainNums.Add( "1 East/West" );
		epaTrainNums.Add( "2 East" );
		epaTrainNums.Add( "2 West" );
		epaTrainNums.Add( "2 East/West" );
		epaTrainNums.Add( "Training 5" );
		epaTrainNums.Add( "8 North" );
		epaTrainNums.Add( "8 South" );
		epaTrainNums.Add( "Training 18" );
	}

	protected void resetBtn_Click(object sender, EventArgs e)
	{
        Page.Response.Redirect( Page.Request.Url.ToString(), true );
	}

    private int getPresentationNumber(DateTime start, DateTime end)
    {
        int presID = -1;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
   
                //@ makes it sqlstatement a variable
            //added "order by newid()
                string sqlquerystatement = @"Select Top 1 * FROM Presentation p WHERE p.ID NOT IN (            
            Select Presentation_ID FROM Reservation r WHERE (@startTime BETWEEN r.Start_Date AND r.End_Date) OR
                                                            (@endTime BETWEEN r.Start_Date AND r.End_Date) OR
                                                            (r.Start_Date BETWEEN @startTime AND @endTime) OR
                                                            (r.End_Date BETWEEN @startTime AND @endTime)) ORDER BY NEWID()";
            
           SqlCommand command = new SqlCommand(sqlquerystatement, connection);
            //"startTime" and "endTime" is the variable in the query
            command.Parameters.Add(new SqlParameter("startTime", start.AddMinutes(-30)));
            command.Parameters.Add(new SqlParameter("endTime", end.AddMinutes(30)));
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                presID = reader.GetInt32(0);
                
            }

            reader.Close();
            connection.Close();
        }

        return presID;
    }

    private void cleanup()
    {
             using (SqlConnection connection = new SqlConnection(connectionString))
             {
                 DateTime todate = DateTime.Now;
                 //we are casting date in the sql to remove the time aspect of it cause of "as date"
                 string sqlquerystatement = @"DELETE 
                                            FROM Reservation
                                            WHERE End_Date < cast(@currentDate as date)";
                 SqlCommand command = new SqlCommand(sqlquerystatement, connection);
                 // instead of putting getdate() method, we put a parameter (DateTime.Now) which does it for us and puts the date that is pulled into a variable which in this case will be put into currentDate
                 command.Parameters.Add(new SqlParameter("currentDate", DateTime.Now));
                 connection.Open();
                 //cause we are not bringing back anyresults. command not connection cause command you are actually doing it.
                 command.ExecuteNonQuery();
                 connection.Close();

             }
             }


	protected void submitBtn_Click(object sender, EventArgs e)
    {
        cleanup();
		string date = "", startTime = "", endTime = "", room = "", body = "", sVal = "";
		string firstName = fname.Text;
        try
        {
           
            getInfo( ref date, ref startTime, ref endTime, ref room, ref body, ref sVal);
            String presName = String.Empty;

            
            if (sVal.Equals("801k", StringComparison.OrdinalIgnoreCase))
            //801k presname is not in the table cause then we would get it mixed up when we pull a random when calepa is selected as building        
            {
                presName = "801 K Pres";
                lblSelectedPres.Text = presName + " was selected for setup.";
                lblSelectedPres.Visible = true;
            }
            else if ( presRadio.SelectedIndex == 0 )
            {
                // reserve presentation package
                DateTime start = DateTime.Parse( date + " " + startTime );
                DateTime end = DateTime.Parse( date + " " + endTime );
                int presID = getPresentationNumber( start, end );

                if ( presID == -1 )
                {
                    // no available presentation package
                    ScriptManager.RegisterStartupScript( this.Page, Page.GetType(), "showAlert", @"<script>document.getElementById('infoAlert').style.display = ""block"";</script>", false );
                    return;
                }
                reserve( start, end, presID );
                presName = "Pres 0" + presID;
                radiobttnchk( presName );
                lblSelectedPres.Text = presName + " was selected for setup.";
                lblSelectedPres.Visible = true;
            }

            ExchangeService service = new ExchangeService();
            service.Credentials = new WebCredentials("HelpCenter@calrecycle.ca.gov", "helpIT2013");
            service.Url = new Uri("https://mail.calrecycle.ca.gov/EWS/Exchange.asmx");

            // Setup Appointment
            Appointment setup = new Appointment(service);
            setup.Subject = "Setup " + presName;
            setup.Body = body;
            setup.Start = DateTime.Parse(date + " " + startTime).AddMinutes(-30);
            setup.End = DateTime.Parse(date + " " + startTime);
            setup.Location = room;
            setup.RequiredAttendees.Add("Bob.Davila@calrecycle.ca.gov");
            //setup.Save();

            // Takedown Appintement
            Appointment takedown = new Appointment(service);
            takedown.Subject = "Takedown " + presName;
            takedown.Body = body;
            takedown.Start = DateTime.Parse(date + " " + endTime);
            takedown.End = DateTime.Parse(date + " " + endTime).AddMinutes(30);
            takedown.Location = room;
            takedown.RequiredAttendees.Add("Bob.Davila@calrecycle.ca.gov");
           // takedown.Save();

            //Send notice to requestor
            Message message = new Message();
            message.setTo(userEmail);
            message.Subject = "Meeting Reservation Confirmation. Ticket #" + ticket.Text;
            #region Email Body
            message.Body = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\" xmlns=\"http://www.w3.org/TR/REC-html40\"><head><meta http-equiv=Content-Type content=\"text/html; charset=us-ascii\"><meta name=Generator content=\"Microsoft Word 15 (filtered medium)\">" +
                          @"<!--[if !mso]><style>v\:* {behavior:url(#default#VML);}
o\:* {behavior:url(#default#VML);}
w\:* {behavior:url(#default#VML);}
.shape {behavior:url(#default#VML);}
</style><![endif]--><style><!--
@font-face
	{font-family:""Cambria Math"";
	panose-1:2 4 5 3 5 4 6 3 2 4;}
@font-face
	{font-family:Calibri;
	panose-1:2 15 5 2 2 2 4 3 2 4;}
/* Style Definitions */
p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:""Calibri"",sans-serif;}
a:link, span.MsoHyperlink
	{mso-style-priority:99;
	color:#0563C1;
	text-decoration:underline;}
a:visited, span.MsoHyperlinkFollowed
	{mso-style-priority:99;
	color:#954F72;
	text-decoration:underline;}
span.EmailStyle17
	{mso-style-type:personal-compose;
	font-family:""Calibri"",sans-serif;
	color:windowtext;}
.MsoChpDefault
	{mso-style-type:export-only;
	font-size:10.0pt;
	font-family:""Calibri"",sans-serif;}
@page WordSection1
	{size:8.5in 11.0in;
	margin:1.0in 1.0in 1.0in 1.0in;}
div.WordSection1
	{page:WordSection1;}
--></style><!--[if gte mso 9]><xml>
<o:shapedefaults v:ext=""edit"" spidmax=""1027"" />
</xml><![endif]--><!--[if gte mso 9]><xml>
<o:shapelayout v:ext=""edit"">
<o:idmap v:ext=""edit"" data=""1"" />
</o:shapelayout></xml><![endif]-->

</head>
	<body lang=EN-US link=""#0563C1"" vlink=""#954F72"">
		<div class=WordSection1>
			<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 style='border-collapse:collapse'>
				<tr>
					<td width=150 colspan=2 valign=top style='width:50.7pt;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal align=center style='text-align:center'>
							<img width=160 height=75 id=""CalRecycle Logo"" src=""S:\CIWMB-INFOTECH\HelpCenter\RKOEN\ResponseTemplates\Reservations\av.png"" alt=""CalRecycle""><o:p></o:p>
						</p>
					</td>
					<td width=150 colspan=1 valign=top style='width:50.7pt;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal align=center style='text-align:center'>
							<img width=180 height=70 id=""CalRecycle Logo"" src=""S:\CIWMB-INFOTECH\HelpCenter\RKOEN\ResponseTemplates\Reservations\calrecycle.gif"" alt=""CalRecycle""><o:p></o:p>
						</p>
					</td>
					<td width=150 colspan=2 valign=top style='width:50.7pt;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal align=center style='text-align:center'>
							<img width=160 height=75 id=""CalRecycle Logo"" src=""S:\CIWMB-INFOTECH\HelpCenter\RKOEN\ResponseTemplates\Reservations\support.png"" alt=""CalRecycle""><o:p></o:p>
						</p>
					</td>
				</tr>
				<tr>
					<td width=23 valign=top style='width:17.6pt;background:#A8D08D;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
					<td width=533 colspan=3 valign=top style='width:399.65pt;background:#A8D08D;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
					<td width=23 valign=top style='width:17.45pt;background:#A8D08D;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
				</tr>
				<tr>
					<td width=23 valign=top style='width:17.6pt;background:#A8D08D;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
					<td width=533 colspan=3 valign=top style='width:399.65pt;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal>Hello " + Request.Form["fname"].ToString() + @",<o:p></o:p></p>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
						<p class=MsoNormal>Your meeting reservation has been created with the following information:<o:p></o:p></p>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
						<p class=MsoNormal><b><u>Date:</b></u> " + date + @"<br><b><u>Room:</b></u> " + room + @"<br><b><u>Time:</b></u> " + startTime + " - " + endTime +
                        @"<br><br><b><u>Comments</b></u><br>" + commentBox.Text.Replace(System.Environment.NewLine, @"<br />") + @"<o:p></o:p></p>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
						<p class=MsoNormal>If you have any questions or concerns, please contact the <a href=""tel:+19163416141""><span style='color:#0563C1'>Help Center</span></a>.<o:p></o:p></p>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
						<p class=MsoNormal>Thank you,<br>IT Services<br>CalRecycle Help Center<o:p></o:p></p>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
					<td width=23 valign=top style='width:17.45pt;background:#A8D08D;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
				</tr>
				<tr>
					<td width=23 valign=top style='width:17.6pt;background:#A8D08D;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
					<td width=533 colspan=3 valign=top style='width:399.65pt;background:#A8D08D;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
					<td width=23 valign=top style='width:17.45pt;background:#A8D08D;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
				</tr>
				<tr>
					<td width=23 valign=top style='width:17.6pt;border-top:none;border-left:solid windowtext 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:none;background:white;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
					<td width=146 valign=top style='width:109.65pt;border:none;border-bottom:solid windowtext 1.0pt;background:white;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal align=center style='text-align:center'>
						<a href=""http://epanet.ca.gov/Rooms/"" title=""Link to Agency Reservation Management System"">
							<img border=0 width=71 height=48 id=""ARMS"" src=""S:\CIWMB-INFOTECH\HelpCenter\RKOEN\ResponseTemplates\Reservations\ARMS.png"" alt=""Agency Reservations Mangagement System"">
						</a>
						</p>
					</td>
					<td width=240 valign=top style='width:180.3pt;border:none;border-bottom:solid windowtext 1.0pt;background:white;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal align=center style='text-align:center'>
						<a href=""http://home.calrecycle.net/Forms/Forms%20Library/CalRecycle731.pdf"" title=""Link to Goto Meeting form"">
							<img border=0 width=150 height=58 id=""GOTO"" src=""S:\CIWMB-INFOTECH\HelpCenter\RKOEN\ResponseTemplates\Reservations\GoToMeetingLogo.png"" alt=""GoToMeeting/Webinar"">
						</a>
						</p>
					</td>
					<td width=146 valign=top style='width:109.7pt;border:none;border-bottom:solid windowtext 1.0pt;background:white;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal align=center style='text-align:center'>
						<a href=""mailto:HelpCenter@calrecycle.ca.gov?subject=Hardware Setup Request&body=Hello,%0D%0DI would like to request a setup for <Insert Date Here>.%0D%0DRoom: <Insert Room Here>%0DStart Time: <Start Time>%0DEnd Time: <End Time>%0DEquipment: Laptop, Projector <etc>%0D%0DThanks"">
							<img border=0 width=118 height=58 id=""Setup"" src=""S:\CIWMB-INFOTECH\HelpCenter\RKOEN\ResponseTemplates\Reservations\Schedule.png"" alt=""Schedule a setup.""><o:p></o:p>
						</a>
						</p>
					</td>
					<td width=23 valign=top style='width:17.45pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;background:white;padding:0in 5.4pt 0in 5.4pt'>
						<p class=MsoNormal><o:p>&nbsp;</o:p></p>
					</td>
				</tr>
			</table>
		</div>
	</body>
</html>";
#endregion
            ScriptManager.RegisterStartupScript( this.Page, Page.GetType(), "showAlert", @"<script>document.getElementById('successAlert').style.display = ""block"";</script>", false );
        }
        catch ( Exception ex )
        {
            (new Message()).sendException(ex);
            ScriptManager.RegisterStartupScript( this.Page, Page.GetType(), "showAlert", @"<script>document.getElementById('dangerAlert').style.display = ""block"";</script>", false );
            
        }

	}

    private void radiobttnchk(string presName)
    {
        //presRadio.Text = "";
        foreach (ListItem rad in presRadio.Items)
        {
            //rad.Selected == false;
            //equals is to compare strings
            if (rad.Value.Equals(presName))
            {
                presRadio.SelectedValue = presName;
            }
            
        }

    }

    void reserve(DateTime Start, DateTime End, int presID)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string insertsqlstatement = @"INSERT INTO Reservation
                                                VALUES(@startTime, @endTime, @presID, @timestamp)";

            SqlCommand command = new SqlCommand(insertsqlstatement, connection);
            //after variable is passed, it comes straight here and gives it a mandatory parameters if its a variable, which assigns the values that were passed into the sql statement
            command.Parameters.Add(new SqlParameter("startTime", Start.AddMinutes(-30)));
            command.Parameters.Add(new SqlParameter("endTime", End.AddMinutes(30)));
            command.Parameters.Add(new SqlParameter("presID", presID));
            command.Parameters.Add(new SqlParameter("timestamp", DateTime.Now));

            connection.Open();

            command.ExecuteNonQuery();
            connection.Close();
            
        }

   }

	private void getInfo(ref string date, ref string ddstartTime, ref string ddendTime, ref string room, ref string body, ref string sVal)
	{
		string DOMAIN_NAME = "ITSERVICES";
		DirectoryEntry dirEntry = new DirectoryEntry( "LDAP://" + DOMAIN_NAME );
		DirectorySearcher dirSearcher = new DirectorySearcher( dirEntry );
		dirSearcher.Filter = "(mail=" + email.Text + "@calrecycle.ca.gov)";
		SearchResult result = dirSearcher.FindOne();
		DirectoryEntry person = result.GetDirectoryEntry();
		string managerName = "", department = "";
		string name = person.Properties["givenName"].Value.ToString() + " " + person.Properties["sn"].Value.ToString();

		string location = person.Properties["physicalDeliveryOfficeName"].Value.ToString();
		string phone = person.Properties["telephoneNumber"].Value.ToString();
		try
		{
			 department = person.Properties["department"].Value.ToString();
		}
		catch { }
		try
		{
			string manager = person.Properties["manager"].Value.ToString();
			string firstManagerName = manager.Substring( manager.IndexOf( "," ) + 2, manager.IndexOf( ",", manager.IndexOf( "," ) ) - 4 );
			string lastManagerName = manager.Substring( 3, manager.IndexOf( "," ) - 4 );
			managerName = firstManagerName + " " + lastManagerName;
		}
		catch
		{
			managerName = "Not Found";
		}
		userEmail = email.Text + "@calrecycle.ca.gov";
		date = calBtn.Text;
		ddstartTime = startTime.Text;
		ddendTime = endTime.Text;

        if (!epaConf.SelectedValue.Equals(""))
        {
            room = epaConf.Text;
        }
        else if (!kTraining.SelectedValue.Equals(""))
        {
            room = kTraining.Text;
            sVal = "801k";
        }
        else if (!epaTraining.SelectedValue.Equals(""))
        {
            room = epaTraining.Text;
        }
        else if (!epaRooms.SelectedValue.Equals(""))
        {
            room = epaRooms.Text;
        }
        else if (!kRooms.SelectedValue.Equals(""))
        {
            room = kRooms.Text;
            sVal = "801k";
        }
            
        

		string link = @"http://epanet.ca.gov/Rooms/RoomDetail.asp?REFERER2=MyMtg.asp&ROOMID=" + getRoomID( room ) + @"&DATE=" + DateTime.Now.ToString( "M/d/yyyy" );

        StringBuilder equipmentSelect = new StringBuilder();
        if ( equipment.Items[0].Selected )
            equipmentSelect.Append( "&emsp;&bull;&ensp;Laptop</br>" );
        if ( equipment.Items[1].Selected )
            equipmentSelect.Append( "&emsp;&bull;&ensp;Projector Screen</br>" );
        if ( equipment.Items[2].Selected )
            equipmentSelect.Append( "&emsp;&bull;&ensp;Projector</br>" );
        if ( equipment.Items[3].Selected )
            equipmentSelect.Append( "&emsp;&bull;&ensp;Speakers</br>" );
        if ( equipment.Items[4].Selected )
            equipmentSelect.Append( "&emsp;&bull;&ensp;Conference Phone</br>" );
        if ( equipment.Items[5].Selected )
            equipmentSelect.Append( "&emsp;&bull;&ensp;Other (see comments)</br>" );

        if ( networkReq.Items[0].Selected )
        {
            equipmentSelect.Append( "&emsp;&bull;&ensp;CalRecycle Network Access Required</br>" );
        }
        else
        {
            equipmentSelect.Append( "&emsp;&bull;&ensp;Wifi Access Required" );
        }

        body = "<b><u>Requestor Information</u></b></br>" +
                  "Name: " + name + ", " + department + "</br>" +
                  "Email: " + userEmail + "</br>" +
                  "Phone: " + phone + "</br>" +
                  "Location: " + location + "</br>" +
                  "Ticket Number: " + ticket.Text + "</br>" +
                  "<a href=" + link + ">Room Reservation</a></br></br>" +
                  "<b><u>Equipment Information</u></b></br>" + equipmentSelect.ToString() + "</br></br>" +
                  "<b><u>Comments</u></b></br>" + commentBox.Text.Replace( System.Environment.NewLine, "</br>" );

       // if (!kTraining.SelectedValue.Equals("") || !kRooms.SelectedValue.Equals("") )
           // sVal = "801k";
        

	}

	private string getRoomID(string room)
	{
		int roomID = 0;
		switch ( room )
		{
			case "110":
				roomID = 3;
				break;

			case "220":
				roomID = 13;
				break;

			case "230":
				roomID = 8;
				break;

			case "240":
				roomID = 9;
				break;

			case "310":
				roomID = 14;
				break;

			case "320":
				roomID = 15;
				break;

			case "330":
				roomID = 17;
				break;

			case "340":
				roomID = 19;
				break;

			case "350":
				roomID = 15;
				break;

			case "410":
				roomID = 20;
				break;

			case "430":
				roomID = 22;
				break;

			case "440":
				roomID = 23;
				break;

			case "450":
				roomID = 24;
				break;

			case "510":
				roomID = 25;
				break;

			case "520":
				roomID = 26;
				break;

			case "550":
				roomID = 29;
				break;

			case "560":
				roomID = 30;
				break;

			case "570":
				roomID = 31;
				break;

			case "610":
				roomID = 32;
				break;

			case "620":
				roomID = 70;
				break;

			case "710":
				roomID = 33;
				break;

			case "720":
				roomID = 67;
				break;

			case "810":
				roomID = 34;
				break;

			case "910":
				roomID = 37;
				break;

			case "1010":
				roomID = 38;
				break;

			case "1110":
				roomID = 39;
				break;

			case "1210":
				roomID = 40;
				break;

			case "1310":
				roomID = 41;
				break;

			case "1410":
				roomID = 42;
				break;

			case "1510":
				roomID = 43;
				break;

			case "1520":
				roomID = 44;
				break;

			case "1530":
				roomID = 45;
				break;

			case "1540":
				roomID = 47;
				break;

			case "1610":
				roomID = 46;
				break;

			case "1630":
				roomID = 48;
				break;

			case "1710":
				roomID = 49;
				break;

			case "1810":
				roomID = 50;
				break;

			case "1910":
				roomID = 53;
				break;

			case "2010":
				roomID = 54;
				break;

			case "2020":
				roomID = 55;
				break;

			case "2110":
				roomID = 56;
				break;

			case "2210":
				roomID = 57;
				break;

			case "2310":
				roomID = 58;
				break;

			case "2320":
				roomID = 59;
				break;

			case "2410":
				roomID = 60;
				break;

			case "2420":
				roomID = 61;
				break;

			case "2510":
				roomID = 62;
				break;

			case "2520":
				roomID = 65;
				break;

			case "2530":
				roomID = 64;
				break;

			case "2540":
				roomID = 63;
				break;

			case "2550":
				roomID = 69;
				break;

			case "1502":
				roomID = 80;
				break;

			case "1702 (Daylight Room)":
				roomID = 81;
				break;

			case "1720":
				roomID = 82;
				break;

			case "1902 (Sue Case)":
				roomID = 83;
				break;

			case "1917":
				roomID = 87;
				break;

			case "1919":
				roomID = 84;
				break;

			case "1 West":
				roomID = 2;
				break;

			case "1 East":
				roomID = 1;
				break;

			case "2 West":
				roomID = 11;
				break;

			case "2 East":
				roomID = 10;
				break;

			case "Byron Sher":
				roomID = 5;
				break;

			case "Coastal":
				roomID = 7;
				break;

			case "Klamath":
				roomID = 79;
				break;

			case "Sierra":
				roomID = 6;
				break;

			case "Training 5":
				roomID = 27;
				break;

			case "8 North":
				roomID = 35;
				break;

			case "8 South":
				roomID = 36;
				break;

			case "Training 18":
				roomID = 52;
				break;
		}
		return roomID.ToString();
	}

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        return (from u in AD_Users where u.StartsWith( prefixText, StringComparison.CurrentCultureIgnoreCase ) select u).Take( count ).ToArray();
    }

   
}