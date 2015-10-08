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
    public partial class Reservation : Form
    {
        Form1 ths;


        public Reservation(Form1 frm)
        {
            InitializeComponent();
            ths = frm;

            CancelReservation_btn.Click += new EventHandler(CancelReservation_btn_Click);
            

        }



        private void Reservation_Load(object sender, EventArgs e)
        {
            
            // Grab Checkboxlist values from form DDS Equipment Reservation 
            // Set Requested Equipment Textbox with those values
            // EquipmentRequested_txtBox.Text = checkboxlistvalues;

            // Pull All Emails From AD In HQ/USERS OU
            DirectoryEntry entry = new DirectoryEntry("LDAP://OU=Users,OU=HQ,OU=Sites,OU=DDS,DC=dds,DC=local");
            DirectorySearcher dSearch = new DirectorySearcher(entry);
            dSearch.SearchScope = SearchScope.Subtree;
            dSearch.Filter = "(objectClass=user)";
            dSearch.PageSize = 1000;
            dSearch.SizeLimit = 0;
            foreach (SearchResult sResultSet in dSearch.FindAll())
            {

                if (sResultSet.Properties["mail"].Count > 0)
                    RequesterEmail_comboBox.Items.Add(sResultSet.Properties["mail"][0].ToString());
            }

            //Take Main Reservation Pick Up Dates and Return Dates
            PickUpDate_txtBox.Text = ths.MainFormPickUpDate_DateTimePicker.Text;
            ReturnDate_txtBox.Text = ths.MainFormReturnDate_DateTimePicker.Text;

            //Take Main Reservation Pick Up Time and Return Time
            PickUpTime_txtBox.Text = ths.PickUpTime_ComboBox.Text;
            ReturnTime_txtBox.Text = ths.ReturnTime_ComboBox.Text;

           //Take Main Equipment Requested
            EquipmentRequested_txtBox.Text = ths.RequestedEquipment_txtBox.Text;

            // Get Current Technician Information
            UserPrincipal user = UserPrincipal.Current;
            string FirstName = user.GivenName;
            string LastName = user.Surname;
            string Email = user.EmailAddress;
            string TelephoneNumber = user.VoiceTelephoneNumber;

            // Set Current Technician Information
            TechEmail_txtBox.Text = Email;
            TechFirstName_txtBox.Text = FirstName;
            TechLastName_txtBox.Text = LastName;
            TechPhoneNumber_txtBox.Text = TelephoneNumber;

        }

        private void Reservation_FormClosed(object sender, FormClosedEventArgs e)
        {
            //// Reset checkboxlist to uncheck state when form close
            Boolean state = false;
            {
                for (int i = 0; i < ths.Laptop_chkListBox.Items.Count; i++)
                    ths.Laptop_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }

            {
                for (int i = 0; i < ths.LTLaptop_chklistbox.Items.Count; i++)
                    ths.LTLaptop_chklistbox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }

            {
                for (int i = 0; i < ths.Projector_chkListBox.Items.Count; i++)
                    ths.Projector_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));

            }
            {
                for (int i = 0; i < ths.FlashDrive_chkListBox.Items.Count; i++)
                    ths.FlashDrive_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }
            {
                for (int i = 0; i < ths.Accessories_chkListBox.Items.Count; i++)
                    ths.Accessories_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }
            {
                for (int i = 0; i < ths.HeadPhones_chkListBox.Items.Count; i++)
                    ths.HeadPhones_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }
            EquipmentRequested_txtBox.Clear();
        }



        private void CancelReservation_btn_Click(object sender, EventArgs e)
        {
             //Reset checkboxlist to uncheck state when form close by cancelled button
            Boolean state = false;
            {
                for (int i = 0; i < ths.Laptop_chkListBox.Items.Count; i++)
                    ths.Laptop_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }

            {
                for (int i = 0; i < ths.LTLaptop_chklistbox.Items.Count; i++)
                    ths.LTLaptop_chklistbox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }

            {
                for (int i = 0; i < ths.Projector_chkListBox.Items.Count; i++)
                    ths.Projector_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
             
            }
            {
                for (int i = 0; i < ths.FlashDrive_chkListBox.Items.Count; i++)
                    ths.FlashDrive_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }
            {
                for (int i = 0; i < ths.Accessories_chkListBox.Items.Count; i++)
                    ths.Accessories_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }
            {
                for (int i = 0; i < ths.HeadPhones_chkListBox.Items.Count; i++)
                    ths.HeadPhones_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }

            EquipmentRequested_txtBox.Clear();
            this.Close();
        }

        private void Magnifying_btn_Click(object sender, EventArgs e)
        {
            // Get user first name and last name by email
            //string mail = RequesterEmail_comboBox.Text;
            //DirectoryEntry entry = new DirectoryEntry();
            //DirectorySearcher adsearcher = new DirectorySearcher(entry);
            //adsearcher.Filter = "(&(objectClass=user)(mail=" + mail + "))";
            //adsearcher.PropertiesToLoad.Add("givenName");
            //adsearcher.PropertiesToLoad.Add("sn");
            //adsearcher.PropertiesToLoad.Add("mail");
            //SearchResult result = adsearcher.FindOne();

            //if (result == null)
            //    MessageBox.Show("Email Does Not Exist !!" + Environment.NewLine + "Please Check Your Spelling !!");

            //if (result != null)
            //{
            //    DirectoryEntry employee = result.GetDirectoryEntry();
            //    string FirstName = employee.Properties["givenName"].Value.ToString();
            //    string LastName = employee.Properties["sn"].Value.ToString();

            //    RequesterFirstName_txtBox.Text = FirstName;
            //    RequesterLastName_txtBox.Text = LastName;
            //}
        }

        private void SubmitReserve_btn_Click(object sender, EventArgs e)
        {
            // Error Providers before submit
            if (RequesterEmail_comboBox.Text == string.Empty)
            {
                errorProvider1.SetError(RequesterEmail_comboBox, "Please Select An Employee Email From The Drop-Down List");
                return;
            }
            else
            {
                errorProvider1.Clear();
            }
            if (RequesterFirstName_txtBox.Text == string.Empty)
            {

                errorProvider2.SetError(RequesterFirstName_txtBox, "Please Select An Email And Click The Mangify Button");
                return;
            }
            else
            {
                errorProvider2.Clear();
                
            }
            if (ServiceRequestNumber_txtBox.Text == string.Empty)
            {

                errorProvider3.SetError(ServiceRequestNumber_txtBox, "Please Include A Service Request Number");
                return;
            }
            else
            {
                errorProvider3.Clear();

            }

            if (PickUpTime_txtBox.Text == string.Empty)
            {
                errorProvider4.SetIconAlignment(PickUpTime_txtBox, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
                errorProvider4.SetError(PickUpTime_txtBox, "Please Select A Pick-Up Time From THe Main Form");
                return;
            }
            else
            {
                errorProvider4.Clear();

            }
            if (ReturnTime_txtBox.Text == string.Empty)
            {
                errorProvider5.SetIconAlignment(ReturnTime_txtBox, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
                errorProvider5.SetError(ReturnTime_txtBox, "Please Select A Return Time From The Main Form");
                return;
            }
            else
            {
                errorProvider5.Clear();

            }
            if (EquipmentRequested_txtBox.Text == string.Empty)
            {
                errorProvider6.SetIconAlignment(EquipmentRequested_txtBox, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
                errorProvider6.SetError(EquipmentRequested_txtBox, "Please Select A Equipment Type From The Main Form");
                return;
            }
            else
            {
                errorProvider6.Clear();

            }
     

            // Establish a connection to mysql and insert data into database
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "insert into database.isdequipments (Status, SR_Number, Equipment_Requested, Requester_Email, Requester_Name, Reservation_PickUp_Time, Reservation_Return_Time, Reservation_PickUp_Date, Reservation_Return_Date, Reserved_By) values('" + "Reserved" + "','" + this.ServiceRequestNumber_txtBox.Text + "', '" + this.EquipmentRequested_txtBox.Text + "' , '" + this.RequesterEmail_comboBox.Text + "' , '" + this.RequesterFirstName_txtBox.Text + " " + this.RequesterLastName_txtBox.Text + "' , '" + this.PickUpTime_txtBox.Text + "' , '" + this.ReturnTime_txtBox.Text + "' , '" + this.PickUpDate_txtBox.Text + "' , '" + this.ReturnDate_txtBox.Text + "' , '" + this.TechFirstName_txtBox.Text + " " + this.TechLastName_txtBox.Text + "'); ";
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


             //Schedule Pick Up Of Laptop On Service Desk Calendar
            try
            {

                // Create the Outlook Application
                Outlook.Application oApp = new Outlook.Application();
                // Create NameSpace
                Outlook.NameSpace oNS = (Outlook.NameSpace)oApp.GetNamespace("MAPI");
                oNS.Logon(null, null, false, false);
                // Get DDS Service Desk Calendar
                Outlook.MAPIFolder oFolder;
                Outlook.Recipient oRecipient;
                string calendarName = "servicedesk@dds.ca.gov";


                // Create a new appointment item
                Outlook.AppointmentItem oMsg = (Outlook.AppointmentItem)oApp.CreateItem(Outlook.OlItemType.olAppointmentItem);
                // Set Recipients
                oRecipient = oNS.CreateRecipient(calendarName);
                oRecipient.Resolve();
                oFolder = oNS.GetSharedDefaultFolder(oRecipient, Outlook.OlDefaultFolders.olFolderCalendar);
                // Set Appointment Start Time and End Time
                oMsg = oFolder.Items.Add("IPM.Appointment") as Outlook.AppointmentItem;
                oMsg.Start = DateTime.Parse(PickUpDate_txtBox.Text + " " + PickUpTime_txtBox.Text);
                oMsg.End = DateTime.Parse(ReturnDate_txtBox.Text + " " + ReturnTime_txtBox.Text);
                //Set the Body
                string message = "The Below Is Information Regarding The IT Equipment Reservation" + Environment.NewLine;
                oMsg.Body = "The Below Is Information Regarding The IT Equipment Reservation" + Environment.NewLine + Environment.NewLine +
                    "Requester Information:" + Environment.NewLine +
                    "Email:" + " " + RequesterEmail_comboBox.Text + Environment.NewLine +
                    "First Name:" + " " + RequesterFirstName_txtBox.Text + Environment.NewLine +
                    "Last Name:" + " " + RequesterLastName_txtBox.Text + Environment.NewLine + Environment.NewLine +
                    "Reservation Information:" + Environment.NewLine +
                    "Pick-Up Time:" + " " + PickUpTime_txtBox.Text + Environment.NewLine + 
                    "Return Time:" + ReturnTime_txtBox.Text + Environment.NewLine +
                    "Pick-Up Date:" + " " + PickUpDate_txtBox.Text + Environment.NewLine + 
                    "Return Date:" + ReturnDate_txtBox.Text + Environment.NewLine +
                    "Service Request Number:" + " " + ServiceRequestNumber_txtBox.Text + Environment.NewLine + Environment.NewLine +
                    "Service Request Link:" + " " + "http://oasis.dds.ca.gov/applications/ISDRequests_Staff/index.cfm?pageAction=ShowDetail&RequestNum=" + ServiceRequestNumber_txtBox.Text + Environment.NewLine +
                    "Reserved By Technician:" + Environment.NewLine +
                    "Email:" + " " + TechEmail_txtBox.Text + Environment.NewLine +
                    "First Name:" + " " + TechFirstName_txtBox.Text + Environment.NewLine +
                    "Last Name:" + " " + TechLastName_txtBox.Text + Environment.NewLine +
                    "Telephone Number:" + " " + TechPhoneNumber_txtBox.Text + Environment.NewLine + Environment.NewLine +
                    "Comments" + Environment.NewLine +
                     Comments_richTextBox.Text;
                //Set the Subject
                oMsg.Subject = "SR #" + " " + ServiceRequestNumber_txtBox.Text + " " + "--" + " " + RequesterFirstName_txtBox.Text + " " + RequesterLastName_txtBox.Text + " " + "--" + " " + "Pick Up" + " " + EquipmentRequested_txtBox.Text;
                //Set the location
                oMsg.Location = "DDS Service Desk -- HQ";
                //save the appointment
                oMsg.Save();
                //Show Confirmation Message
                
            }
            catch (System.Exception ex)
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
                oMsg.HTMLBody = "Hello" + " " + RequesterFirstName_txtBox.Text + " " + RequesterLastName_txtBox.Text + ","
                    + "<br /> <br /> The following equipment has been reserved for you !!"
                    + "<br /> <br /><strong><u><i> Equipment Reservation Information: </strong></u></i>"
                    + "<br /><br /><strong> Requester Name: </strong>" + " " + RequesterFirstName_txtBox.Text + " " + RequesterLastName_txtBox.Text
                    + "<br /><strong> Pick-Up Time: </strong>" + " " + PickUpTime_txtBox.Text
                    + "<br /><strong> Return Time: </strong>" + " " + ReturnTime_txtBox.Text
                    + "<br /><strong> Checkout Date: </strong>" + " " + PickUpDate_txtBox.Text 
                    + "<br /><strong> Return Date: </strong>" + " " + ReturnDate_txtBox.Text
                    + "<br /><strong> Equipment Requested: </strong>" + " " + EquipmentRequested_txtBox.Text
                    + "<br /><strong> Service Request Number: </strong>" + " " + ServiceRequestNumber_txtBox.Text
                    + "<br /><strong> Additional Comments: </strong>" + " " + Comments_richTextBox.Text
                    + "<br /><br />Please come pick up the equipment at the requested date and time.  If this information is incorrect or if you need to make changes to this reservation, please contact ServiceDesk at 653-3329 or ServiceDesk@dds.ca.gov."
                    + "<br /><br />Thank You,"
                    + "<br /><br /><strong><big> Service Desk</strong></big>"
                    + "<br /><strong><big> Department of Developmental Services</strong></big>"
                    + "<br /><strong><big> Information Services Division | Service Desk</strong></big>"
                    + "<br /><strong><big> W: (916) 653-3329 |</strong></big>"
                    + "<br /><strong><big> servicedesk@dds.ca.gov</strong></big>";
                //Subject line
                oMsg.Subject = ("SR" + " " + ServiceRequestNumber_txtBox.Text + " " + "--" + " " + "ISD Equipment Reservation Confirmation !!");
                // Add a recipient
                oMsg.To = RequesterEmail_comboBox.Text;
                // Add a CC
                //oMsg.CC = ""
                // Send.
                ((Outlook._MailItem)oMsg).Send();
                // Clean up.
                oMsg = null;
                oApp = null;
                //Validate drop then list and if successful, Show Message

                {
                    MessageBox.Show("Equipment has been successfully reserved and schedule on ServiceDesk calendar." + Environment.NewLine + "A reservation notice has been sent to the requester");

                }
            }//end of try block
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }//end of catch



            //Close Application
            ths.RequestedEquipment_txtBox.Clear();
            this.Close();
            }

        private void ServiceRequestNumber_txtBox_TextChanged(object sender, EventArgs e)
        {
            // Make Service Request Textbox a maximum of 5 characters
            ServiceRequestNumber_txtBox.MaxLength = 5;
        }

        private void RequesterEmail_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get user first name and last name by email
            string mail = RequesterEmail_comboBox.Text;
            DirectoryEntry entry = new DirectoryEntry();
            DirectorySearcher adsearcher = new DirectorySearcher(entry);
            adsearcher.Filter = "(&(objectClass=user)(mail=" + mail + "))";
            adsearcher.PropertiesToLoad.Add("givenName");
            adsearcher.PropertiesToLoad.Add("sn");
            adsearcher.PropertiesToLoad.Add("mail");
            SearchResult result = adsearcher.FindOne();

            if (result == null)
                MessageBox.Show("Email Does Not Exist !!" + Environment.NewLine + "Please Check Your Spelling !!");

            if (result != null)
            {
                DirectoryEntry employee = result.GetDirectoryEntry();
                string FirstName = employee.Properties["givenName"].Value.ToString();
                string LastName = employee.Properties["sn"].Value.ToString();

                RequesterFirstName_txtBox.Text = FirstName;
                RequesterLastName_txtBox.Text = LastName;
            }
        }

        
            }


        
    
}

