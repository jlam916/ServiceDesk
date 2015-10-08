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
using System.DirectoryServices;
using MySql.Data.MySqlClient;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace ISD_Equipment_Reservation
{
    public partial class Inventory : Form
    {

        MainWindow ths;
        MySqlDataAdapter sda;
        DataTable dt;

        public Inventory(MainWindow frm)
        {
            ths = frm;
            InitializeComponent();
            BindGrid();
            
        }

        private void Inventory_Load(object sender, EventArgs e)
        {
            //load new inventory count
            StandKeyboardtxtbox.Text = Properties.Settings.Default.textbox;
            ErgoKeyboardtxtBox.Text = Properties.Settings.Default.ETextBox;
            MousetxtBox.Text = Properties.Settings.Default.MTextBox;
            ErgoMousetxtBox.Text = Properties.Settings.Default.EMTextBox;
            HeadPhonestxtBox.Text = Properties.Settings.Default.HeadTextBox;
            MiniKeyboard_txtBox.Text = Properties.Settings.Default.MiniTextBox;
            Kensington_txtBox.Text = Properties.Settings.Default.KensingtonTextBox;
            Adesso_txtBox.Text = Properties.Settings.Default.AdessoTextBox;

            //Auto date label column
            DateTime datetime = DateTime.Today;
            this.SkTime_lbl.Text = datetime.ToString("MM/dd/yyyy");
            this.miniTime_lbl.Text = datetime.ToString("MM/dd/yyyy");
            this.EkTime_lbl.Text = datetime.ToString("MM/dd/yyyy");
            this.MouseDate_lbl.Text = datetime.ToString("MM/dd/yyyy");
            this.KingsontonTime_lbl.Text = datetime.ToString("MM/dd/yyyy");
            this.AdessoTime_lbl.Text = datetime.ToString("MM/dd/yyyy");
            this.EmTime_lbl.Text = datetime.ToString("MM/dd/yyyy");
            this.HeadphonesTime_lbl.Text = datetime.ToString("MM/dd/yyyy");

            // Get Current Technician Information
            UserPrincipal user = UserPrincipal.Current;
            string FirstName = user.GivenName;
            string LastName = user.Surname;

            // Set Current Technician Information
            DistributedBy_txtBox.Text = FirstName + " " + LastName;

            //Pull ALl User from AD
            DirectoryEntry entry = new DirectoryEntry("LDAP://OU=Users,OU=HQ,OU=Sites,OU=DDS,DC=dds,DC=local");
            DirectorySearcher dSearch = new DirectorySearcher(entry);
            dSearch.SearchScope = SearchScope.Subtree;
            dSearch.Filter = "(objectClass=user)";
            dSearch.PageSize = 1000;
            dSearch.SizeLimit = 0;
            foreach (SearchResult sResultSet in dSearch.FindAll())
            {

                if (sResultSet.Properties["mail"].Count > 0)
                    DistributedTo_comboBox.Items.Add(sResultSet.Properties["mail"][0].ToString());
                
            }

        }

        private void Inventory_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.textbox = StandKeyboardtxtbox.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.ETextBox = ErgoKeyboardtxtBox.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.MTextBox = MousetxtBox.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.EMTextBox = ErgoMousetxtBox.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.HeadTextBox = HeadPhonestxtBox.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.MiniTextBox = MiniKeyboard_txtBox.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.AdessoTextBox = Adesso_txtBox.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.KensingtonTextBox = Kensington_txtBox.Text;
            Properties.Settings.Default.Save();
        }

        private void Submit_btn_Click(object sender, EventArgs e)
        {
            // Error Provider Service Request TextBox
            if (ServiceRequesttxtBox.Text == string.Empty)
            {
                errorProvider1.SetError(ServiceRequesttxtBox, "Please Enter A Service Request Number.  If none available, enter N/A.");
                return;
            }
            else
            {
                errorProvider1.Clear();
            }
            // Error Provider for Equipment Type TextBox
            if (EquipmentType_comboBox.Text == string.Empty)
            {
                errorProvider2.SetError(EquipmentType_comboBox, "Please Select An Equipment Type.");
                return;
            }
            else
            {
                errorProvider2.Clear();
            }
            // Error Provider for Quantities Given
            if (QuantitiesGiven_ComboBox.Text == string.Empty)
            {
                errorProvider3.SetError(QuantitiesGiven_ComboBox, "Please Select A Number.");
                return;
            }
            else
            {
                errorProvider3.Clear();

            }
            // Error Provider for Email TextBox
            if (DistributedTo_comboBox.Text == string.Empty)
            {
                errorProvider4.SetError(DistributedTo_comboBox, "Please Select An Email.");
                return;
                
            }
            else
            {
                errorProvider4.Clear();
            }


            // this is the submit button to assign equipment and update sql data
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "insert into database.newisdiventory (Date, SR_Number, Equipment_Type, Quantities_Given, Distributed_To, Distributed_By) values('" + this.dateTimePicker1.Text + "', '" + this.ServiceRequesttxtBox.Text + "' , '" + this.EquipmentType_comboBox.Text + "' , '" + this.QuantitiesGiven_ComboBox.Text + "' , '" + this.FirstName_txtBox.Text + "' , '" + this.DistributedBy_txtBox.Text + "'); ";
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

            // Refesh the datagrid
            string NewConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string NewQuery = "select * from database.newisdiventory";
            MySqlConnection NewconDatabase = new MySqlConnection(NewConnection);
            MySqlCommand NewcmdDatabase = new MySqlCommand(NewQuery, NewconDatabase);
            MySqlDataAdapter NewDataAdapter = new MySqlDataAdapter(NewcmdDatabase);
            DataTable dt = new DataTable();
            NewDataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;

            // Subtracting from the total count depending on item chosen

            if (EquipmentType_comboBox.Text == "Wireless Logitech Keyboard")
            {
                int a = Convert.ToInt32(StandKeyboardtxtbox.Text);
                int b = Convert.ToInt32(QuantitiesGiven_ComboBox.Text);
                int c = a - b;
                StandKeyboardtxtbox.Text = Convert.ToString(c);

            }

            if (EquipmentType_comboBox.Text == "HP Mini Keyboard")
            {
                int a = Convert.ToInt32(MiniKeyboard_txtBox.Text);
                int b = Convert.ToInt32(QuantitiesGiven_ComboBox.Text);
                int c = a - b;
                MiniKeyboard_txtBox.Text = Convert.ToString(c);
            }

            if (EquipmentType_comboBox.Text == "Microsoft 4000 Ergo Keyboard")
            {
                int a = Convert.ToInt32(ErgoKeyboardtxtBox.Text);
                int b = Convert.ToInt32(QuantitiesGiven_ComboBox.Text);
                int c = a - b;
                ErgoKeyboardtxtBox.Text = Convert.ToString(c);

            }

            if (EquipmentType_comboBox.Text == "Wireless Logitech Mouse")
            {
                int a = Convert.ToInt32(MousetxtBox.Text);
                int b = Convert.ToInt32(QuantitiesGiven_ComboBox.Text);
                int c = a - b;
                MousetxtBox.Text = Convert.ToString(c);
            }

            if (EquipmentType_comboBox.Text == "Kensington Trackball Mouse")
            {
                int a = Convert.ToInt32(Kensington_txtBox.Text);
                int b = Convert.ToInt32(QuantitiesGiven_ComboBox.Text);
                int c = a - b;
                Kensington_txtBox.Text = Convert.ToString(c);
            }

            if (EquipmentType_comboBox.Text == "Adesso Ergo Mouse e10")
            {
                int a = Convert.ToInt32(Adesso_txtBox.Text);
                int b = Convert.ToInt32(QuantitiesGiven_ComboBox.Text);
                int c = a - b;
                Adesso_txtBox.Text = Convert.ToString(c);
            }

            if (EquipmentType_comboBox.Text == "Logitech M570 Ergo Mouse")
            {
                int a = Convert.ToInt32(ErgoMousetxtBox.Text);
                int b = Convert.ToInt32(QuantitiesGiven_ComboBox.Text);
                int c = a - b;
                ErgoMousetxtBox.Text = Convert.ToString(c);
            }

            if (EquipmentType_comboBox.Text == "Headphones")
            {
                int a = Convert.ToInt32(HeadPhonestxtBox.Text);
                int b = Convert.ToInt32(QuantitiesGiven_ComboBox.Text);
                int c = a - b;
                HeadPhonestxtBox.Text = Convert.ToString(c);
            }

            // Convert textbox to intergers and if they are less than 5 in stock, automatically send email notification to lisa tom
            {
                int SK = Convert.ToInt32(StandKeyboardtxtbox.Text);
                int MK = Convert.ToInt32(MiniKeyboard_txtBox.Text);
                int EK = Convert.ToInt32(ErgoKeyboardtxtBox.Text);
                int SM = Convert.ToInt32(MousetxtBox.Text);
                int KM = Convert.ToInt32(Kensington_txtBox.Text);
                int AM = Convert.ToInt32(Adesso_txtBox.Text);
                int LM = Convert.ToInt32(ErgoMousetxtBox.Text);
                int HP = Convert.ToInt32(HeadPhonestxtBox.Text);
                int Total = 3;

                if (SK <= Total && EquipmentType_comboBox.Text == "Wireless Logitech Keyboard" || MK <= Total && EquipmentType_comboBox.Text == "HP Mini Keyboard" || EK <= Total && EquipmentType_comboBox.Text == "Microsoft 4000 Ergo Keyboard" || SM <= Total && EquipmentType_comboBox.Text == "Wireless Logitech Mouse" || KM <= Total && EquipmentType_comboBox.Text == "Kensington Trackball Mouse" || AM <= Total && EquipmentType_comboBox.Text == "Adesso Ergo Mouse e10"  || LM <= Total && EquipmentType_comboBox.Text == "Logitech M570 Ergo Mouse"|| HP <= Total && EquipmentType_comboBox.Text == "Headphones") 
                {
                    Outlook.Application oApp = new Outlook.Application();
                    Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                    oMsg.HTMLBody = "Hello Lisa Tom"
                        + "<br /> <br /> An important reminder that" + " " + EquipmentType_comboBox.Text + " will need to be restock soon.  The current inventory count is 3 or less";
                    oMsg.Subject = ("Low Inventory Reminder --" + " " + EquipmentType_comboBox.Text);
                    oMsg.To = "Jacky.Lam@dds.ca.gov";
                    ((Outlook._MailItem)oMsg).Send();
                    oMsg = null;
                    oApp = null;

                   
                }

            }


            MessageBox.Show("Equipment Has Been Distributed And Log.  Current Inventory Count Is Shown");
        }

        private void BindGrid()
        {
            // show sql data upon open
            
            dataGridView1.DefaultCellStyle.BackColor = Color.Khaki;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            dataGridView1.ReadOnly = true;
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            MySqlConnection conDatabase = new MySqlConnection(myConnection);
            sda = new MySqlDataAdapter("Select * from database.newisdiventory;", conDatabase);
            dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void DistributedTo_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get user first name and last name by email
            string mail = DistributedTo_comboBox.Text;
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

                FirstName_txtBox.Text = FirstName + " " + LastName;
               
                
              
            }
        }

        private void SkConfirm_btn_Click(object sender, EventArgs e)
        {
            // restock standard keyboard
            StandKeyboardtxtbox.Text = ReStockSKtxtBox.Text;
            MessageBox.Show("Wireless Logitech Keyboard New Amount Has Been Reflected");
        }

        private void EkConfirm_btn_Click(object sender, EventArgs e)
        {
            // restock ergo keyboard
            ErgoKeyboardtxtBox.Text = ReStockEKtxtBox.Text;
            MessageBox.Show("Microsoft 4000 Ergo Keyboard New Amount Has Been Reflected");
        }

        private void ServiceRequesttxtBox_TextChanged(object sender, EventArgs e)
        {
            // Set textbox to max of 5 characters
            ServiceRequesttxtBox.MaxLength = 5;
        }

        private void MouseConfirm_btn_Click(object sender, EventArgs e)
        {
            MousetxtBox.Text = RestockMousetxtBox.Text;
            MessageBox.Show("Wireless Logitech Mouse New Amount Has Been Reflected");
        }

        private void EMouseConfirm_btn_Click(object sender, EventArgs e)
        {
            ErgoMousetxtBox.Text = ReStockEMousetxtBox.Text;
            MessageBox.Show("Logitech M570 Ergo Mouse New Amount Has Been Reflected");
        }

        private void HeadphonesConfirm_btn_Click(object sender, EventArgs e)
        {
            HeadPhonestxtBox.Text = ReStockHeadphonestxtBox.Text;
            MessageBox.Show("Headphones New Amount Has Been Reflected");
        }

        private void MiniConfirm_btn_Click(object sender, EventArgs e)
        {
            MiniKeyboard_txtBox.Text = RestockMini_txtBox.Text;
            MessageBox.Show("HP Mini Keyboard New Amount Has Been Reflected");
        }

        private void KensingtonConfirm_btn_Click(object sender, EventArgs e)
        {
            Kensington_txtBox.Text = RestockKensington_txtBox.Text;
            MessageBox.Show("Kensington Trackball Mouse New Amount Has Been Reflected");
        }

        private void AdessoConfirm_btn_Click(object sender, EventArgs e)
        {
            Adesso_txtBox.Text = RestockAdesso_txtBox.Text;
            MessageBox.Show("Adesso Ergo Mouse e10 New Amount Has Been Reflected");
        }

 
    }
}
