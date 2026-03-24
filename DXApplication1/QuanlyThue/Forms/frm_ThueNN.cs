using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bussiness;
using DevExpress.XtraGrid.Views.Grid;
using DataLayer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;

namespace QuanlyThue.Forms
{
    public partial class frm_ThueNN : DevExpress.XtraEditors.XtraForm
    {
        ThueNNBussiness _thuenn;
        bool them;
        public frm_ThueNN()
        {
            InitializeComponent();
        }
        void ShowHiheControl(bool t)
        {
            cmdAdd.Visible = t;
            cmdExit.Visible = t;
            cmdDel.Visible = t;
            cmdEdit.Visible = t;
            cmdSave.Visible = !t;
            cmdCancel.Visible = !t;
            cmdPrint.Visible = t;

        }

        private void frm_ThueNN_Load(object sender, EventArgs e)
        {
            _thuenn = new ThueNNBussiness();
            loadMadonvi(MyFunction._Nhomfc);
            ShowHiheControl(true);
            cmbThang.Text = DateTime.Now.ToString("MM");
            cmbNam.Text = DateTime.Now.ToString("yyyy");
            _Enable(false); them = false;

        }
        void loadMadonvi(string nhomfc)
        {
            string nhom2 = MyFunction.RunSQL_String("select ghichu from DSNV where manvql='" + MyFunction._UserName+ "'").Trim();
            searchMadonvi.Properties.DataSource = MyFunction.GetDataTable("select madv,madv+'-'+tendv as tendv from q_dmdv where (nhomfc='" + nhomfc + "' or nhomfc='"+nhom2+"') " +
                "and len(madv)>5 AND TRANGTHAI=N'Đang làm việc' order by madv");
            searchMadonvi.Properties.DisplayMember = "tendv";
            searchMadonvi.Properties.ValueMember = "madv";

        }
        void loadNhanvien(string MADV)
        {
            searchTennhanvien.Properties.DataSource = MyFunction.GetDataTable("select manv,hoten from q_hsc where madv='" + MADV + "' and tentrangthailamviec=N'Đang làm việc'");
            searchTennhanvien.Properties.DisplayMember = "hoten";
            searchTennhanvien.Properties.ValueMember = "manv";
            cmbMachuong.SelectedIndex = 0;

        }
        void _Enable(bool tt)
        {
            tableLayoutPanel5.Enabled= tableLayoutPanel6.Enabled = tt;// tableLayoutPanel2.Enabled = tt;
            //searchTennhanvien.Enabled = txtDiengiai.Enabled = txtDiscription.Enabled = txtKhac.Enabled = txtLuongUSD.Enabled = txtLuongVND.Enabled = txtPhucap.Enabled = txtSohd.Enabled = tt;
            //txtThuong.Enabled = txtTiennha.Enabled = txtTNCN.Enabled = txtTTN.Enabled = cmbKythue.Enabled = tt;

        }
        private void cmdAdd_Click(object sender, EventArgs e)
        {
            them = true; xoatrang();
            ShowHiheControl(false);
            _Enable(true);
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            them = false;
            ShowHiheControl(false); _Enable(true);
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            T_THUENN _t_thuenn = new T_THUENN();
            if (String.IsNullOrEmpty(searchMadonvi.EditValue.ToString()) || searchTennhanvien.EditValue == null)
            {
                MessageBox.Show("Bạn chưa chọn Mã Đơn Vị Hoặc chưa chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            _t_thuenn.MADV = searchMadonvi.EditValue.ToString().Substring(0, 5);
            _t_thuenn.MANV = searchTennhanvien.EditValue.ToString();
            _t_thuenn.HODEM = MyFunction.RunSQL_String("select hodem from q_hsc where manv='" + _t_thuenn.MANV + "'");
            _t_thuenn.TEN =_t_thuenn.HODEM+" "+  MyFunction.RunSQL_String("select ten from q_hsc where manv='" + _t_thuenn.MANV + "'");
           // _t_thuenn.LGTHANG = DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy");
            _t_thuenn.DNVHG = double.Parse(txtDNVHG.Text, new CultureInfo("vi-VN"));
            //_t_thuenn.NGAYTIENVE = double.Parse(txt.Text);
            if (cmbKythue.Text == "Tháng")
                _t_thuenn.KYTHUE = cmbThang.Text + "/" + cmbNam.Text;
            else if (cmbKythue.Text == "Quý 1")
                _t_thuenn.KYTHUE = "Q1" + "/" + cmbNam.Text;
            else if (cmbKythue.Text == "Quý 2")
                _t_thuenn.KYTHUE = "Q2" + "/" + cmbNam.Text;
            else if (cmbKythue.Text == "Quý 3")
                _t_thuenn.KYTHUE = "Q3" + "/" + cmbNam.Text;
            else if (cmbKythue.Text == "Quý 4")
                _t_thuenn.KYTHUE = "Q4" + "/" + cmbNam.Text;
            _t_thuenn.LCV_USD = double.Parse(txtLuongUSD.Text, new CultureInfo("vi-VN"));
            _t_thuenn.NVBHXH = double.Parse(txtBHNV.Text.Replace(",",""), new CultureInfo("vi-VN") );
            _t_thuenn.LGTNCN = double.Parse(txtLuongTNCN.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.TNCN = double.Parse(txtTNCN.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.TTN = double.Parse(txtTTN.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.DIENGIAI = cmbDiengiai.Text;
            _t_thuenn.DISCRIPTION = cmbDiscription.Text;
            _t_thuenn.DLCV = double.Parse(txtLuongVND.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.PDV = double.Parse(txtPDV.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.GT_BANTHAN = double.Parse(txtBT.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.GT_PHUTHUOC = double.Parse(txtNPT.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.LGTHANG = cmbThang.Text + cmbNam.Text;
            _t_thuenn.PHUCAP = double.Parse(txtPhucap.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.THUONG = double.Parse(txtThuong.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.TIENNHA = double.Parse(txtTiennha.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.KHAC = double.Parse(txtKhac.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.SOHD = txtSohd.Text.ToUpper();
            _t_thuenn.TONGTIEN_DN = double.Parse(txtTongtien_DN.Text.Replace(",", ""), new CultureInfo("vi-VN"));
            _t_thuenn.NGUOISD = MyFunction._UserName.ToUpper();
            _t_thuenn.TGLG = double.Parse(MyFunction.RunSQL_String("select TGLG from TIGIA where THANGNAM='" + _t_thuenn.LGTHANG + "'").Replace(",", ""));
            _t_thuenn.NGAYTAO = DateTime.Now.Date.ToString("dd/MM/yyyy");
            if (them) //Thêm mới
            {
                if (string.IsNullOrEmpty(txtSohd.Text) || string.IsNullOrEmpty(cmbKythue.Text))
                {
                    MessageBox.Show("Số hóa đơn và kỳ thuế không được để trống?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    return;
                }
                else if (!string.IsNullOrEmpty(MyFunction.RunSQL_String("select sohd from chotsolieu where DV='" + searchMadonvi.EditValue.ToString() + "' and SOHD='" + txtSohd.Text + "' and tinhtrang='1'")))
                {
                    MessageBox.Show("Số giấy báo này đã chốt, Bạn nhập lại số giấy báo!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    return;
                }
                else if (MessageBox.Show("Bạn có chắc chắn thêm dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    if (_thuenn.Insert(_t_thuenn) == 1)
                    {
                        MessageBox.Show("Đã thêm thành công dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //gridControlDanhsachGB.DataSource = _thuenn.Getbykey(searchMadonvi.EditValue.ToString().Substring(0, 5), null);
                        ShowHiheControl(true); _Enable(false); them = false;
                    }
                    else
                        MessageBox.Show("Đã có lỗi khi thêm dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else //Cập nhật lại Thuế Nước Ngoài
            {
                if (gridView1.RowCount > 0)
                    if (MessageBox.Show("Bạn có chắc chắn cập nhật lại dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        _t_thuenn.Id = int.Parse(gridView1.GetFocusedRowCellValue("Id").ToString());
                        if (_thuenn.Update(_t_thuenn) == 1)
                        {
                            MessageBox.Show("Đã cập nhật thành công dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowHiheControl(true); _Enable(false); them = false;

                        }
                        else
                            MessageBox.Show("Đã có lỗi khi cập nhật dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

            }
            gridControlDanhsachGB.DataSource = _thuenn.Getbykey(searchMadonvi.EditValue.ToString().Substring(0, 5), null);

        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            ShowHiheControl(true); _Enable(false);them=false;
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void searchMadonvi_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchMadonvi.EditValue.ToString()))
            {
                if(chkALLnv.Checked)
                gridControlDanhsachGB.DataSource = MyFunction.GetDataTable("select * from t_thuenn where madv='"+ searchMadonvi.EditValue.ToString().Substring(0, 5)+"' and manv in(select manv from q_hsc where madv='"+ searchMadonvi.EditValue.ToString()+"' and trangthai='1') order by id desc");
                else
                    gridControlDanhsachGB.DataSource = MyFunction.GetDataTable("select * from t_thuenn where madv='" + searchMadonvi.EditValue.ToString().Substring(0, 5) + "' and manv in(select manv from q_hsc where madv='" + searchMadonvi.EditValue.ToString() + "') order by id desc");

                //_thuenn.Getbykey(searchMadonvi.EditValue.ToString().Substring(0, 5), null);
                loadNhanvien(searchMadonvi.EditValue.ToString());
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridView1.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
            {
                if (e.Info.IsRowIndicator) //Nếu là dòng Indicator
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1;//Không hiển thị hình
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();//Số thứ tự tăng dần

                    }
                    SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);//Lấy kích thước của vùng hiển thị Text
                    Int32 _with = Convert.ToInt32(_size.Width + 20);
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, gridView1); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, gridView1); }));//Tăng kích thước nếu text vượt quá

            }
        }
        bool cal(Int32 _with, GridView _view)
        {
            _view.IndicatorWidth = _view.IndicatorWidth < _with ? _with : _view.IndicatorWidth;
            return true;
        }
        void gangiatri()
        {
            if (gridView1.RowCount > 0 && gridView1.FocusedRowHandle >= 0)
            {
                try
                {
                    searchTennhanvien.EditValue = gridView1.GetFocusedRowCellValue("MANV").ToString();
                    txtLuongUSD.Text = double.Parse(gridView1.GetFocusedRowCellValue("LCV_USD").ToString()).ToString("N0");
                    txtLuongVND.Text = double.Parse(gridView1.GetFocusedRowCellValue("DLCV").ToString()).ToString("N0");
                    txtPhucap.Text = double.Parse(gridView1.GetFocusedRowCellValue("PHUCAP").ToString()).ToString("N0");
                    txtThuong.Text = double.Parse(gridView1.GetFocusedRowCellValue("THUONG").ToString()).ToString("N0");
                    txtKhac.Text = double.Parse(gridView1.GetFocusedRowCellValue("KHAC").ToString()).ToString("N0");
                    txtTiennha.Text = double.Parse(gridView1.GetFocusedRowCellValue("TIENNHA").ToString()).ToString("N0");
                    txtLuongTNCN.Text = double.Parse(gridView1.GetFocusedRowCellValue("LGTNCN").ToString()).ToString("N0");
                    txtTNCN.Text = double.Parse(gridView1.GetFocusedRowCellValue("TNCN").ToString()).ToString("N0");
                    txtTTN.Text = double.Parse(gridView1.GetFocusedRowCellValue("TTN").ToString()).ToString("N0");
                    txtBT.Text = double.Parse(gridView1.GetFocusedRowCellValue("GT_BANTHAN").ToString()).ToString("N0");
                    txtNPT.Text = double.Parse(gridView1.GetFocusedRowCellValue("GT_PHUTHUOC").ToString()).ToString("N0");
                    txtBHNV.Text = double.Parse(gridView1.GetFocusedRowCellValue("NVBHXH").ToString()).ToString("N0");
                    txtDNVHG.Text = double.Parse(gridView1.GetFocusedRowCellValue("DNVHG").ToString()).ToString("N0");
                    txtTongtien_DN.Text = double.Parse(gridView1.GetFocusedRowCellValue("TONGTIEN_DN").ToString()).ToString("N0");
                    txtPDV.Text = double.Parse(gridView1.GetFocusedRowCellValue("PDV").ToString()).ToString("N0");
                    txtSohd.Text = MyFunction.RunSQL_String("select sohd from t_thuenn where id='" + gridView1.GetFocusedRowCellValue("Id").ToString() + "'");
                    cmbDiengiai.Text = MyFunction.RunSQL_String("select diengiai from t_thuenn where id='" + gridView1.GetFocusedRowCellValue("Id").ToString() + "'");
                    cmbDiscription.Text = MyFunction.RunSQL_String("select discription from t_thuenn where id='" + gridView1.GetFocusedRowCellValue("Id").ToString() + "'");
                    if (string.IsNullOrEmpty(MyFunction.RunSQL_String("select kythue from t_thuenn where id='" + gridView1.GetFocusedRowCellValue("Id").ToString() + "'")))
                        cmbKythue.Text = null;
                    else if (MyFunction.RunSQL_String("select kythue from t_thuenn where id='" + gridView1.GetFocusedRowCellValue("Id").ToString() + "'").Substring(0, 2) == "Q1")
                        cmbKythue.Text = "Quý 1";
                    else if (MyFunction.RunSQL_String("select kythue from t_thuenn where id='" + gridView1.GetFocusedRowCellValue("Id").ToString() + "'").Substring(0, 2) == "Q2")
                        cmbKythue.Text = "Quý 2";
                    else if (MyFunction.RunSQL_String("select kythue from t_thuenn where id='" + gridView1.GetFocusedRowCellValue("Id").ToString() + "'").Substring(0, 2) == "Q3")
                        cmbKythue.Text = "Quý 3";
                    else if (MyFunction.RunSQL_String("select kythue from t_thuenn where id='" + gridView1.GetFocusedRowCellValue("Id").ToString() + "'").Substring(0, 2) == "Q4")
                        cmbKythue.Text = "Quý 4";
                    else
                        cmbKythue.Text = "Tháng";
                    cmbThang.Text = gridView1.GetFocusedRowCellValue("LGTHANG").ToString().Substring(0, 2);
                    cmbNam.Text = gridView1.GetFocusedRowCellValue("LGTHANG").ToString().Substring(2, 4);
                    //Kiem tra so hoa don da chot so lieu chua
                    if (them == false)
                        if (!string.IsNullOrEmpty(MyFunction.RunSQL_String("select sohd from chotsolieu where DV='" + searchMadonvi.EditValue.ToString() + "' and SOHD='" + txtSohd.Text + "' and tinhtrang='1'")))
                        { cmdHuyGB.Enabled = true; cmdEdit.Visible = false; cmdDel.Visible = false; }
                        else
                        { cmdHuyGB.Enabled = false; cmdEdit.Visible = true; cmdDel.Visible = true; }



                }

                catch { }
            }
        }
        void xoatrang()
        {
            searchTennhanvien.Text = null;
            txtLuongUSD.Text = "0";
            txtLuongVND.Text = "0";
            txtPhucap.Text = "0";
            txtThuong.Text = "0";
            txtKhac.Text = "0";
            txtTiennha.Text = "0";
            txtLuongTNCN.Text = "0";
            txtTNCN.Text = "0";
            txtTTN.Text = "0";
            txtBT.Text = "11.000.000";
            txtNPT.Text = "4.400.000";
            txtBHNV.Text = "0";
            txtDNVHG.Text = "0";
            txtTongtien_DN.Text = "0";
            txtPDV.Text = "0";
            txtSohd.Text = null;
            cmbDiengiai.Text = null;
            cmbDiscription.Text = null;
            cmbThang.Text = DateTime.Now.ToString("MM");
            cmbNam.Text = DateTime.Now.ToString("yyyy");
        }

        private void gridView1_Click(object sender, EventArgs e)
        {

            gangiatri();

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gangiatri();
        }

        private void cmdDel_Click(object sender, EventArgs e)
        {
            T_THUENN _t_thuenn = new T_THUENN();
            if (MessageBox.Show("Bạn có chắc chắn cập nhật lại dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                _t_thuenn.Id = int.Parse(gridView1.GetFocusedRowCellValue("Id").ToString());
                if (_thuenn.DeleteByID(_t_thuenn) == 1)
                {
                    MessageBox.Show("Đã xóa thành công dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //ShowHiheControl(true); _Enable(false); them = false;
                    gridControlDanhsachGB.DataSource = _thuenn.Getbykey(searchMadonvi.EditValue.ToString().Substring(0, 5), null);

                }
                else
                    MessageBox.Show("Đã có lỗi khi cập nhật dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void cmdHuyGB_Click(object sender, EventArgs e)
        {

            InputBox.SetLanguage(InputBox.Language.English);

            //Save the DialogResult as res
            DialogResult res = InputBox.ShowDialog("Hãy nhập lý do hủy giấy báo",
            "Combo InputBox",   //Text message (mandatory), Title (optional)
                InputBox.Icon.Nothing, //Set icon type (default info)
                InputBox.Buttons.OkCancel, //Set buttons (default ok)
                InputBox.Type.TextBox, //Set type (default nothing)
                new string[] { "Giá trị 1", "Giá trị 2", "Giá trị 3" }, //String field as ComboBox items (default null)
                true, //Set visible in taskbar (default false)
                new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold)); //Set font (default by system)

            //Check InputBox result
            if (InputBox.ResultValue.Length > 5 && res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
            {
                if (MessageBox.Show("Bạn có chắc chắn yêu cầu hủy chốt giấy báo: " + txtSohd.Text + " ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    MyFunction.RunSQL("update chotsolieu set ghichu=N'" + InputBox.ResultValue + "',nguoiyc_huy='" + MyFunction._UserName + "',ngayyc_huy='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where sohd='" + txtSohd.Text + "' and dv='" + searchMadonvi.EditValue.ToString() + "'");
                    MessageBox.Show("Đã yêu cầu hủy chốt giấy báo:" + txtSohd.Text, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cmdHuyGB.Enabled = false;
                }
            }
            else
                MessageBox.Show("Bạn phải nhập lý do hủy giấy báo", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSohd.Text))
            {
                MessageBox.Show("Bạn chưa chọn số hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            else
            {
                MyFunction._frm = "frm_ThueNN";
                Frm_Baocao frm = new Frm_Baocao();
                frm.Show();
            }
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {

            gangiatri();
        }

        private void txtSohd_Leave(object sender, EventArgs e)
        {
            //if(txtSohd.Text.Length >=18) 
            //{
            //    MessageBox.Show("Chiều dài tối đa không được quá 18 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //}
        }

        private void txtLuongUSD_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtLuongUSD.Text))
            { txtLuongUSD.Text = "0"; }
        }

        private void txtLuongVND_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLuongVND.Text))
            { txtLuongVND.Text = "0"; }
        }

        private void txtPhucap_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhucap.Text))
            { txtPhucap.Text = "0"; }
        }

        private void txtThuong_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtThuong.Text))
            { txtThuong.Text = "0"; }
        }

        private void txtKhac_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKhac.Text))
            { txtKhac.Text = "0"; }
        }

        private void txtTiennha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTiennha.Text))
            { txtTiennha.Text = "0"; }
        }

        private void txtLuongTNCN_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLuongTNCN.Text))
            { txtLuongTNCN.Text = "0"; }
        }

        private void txtTNCN_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTNCN.Text))
            { txtTNCN.Text = "0"; }
        }

        private void txtTTN_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTTN.Text))
            { txtTTN.Text = "0"; }
        }

        private void txtBT_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBT.Text))
            { txtBT.Text = "0"; }
        }

        private void txtNPT_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNPT.Text))
            { txtNPT.Text = "0"; }
        }

        private void txtBHNV_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBHNV.Text))
            { txtBHNV.Text = "0"; }
        }

        private void txtDNVHG_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDNVHG.Text))
            { txtDNVHG.Text = "0"; }
        }

        private void txtPDV_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPDV.Text))
            { txtPDV.Text = "0"; }
        }

        private void txtTongtien_DN_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTongtien_DN.Text))
            { txtTongtien_DN.Text = "0"; }
        }

        private void frm_ThueNN_Shown(object sender, EventArgs e)
        {
            if (MyFunction.RunSQL_String("select them from phanquyen where id_chucnang='2' and iduser='" + MyFunction._UserName + "'") == "True")
            {
                cmdAdd.Visible=true;
            }
            else { cmdAdd.Visible=false; }
            if (MyFunction.RunSQL_String("select sua from phanquyen where id_chucnang='2' and iduser='" + MyFunction._UserName + "'") == "True")
            {
                cmdEdit.Visible = true;
            }
            else { cmdEdit.Visible = false; }
            if (MyFunction.RunSQL_String("select xoa from phanquyen where id_chucnang='2' and iduser='" + MyFunction._UserName + "'") == "True")
            {
                cmdDel.Visible = true;
            }
            else { cmdDel.Visible = false; }

        }

        private void chkALLnv_CheckedChanged(object sender, EventArgs e)
        {
            if (chkALLnv.Checked)
                chkALLnv.Text = "Nhân viên đang làm việc";
            else
                chkALLnv .Text="Tất cả nhân viên";
            searchMadonvi_EditValueChanged(sender, e);
        }

        private void cmbKythue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKythue.Text == "Tháng")
            {
                cmbThang.Enabled = false;
                cmbNam.Enabled = false;
                cmbThang.Text = DateTime.Now.ToString("MM");
                cmbNam.Text = DateTime.Now.ToString("yyyy");
            }
            else if (cmbKythue.Text == "Quý 1" || cmbKythue.Text == "Quý 2" || cmbKythue.Text == "Quý 3" || cmbKythue.Text == "Quý 4")
            {
                cmbThang.Enabled = true;
                cmbNam.Enabled = true;

            }
        }
    }
}