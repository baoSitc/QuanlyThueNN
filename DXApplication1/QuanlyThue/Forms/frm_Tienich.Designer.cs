namespace QuanlyThue.Forms
{
    partial class frm_Tienich
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Tienich));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cmdExit = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioDoiTenHoaDon = new System.Windows.Forms.RadioButton();
            this.radioOp4 = new System.Windows.Forms.RadioButton();
            this.cmdThuchien = new DevExpress.XtraEditors.SimpleButton();
            this.txtChuoi = new System.Windows.Forms.TextBox();
            this.radioOp3 = new System.Windows.Forms.RadioButton();
            this.radioOp2 = new System.Windows.Forms.RadioButton();
            this.radioOp1 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(443, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cmdExit
            // 
            this.cmdExit.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdExit.Image = ((System.Drawing.Image)(resources.GetObject("cmdExit.Image")));
            this.cmdExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(58, 22);
            this.cmdExit.Text = "Thoát";
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(443, 250);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Đổi tên file hàng loạt";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.radioDoiTenHoaDon);
            this.panel1.Controls.Add(this.radioOp4);
            this.panel1.Controls.Add(this.cmdThuchien);
            this.panel1.Controls.Add(this.txtChuoi);
            this.panel1.Controls.Add(this.radioOp3);
            this.panel1.Controls.Add(this.radioOp2);
            this.panel1.Controls.Add(this.radioOp1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 230);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // radioDoiTenHoaDon
            // 
            this.radioDoiTenHoaDon.AutoSize = true;
            this.radioDoiTenHoaDon.ForeColor = System.Drawing.Color.Blue;
            this.radioDoiTenHoaDon.Location = new System.Drawing.Point(9, 122);
            this.radioDoiTenHoaDon.Name = "radioDoiTenHoaDon";
            this.radioDoiTenHoaDon.Size = new System.Drawing.Size(170, 17);
            this.radioDoiTenHoaDon.TabIndex = 6;
            this.radioDoiTenHoaDon.TabStop = true;
            this.radioDoiTenHoaDon.Text = "Đổi tên File Hóa đơn hàng loạt";
            this.radioDoiTenHoaDon.UseVisualStyleBackColor = true;
            // 
            // radioOp4
            // 
            this.radioOp4.AutoSize = true;
            this.radioOp4.Location = new System.Drawing.Point(9, 99);
            this.radioOp4.Name = "radioOp4";
            this.radioOp4.Size = new System.Drawing.Size(109, 17);
            this.radioOp4.TabIndex = 5;
            this.radioOp4.TabStop = true;
            this.radioOp4.Text = "Tim va doi ten file";
            this.radioOp4.UseVisualStyleBackColor = true;
            // 
            // cmdThuchien
            // 
            this.cmdThuchien.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("cmdThuchien.ImageOptions.Image")));
            this.cmdThuchien.Location = new System.Drawing.Point(239, 72);
            this.cmdThuchien.Name = "cmdThuchien";
            this.cmdThuchien.Size = new System.Drawing.Size(90, 23);
            this.cmdThuchien.TabIndex = 4;
            this.cmdThuchien.Text = "Thực hiện";
            this.cmdThuchien.Click += new System.EventHandler(this.cmdThuchien_Click);
            // 
            // txtChuoi
            // 
            this.txtChuoi.Enabled = false;
            this.txtChuoi.Location = new System.Drawing.Point(9, 72);
            this.txtChuoi.Name = "txtChuoi";
            this.txtChuoi.Size = new System.Drawing.Size(224, 21);
            this.txtChuoi.TabIndex = 3;
            // 
            // radioOp3
            // 
            this.radioOp3.AutoSize = true;
            this.radioOp3.Location = new System.Drawing.Point(9, 49);
            this.radioOp3.Name = "radioOp3";
            this.radioOp3.Size = new System.Drawing.Size(234, 17);
            this.radioOp3.TabIndex = 2;
            this.radioOp3.Text = "Giữ 5 ký tự đầu của file và thêm chuổi ký tự";
            this.radioOp3.UseVisualStyleBackColor = true;
            // 
            // radioOp2
            // 
            this.radioOp2.AutoSize = true;
            this.radioOp2.Location = new System.Drawing.Point(9, 26);
            this.radioOp2.Name = "radioOp2";
            this.radioOp2.Size = new System.Drawing.Size(224, 17);
            this.radioOp2.TabIndex = 1;
            this.radioOp2.Text = "Giữ nguyên phần tên và thêm chuỗi ký tự";
            this.radioOp2.UseVisualStyleBackColor = true;
            // 
            // radioOp1
            // 
            this.radioOp1.AutoSize = true;
            this.radioOp1.Checked = true;
            this.radioOp1.Location = new System.Drawing.Point(9, 3);
            this.radioOp1.Name = "radioOp1";
            this.radioOp1.Size = new System.Drawing.Size(430, 17);
            this.radioOp1.TabIndex = 0;
            this.radioOp1.TabStop = true;
            this.radioOp1.Text = "Đổi tên file thành tất cả chữ HOA, loại bỏ khoản trắng, thay thế bằng dấu dạch dư" +
    "ới";
            this.radioOp1.UseVisualStyleBackColor = true;
            this.radioOp1.CheckedChanged += new System.EventHandler(this.radioOp1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Blue;
            this.button1.Location = new System.Drawing.Point(80, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(187, 37);
            this.button1.TabIndex = 7;
            this.button1.Text = "Join PDF";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frm_Tienich
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 270);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frm_Tienich";
            this.Text = "Tiện ích";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton cmdExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioOp1;
        private System.Windows.Forms.RadioButton radioOp2;
        private System.Windows.Forms.TextBox txtChuoi;
        private System.Windows.Forms.RadioButton radioOp3;
        private DevExpress.XtraEditors.SimpleButton cmdThuchien;
        private System.Windows.Forms.RadioButton radioOp4;
        private System.Windows.Forms.RadioButton radioDoiTenHoaDon;
        private System.Windows.Forms.Button button1;
    }
}