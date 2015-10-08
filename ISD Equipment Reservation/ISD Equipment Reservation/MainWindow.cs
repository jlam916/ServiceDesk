using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ISD_Equipment_Reservation
{
    public partial class MainWindow : Form
    {
        Form1 EquipmentReservationForm;
        Inventory InventoryChecklistForm;
        Conference_Room_Set_Ups ConferenceRoomForm;
        

        public MainWindow()
        {
            InitializeComponent();
            EquipmentReservationForm = new Form1(this);
            InventoryChecklistForm = new Inventory(this);
            ConferenceRoomForm = new Conference_Room_Set_Ups(this);
        }

        private void EquipmentReservation_btn_Click(object sender, EventArgs e)
        {
            EquipmentReservationForm.ShowDialog();
        }

        private void Inventory_btn_Click(object sender, EventArgs e)
        {
            InventoryChecklistForm.ShowDialog();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void RoomSetUps_btn_Click(object sender, EventArgs e)
        {
            ConferenceRoomForm.ShowDialog();
        }
    }
}
