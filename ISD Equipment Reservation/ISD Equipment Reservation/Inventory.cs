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

        private void BindGrid()
        {
            // show sql data upon open

            dataGridView1.DefaultCellStyle.BackColor = Color.Khaki;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            dataGridView1.ReadOnly = true;
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            MySqlConnection conDatabase = new MySqlConnection(myConnection);
            sda = new MySqlDataAdapter("Select * from database.finalisdinventory;", conDatabase);
            dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;

            dataGridView2.DefaultCellStyle.BackColor = Color.Khaki;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            dataGridView2.ReadOnly = true;
            string myConnection1 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            MySqlConnection conDatabase1 = new MySqlConnection(myConnection1);
            sda = new MySqlDataAdapter("Select * from database.wirelesslogitech;", conDatabase1);
            dt = new DataTable();
            sda.Fill(dt);
            dataGridView2.DataSource = dt;

        }

        private void Inventory_Load(object sender, EventArgs e)
        {
            //load new inventory count
            //StandKeyboardtxtbox.Text = Properties.Settings.Default.textbox;
            //ErgoKeyboardtxtBox.Text = Properties.Settings.Default.ETextBox;
            //MousetxtBox.Text = Properties.Settings.Default.MTextBox;
            //ErgoMousetxtBox.Text = Properties.Settings.Default.EMTextBox;
            //HeadPhonestxtBox.Text = Properties.Settings.Default.HeadTextBox;
            //MiniKeyboard_txtBox.Text = Properties.Settings.Default.MiniTextBox;
            //Kensington_txtBox.Text = Properties.Settings.Default.KensingtonTextBox;
            //Adesso_txtBox.Text = Properties.Settings.Default.AdessoTextBox;

            //Auto date Date Textbox
            //Auto date label column
            DateTime datetime = DateTime.Today;
            this.dateTimePicker1.Text = datetime.ToString("MM/dd/yyyy");
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

            //Display Data From DataGridView To Pass Onto Textbox
            StandKeyboardtxtbox.Text = dataGridView2.Rows[0].Cells["Count"].Value.ToString();
            MiniKeyboard_txtBox.Text = dataGridView2.Rows[1].Cells["Count"].Value.ToString();
            ErgoKeyboardtxtBox.Text = dataGridView2.Rows[2].Cells["Count"].Value.ToString();
            MousetxtBox.Text = dataGridView2.Rows[3].Cells["Count"].Value.ToString();
            Kensington_txtBox.Text = dataGridView2.Rows[4].Cells["Count"].Value.ToString();
            Adesso_txtBox.Text = dataGridView2.Rows[5].Cells["Count"].Value.ToString();
            ErgoMousetxtBox.Text = dataGridView2.Rows[6].Cells["Count"].Value.ToString();
            HeadPhonestxtBox.Text = dataGridView2.Rows[7].Cells["Count"].Value.ToString();
            



        }

        private void Inventory_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Properties.Settings.Default.textbox = StandKeyboardtxtbox.Text;
            //Properties.Settings.Default.Save();
            //Properties.Settings.Default.ETextBox = ErgoKeyboardtxtBox.Text;
            //Properties.Settings.Default.Save();
            //Properties.Settings.Default.MTextBox = MousetxtBox.Text;
            //Properties.Settings.Default.Save();
            //Properties.Settings.Default.EMTextBox = ErgoMousetxtBox.Text;
            //Properties.Settings.Default.Save();
            //Properties.Settings.Default.HeadTextBox = HeadPhonestxtBox.Text;
            //Properties.Settings.Default.Save();
            //Properties.Settings.Default.MiniTextBox = MiniKeyboard_txtBox.Text;
            //Properties.Settings.Default.Save();
            //Properties.Settings.Default.AdessoTextBox = Adesso_txtBox.Text;
            //Properties.Settings.Default.Save();
            //Properties.Settings.Default.KensingtonTextBox = Kensington_txtBox.Text;
            //Properties.Settings.Default.Save();
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
            string Query = "insert into database.finalisdinventory (Date, SR_Number, Equipment_Type, Quantities_Given, Distributed_To, Distributed_By) values('" + this.dateTimePicker1.Text + "' , '" + this.ServiceRequesttxtBox.Text + "' , '" + this.EquipmentType_comboBox.Text + "' , '" + this.QuantitiesGiven_ComboBox.Text + "' , '" + this.FirstName_txtBox.Text + "' , '" + this.DistributedBy_txtBox.Text + "'); ";
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

            //Refesh the datagrid
            string NewConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string NewQuery = "select * from database.finalisdinventory";
            MySqlConnection NewconDatabase = new MySqlConnection(NewConnection);
            MySqlCommand NewcmdDatabase = new MySqlCommand(NewQuery, NewconDatabase);
            MySqlDataAdapter NewDataAdapter = new MySqlDataAdapter(NewcmdDatabase);
            DataTable dt = new DataTable();
            NewDataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;

            // Subtracting from the total count depending on item chosen

            if (EquipmentType_comboBox.Text == "Wireless Logitech Keyboard")
            {
                float value1, value2, result = 0;
                value1 = float.Parse(StandKeyboardtxtbox.Text);
                value2 = float.Parse(QuantitiesGiven_ComboBox.Text);
                result = value1 - value2;
                StandKeyboardtxtbox.Text = result.ToString();
                // this is the submit button to assign equipment and update sql data
                string myConnection1 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                string Query1 = "update database.wirelesslogitech set Count= '" + this.StandKeyboardtxtbox.Text + "' where Equipments= '" + "1" + "';";
                MySqlConnection conDatabase1 = new MySqlConnection(myConnection1);
                MySqlCommand cmdDatabase1 = new MySqlCommand(Query1, conDatabase1);
                MySqlDataReader myReader1;

                try
                {
                    conDatabase1.Open();
                    myReader1 = cmdDatabase1.ExecuteReader();
                    while (myReader1.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }



            if (EquipmentType_comboBox.Text == "Mini Keyboard")
            {
                float value1, value2, result = 0;
                value1 = float.Parse(MiniKeyboard_txtBox.Text);
                value2 = float.Parse(QuantitiesGiven_ComboBox.Text);
                result = value1 - value2;
                MiniKeyboard_txtBox.Text = result.ToString();
                // this is the submit button to assign equipment and update sql data
                string myConnection2 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                string Query2 = "update database.wirelesslogitech set Count= '" + this.MiniKeyboard_txtBox.Text + "' where Equipments= '" + "2" + "';";
                MySqlConnection conDatabase2 = new MySqlConnection(myConnection2);
                MySqlCommand cmdDatabase2 = new MySqlCommand(Query2, conDatabase2);
                MySqlDataReader myReader2;

                try
                {
                    conDatabase2.Open();
                    myReader2 = cmdDatabase2.ExecuteReader();
                    while (myReader2.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }

            if (EquipmentType_comboBox.Text == "Microsoft 4000 Ergo Keyboard")
            {
                float value1, value2, result = 0;
                value1 = float.Parse(ErgoKeyboardtxtBox.Text);
                value2 = float.Parse(QuantitiesGiven_ComboBox.Text);
                result = value1 - value2;
                ErgoKeyboardtxtBox.Text = result.ToString();
                // this is the submit button to assign equipment and update sql data
                string myConnection3 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                string Query3 = "update database.wirelesslogitech set Count= '" + this.ErgoKeyboardtxtBox.Text + "' where Equipments= '" + "3" + "';";
                MySqlConnection conDatabase3 = new MySqlConnection(myConnection3);
                MySqlCommand cmdDatabase3 = new MySqlCommand(Query3, conDatabase3);
                MySqlDataReader myReader3;

                try
                {
                    conDatabase3.Open();
                    myReader3 = cmdDatabase3.ExecuteReader();
                    while (myReader3.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

            if (EquipmentType_comboBox.Text == "Wireless Logitech Mouse")
            {
                float value1, value2, result = 0;
                value1 = float.Parse(MousetxtBox.Text);
                value2 = float.Parse(QuantitiesGiven_ComboBox.Text);
                result = value1 - value2;
                MousetxtBox.Text = result.ToString();
                // this is the submit button to assign equipment and update sql data
                string myConnection4 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                string Query4 = "update database.wirelesslogitech set Count= '" + this.MousetxtBox.Text + "' where Equipments= '" + "4" + "';";
                MySqlConnection conDatabase4 = new MySqlConnection(myConnection4);
                MySqlCommand cmdDatabase4 = new MySqlCommand(Query4, conDatabase4);
                MySqlDataReader myReader4;

                try
                {
                    conDatabase4.Open();
                    myReader4 = cmdDatabase4.ExecuteReader();
                    while (myReader4.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (EquipmentType_comboBox.Text == "Kensington Trackball Mouse")
            {
                float value1, value2, result = 0;
                value1 = float.Parse(Kensington_txtBox.Text);
                value2 = float.Parse(QuantitiesGiven_ComboBox.Text);
                result = value1 - value2;
                Kensington_txtBox.Text = result.ToString();
                // this is the submit button to assign equipment and update sql data
                string myConnection5 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                string Query5 = "update database.wirelesslogitech set Count= '" + this.Kensington_txtBox.Text + "' where Equipments= '" + "5" + "';";
                MySqlConnection conDatabase5 = new MySqlConnection(myConnection5);
                MySqlCommand cmdDatabase5 = new MySqlCommand(Query5, conDatabase5);
                MySqlDataReader myReader5;

                try
                {
                    conDatabase5.Open();
                    myReader5 = cmdDatabase5.ExecuteReader();
                    while (myReader5.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (EquipmentType_comboBox.Text == "Adesso Ergo Mouse e10")
            {
                float value1, value2, result = 0;
                value1 = float.Parse(Adesso_txtBox.Text);
                value2 = float.Parse(QuantitiesGiven_ComboBox.Text);
                result = value1 - value2;
                Adesso_txtBox.Text = result.ToString();
                // this is the submit button to assign equipment and update sql data
                string myConnection6 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                string Query6 = "update database.wirelesslogitech set Count= '" + this.Adesso_txtBox.Text + "' where Equipments= '" + "6" + "';";
                MySqlConnection conDatabase6 = new MySqlConnection(myConnection6);
                MySqlCommand cmdDatabase6 = new MySqlCommand(Query6, conDatabase6);
                MySqlDataReader myReader6;

                try
                {
                    conDatabase6.Open();
                    myReader6 = cmdDatabase6.ExecuteReader();
                    while (myReader6.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (EquipmentType_comboBox.Text == "Logitech M570 Ergo Mouse")
            {
                float value1, value2, result = 0;
                value1 = float.Parse(ErgoMousetxtBox.Text);
                value2 = float.Parse(QuantitiesGiven_ComboBox.Text);
                result = value1 - value2;
                ErgoMousetxtBox.Text = result.ToString();
                // this is the submit button to assign equipment and update sql data
                string myConnection7 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                string Query7 = "update database.wirelesslogitech set Count= '" + this.ErgoMousetxtBox.Text + "' where Equipments= '" + "7" + "';";
                MySqlConnection conDatabase7 = new MySqlConnection(myConnection7);
                MySqlCommand cmdDatabase7 = new MySqlCommand(Query7, conDatabase7);
                MySqlDataReader myReader7;

                try
                {
                    conDatabase7.Open();
                    myReader7 = cmdDatabase7.ExecuteReader();
                    while (myReader7.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (EquipmentType_comboBox.Text == "Headphones")
            {
                float value1, value2, result = 0;
                value1 = float.Parse(HeadPhonestxtBox.Text);
                value2 = float.Parse(QuantitiesGiven_ComboBox.Text);
                result = value1 - value2;
                HeadPhonestxtBox.Text = result.ToString();
                // this is the submit button to assign equipment and update sql data
                string myConnection8 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
                string Query8 = "update database.wirelesslogitech set Count= '" + this.HeadPhonestxtBox.Text + "' where Equipments= '" + "8" + "';";
                MySqlConnection conDatabase8 = new MySqlConnection(myConnection8);
                MySqlCommand cmdDatabase8 = new MySqlCommand(Query8, conDatabase8);
                MySqlDataReader myReader8;

                try
                {
                    conDatabase8.Open();
                    myReader8 = cmdDatabase8.ExecuteReader();
                    while (myReader8.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            // Convert textbox to intergers and if they are less than 5 in stock, automatically send email notification to lisa tom
            //{
                //int SK = Convert.ToInt32(StandKeyboardtxtbox.Text);
                //int MK = Convert.ToInt32(MiniKeyboard_txtBox.Text);
                //int EK = Convert.ToInt32(ErgoKeyboardtxtBox.Text);
                //int SM = Convert.ToInt32(MousetxtBox.Text);
                //int KM = Convert.ToInt32(Kensington_txtBox.Text);
                //int AM = Convert.ToInt32(Adesso_txtBox.Text);
                //int LM = Convert.ToInt32(ErgoMousetxtBox.Text);
                //int HP = Convert.ToInt32(HeadPhonestxtBox.Text);
                //int Total = 3;
                //float SK, MK, EK, SM, KM, AM, LM, HP, Total = 3;
                //SK = float.Parse(StandKeyboardtxtbox.Text);
                //MK = float.Parse(MiniKeyboard_txtBox.Text);
                //EK = float.Parse(ErgoKeyboardtxtBox.Text);
                //SM = float.Parse(MousetxtBox.Text);
                //KM = float.Parse(Kensington_txtBox.Text);
                //AM = float.Parse(Adesso_txtBox.Text);
                //LM = float.Parse(ErgoMousetxtBox.Text);
                //HP = float.Parse(HeadPhonestxtBox.Text);
                //int value;
                

                //if (Int32.TryParse(StandKeyboardtxtbox.Text, out value))
                //{
                //    if (value <= 3 && EquipmentType_comboBox.Text == "Wireless Logitech Keyboard")
                //    try
                //    {
                //        Outlook.Application oApp = new Outlook.Application();
                //        Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                //        oMsg.HTMLBody = "Hello Lisa Tom"
                //          + "<br /> <br /> An important reminder that" + " " + EquipmentType_comboBox.Text + " will need to be restock soon.  The current inventory count is 3 or less";
                //        oMsg.Subject = ("Low Inventory Reminder --" + " " + EquipmentType_comboBox.Text);
                //        oMsg.To = "Jacky.Lam@dds.ca.gov";
                //        ((Outlook._MailItem)oMsg).Send();
                //        oMsg = null;
                //        oApp = null;
                //    }
                //    catch (System.Exception ex)
                //    {
                //        MessageBox.Show(ex.Message);
                //    }
                //}
                    //|| MK <= Total && EquipmentType_comboBox.Text == "HP Mini Keyboard" || EK <= Total && EquipmentType_comboBox.Text == "Microsoft 4000 Ergo Keyboard" || SM <= Total && EquipmentType_comboBox.Text == "Wireless Logitech Mouse" || KM <= Total && EquipmentType_comboBox.Text == "Kensington Trackball Mouse" || AM <= Total && EquipmentType_comboBox.Text == "Adesso Ergo Mouse e10" || LM <= Total && EquipmentType_comboBox.Text == "Logitech M570 Ergo Mouse" || HP <= Total && EquipmentType_comboBox.Text == "Headphones")
                //{
                //    Outlook.Application oApp = new Outlook.Application();
                //    Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                //    oMsg.HTMLBody = "Hello Lisa Tom"
                //        + "<br /> <br /> An important reminder that" + " " + EquipmentType_comboBox.Text + " will need to be restock soon.  The current inventory count is 3 or less";
                //    oMsg.Subject = ("Low Inventory Reminder --" + " " + EquipmentType_comboBox.Text);
                //    oMsg.To = "Jacky.Lam@dds.ca.gov";
                //    ((Outlook._MailItem)oMsg).Send();
                //    oMsg = null;
                //    oApp = null;


                //}

           // }


            MessageBox.Show("Equipment Has Been Distributed And Log.  Current Inventory Count Is Shown.  Please Notify Lisa If Inventory Count Is Low");
        }

        //private void BindGrid()
        //{
        //    // show sql data upon open

        //    dataGridView1.DefaultCellStyle.BackColor = Color.Khaki;
        //    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;
        //    dataGridView1.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
        //    dataGridView1.ReadOnly = true;
        //    string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
        //    MySqlConnection conDatabase = new MySqlConnection(myConnection);
        //    sda = new MySqlDataAdapter("Select * from database.newisdiventory;", conDatabase);
        //    dt = new DataTable();
        //    sda.Fill(dt);
        //    dataGridView1.DataSource = dt;

        //    dataGridView2.DefaultCellStyle.BackColor = Color.Khaki;
        //    dataGridView2.DefaultCellStyle.SelectionBackColor = Color.White;
        //    dataGridView2.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
        //    dataGridView2.ReadOnly = true;
        //    string myConnection1 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
        //    MySqlConnection conDatabase1 = new MySqlConnection(myConnection1);
        //    sda = new MySqlDataAdapter("Select * from database.wirelesslogitech;", conDatabase1);
        //    dt = new DataTable();
        //    sda.Fill(dt);
        //    dataGridView2.DataSource = dt;

        //}

        //private void BindGrid(2)

        //{
            //dataGridView2.DefaultCellStyle.BackColor = Color.Khaki;
            //dataGridView2.DefaultCellStyle.SelectionBackColor = Color.White;
            //dataGridView2.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            //dataGridView2.ReadOnly = true;
            //string myConnection1 = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            //MySqlConnection conDatabase1 = new MySqlConnection(myConnection1);
            //sda = new MySqlDataAdapter("Select * from database.wirelesslogitech;", conDatabase1);
            //dt = new DataTable();
            //sda.Fill(dt);
            //dataGridView2.DataSource = dt;
        //}

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
            // this is the submit button to assign equipment and update sql data
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "update database.wirelesslogitech set Count= '" + this.StandKeyboardtxtbox.Text + "' where Equipments= '" + "1" + "';";
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
            MessageBox.Show("Wireless Logitech Keyboard New Amount Has Been Reflected");
        }

        private void EkConfirm_btn_Click(object sender, EventArgs e)
        {
            // restock ergo keyboard
            ErgoKeyboardtxtBox.Text = ReStockEKtxtBox.Text;
            // this is the submit button to assign equipment and update sql data
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "update database.wirelesslogitech set Count= '" + this.ErgoKeyboardtxtBox.Text + "' where Equipments= '" + "3" + "';";
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
            // this is the submit button to assign equipment and update sql data
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "update database.wirelesslogitech set Count= '" + this.MousetxtBox.Text + "' where Equipments= '" + "4" + "';";
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
            MessageBox.Show("Wireless Logitech Mouse New Amount Has Been Reflected");
        }

        private void EMouseConfirm_btn_Click(object sender, EventArgs e)
        {
            ErgoMousetxtBox.Text = ReStockEMousetxtBox.Text;
            // this is the submit button to assign equipment and update sql data
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "update database.wirelesslogitech set Count= '" + this.ErgoMousetxtBox.Text + "' where Equipments= '" + "7" + "';";
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
            MessageBox.Show("Logitech M570 Ergo Mouse New Amount Has Been Reflected");
        }

        private void HeadphonesConfirm_btn_Click(object sender, EventArgs e)
        {
            HeadPhonestxtBox.Text = ReStockHeadphonestxtBox.Text;
            // this is the submit button to assign equipment and update sql data
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "update database.wirelesslogitech set Count= '" + this.HeadPhonestxtBox.Text + "' where Equipments= '" + "8" + "';";
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
            MessageBox.Show("Headphones New Amount Has Been Reflected");
        }

        private void MiniConfirm_btn_Click(object sender, EventArgs e)
        {
            MiniKeyboard_txtBox.Text = RestockMini_txtBox.Text;
            // this is the submit button to assign equipment and update sql data
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "update database.wirelesslogitech set Count= '" + this.MiniKeyboard_txtBox.Text + "' where Equipments= '" + "2" + "';";
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
            MessageBox.Show("Mini Keyboard New Amount Has Been Reflected");
        }

        private void KensingtonConfirm_btn_Click(object sender, EventArgs e)
        {
            Kensington_txtBox.Text = RestockKensington_txtBox.Text;
            // this is the submit button to assign equipment and update sql data
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "update database.wirelesslogitech set Count= '" + this.Kensington_txtBox.Text + "' where Equipments= '" + "5" + "';";
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
            MessageBox.Show("Kensington Trackball Mouse New Amount Has Been Reflected");
        }

        private void AdessoConfirm_btn_Click(object sender, EventArgs e)
        {
            Adesso_txtBox.Text = RestockAdesso_txtBox.Text;
            // this is the submit button to assign equipment and update sql data
            string myConnection = "datasource=10.96.75.20;port=3306;username=root;password=Root";
            string Query = "update database.wirelesslogitech set Count= '" + this.Adesso_txtBox.Text + "' where Equipments= '" + "6" + "';";
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
            MessageBox.Show("Adesso Ergo Mouse e10 New Amount Has Been Reflected");
        }

 
    }
}
