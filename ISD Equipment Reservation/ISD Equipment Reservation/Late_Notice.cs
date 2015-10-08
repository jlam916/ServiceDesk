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
    public partial class Late_Notice : Form
    {
        Form1 ToLateNoticeForm;
        public Late_Notice(Form1 frmMainForm)
        {
            InitializeComponent();
            ToLateNoticeForm = frmMainForm;
        }

        private void Late_Notice_Load(object sender, EventArgs e)
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



            // Send Outlook Email Notification
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 


                oMsg.HTMLBody = "Hello" + " " + RequesterName_txtBox.Text + ","
                    + "<br /> <br /> Our records indicate that you still have the following equipment" + " " + "<strong>Check-Out</strong>."
                    + "<br /> <br /><strong><u><i> Equipment Check-Out Information: </strong></u></i>"
                    + "<br /><br /><strong> Requester Name: </strong>" + " " + RequesterName_txtBox.Text
                    + "<br /><strong> Requested Check-Out Date: </strong>" + " " + PickUpDate_txtBox.Text + " " + "to" + " " + ReturnDate_txtBox.Text
                    + "<br /><strong> Equipment Check-Out: </strong>" + " " + EquipmentRequested_txtBox.Text
                    + "<br /><strong> Service Request Number: </strong>" + " " + ServiceRequestNumber_txtBox.Text
                    + "<br /><br />If you still have this equipment, please return it immediately.  If this information is incorrect or if you need to extend the checkout duration of this equipment, please contact ServiceDesk at 653-3329 or ServiceDesk@dds.ca.gov."
                    + "<br /><br />Thank You"
                    + "<br /><br /><br /><strong><big> Service Desk</strong></big>"
                    + "<br /><strong><big> Department of Developmental Services</strong></big>"
                    + "<br /><strong><big> Information Services Division | Service Desk</strong></big>"
                    + "<br /><strong><big> W: (916) 653-3329 |</strong></big>"
                    + "<br /><strong><big> servicedesk@dds.ca.gov</strong></big>";
                //Subject line
                oMsg.Subject = ("SR" + " " + ServiceRequestNumber_txtBox.Text + " " + "--" + " " + "ISD Equipment Check-Out Reminder !!");
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
                    MessageBox.Show("A late notice has been sent to the requester");

                }
            }//end of try block
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }//end of catch

            //Refresh the DataGridview 
            ToLateNoticeForm.tabControl1.SelectedIndex = ToLateNoticeForm.tabControl1.SelectedIndex = (ToLateNoticeForm.tabControl1.SelectedIndex + 1 < ToLateNoticeForm.tabControl1.TabCount) ?
                             ToLateNoticeForm.tabControl1.SelectedIndex + 1 : ToLateNoticeForm.tabControl1.SelectedIndex;

            ToLateNoticeForm.tabControl1.SelectedIndex = ToLateNoticeForm.tabControl1.SelectedIndex = (ToLateNoticeForm.tabControl1.SelectedIndex - 1 < ToLateNoticeForm.tabControl1.TabCount) ?
                             ToLateNoticeForm.tabControl1.SelectedIndex - 1 : ToLateNoticeForm.tabControl1.SelectedIndex;


            //Close Application
            this.Close();
        }


    }
}
