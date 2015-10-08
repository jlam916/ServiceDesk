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

namespace ISD_Equipment_Reservation
{
    public partial class Conference_Room_Set_Ups : Form
    {
        MainWindow ths;

        public Conference_Room_Set_Ups(MainWindow frm)
        {
            ths = frm;
            InitializeComponent();
        }

        private void Conference_Room_Set_Ups_Load(object sender, EventArgs e)
        {
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

            // Get Current Technician Information
            UserPrincipal user = UserPrincipal.Current;
            string FirstName = user.GivenName;
            string LastName = user.Surname;

            // Set Current Technician Information
            ScheduleBy_txtBox.Text = FirstName + " " + LastName;
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

        private void ServiceRequestNumber_txtBox_TextChanged(object sender, EventArgs e)
        {
            ServiceRequestNumber_txtBox.MaxLength = 5;
        }

        private void SubmitReserve_btn_Click(object sender, EventArgs e)
        {
            // Error Providers Before Submit
            if (RequesterEmail_comboBox.Text == string.Empty)
            {
                errorProvider1.SetError(RequesterEmail_comboBox, "Please Enter An Email Address.");
                return;
            }
            else
            {
                errorProvider1.Clear();
            }
            if (ServiceRequestNumber_txtBox.Text == string.Empty)
            {
                errorProvider2.SetError(ServiceRequestNumber_txtBox, "Please Enter A Service Request Number.");
                return;
            }
            else
            {
                errorProvider2.Clear();
            }
            if (MeetingStartTime_comboBox.Text == string.Empty)
            {
                errorProvider3.SetError(MeetingStartTime_comboBox, "Please Select A Meeting Start Time.");
                return;
            }
            else
            {
                errorProvider3.Clear();
            }
            if (MeetingEndTime_comboBox.Text == string.Empty)
            {
                errorProvider4.SetError(MeetingEndTime_comboBox, "Please Select A Meeting End Time.");
                return;
            }
            else
            {
                errorProvider4.Clear();
            }
            if (MeetingLocation_comboBox.Text == string.Empty)
            {
                errorProvider5.SetError(MeetingLocation_comboBox, "Please Select A Meeting Location.");
                return;
            }
            else
            {
                errorProvider5.Clear();
            }
            if (InviteAttendee_txtBox.Text == string.Empty)
            {
                errorProvider6.SetError(InviteAttendee_txtBox, "Please Select An Invite Attendee.");
                return;
            }
            else
            {
                errorProvider6.Clear();
            }

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


                // Create a new setup appointment item
                Outlook.AppointmentItem oMsg = (Outlook.AppointmentItem)oApp.CreateItem(Outlook.OlItemType.olAppointmentItem);
                // Set Recipients
                oRecipient = oNS.CreateRecipient(calendarName);
                
                oRecipient.Resolve();
                oFolder = oNS.GetSharedDefaultFolder(oRecipient, Outlook.OlDefaultFolders.olFolderCalendar);
                // Set Appointment Start Time and End Time
                oMsg = oFolder.Items.Add("IPM.Appointment") as Outlook.AppointmentItem;
                oMsg.Start = DateTime.Parse(dateTimePicker1.Text + " " + MeetingStartTime_comboBox.Text).AddMinutes( -30 );
                oMsg.End = DateTime.Parse(dateTimePicker1.Text + " " + MeetingStartTime_comboBox.Text);
                //Set the Body
                oMsg.Body = "The Below Is Information Regarding The Meeting Set-Up." + Environment.NewLine + Environment.NewLine +
                    "Requester Information:" + Environment.NewLine +
                    "Email:" + " " + RequesterEmail_comboBox.Text + Environment.NewLine +
                    "First Name:" + " " + RequesterFirstName_txtBox.Text + Environment.NewLine +
                    "Last Name:" + " " + RequesterLastName_txtBox.Text + Environment.NewLine + Environment.NewLine +
                    "Meeting Information:" + Environment.NewLine +
                    "Meeting Date:" + " " + dateTimePicker1.Text + Environment.NewLine +
                    "Start Time:" + " " + MeetingStartTime_comboBox.Text + Environment.NewLine +
                    "End Time:" + " " + MeetingEndTime_comboBox.Text + Environment.NewLine +
                    "Meeting Location:" + " " + MeetingLocation_comboBox.Text + Environment.NewLine +
                    "Service Request Number:" + " " + " " + ServiceRequestNumber_txtBox.Text + Environment.NewLine +
                    "Service Request Link:" + " " + " " + "http://oasis.dds.ca.gov/applications/ISDRequests_Staff/index.cfm?pageAction=ShowDetail&RequestNum=" + ServiceRequestNumber_txtBox.Text + Environment.NewLine + Environment.NewLine +
                    "Reserved By Technician:" + Environment.NewLine +
                    "Name:" + " " + ScheduleBy_txtBox.Text + Environment.NewLine +
                    "Set-Up Technician Invited:" + " " + InviteAttendee_txtBox.Text + Environment.NewLine + Environment.NewLine + 
                    "Comments" + Environment.NewLine +
                     Comments_richTextBox.Text;
                //Set the Subject
                oMsg.Subject = "SR #" + " " + ServiceRequestNumber_txtBox.Text + " " + "--" + " Set-Up " + InviteAttendee_txtBox.Text;
                //Set the location
                oMsg.Location = MeetingLocation_comboBox.Text;
                //Invite Set-Up Technician
                oMsg.MeetingStatus = Outlook.OlMeetingStatus.olMeeting;
                oRecipient = oMsg.Recipients.Add(InviteAttendee_txtBox.Text);
                oRecipient.Type = (int)Outlook.OlMeetingRecipientType.olRequired;
                ((Outlook._AppointmentItem)oMsg).Send();
                //save the appointment
                oMsg.Save();

                // create the take-down appointment
                Outlook.AppointmentItem oMsgtakedown = (Outlook.AppointmentItem)oApp.CreateItem(Outlook.OlItemType.olAppointmentItem);
                // Set Recipients
                oRecipient = oNS.CreateRecipient(calendarName);
                oRecipient.Resolve();
                oFolder = oNS.GetSharedDefaultFolder(oRecipient, Outlook.OlDefaultFolders.olFolderCalendar);
                // Set Appointment Start Time and End Time
                oMsg = oFolder.Items.Add("IPM.Appointment") as Outlook.AppointmentItem;
                oMsg.Start = DateTime.Parse(dateTimePicker1.Text + " " + MeetingEndTime_comboBox.Text);
                oMsg.End = DateTime.Parse(dateTimePicker1.Text + " " + MeetingEndTime_comboBox.Text).AddMinutes(30); ;
                //Set the Body
                oMsg.Body = "The Below Is Information Regarding Meeting Set-Up" + Environment.NewLine + Environment.NewLine +
                    "Requester Information:" + Environment.NewLine +
                    "Email:" + " " + RequesterEmail_comboBox.Text + Environment.NewLine +
                    "First Name:" + " " + RequesterFirstName_txtBox.Text + Environment.NewLine +
                    "Last Name:" + " " + RequesterLastName_txtBox.Text + Environment.NewLine + Environment.NewLine +
                    "Meeting Information:" + Environment.NewLine +
                    "Meeting Date:" + " " + dateTimePicker1.Text + Environment.NewLine +
                    "Start Time:" + " " + MeetingStartTime_comboBox.Text + Environment.NewLine +
                    "End Time:" + " " + MeetingEndTime_comboBox.Text + Environment.NewLine +
                    "Meeting Location:" + " " + MeetingLocation_comboBox.Text + Environment.NewLine +
                    "Service Request Number:" + " " + ServiceRequestNumber_txtBox.Text + Environment.NewLine +
                    "Service Request Link:" + " " + "http://oasis.dds.ca.gov/applications/ISDRequests_Staff/index.cfm?pageAction=ShowDetail&RequestNum=" + ServiceRequestNumber_txtBox.Text + Environment.NewLine + Environment.NewLine +
                    "Reserved By Technician:" + Environment.NewLine +
                    "Name:" + " " + ScheduleBy_txtBox.Text + Environment.NewLine +
                    "Take-down Technician Invited:" + " " + InviteAttendee_txtBox.Text + Environment.NewLine + Environment.NewLine +
                    "Comments" + Environment.NewLine +
                     Comments_richTextBox.Text;
                //Set the Subject
                oMsg.Subject = "SR #" + " " + ServiceRequestNumber_txtBox.Text + " " + "--" + " Take-Down " + InviteAttendee_txtBox.Text;
                //Set the location
                oMsg.Location = MeetingLocation_comboBox.Text;
                //Invite Set-Up Technician
                oMsg.MeetingStatus = Outlook.OlMeetingStatus.olMeeting;
                oRecipient = oMsg.Recipients.Add(InviteAttendee_txtBox.Text);
                oRecipient.Type = (int)Outlook.OlMeetingRecipientType.olRequired;
                ((Outlook._AppointmentItem)oMsg).Send();
                //save the appointment
                oMsg.Save();
                

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            {
                MessageBox.Show("Meeting Room Set-Up And Take-Down Has Been Added To Service Desk Calendar.");

            }

        }
    }
}
