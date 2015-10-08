using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;
using DirectorySearcher = System.DirectoryServices.DirectorySearcher;
using SearchResult = System.DirectoryServices.SearchResult;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.DirectoryServices;
using MySql.Data.MySqlClient;

namespace ISD_Equipment_Reservation
{
    public partial class Check_In : Form
    {
        Form1 ToCheckInForm;
        public Check_In(Form1 frmMainForm)
        {
            InitializeComponent();
            ToCheckInForm = frmMainForm;
        }

        private void Check_In_Load(object sender, EventArgs e)
        {
            // Get Current Date
            DateTime datetTime = DateTime.Today;
            this.Date_lbl.Text = datetTime.ToString("MM/dd/yyyy");

            // Get Current Technician Information
            UserPrincipal user = UserPrincipal.Current;
            string FirstName = user.GivenName;
            string LastName = user.Surname;
            string Email = user.EmailAddress;


            // Set Current Technician Information
            TechEmail_txtBox.Text = Email;
            TechName_txtBox.Text = FirstName + " " + LastName;
        }

        private void Submit_btn_Click(object sender, EventArgs e)
        {
            // Error Providers To Check Requester Information
            if (RequestedEmail_txtBox.Text == string.Empty)
            {
                errorProvider1.SetError(Submit_btn, "Requester Information Textbox Cannot Be Blank, Please Refresh The Data Table And Select The Data Again.");
                return;
            }

            {
                errorProvider1.Clear();
            }


             // Update Status of equipment to Check-Out
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "update database.isdequipments set Log= '" + this.RecordNumber_txtBox.Text + "',Status= '" + "Check-In" + "',Check_In_By= '" + this.TechName_txtBox.Text + "',Check_In_Date= '" + this.Date_lbl.Text + "' where Log='" + this.RecordNumber_txtBox.Text + "'; ";
            MySqlConnection conDatabase = new MySqlConnection(myConnection);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;

            try
            {
                conDatabase.Open();
                myReader = cmdDatabase.ExecuteReader();
                while (myReader.Read())
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Send Outlook Email Notification
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 


                oMsg.HTMLBody = "Hello" + " " + RequesterName_txtBox.Text + ","
                    + "<br /> <br /> The status of the following equipment has been change to" + " " + "<strong>Check-In. </strong>" 
                    + "<br /> <br /><strong><u><i> Equipment Check-In Information: </strong></u></i>"
                    + "<br /><br /><strong> Requester Name: </strong>" + " " + RequesterName_txtBox.Text
                    + "<br /><strong> Check-Out Date: </strong>" + " " + PickUpDate_txtBox.Text
                    + "<br /><strong> Return Date: </strong>" + " " + Date_lbl.Text
                    + "<br /><strong> Equipment Check-In: </strong>" + " " + EquipmentRequested_txtBox.Text
                    + "<br /><strong> Service Request Number: </strong>" + " " + ServiceRequestNumber_txtBox.Text
                    + "<br /><br />If this information is incorrect or if you did not check-in the above equipment, please contact ServiceDesk at 653-3329 or ServiceDesk@dds.ca.gov."
                    + "<br /><br />Thank You,"
                    + "<br /><br /><strong><big> Service Desk</strong></big>"
                    + "<br /><strong><big> Department of Developmental Services</strong></big>"
                    + "<br /><strong><big> Information Services Division | Service Desk</strong></big>"
                    + "<br /><strong><big> W: (916) 653-3329 |</strong></big>"
                    + "<br /><strong><big> servicedesk@dds.ca.gov</strong></big>";
                //Subject line
                oMsg.Subject = ("SR" + " " + ServiceRequestNumber_txtBox.Text + " " + "--" + " " + "ISD Equipment Check-In Confirmation !!");
                // Add a recipient
                oMsg.To = RequestedEmail_txtBox.Text;
                // Add a CC
                //oMsg.CC = ""
                // Send.
                ((Outlook._MailItem)oMsg).Send();
                // Clean up.
                oMsg = null;
                oApp = null;
                //Validate drop then list and if successful, Show Message

                {
                    MessageBox.Show("Equipment has been successfully check-in." + Environment.NewLine + "A check-in notice has been sent to the requester");

                }
            }//end of try block
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }//end of catch

            //Refresh the DataGridview
            ToCheckInForm.tabControl1.SelectedIndex = ToCheckInForm.tabControl1.SelectedIndex = (ToCheckInForm.tabControl1.SelectedIndex + 1 < ToCheckInForm.tabControl1.TabCount) ?
                             ToCheckInForm.tabControl1.SelectedIndex + 1 : ToCheckInForm.tabControl1.SelectedIndex;

            ToCheckInForm.tabControl1.SelectedIndex = ToCheckInForm.tabControl1.SelectedIndex = (ToCheckInForm.tabControl1.SelectedIndex - 1 < ToCheckInForm.tabControl1.TabCount) ?
                             ToCheckInForm.tabControl1.SelectedIndex - 1 : ToCheckInForm.tabControl1.SelectedIndex;
            

            //Close Application
            this.Close();
            }

        }
    }

