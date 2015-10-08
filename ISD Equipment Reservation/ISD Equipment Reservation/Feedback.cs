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
    public partial class Feedback : Form
    {
        Form1 MainForm;
        public Feedback(Form1 frm)
        {
            InitializeComponent();
            MainForm = frm;
        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Feedback_Load(object sender, EventArgs e)
        {
            // Get Current Technician Information
            UserPrincipal user = UserPrincipal.Current;
            string FirstName = user.GivenName;
            string LastName = user.Surname;
            string Email = user.EmailAddress;

            Email_txtBox.Text = Email;
            Name_txtBox.Text = FirstName + " " + LastName;
        }

        private void Send_btn_Click(object sender, EventArgs e)
        {
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                oMsg.HTMLBody = "Hello" + " " + "Jacky Lam,"
                    + "<br /> <br /> The following feedback has been left for you."
                    + "<br /> <br /><strong><u><i> Feedback Information: </strong></u></i>"
                    + "<br /><br /><strong> Employee Name: </strong>" + " " + Name_txtBox.Text
                    + "<br /><strong> Comments: </strong>" + " " + Message_richTextBox.Text
                    + "<br /><br /> Thank You"
                    + "<br />" + Name_txtBox.Text;
                //Subject line
                oMsg.Subject = ("You have received a feedback from" + " " + Name_txtBox.Text);
                // Add a recipient
                oMsg.To = "Jacky.lam@dds.ca.gov";
                // Add a CC
                //oMsg.CC = ""
                // Send.
                ((Outlook._MailItem)oMsg).Send();
                // Clean up.
                oMsg = null;
                oApp = null;
                //Validate drop then list and if successful, Show Message

                {
                    MessageBox.Show("Email has been sent successfully" + Environment.NewLine + "Thank You for your feedback !!");

                }
            }//end of try block
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }//end of catch

            //Close Application
            this.Close();
        }
    }
}
