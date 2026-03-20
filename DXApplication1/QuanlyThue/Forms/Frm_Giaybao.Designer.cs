namespace QuanlyThue.Forms
{
    partial class Frm_Giaybao
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Giaybao));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cmdFilterGB = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.exportTCKTTheongay = new System.Windows.Forms.ToolStripMenuItem();
            this.exportTCKTTheobang = new System.Windows.Forms.ToolStripMenuItem();
            this.exportExcelMisaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdChuyendl = new System.Windows.Forms.ToolStripButton();
            this.cmdCloseGB = new System.Windows.Forms.ToolStripButton();
            this.groupGiaybao = new DevExpress.XtraEditors.GroupControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.radioChuain = new System.Windows.Forms.RadioButton();
            this.radioDain = new System.Windows.Forms.RadioButton();
            this.txtTungay = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDenngay = new System.Windows.Forms.DateTimePicker();
            this.gridGiaybao = new DevExpress.XtraGrid.GridControl();
            this.ViewGiaybao = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.superGridControl1 = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.gridColumn1 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn2 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn3 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn4 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn5 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn6 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn7 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn8 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn9 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn10 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn15 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn11 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn12 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn13 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn14 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn16 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn17 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupGiaybao)).BeginInit();
            this.groupGiaybao.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGiaybao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewGiaybao)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdFilterGB,
            this.toolStripDropDownButton1,
            this.cmdChuyendl,
            this.cmdCloseGB});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1473, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cmdFilterGB
            // 
            this.cmdFilterGB.Image = global::QuanlyThue.Properties.Resources.Report;
            this.cmdFilterGB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdFilterGB.Name = "cmdFilterGB";
            this.cmdFilterGB.Size = new System.Drawing.Size(93, 24);
            this.cmdFilterGB.Text = "Lọc Dữ Liệu";
            this.cmdFilterGB.Click += new System.EventHandler(this.cmdFilterGB_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportTCKTTheongay,
            this.exportTCKTTheobang,
            this.exportExcelMisaToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::QuanlyThue.Properties.Resources.xlsx_file_format_extension;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(104, 24);
            this.toolStripDropDownButton1.Text = "Export Excel";
            // 
            // exportTCKTTheongay
            // 
            this.exportTCKTTheongay.Image = global::QuanlyThue.Properties.Resources.Report;
            this.exportTCKTTheongay.Name = "exportTCKTTheongay";
            this.exportTCKTTheongay.Size = new System.Drawing.Size(330, 22);
            this.exportTCKTTheongay.Text = "Export Excel Giấy Báo Đã Chốt Theo Ngày (TCKT)";
            this.exportTCKTTheongay.Click += new System.EventHandler(this.exportTCKTTheongay_Click);
            // 
            // exportTCKTTheobang
            // 
            this.exportTCKTTheobang.Image = global::QuanlyThue.Properties.Resources.xlsx_file_format_extension;
            this.exportTCKTTheobang.Name = "exportTCKTTheobang";
            this.exportTCKTTheobang.Size = new System.Drawing.Size(330, 22);
            this.exportTCKTTheobang.Text = "Export Excel Giấy Báo Theo Bảng (TCKT)";
            this.exportTCKTTheobang.Click += new System.EventHandler(this.exportTCKTTheobang_Click);
            // 
            // exportExcelMisaToolStripMenuItem
            // 
            this.exportExcelMisaToolStripMenuItem.Image = global::QuanlyThue.Properties.Resources.Logo_Misa_2;
            this.exportExcelMisaToolStripMenuItem.Name = "exportExcelMisaToolStripMenuItem";
            this.exportExcelMisaToolStripMenuItem.Size = new System.Drawing.Size(330, 22);
            this.exportExcelMisaToolStripMenuItem.Text = "Export Excel Misa";
            this.exportExcelMisaToolStripMenuItem.Click += new System.EventHandler(this.exportExcelMisaToolStripMenuItem_Click);
            // 
            // cmdChuyendl
            // 
            this.cmdChuyendl.Image = global::QuanlyThue.Properties.Resources.Refesh;
            this.cmdChuyendl.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdChuyendl.Name = "cmdChuyendl";
            this.cmdChuyendl.Size = new System.Drawing.Size(115, 24);
            this.cmdChuyendl.Text = "Chuyển Dữ Liệu";
            this.cmdChuyendl.Click += new System.EventHandler(this.cmdChuyendl_Click);
            // 
            // cmdCloseGB
            // 
            this.cmdCloseGB.Image = global::QuanlyThue.Properties.Resources._329;
            this.cmdCloseGB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdCloseGB.Name = "cmdCloseGB";
            this.cmdCloseGB.Size = new System.Drawing.Size(60, 24);
            this.cmdCloseGB.Text = "Đóng";
            this.cmdCloseGB.Click += new System.EventHandler(this.cmdCloseGB_Click);
            // 
            // groupGiaybao
            // 
            this.groupGiaybao.Appearance.BackColor = System.Drawing.Color.IndianRed;
            this.groupGiaybao.Appearance.Options.UseBackColor = true;
            this.groupGiaybao.Appearance.Options.UseForeColor = true;
            this.groupGiaybao.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupGiaybao.AppearanceCaption.ForeColor = System.Drawing.Color.Black;
            this.groupGiaybao.AppearanceCaption.Options.UseFont = true;
            this.groupGiaybao.AppearanceCaption.Options.UseForeColor = true;
            this.groupGiaybao.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("groupGiaybao.CaptionImageOptions.Image")));
            this.groupGiaybao.Controls.Add(this.groupBox2);
            this.groupGiaybao.Controls.Add(this.groupBox1);
            this.groupGiaybao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupGiaybao.Location = new System.Drawing.Point(0, 27);
            this.groupGiaybao.Name = "groupGiaybao";
            this.groupGiaybao.Size = new System.Drawing.Size(1473, 615);
            this.groupGiaybao.TabIndex = 4;
            this.groupGiaybao.Text = "Bảng liệt kê giấy báo";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Controls.Add(this.gridGiaybao);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(2, 169);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1469, 444);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tùy chọn giấy báo";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioChuain, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioDain, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtTungay, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDenngay, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1463, 29);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(165, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Từ ngày:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioChuain
            // 
            this.radioChuain.AutoSize = true;
            this.radioChuain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioChuain.Location = new System.Drawing.Point(975, 3);
            this.radioChuain.Name = "radioChuain";
            this.radioChuain.Size = new System.Drawing.Size(156, 23);
            this.radioChuain.TabIndex = 5;
            this.radioChuain.Text = "Chưa in";
            this.radioChuain.UseVisualStyleBackColor = true;
            this.radioChuain.CheckedChanged += new System.EventHandler(this.radioChuain_CheckedChanged);
            // 
            // radioDain
            // 
            this.radioDain.AutoSize = true;
            this.radioDain.Checked = true;
            this.radioDain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioDain.Location = new System.Drawing.Point(813, 3);
            this.radioDain.Name = "radioDain";
            this.radioDain.Size = new System.Drawing.Size(156, 23);
            this.radioDain.TabIndex = 4;
            this.radioDain.TabStop = true;
            this.radioDain.Text = "Đã in";
            this.radioDain.UseVisualStyleBackColor = true;
            this.radioDain.CheckedChanged += new System.EventHandler(this.radioDain_CheckedChanged);
            // 
            // txtTungay
            // 
            this.txtTungay.CalendarTitleForeColor = System.Drawing.Color.Blue;
            this.txtTungay.CalendarTrailingForeColor = System.Drawing.Color.Blue;
            this.txtTungay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTungay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtTungay.Location = new System.Drawing.Point(327, 3);
            this.txtTungay.Name = "txtTungay";
            this.txtTungay.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtTungay.Size = new System.Drawing.Size(156, 21);
            this.txtTungay.TabIndex = 1;
            this.txtTungay.ValueChanged += new System.EventHandler(this.txtTungay_ValueChanged);
            this.txtTungay.Validating += new System.ComponentModel.CancelEventHandler(this.txtTungay_Validating);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(489, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "Đến ngày:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDenngay
            // 
            this.txtDenngay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDenngay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDenngay.Location = new System.Drawing.Point(651, 3);
            this.txtDenngay.Name = "txtDenngay";
            this.txtDenngay.Size = new System.Drawing.Size(156, 21);
            this.txtDenngay.TabIndex = 3;
            this.txtDenngay.ValueChanged += new System.EventHandler(this.txtDenngay_ValueChanged);
            this.txtDenngay.Validating += new System.ComponentModel.CancelEventHandler(this.txtDenngay_Validating);
            // 
            // gridGiaybao
            // 
            this.gridGiaybao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridGiaybao.Location = new System.Drawing.Point(3, 17);
            this.gridGiaybao.MainView = this.ViewGiaybao;
            this.gridGiaybao.Name = "gridGiaybao";
            this.gridGiaybao.Size = new System.Drawing.Size(1463, 424);
            this.gridGiaybao.TabIndex = 7;
            this.gridGiaybao.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ViewGiaybao});
            // 
            // ViewGiaybao
            // 
            this.ViewGiaybao.GridControl = this.gridGiaybao;
            this.ViewGiaybao.Name = "ViewGiaybao";
            this.ViewGiaybao.OptionsView.ShowAutoFilterRow = true;
            this.ViewGiaybao.OptionsView.ShowFooter = true;
            this.ViewGiaybao.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.ViewGiaybao_CustomDrawRowIndicator_1);
            this.ViewGiaybao.DoubleClick += new System.EventHandler(this.ViewGiaybao_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Salmon;
            this.groupBox1.Controls.Add(this.superGridControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(2, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1469, 136);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Giấy báo yêu cầu hủy";
            // 
            // superGridControl1
            // 
            this.superGridControl1.BackColor = System.Drawing.Color.White;
            this.superGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superGridControl1.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.superGridControl1.ForeColor = System.Drawing.Color.Black;
            this.superGridControl1.Location = new System.Drawing.Point(3, 17);
            this.superGridControl1.Name = "superGridControl1";
            // 
            // 
            // 
            this.superGridControl1.PrimaryGrid.Checked = true;
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn1);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn2);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn3);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn4);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn5);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn6);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn7);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn8);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn9);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn10);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn15);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn11);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn12);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn13);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn14);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn16);
            this.superGridControl1.PrimaryGrid.Columns.Add(this.gridColumn17);
            this.superGridControl1.PrimaryGrid.ShowRowGridIndex = true;
            this.superGridControl1.PrimaryGrid.UseAlternateRowStyle = true;
            this.superGridControl1.Size = new System.Drawing.Size(1463, 116);
            this.superGridControl1.TabIndex = 0;
            this.superGridControl1.Text = "superGridControl1";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Name = "Lương tháng";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Name = "Mã đơn vị";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Name = "Số hóa đơn";
            // 
            // gridColumn4
            // 
            this.gridColumn4.DefaultNewRowCellValue = "0";
            this.gridColumn4.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridDoubleInputEditControl);
            this.gridColumn4.Name = "Tổng bảo hiểm";
            // 
            // gridColumn5
            // 
            this.gridColumn5.DefaultNewRowCellValue = "0";
            this.gridColumn5.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridDoubleInputEditControl);
            this.gridColumn5.Name = "Tổng thuế";
            // 
            // gridColumn6
            // 
            this.gridColumn6.DefaultNewRowCellValue = "0";
            this.gridColumn6.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridDoubleInputEditControl);
            this.gridColumn6.Name = "Lương NV";
            // 
            // gridColumn7
            // 
            this.gridColumn7.DefaultNewRowCellValue = "0";
            this.gridColumn7.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridDoubleInputEditControl);
            this.gridColumn7.Name = "Dịch vụ phí";
            // 
            // gridColumn8
            // 
            this.gridColumn8.DefaultNewRowCellValue = "0";
            this.gridColumn8.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridDoubleInputEditControl);
            this.gridColumn8.Name = "Phí CĐ";
            // 
            // gridColumn9
            // 
            this.gridColumn9.DefaultNewRowCellValue = "0";
            this.gridColumn9.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridDoubleInputEditControl);
            this.gridColumn9.Name = "Phí CĐ 138";
            // 
            // gridColumn10
            // 
            this.gridColumn10.DefaultNewRowCellValue = "0";
            this.gridColumn10.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridDoubleInputEditControl);
            this.gridColumn10.Name = "Tổng tiền";
            // 
            // gridColumn15
            // 
            this.gridColumn15.Name = "Người làm";
            // 
            // gridColumn11
            // 
            this.gridColumn11.DefaultNewRowCellValue = "0";
            this.gridColumn11.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridDateTimeInputEditControl);
            this.gridColumn11.Name = "Ngày in";
            // 
            // gridColumn12
            // 
            this.gridColumn12.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridDateTimeInputEditControl);
            this.gridColumn12.Name = "Ngày hủy";
            // 
            // gridColumn13
            // 
            this.gridColumn13.Name = "Nội dung hủy";
            // 
            // gridColumn14
            // 
            this.gridColumn14.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.gridColumn14.Name = "Ghi chú";
            // 
            // gridColumn16
            // 
            this.gridColumn16.CellStyles.Default.ImageAlignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
            this.gridColumn16.DefaultNewRowCellValue = "true";
            this.gridColumn16.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridSwitchButtonEditControl);
            this.gridColumn16.Name = "Hủy chốt";
            // 
            // gridColumn17
            // 
            this.gridColumn17.Name = "ID";
            this.gridColumn17.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Frm_Giaybao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1473, 642);
            this.Controls.Add(this.groupGiaybao);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Frm_Giaybao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý giấy báo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Frm_Giaybao_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupGiaybao)).EndInit();
            this.groupGiaybao.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGiaybao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewGiaybao)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton cmdFilterGB;
        private System.Windows.Forms.ToolStripButton cmdCloseGB;
        private System.Windows.Forms.DateTimePicker txtTungay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioChuain;
        private System.Windows.Forms.RadioButton radioDain;
        private System.Windows.Forms.DateTimePicker txtDenngay;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.GroupControl groupGiaybao;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem exportTCKTTheongay;
        private System.Windows.Forms.ToolStripMenuItem exportTCKTTheobang;
        private System.Windows.Forms.ToolStripMenuItem exportExcelMisaToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton cmdChuyendl;
        private DevExpress.XtraGrid.GridControl gridGiaybao;
        private DevExpress.XtraGrid.Views.Grid.GridView ViewGiaybao;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl superGridControl1;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn1;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn2;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn3;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn4;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn5;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn6;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn7;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn8;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn9;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn10;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn11;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn12;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn13;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn14;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn15;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn16;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn17;
    }
}