using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace ISD_Equipment_Reservation
{
    public partial class Form1 : Form
    {
        MySqlDataAdapter sda;
        DataTable dt;
        Check_Out checkOutForm;
        Check_In checkInForm;
        Cancel_Reservation CancelForm;
        Late_Notice LateNoticeForm;
        Feedback FeedbackForm;
        About_DDS_IT_Equipment_Checkout AboutForm;
        Reservation ReservationForm;

        MainWindow ths;
        public Form1(MainWindow frm)
        {
            InitializeComponent();
            ths = frm;
            checkOutForm = new Check_Out(this);
            checkInForm = new Check_In(this);
            CancelForm = new Cancel_Reservation(this);
            LateNoticeForm = new Late_Notice(this);
            FeedbackForm = new Feedback(this);
            AboutForm = new About_DDS_IT_Equipment_Checkout(this);
            ReservationForm = new Reservation(this);

            this.helpProvider1.SetShowHelp(this.CheckOut_btn, true);
            this.helpProvider1.SetHelpString(this.CheckOut_btn, "Please select a reservation and click here");
        }



        private void Reservation_btn_Click(object sender, EventArgs e)
        {

            //Take Laptop Checkbox list values
            for (int i = 0; i < Laptop_chkListBox.Items.Count; i++)

                if (Laptop_chkListBox.GetItemChecked(i))
                {
                    string ListBox1 = (string)Laptop_chkListBox.Items[i];
                    RequestedEquipment_txtBox.Text += ListBox1;


                }

            //Take Long Term Checkbox list Values

            for (int i = 0; i < LTLaptop_chklistbox.Items.Count; i++)
            {
                if (LTLaptop_chklistbox.GetItemChecked(i))
                {
                    string ListBox2 = (string)LTLaptop_chklistbox.Items[i];
                    RequestedEquipment_txtBox.Text += ListBox2;
                }
            }
            // Take Projector Checkbox List Values

            for (int i = 0; i < Projector_chkListBox.Items.Count; i++)
            {
                if (Projector_chkListBox.GetItemChecked(i))
                {
                    string ListBox3 = (string)Projector_chkListBox.Items[i];
                    RequestedEquipment_txtBox.Text += ListBox3;
                }
            }
            // Take FlashDrives Checkbox List Values

            for (int i = 0; i < FlashDrive_chkListBox.Items.Count; i++)
            {
                if (FlashDrive_chkListBox.GetItemChecked(i))
                {
                    string ListBox4 = (string)FlashDrive_chkListBox.Items[i];
                    RequestedEquipment_txtBox.Text += ListBox4;
                }
            }

            // Take Accessories Checkbox List Values

            for (int i = 0; i < Accessories_chkListBox.Items.Count; i++)
            {
                if (Accessories_chkListBox.GetItemChecked(i))
                {
                    string ListBox5 = (string)Accessories_chkListBox.Items[i];
                    RequestedEquipment_txtBox.Text += ListBox5;
                }
            }

            // Take HeadPhones Checkbox List Values

            for (int i = 0; i < HeadPhones_chkListBox.Items.Count; i++)
            {
                if (HeadPhones_chkListBox.GetItemChecked(i))
                {
                    string ListBox6 = (string)HeadPhones_chkListBox.Items[i];
                    RequestedEquipment_txtBox.Text += ListBox6;
                }
            }

            // Error Provider to checkatleast one checkbox is checked
            if (RequestedEquipment_txtBox.Text == string.Empty)
            {
                errorProvider3.SetError(Reservation_btn, "Please Check Atleast One Equipment");
                return;
            }
            else
            {
                errorProvider3.Clear();
                //Open Second Form
                ReservationForm.ShowDialog();

            }


         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Show Current Date
            DateTime datetTime = DateTime.Today;
            this.Date_lbl.Text = datetTime.ToString("MM/dd/yyyy");
            this.helpProvider1.SetShowHelp(this.SearchResult_txtBox, true);
            this.helpProvider1.SetHelpString(this.SearchResult_txtBox, "Please select a reservation and click here");



        }


        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            // Display SQL Database data to cart tab page
            if (tabControl1.SelectedTab == tabControl1.TabPages["Cart_tabPage"])
            {
                Cart_dataGridView.DefaultCellStyle.SelectionBackColor = Color.GreenYellow;
                Cart_dataGridView.DefaultCellStyle.SelectionForeColor = Color.Blue;
                Cart_dataGridView.ReadOnly = true;
                string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                MySqlConnection conDatabase = new MySqlConnection(myConnection);
                sda = new MySqlDataAdapter("Select * from database.isdequipments;", conDatabase);
                dt = new DataTable();
                sda.Fill(dt);
                Cart_dataGridView.DataSource = dt;
            }

            // Display SQL Database data to checkout tab page

            if (tabControl1.SelectedTab == tabControl1.TabPages["CheckOut_tabPage"])
            {
                CheckOut_dataGridView.DefaultCellStyle.SelectionBackColor = Color.GreenYellow;
                CheckOut_dataGridView.DefaultCellStyle.SelectionForeColor = Color.Blue;
                CheckOut_dataGridView.ReadOnly = true;
                string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                MySqlConnection conDatabase = new MySqlConnection(myConnection);
                sda = new MySqlDataAdapter("Select Log, Status, SR_Number, Equipment_Requested, Requester_Email, Requester_Name, Reservation_PickUp_Time, Reservation_Return_Time, Reservation_PickUp_Date, Reservation_Return_Date, Reserved_By, Check_Out_By, Check_Out_Date from database.isdequipments where Status = 'Check-Out';", conDatabase);
                dt = new DataTable();
                sda.Fill(dt);
                CheckOut_dataGridView.DataSource = dt;

           
              }

                
            // Display SQL Database data to Checkin Page

            if (tabControl1.SelectedTab == tabControl1.TabPages["CheckIn_tabPage"])
            {
                CheckIn_dataGridView.DefaultCellStyle.SelectionBackColor = Color.GreenYellow;
                CheckIn_dataGridView.DefaultCellStyle.SelectionForeColor = Color.Blue;
                CheckIn_dataGridView.ReadOnly = true;
                string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                MySqlConnection conDatabase = new MySqlConnection(myConnection);
                sda = new MySqlDataAdapter("Select Log, Status, SR_Number, Equipment_Requested, Requester_Email, Requester_Name, Reservation_PickUp_Time, Reservation_Return_Time, Reservation_PickUp_Date, Reservation_Return_Date, Reserved_By, Check_Out_By, Check_Out_Date, Check_In_By, Check_In_Date from database.isdequipments where Status = 'Check-In';", conDatabase);
                dt = new DataTable();
                sda.Fill(dt);
                CheckIn_dataGridView.DataSource = dt;

            }

            // Display SQL Database data to Reservation tab page

            if (tabControl1.SelectedTab == tabControl1.TabPages["Reservation_tabPage"])
            {
                Reservation_dataGridView.DefaultCellStyle.SelectionBackColor = Color.GreenYellow;
                Reservation_dataGridView.DefaultCellStyle.SelectionForeColor = Color.Blue;
                Reservation_dataGridView.ReadOnly = true;
                string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                MySqlConnection conDatabase = new MySqlConnection(myConnection);
                sda = new MySqlDataAdapter("Select Log, Status, SR_Number, Equipment_Requested, Requester_Email, Requester_Name, Reservation_PickUp_Time, Reservation_Return_Time, Reservation_PickUp_Date, Reservation_Return_Date, Reserved_By from database.isdequipments where Status = 'Reserved';", conDatabase);
                dt = new DataTable();
                sda.Fill(dt);
                Reservation_dataGridView.DataSource = dt;
            }

            // Display SQL Database data to Cancellation tab page

            if (tabControl1.SelectedTab == tabControl1.TabPages["Cancellations_tabPage"])
            {
                Cancellations_dataGridView.DefaultCellStyle.SelectionBackColor = Color.GreenYellow;
                Cancellations_dataGridView.DefaultCellStyle.SelectionForeColor = Color.Blue;
                Cancellations_dataGridView.ReadOnly = true;
                string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                MySqlConnection conDatabase = new MySqlConnection(myConnection);
                sda = new MySqlDataAdapter("Select Log, Status, SR_Number, Equipment_Requested, Requester_Email, Requester_Name, Reservation_PickUp_Time, Reservation_Return_Time, Reservation_PickUp_Date, Reservation_Return_Date, Reserved_By, Cancelled_By, Cancelled_Date from database.isdequipments where Status = 'Cancelled';", conDatabase);
                dt = new DataTable();
                sda.Fill(dt);
                Cancellations_dataGridView.DataSource = dt;
            }
        }

        private void CheckOut_btn_Click(object sender, EventArgs e)
        {
            
            // Show CheckOut Form
            checkOutForm.ShowDialog();

        }

        private void Reservation_dataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            //display data from datagridview to pass onto CheckOut Form textbox 
            checkOutForm.RecordNumber_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Log"].Value.ToString();
            checkOutForm.RequestedEmail_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Requester_Email"].Value.ToString();
            checkOutForm.RequesterName_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Requester_Name"].Value.ToString();
            checkOutForm.ServiceRequestNumber_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["SR_Number"].Value.ToString();
            checkOutForm.PickUpDate_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Reservation_PickUp_Date"].Value.ToString();
            checkOutForm.ReturnDate_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Reservation_Return_Date"].Value.ToString();
            checkOutForm.EquipmentRequested_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Equipment_Requested"].Value.ToString();

            //display data from datagridview to pass onto Checkin Form Textbox
            CancelForm.RecordNumber_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Log"].Value.ToString();
            CancelForm.RequestedEmail_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Requester_Email"].Value.ToString();
            CancelForm.RequesterName_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Requester_Name"].Value.ToString();
            CancelForm.ServiceRequestNumber_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["SR_Number"].Value.ToString();
            CancelForm.PickUpDate_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Reservation_PickUp_Date"].Value.ToString();
            CancelForm.ReturnDate_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Reservation_Return_Date"].Value.ToString();
            CancelForm.EquipmentRequested_txtBox.Text = Reservation_dataGridView.SelectedRows[0].Cells["Equipment_Requested"].Value.ToString();
        }


        private void Refresh_btn_Click(object sender, EventArgs e)
        {
            // Refresh Reservation DataGrid
            tabControl1.SelectedIndex = tabControl1.SelectedIndex = (tabControl1.SelectedIndex + 1 < tabControl1.TabCount) ?
                             tabControl1.SelectedIndex + 1 : tabControl1.SelectedIndex;

            tabControl1.SelectedIndex = tabControl1.SelectedIndex = (tabControl1.SelectedIndex - 1 < tabControl1.TabCount) ?
                             tabControl1.SelectedIndex - 1 : tabControl1.SelectedIndex;
        }

        private void CheckIn_btn_Click(object sender, EventArgs e)
        {
            // Show Check-In Form
            checkInForm.ShowDialog();
        }


        private void CheckOut_dataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            //display data from datagridview to pass onto CheckOut Form textbox
            checkInForm.RecordNumber_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Log"].Value.ToString();
            checkInForm.RequestedEmail_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Requester_Email"].Value.ToString();
            checkInForm.RequesterName_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Requester_Name"].Value.ToString();
            checkInForm.ServiceRequestNumber_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["SR_Number"].Value.ToString();
            checkInForm.PickUpDate_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Reservation_PickUp_Date"].Value.ToString();
            checkInForm.ReturnDate_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Reservation_Return_Date"].Value.ToString();
            checkInForm.EquipmentRequested_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Equipment_Requested"].Value.ToString();


            //display data from datagridview to pass onto LateNotice Form textbox
            LateNoticeForm.RecordNumber_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Log"].Value.ToString();
            LateNoticeForm.RequestedEmail_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Requester_Email"].Value.ToString();
            LateNoticeForm.RequesterName_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Requester_Name"].Value.ToString();
            LateNoticeForm.ServiceRequestNumber_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["SR_Number"].Value.ToString();
            LateNoticeForm.PickUpDate_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Reservation_PickUp_Date"].Value.ToString();
            LateNoticeForm.ReturnDate_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Reservation_Return_Date"].Value.ToString();
            LateNoticeForm.EquipmentRequested_txtBox.Text = CheckOut_dataGridView.SelectedRows[0].Cells["Equipment_Requested"].Value.ToString();

        }

        private void CancelReservation_btn_Click(object sender, EventArgs e)
        {
            // Show Cancellation Form
            CancelForm.ShowDialog();
        }

        private void CheckOutRefresh_btn_Click(object sender, EventArgs e)
        {
            // refresh checkout datagrid
            tabControl1.SelectedIndex = tabControl1.SelectedIndex = (tabControl1.SelectedIndex + 1 < tabControl1.TabCount) ?
                 tabControl1.SelectedIndex + 1 : tabControl1.SelectedIndex;

            tabControl1.SelectedIndex = tabControl1.SelectedIndex = (tabControl1.SelectedIndex - 1 < tabControl1.TabCount) ?
                             tabControl1.SelectedIndex - 1 : tabControl1.SelectedIndex;

            CheckOut_dataGridView.DefaultCellStyle.BackColor = Color.White;


        }
                    

        private void LateNotice_btn_Click(object sender, EventArgs e)
        {
            // Show LateNotice Form
            LateNoticeForm.ShowDialog();
        }

        private void SearchResult_txtBox_TextChanged(object sender, EventArgs e)
        {
            //Search Reservation Datagrid
            
            if (SearchBy_comboBox.Text == "SR #")
            {
                
                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("SR_Number LIKE '%{0}%'", SearchResult_txtBox.Text);
                Reservation_dataGridView.DataSource = DV;
               
            }
            if (SearchBy_comboBox.Text == "Equipment Type")
            {
                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Equipment_Requested LIKE '%{0}%'", SearchResult_txtBox.Text);
                Reservation_dataGridView.DataSource = DV;
            }
            if (SearchBy_comboBox.Text == "Requester Email")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Requester_Email LIKE '%{0}%'", SearchResult_txtBox.Text);
                Reservation_dataGridView.DataSource = DV;
            }
            if (SearchBy_comboBox.Text == "Requester Name")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Requester_Name LIKE '%{0}%'", SearchResult_txtBox.Text);
                Reservation_dataGridView.DataSource = DV;
            }
            if (SearchBy_comboBox.Text == "Reserved By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Reserved_By LIKE '%{0}%'", SearchResult_txtBox.Text);
                Reservation_dataGridView.DataSource = DV;
            }

        }

        private void CheckOutSearchResult_txtBox_TextChanged_1(object sender, EventArgs e)
        {
            //Search Check-Out Datagrid
            if (CheckoutSearch_comboBox.Text == "SR #")
            {
                
                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("SR_Number LIKE '%{0}%'", CheckOutSearchResult_txtBox.Text);
                CheckOut_dataGridView.DataSource = DV;
            }

            if (CheckoutSearch_comboBox.Text == "Equipment Type")
            {
               
                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Equipment_Requested LIKE '%{0}%'", CheckOutSearchResult_txtBox.Text);
                CheckOut_dataGridView.DataSource = DV;
            }

            if (CheckoutSearch_comboBox.Text == "Requester Email")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Requester_Email LIKE '%{0}%'", CheckOutSearchResult_txtBox.Text);
                CheckOut_dataGridView.DataSource = DV;
            }

            if (CheckoutSearch_comboBox.Text == "Requester Name")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Requester_Name LIKE '%{0}%'", CheckOutSearchResult_txtBox.Text);
                CheckOut_dataGridView.DataSource = DV;
            }

            if (CheckoutSearch_comboBox.Text == "Reserved By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Reserved_By LIKE '%{0}%'", CheckOutSearchResult_txtBox.Text);
                CheckOut_dataGridView.DataSource = DV;
            }

            if (CheckoutSearch_comboBox.Text == "Check-Out By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Check_Out_By LIKE '%{0}%'", CheckOutSearchResult_txtBox.Text);
                CheckOut_dataGridView.DataSource = DV;
            }


        }

        private void CheckInSearchResults_txtBox_TextChanged(object sender, EventArgs e)
        {
            //Search Check-In Datagrid
            if (CheckInSearchBy_comboBox.Text == "SR #")
            {
                
                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("SR_Number LIKE '%{0}%'", CheckInSearchResults_txtBox.Text);
                CheckIn_dataGridView.DataSource = DV;
            }

            if (CheckInSearchBy_comboBox.Text == "Equipment Type")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Equipment_Requested LIKE '%{0}%'", CheckInSearchResults_txtBox.Text);
                CheckIn_dataGridView.DataSource = DV;
            }

            if (CheckInSearchBy_comboBox.Text == "Requester Email")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Requester_Email LIKE '%{0}%'", CheckInSearchResults_txtBox.Text);
                CheckIn_dataGridView.DataSource = DV;
            }

            if (CheckInSearchBy_comboBox.Text == "Requester Name")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Requester_Name LIKE '%{0}%'", CheckInSearchResults_txtBox.Text);
                CheckIn_dataGridView.DataSource = DV;
            }

            if (CheckInSearchBy_comboBox.Text == "Reserved By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Reserved_By LIKE '%{0}%'", CheckInSearchResults_txtBox.Text);
                CheckIn_dataGridView.DataSource = DV;
            }

            if (CheckInSearchBy_comboBox.Text == "Check-Out By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Check_Out_By LIKE '%{0}%'", CheckInSearchResults_txtBox.Text);
                CheckIn_dataGridView.DataSource = DV;
            }

            if (CheckInSearchBy_comboBox.Text == "Check-In By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Check_In_By LIKE '%{0}%'", CheckInSearchResults_txtBox.Text);
                CheckIn_dataGridView.DataSource = DV;
            }
        }

        private void CancellationSearchResult_txtBox_TextChanged(object sender, EventArgs e)
        {
            //Search Cancellation Datagrid
            if (CancellationSearch_comboBox.Text == "SR #")
            {
                
                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("SR_Number LIKE '%{0}%'", CancellationSearchResult_txtBox.Text);
                Cancellations_dataGridView.DataSource = DV;
            }

            if (CancellationSearch_comboBox.Text == "Equipment Type")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Equipment_Requested LIKE '%{0}%'", CancellationSearchResult_txtBox.Text);
                Cancellations_dataGridView.DataSource = DV;
            }

            if (CancellationSearch_comboBox.Text == "Requester Email")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Requester_Email LIKE '%{0}%'", CancellationSearchResult_txtBox.Text);
                Cancellations_dataGridView.DataSource = DV;
            }

            if (CancellationSearch_comboBox.Text == "Requester Name")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Requester_Name LIKE '%{0}%'", CancellationSearchResult_txtBox.Text);
                Cancellations_dataGridView.DataSource = DV;
            }

            if (CancellationSearch_comboBox.Text == "Cancelled By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Cancelled_By LIKE '%{0}%'", CancellationSearchResult_txtBox.Text);
                Cancellations_dataGridView.DataSource = DV;
            }
            

            
        }

        private void CartSearchResult_txtBox_TextChanged(object sender, EventArgs e)
        {
            //Search Cart Datagrid
            if (CartSearch_comboBox.Text == "SR #")
            {
                
                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("SR_Number LIKE '%{0}%'", CartSearchResult_txtBox.Text);
                Cart_dataGridView.DataSource = DV;
            }

            if (CartSearch_comboBox.Text == "Equipment Type")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Equipment_Requested LIKE '%{0}%'", CartSearchResult_txtBox.Text);
                Cart_dataGridView.DataSource = DV;
            }

            if (CartSearch_comboBox.Text == "Requester Email")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Requester_Email LIKE '%{0}%'", CartSearchResult_txtBox.Text);
                Cart_dataGridView.DataSource = DV;
            }

            if (CartSearch_comboBox.Text == "Requester Name")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Requester_Name LIKE '%{0}%'", CartSearchResult_txtBox.Text);
                Cart_dataGridView.DataSource = DV;
            }

            if (CartSearch_comboBox.Text == "Reserved By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Reserved_By LIKE '%{0}%'", CartSearchResult_txtBox.Text);
                Cart_dataGridView.DataSource = DV;
            }

            if (CartSearch_comboBox.Text == "Check-Out By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Check_Out_By LIKE '%{0}%'", CartSearchResult_txtBox.Text);
                Cart_dataGridView.DataSource = DV;
            }

            if (CartSearch_comboBox.Text == "Check-In By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Check_In_By LIKE '%{0}%'", CartSearchResult_txtBox.Text);
                Cart_dataGridView.DataSource = DV;
            }
            if (CartSearch_comboBox.Text == "Cancelled By")
            {

                DataView DV = new DataView(dt);
                DV.RowFilter = string.Format("Cancelled_By LIKE '%{0}%'", CartSearchResult_txtBox.Text);
                Cart_dataGridView.DataSource = DV;
            }

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close the Application
            Application.Exit();
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Select Cart_tabPage
            tabControl1.SelectedIndex = 5;
        }

        private void cancellationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Select Cancellations_tabPage
            tabControl1.SelectedIndex = 4;
        }

        private void checkInsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Select CheckIn_tabPage
            tabControl1.SelectedIndex = 3;
        }

        private void checkOutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Select CheckOut_tabPage
            tabControl1.SelectedIndex = 2;
        }

        private void reservationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Select Reservation_tabPage
            tabControl1.SelectedIndex = 1;
        }

        private void equipmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Select Equipment_tabPage
            tabControl1.SelectedIndex = 0;
        }

        private void giveFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show Feedback Form
            FeedbackForm.ShowDialog();
        }

        private void aboutDDSITEquipmentCheckoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show About Form
            AboutForm.ShowDialog();
        }

        private void CheckInRefresh_btn_Click(object sender, EventArgs e)
        {
            // refresh CheckIn datagrid
            tabControl1.SelectedIndex = tabControl1.SelectedIndex = (tabControl1.SelectedIndex + 1 < tabControl1.TabCount) ?
                 tabControl1.SelectedIndex + 1 : tabControl1.SelectedIndex;

            tabControl1.SelectedIndex = tabControl1.SelectedIndex = (tabControl1.SelectedIndex - 1 < tabControl1.TabCount) ?
                             tabControl1.SelectedIndex - 1 : tabControl1.SelectedIndex;
        }

        private void CancellationRefresh_btn_Click(object sender, EventArgs e)
        {
            // refresh Cancellation datagrid
            tabControl1.SelectedIndex = tabControl1.SelectedIndex = (tabControl1.SelectedIndex + 1 < tabControl1.TabCount) ?
                 tabControl1.SelectedIndex + 1 : tabControl1.SelectedIndex;

            tabControl1.SelectedIndex = tabControl1.SelectedIndex = (tabControl1.SelectedIndex - 1 < tabControl1.TabCount) ?
                             tabControl1.SelectedIndex - 1 : tabControl1.SelectedIndex;
        }

        private void CartRefresh_btn_Click(object sender, EventArgs e)
        {
            // refresh cart datagrid
            tabControl1.SelectedIndex = tabControl1.SelectedIndex = (tabControl1.SelectedIndex + 1 < tabControl1.TabCount) ?
                 tabControl1.SelectedIndex + 1 : tabControl1.SelectedIndex;

            tabControl1.SelectedIndex = tabControl1.SelectedIndex = (tabControl1.SelectedIndex - 1 < tabControl1.TabCount) ?
                             tabControl1.SelectedIndex - 1 : tabControl1.SelectedIndex;
        }



        private void LateCheckOut_btn_Click(object sender, EventArgs e)
        {
            // Get Late Returns
            DataView DV = new DataView(dt);
            DV.RowFilter = "Reservation_Return_Date <= '" + Date_lbl.Text + "'";
            CheckOut_dataGridView.DataSource = DV;
            CheckOut_dataGridView.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            CheckOut_dataGridView.DefaultCellStyle.BackColor = Color.LightGreen;
        
        }

        private void howToReserveEquipmentToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Open up help web page
            System.Diagnostics.Process.Start("http://HQDT380060/ServiceDesk/Reservations.aspx");
        }


        private void Laptop_chkListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RequestedEquipment_txtBox.Clear();
        }

        private void Availability_btn_Click(object sender, EventArgs e)
        {

            // Error Providers to check require textbox and checklist before checking availability
            if (PickUpTime_ComboBox.Text == string.Empty)
            {
                errorProvider1.SetError(PickUpTime_ComboBox, "Please Select A Pick-Up Time From The Drop-Down List");
                return;
            }
            else
            {
                errorProvider1.Clear();
            }
            if (ReturnTime_ComboBox.Text == string.Empty)
            {
                errorProvider2.SetError(ReturnTime_ComboBox, "Please Select A Return Time From The Drop-Down List");
                return;
            }
            else
            {
                errorProvider2.Clear();
            }



            // Populate data from sql database and filter it with the equipment requested and date to check availability

            Availability_datagrid.DefaultCellStyle.BackColor = Color.LightGreen;
            Availability_datagrid.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            Availability_datagrid.DefaultCellStyle.SelectionForeColor = Color.Blue;
            Availability_datagrid.ReadOnly = true;
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            MySqlConnection conDatabase = new MySqlConnection(myConnection);
            sda = new MySqlDataAdapter("Select Log, Status, SR_Number, Equipment_Requested, Requester_Name, Reservation_PickUp_Date, Reservation_Return_Date from database.isdequipments where (Status = 'Reserved' OR Status = 'Check-Out') AND (Equipment_Requested LIKE '%Laptop%' OR Equipment_Requested LIKE '%LT Laptop%' OR Equipment_Requested LIKE '%Projector%' OR Equipment_Requested LIKE '%Flash Drive%' OR Equipment_Requested LIKE '%Ergonomic Keyboard%' OR Equipment_Requested LIKE '%Ergonomic Mouse%' OR Equipment_Requested LIKE '%Surge Protectors%' OR Equipment_Requested LIKE '%Docking Station%' OR Equipment_Requested LIKE '%Headphones%') ;", conDatabase);
            //sda = new MySqlDataAdapter("Select Log, Status, SR_Number, Equipment_Requested, Requester_Name, Reservation_PickUp_Date, Reservation_Return_Date from database.isdequipments where (Reservation_PickUp_Date BETWEEN '" + MainFormPickUpDate_DateTimePicker.Text + "' AND '" + MainFormReturnDate_DateTimePicker.Text + "' OR Reservation_Return_Date BETWEEN '" + MainFormPickUpDate_DateTimePicker.Text + "' AND '" + MainFormReturnDate_DateTimePicker.Text + "')  AND (Status = 'Reserved' OR Status = 'Check-Out') AND (Equipment_Requested LIKE '%Laptop%' OR Equipment_Requested LIKE '%LT Laptop%' OR Equipment_Requested LIKE '%Projector%' OR Equipment_Requested LIKE '%Flash Drive%' OR Equipment_Requested LIKE '%Ergonomic Keyboard%' OR Equipment_Requested LIKE '%Ergonomic Mouse%' OR Equipment_Requested LIKE '%Surge Protectors%' OR Equipment_Requested LIKE '%Docking Station%' OR Equipment_Requested LIKE '%Headphones%') ;", conDatabase);
            dt = new DataTable();
            sda.Fill(dt);
            Availability_datagrid.DataSource = dt;

            //MessageBox.Show("Below Are Equipments Already Reserved Or Check-Out!!" + Environment.NewLine + "Please Examine The Dates And Select An Equipment Below That Is Not Check-Out Or Reserved.");

            // If datagrid is empthy, it means no equipment has been reserved for the dates selected
            if (dt.Rows.Count == 0)
            {
                //Availability_datagrid.DefaultCellStyle.BackColor = Color.LightGreen;
                //Availability_datagrid.DefaultCellStyle.SelectionBackColor = Color.Yellow;
                //Availability_datagrid.DefaultCellStyle.SelectionForeColor = Color.Blue;
                //Availability_datagrid.ReadOnly = true;
                //string myConnection1 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                //MySqlConnection conDatabase1 = new MySqlConnection(myConnection1);
                //sda = new MySqlDataAdapter("Select Log, Status, SR_Number, Equipment_Requested, Requester_Name, Reservation_PickUp_Date, Reservation_Return_Date from database.isdequipments where (Status = 'Reserved' OR Status = 'Check-Out') AND (Equipment_Requested LIKE '%Laptop%' OR Equipment_Requested LIKE '%LT Laptop%' OR Equipment_Requested LIKE '%Projector%' OR Equipment_Requested LIKE '%Flash Drive%' OR Equipment_Requested LIKE '%Ergonomic Keyboard%' OR Equipment_Requested LIKE '%Ergonomic Mouse%' OR Equipment_Requested LIKE '%Surge Protectors%' OR Equipment_Requested LIKE '%Docking Station%' OR Equipment_Requested LIKE '%Headphones%') ;", conDatabase);
                //dt = new DataTable();
                //sda.Fill(dt);
                //Availability_datagrid.DataSource = dt;
                MessageBox.Show("No Equipment Has Been Reserved Or Check-Out !!" + Environment.NewLine + "Please Select Any Equipment Below To Continue Reservation.");
                

                //MessageBox.Show("No Equipment Has Been Reserved or Check-Out For The Dates Selected." + Environment.NewLine + "Please Select Any Equipment Below And Continue With The Reservation");
                //if (dialogResult == DialogResult.Yes)
                //{
                //    ReservationForm.ShowDialog();
                //}
                //if (dialogResult == DialogResult.No)
                //{
                //    Boolean state = false;
                //    {
                //        for (int i = 0; i <Laptop_chkListBox.Items.Count; i++)
                //            Laptop_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //    }

                //    {
                //        for (int i = 0; i <LTLaptop_chklistbox.Items.Count; i++)
                //            LTLaptop_chklistbox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //    }

                //    {
                //        for (int i = 0; i <Projector_chkListBox.Items.Count; i++)
                //            Projector_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));

                //    }
                //    {
                //        for (int i = 0; i <FlashDrive_chkListBox.Items.Count; i++)
                //            FlashDrive_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //    }
                //    {
                //        for (int i = 0; i < Accessories_chkListBox.Items.Count; i++)
                //            Accessories_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //    }
                //    {
                //        for (int i = 0; i <HeadPhones_chkListBox.Items.Count; i++)
                //            HeadPhones_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //    }
                //    RequestedEquipment_txtBox.Clear();
                //}
            }

            // if datagrid is not empthy
            if (dt.Rows.Count != 0)
            {
                MessageBox.Show("Below Are Equipments Already Reserved Or Check-Out!!" + Environment.NewLine + "Please Examine The Dates And Select An Equipment Below That Is Not Check-Out Or Reserved.");
                //    Boolean state = false;
                //    {
                //        for (int i = 0; i < Laptop_chkListBox.Items.Count; i++)
                //            Laptop_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //    }

                //    {
                //        for (int i = 0; i < LTLaptop_chklistbox.Items.Count; i++)
                //            LTLaptop_chklistbox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //    }

                //    {
                //        for (int i = 0; i < Projector_chkListBox.Items.Count; i++)
                //            Projector_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));

                //    }
                //    {
                //        for (int i = 0; i < FlashDrive_chkListBox.Items.Count; i++)
                //            FlashDrive_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //    }
                //    {
                //        for (int i = 0; i < Accessories_chkListBox.Items.Count; i++)
                //            Accessories_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //    }
                //    {
                //        for (int i = 0; i < HeadPhones_chkListBox.Items.Count; i++)
                //            HeadPhones_chkListBox.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //    }
                //    RequestedEquipment_txtBox.Clear();
                //}
            }
        }

        private void LTLaptop_chklistbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear Requested Equipment Textbox when checklist changes
            RequestedEquipment_txtBox.Clear();
        }

        private void Projector_chkListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear Requested Equipment Textbox when checklist changes
            RequestedEquipment_txtBox.Clear();
        }

        private void FlashDrive_chkListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear Requested Equipment Textbox when checklist changes
            RequestedEquipment_txtBox.Clear();
        }

        private void Accessories_chkListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear Requested Equipment Textbox when checklist changes
            RequestedEquipment_txtBox.Clear();
        }

        private void HeadPhones_chkListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Open up help web page
            RequestedEquipment_txtBox.Clear();
        }

        private void howToCheckOutEquipmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open up help web page
            System.Diagnostics.Process.Start("http://HQDT380060/ServiceDesk/Check-Outs.aspx");
        }

        private void howToCheckInAReservationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open up help web page
            System.Diagnostics.Process.Start("http://HQDT380060/ServiceDesk/Check-Ins.aspx");
        }

        private void howToCancelAReservationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open up help web page
            System.Diagnostics.Process.Start("http://HQDT380060/ServiceDesk/Cancellations.aspx");
        }

        private void howToSendALateNoticeToRequesterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open up help web page
            System.Diagnostics.Process.Start("http://HQDT380060/ServiceDesk/Late%20Check-Outs.aspx");
        }

        private void Date_lbl_Click(object sender, EventArgs e)
        {

        }

        private void Availability_datagrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        }
    }

   