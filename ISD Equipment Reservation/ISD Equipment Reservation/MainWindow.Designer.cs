namespace ISD_Equipment_Reservation
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Question_lbl = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.RoomSetUps_btn = new System.Windows.Forms.Button();
            this.Inventory_btn = new System.Windows.Forms.Button();
            this.EquipmentReservation_btn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Question_lbl
            // 
            this.Question_lbl.AutoSize = true;
            this.Question_lbl.Font = new System.Drawing.Font("Comic Sans MS", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Question_lbl.Location = new System.Drawing.Point(414, 109);
            this.Question_lbl.Name = "Question_lbl";
            this.Question_lbl.Size = new System.Drawing.Size(435, 40);
            this.Question_lbl.TabIndex = 1;
            this.Question_lbl.Text = "What Would You Like To Do?";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(142, 216);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(979, 206);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33444F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33444F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33112F));
            this.tableLayoutPanel1.Controls.Add(this.RoomSetUps_btn, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Inventory_btn, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.EquipmentReservation_btn, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(976, 203);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // RoomSetUps_btn
            // 
            this.RoomSetUps_btn.BackColor = System.Drawing.Color.Cornsilk;
            this.RoomSetUps_btn.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold);
            this.RoomSetUps_btn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RoomSetUps_btn.Location = new System.Drawing.Point(341, 3);
            this.RoomSetUps_btn.Name = "RoomSetUps_btn";
            this.RoomSetUps_btn.Size = new System.Drawing.Size(292, 197);
            this.RoomSetUps_btn.TabIndex = 1;
            this.RoomSetUps_btn.Text = "Conference Room Set-Ups";
            this.RoomSetUps_btn.UseVisualStyleBackColor = false;
            this.RoomSetUps_btn.Click += new System.EventHandler(this.RoomSetUps_btn_Click);
            // 
            // Inventory_btn
            // 
            this.Inventory_btn.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Inventory_btn.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold);
            this.Inventory_btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Inventory_btn.Location = new System.Drawing.Point(679, 3);
            this.Inventory_btn.Name = "Inventory_btn";
            this.Inventory_btn.Size = new System.Drawing.Size(294, 197);
            this.Inventory_btn.TabIndex = 2;
            this.Inventory_btn.Text = "Equipment Iventory";
            this.Inventory_btn.UseVisualStyleBackColor = false;
            this.Inventory_btn.Click += new System.EventHandler(this.Inventory_btn_Click);
            // 
            // EquipmentReservation_btn
            // 
            this.EquipmentReservation_btn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.EquipmentReservation_btn.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EquipmentReservation_btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.EquipmentReservation_btn.Location = new System.Drawing.Point(3, 3);
            this.EquipmentReservation_btn.Name = "EquipmentReservation_btn";
            this.EquipmentReservation_btn.Size = new System.Drawing.Size(292, 197);
            this.EquipmentReservation_btn.TabIndex = 0;
            this.EquipmentReservation_btn.Text = "Reserve ISD Equipment";
            this.EquipmentReservation_btn.UseVisualStyleBackColor = false;
            this.EquipmentReservation_btn.Click += new System.EventHandler(this.EquipmentReservation_btn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::ISD_Equipment_Reservation.Properties.Resources.Main_Page_2;
            this.pictureBox1.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1265, 76);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 585);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Question_lbl);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainWindow";
            this.Text = "Main Window";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label Question_lbl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button RoomSetUps_btn;
        private System.Windows.Forms.Button Inventory_btn;
        private System.Windows.Forms.Button EquipmentReservation_btn;
    }
}