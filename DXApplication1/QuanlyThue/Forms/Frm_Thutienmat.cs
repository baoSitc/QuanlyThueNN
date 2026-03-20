using Bussiness;
using DataLayer;
using DevExpress.DataAccess.Design;
using DevExpress.DataAccess.Native.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanlyThue.Forms
{
    public partial class Frm_Thutienmat : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Thutienmat()
        {
            InitializeComponent();
        }
        bool them=false;
        TienmatBussiness _tienmatBussiness;
        ChotsolieuBussiness _chotsl;
        void ShowHiheControl(bool t)
        {
            cmdAdd.Visible = t;
            cmdExit.Visible = t;
            cmdDel.Visible = false;
            cmdEdit.Visible = t;
            cmdSave.Visible = !t;
            cmdCancel.Visible = !t;
            cmdPrint.Visible = t;

        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            them = true; xoatrang();
            ShowHiheControl(false);
            _Enable(true);
            txtSopt.Text = MyFunction.RunSQL_String("select max(phieuthu)+1 from t_tienmat");
        }
        void xoatrang()
        {
           searchMadonvi.Text = null;
            txtLuong.Text = "0";
            txtBHTN.Text = "0";
            txtBHXH.Text = "0";
            txtDVP.Text = "0";
            txtLydo.Text = null;
            txtPCD.Text = "0";
            cmbSohd.Text = null;
            txtTTN.Text = "0";
            txtSopt.Text="0";
            txtTong.Text = "0";            
            cmbThang.Text = ("0"+DateTime.Now.Month.ToString()).Substring(0,2);
            cmbNam.Text = DateTime.Now.Year.ToString();
        }
        void _Enable(bool tt)
        {
          //tableLayoutPanel2.Enabled = tt;
          txtBHTN.Enabled=txtBHXH.Enabled=txtDVP.Enabled=txtLuong.Enabled=txtLydo.Enabled=txtPCD.Enabled=txtTTN.Enabled= tt;
            txtTong.Enabled = false;
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            them = false;
            ShowHiheControl(false); _Enable(true);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            ShowHiheControl(true); _Enable(false); them = false;xoatrang();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void loadMadonvi(string nhomfc)
        {

            searchMadonvi.Properties.DataSource = MyFunction.GetDataTable("select madv,madv+'-'+tendv as tendv,trangthai from q_dmdv where nhomfc='" + nhomfc + "' order by trangthai,madv");
            searchMadonvi.Properties.DisplayMember = "tendv";
            searchMadonvi.Properties.ValueMember = "madv";

        }
        void loadSohd(string thang,string nam)
        {
            if (them)
            {
                cmbSohd.DataSource = MyFunction.GetDataTable("select sohd from chotsolieu where DV='" + searchMadonvi.EditValue + "' and lgthang='" + cmbThang.Text + cmbNam.Text + "' and tinhtrang='1'");
                //if(cmbSohd.DataSource==null) 
                //{
                //    cmbSohd.Items.Add(new)
                //}
                cmbSohd.DisplayMember = "sohd";
                cmbSohd.ValueMember = "sohd";
            }

        }

        private void Frm_Thutienmat_Load(object sender, EventArgs e)
        {
            _tienmatBussiness = new TienmatBussiness();
            loadMadonvi(MyFunction._Nhomfc);
            ShowHiheControl(true);
            cmbThang.Text = ("0"+DateTime.Now.Month.ToString()).Substring(0,2);
            cmbNam.Text = DateTime.Now.Year.ToString();
            _Enable(false); them = false;
        }

        private void searchMadonvi_EditValueChanged(object sender, EventArgs e)
        {
            
            if (searchMadonvi.EditValue != null)
            {
                
               // gridTienmat.DataSource = _tienmatBussiness.GetByMadv(searchMadonvi.EditValue.ToString());
                loadSohd(cmbThang.Text,cmbNam.Text);
                if (GVTienmat.RowCount<=0)
                {
                    txtLuong.Text = "0";
                    txtBHTN.Text = "0";
                    txtBHXH.Text = "0";
                    txtDVP.Text = "0";
                    txtLydo.Text = null;
                    txtPCD.Text = "0";
                    cmbSohd.Text = null;
                    txtTTN.Text = "0";
                    txtSopt.Text = "0";
                    txtTong.Text = "0";
                    cmbThang.Text = ("0" + DateTime.Now.Month.ToString()).Substring(0, 2);
                    cmbNam.Text = DateTime.Now.Year.ToString();
                }
            }
           
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            T_TIENMAT _tienmat = new T_TIENMAT(); 
            if (searchMadonvi.EditValue==null || cmbSohd.Text == null)
            {
                MessageBox.Show("Bạn chưa chọn Mã Đơn Vị Hoặc chưa nhập số hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            
            _tienmat.CDP = double.Parse(txtPCD.Text);
            _tienmat.LUONG = double.Parse(txtLuong.Text);
            _tienmat.PHIEUTHU = int.Parse(txtSopt.Text);
            _tienmat.BHXH_YT = double.Parse(txtBHXH.Text);
            _tienmat.BHTN = double.Parse(txtBHTN.Text);
            _tienmat.DVP = double.Parse(txtDVP.Text);
            _tienmat.NOIDUNGTRA = txtLydo.Text;
            _tienmat.T_LUONG = double.Parse(txtTong.Text);
            _tienmat.SOHD = cmbSohd.Text;
            _tienmat.MADV = them==false? GVTienmat.GetFocusedRowCellValue("MADV").ToString(): searchMadonvi.EditValue.ToString();
            _tienmat.TENDV = MyFunction.RunSQL_String("select tendv from q_dmdv where madv='" + _tienmat.MADV + "'");
            _tienmat.TTN = double.Parse(txtTTN.Text);
            _tienmat.NGAYTRA = DateTime.Now.Date.ToString("dd/MM/yyyy");
            _tienmat.NGUOISD = MyFunction._UserName;
            _tienmat.THANGCN = ("0" + DateTime.Now.Month.ToString()).Substring(0, 2) + DateTime.Now.Year;

            if (them) //Thêm mới
            {
               
                 if (MessageBox.Show("Bạn có chắc chắn thêm dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    if (_tienmatBussiness.Insert(_tienmat) == 1)
                    {
                        MessageBox.Show("Đã thêm thành công dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //gridControlDanhsachGB.DataSource = _thuenn.Getbykey(searchMadonvi.EditValue.ToString().Substring(0, 5), null);
                        ShowHiheControl(true); _Enable(false); them = false;
                    }
                    else
                        MessageBox.Show("Đã có lỗi khi thêm dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else //Cập nhật lại thu tiền mặt
            {
                if (GVTienmat.RowCount > 0)
                    if (MessageBox.Show("Bạn có chắc chắn cập nhật lại dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        _tienmat.ID = int.Parse(GVTienmat.GetFocusedRowCellValue("ID").ToString());
                        if (_tienmatBussiness.Update(_tienmat) == 1)
                        {
                            MessageBox.Show("Đã cập nhật thành công dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowHiheControl(true); _Enable(false); them = false;

                        }
                        else
                            MessageBox.Show("Đã có lỗi khi cập nhật dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

            }
            gridTienmat.DataSource = _tienmatBussiness.GetByMadv(searchMadonvi.EditValue.ToString());
        }
       
        bool cal(Int32 _with, GridView _view)
        {
            _view.IndicatorWidth = _view.IndicatorWidth < _with ? _with : _view.IndicatorWidth;
            return true;
        }
        void gangiatri()
        {
            if (them)
            {
                if (GVTienmat.RowCount > 0 && GVTienmat.FocusedRowHandle >= 0)
                {
                    try
                    {
                        txtLuong.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("DNVHG").ToString()).ToString("N0");
                        txtBHTN.Text = "0";
                        txtBHXH.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("TONGBH").ToString()).ToString("N0");
                        txtDVP.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("DVP").ToString()).ToString("N0");
                        txtLydo.Text = GVTienmat.GetFocusedRowCellValue("Note")is  null?"":GVTienmat.GetFocusedRowCellValue("Note").ToString();
                        txtPCD.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("CDP").ToString()).ToString("N0");
                        cmbSohd.Text = GVTienmat.GetFocusedRowCellValue("SOHD") is null ? "" : GVTienmat.GetFocusedRowCellValue("SOHD").ToString();
                        txtTTN.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("TONGTHUE").ToString()).ToString("N0");
                        //txtSopt.Text = GVTienmat.GetFocusedRowCellValue("PHIEUTHU").ToString();
                        txtTong.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("TONGTIENDN").ToString()).ToString("N0");
                        searchMadonvi.EditValue= GVTienmat.GetFocusedRowCellValue("DV").ToString();
                    }

                    catch { }
                }
            }
            else
            {
                if (GVTienmat.RowCount > 0 && GVTienmat.FocusedRowHandle >= 0)
                {
                    try
                    {
                        txtLuong.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("LUONG").ToString()).ToString("N0");
                        txtBHTN.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("BHTN").ToString()).ToString("N0");
                        txtBHXH.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("BHXH_YT").ToString()).ToString("N0");
                        txtDVP.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("DVP").ToString()).ToString("N0");
                        txtLydo.Text = GVTienmat.GetFocusedRowCellValue("NOIDUNGTRA") is null ? "" : GVTienmat.GetFocusedRowCellValue("NOIDUNGTRA").ToString();
                        txtPCD.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("CDP").ToString()).ToString("N0");
                        cmbSohd.Text = GVTienmat.GetFocusedRowCellValue("SOHD") is null ? "" : GVTienmat.GetFocusedRowCellValue("SOHD").ToString();
                        txtTTN.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("TTN").ToString()).ToString("N0");
                        txtSopt.Text = GVTienmat.GetFocusedRowCellValue("PHIEUTHU").ToString();
                        txtTong.Text = double.Parse(GVTienmat.GetFocusedRowCellValue("T_LUONG").ToString()).ToString("N0");
                    }

                    catch { }
                }

            }

        }

        private void GVTienmat_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!GVTienmat.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
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
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, GVTienmat); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, GVTienmat); }));//Tăng kích thước nếu text vượt quá

            }
        }

        private void GVTienmat_Click(object sender, EventArgs e)
        {

            gangiatri();
        }

        private void GVTienmat_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            gangiatri();
        }

        private void GVTienmat_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            
            gangiatri();
        }

        private void GVTienmat_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gangiatri();
        }

        private void txtLuong_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTong.Text = (double.Parse(txtLuong.Text) + double.Parse(txtBHXH.Text) + double.Parse(txtBHTN.Text) + double.Parse(txtDVP.Text) + double.Parse(txtPCD.Text) + double.Parse(txtTTN.Text)).ToString("N0");
            }
            catch { }
            
        }

        private void txtBHXH_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTong.Text = (double.Parse(txtLuong.Text) + double.Parse(txtBHXH.Text) + double.Parse(txtBHTN.Text) + double.Parse(txtDVP.Text) + double.Parse(txtPCD.Text) + double.Parse(txtTTN.Text)).ToString("N0");
            }
            catch { }
        }

        private void txtBHTN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTong.Text = (double.Parse(txtLuong.Text) + double.Parse(txtBHXH.Text) + double.Parse(txtBHTN.Text) + double.Parse(txtDVP.Text) + double.Parse(txtPCD.Text) + double.Parse(txtTTN.Text)).ToString("N0");
            }
            catch { }
        }

        private void txtTTN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTong.Text = (double.Parse(txtLuong.Text) + double.Parse(txtBHXH.Text) + double.Parse(txtBHTN.Text) + double.Parse(txtDVP.Text) + double.Parse(txtPCD.Text) + double.Parse(txtTTN.Text)).ToString("N0");
            }
            catch { }
        }

        private void txtPCD_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTong.Text = (double.Parse(txtLuong.Text) + double.Parse(txtBHXH.Text) + double.Parse(txtBHTN.Text) + double.Parse(txtDVP.Text) + double.Parse(txtPCD.Text) + double.Parse(txtTTN.Text)).ToString("N0");
            }
            catch { }
        }

        private void txtDVP_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTong.Text = (double.Parse(txtLuong.Text) + double.Parse(txtBHXH.Text) + double.Parse(txtBHTN.Text) + double.Parse(txtDVP.Text) + double.Parse(txtPCD.Text) + double.Parse(txtTTN.Text)).ToString("N0");
            }
            catch { }
        }

        private void cmbSohd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lấy giá trị gán cho textbox
            if(cmbSohd.SelectedIndex>=0 && them && MyFunction.RunSQL_String("select sohd from chotsolieu where DV='" + searchMadonvi.EditValue + "' and lgthang='" + cmbThang.Text + cmbNam.Text + "' and tinhtrang='1' and SOHD=N'" + cmbSohd.Text + "'")!="")
            {
                txtBHXH.Text = Double.Parse(MyFunction.RunSQL_String("select tongbh from chotsolieu where DV='" + searchMadonvi.EditValue + "' and lgthang='" + cmbThang.Text + cmbNam.Text + "' and tinhtrang='1' and SOHD=N'" + cmbSohd.Text + "'")).ToString("N0");
                txtDVP.Text = Double.Parse(MyFunction.RunSQL_String("select DVP from chotsolieu where DV='" + searchMadonvi.EditValue + "' and lgthang='" + cmbThang.Text + cmbNam.Text + "' and tinhtrang='1' and SOHD=N'" + cmbSohd.Text + "'")).ToString("N0");
                txtPCD.Text = Double.Parse(MyFunction.RunSQL_String("select CDP from chotsolieu where DV='" + searchMadonvi.EditValue + "' and lgthang='" + cmbThang.Text + cmbNam.Text + "' and tinhtrang='1' and SOHD=N'" + cmbSohd.Text + "'")).ToString("N0");
                txtTTN.Text = Double.Parse(MyFunction.RunSQL_String("select tongthue from chotsolieu where DV='" + searchMadonvi.EditValue + "' and lgthang='" + cmbThang.Text + cmbNam.Text + "' and tinhtrang='1' and SOHD=N'" + cmbSohd.Text + "'")).ToString("N0");
                txtLuong.Text = Double.Parse(MyFunction.RunSQL_String("select DNVHG from chotsolieu where DV='" + searchMadonvi.EditValue + "' and lgthang='" + cmbThang.Text + cmbNam.Text + "' and tinhtrang='1' and SOHD=N'" + cmbSohd.Text + "'")).ToString("N0");
                txtLydo.Text = MyFunction.RunSQL_String("select note from chotsolieu where DV='" + searchMadonvi.EditValue + "' and lgthang='" + cmbThang.Text + cmbNam.Text + "' and tinhtrang='1' and SOHD=N'" + cmbSohd.Text + "'");
                txtSopt.Text = MyFunction.RunSQL_String("select max(phieuthu)+1 from t_tienmat");
            }
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GVTienmat.GetFocusedRowCellValue("ID").ToString()))
            {
                MyFunction._frm = "Frm_Thutienmat";
                MyFunction._temp = GVTienmat.GetFocusedRowCellValue("ID").ToString();
                Frm_Baocao frm = new Frm_Baocao();
                frm.Show();
            }
        }

        private void txtLuong_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLuong.Text))
                txtLuong.Text = "0";
        }

        private void txtLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtBHXH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtBHXH_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtBHXH.Text))
                txtBHXH.Text = "0";
        }

        private void txtBHTN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtBHTN_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtBHTN.Text))
                txtBHTN.Text = "0";
        }

        private void txtTTN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtTTN_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtTTN.Text))
                txtTTN.Text = "0";
        }

        private void txtPCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPCD_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtPCD.Text))
                txtPCD.Text = "0";
        }

        private void txtDVP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtDVP_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtDVP.Text))
                txtDVP.Text = "0";
        }

        void loadGridTienMat(string thang,string nam)
        {
            string thangnam = thang + nam;
            if (them)
            {
                _chotsl = new ChotsolieuBussiness();
                CHOTSOLIEU _CHOTSL;
                List<CHOTSOLIEU> _lstChotsl = new List<CHOTSOLIEU>();
                System.Data.DataTable dt = MyFunction.GetDataTable("select MADV from q_dmdv where nhomfc='" + MyFunction._Nhomfc + "'");
                foreach (DataRow row in dt.Rows)
                {
                    _CHOTSL = new CHOTSOLIEU();_CHOTSL = _chotsl.GetByMADV(row["MADV"].ToString(), thangnam);
                    if (_CHOTSL !=null)
                    _lstChotsl.Add(_CHOTSL);
                }

                gridTienmat.DataSource = _lstChotsl;
                GVTienmat.Columns.Clear();
                GVTienmat.OptionsView.ShowGroupPanel = true;
                GVTienmat.OptionsView.ShowFooter = true;

                DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "DV"; col.Caption = "Mã đơn vị"; GVTienmat.Columns.Add(col); GVTienmat.Columns[0].Visible = true;
                DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                col1.FieldName = "SOHD"; col1.Caption = "Số hóa đơn"; GVTienmat.Columns.Add(col1); GVTienmat.Columns[1].Visible = true;
                DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();     
                col2.FieldName = "TONGTHUE"; col2.Caption = "Tổng Thuế"; GVTienmat.Columns.Add(col2); GVTienmat.Columns[2].Visible = true;
              
                GVTienmat.Columns[2].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[2].DisplayFormat.FormatString = "N0";

                DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
                col3.FieldName = "TONGBH"; col3.Caption = "Tổng Bảo Hiểm"; GVTienmat.Columns.Add(col3); GVTienmat.Columns[3].Visible = true;               
                GVTienmat.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[3].DisplayFormat.FormatString = "N0";

                DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
                col4.FieldName = "DNVHG"; col4.Caption = "Tổng Lương"; GVTienmat.Columns.Add(col4); GVTienmat.Columns[4].Visible = true;
                
                GVTienmat.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[4].DisplayFormat.FormatString = "N0";
                GVTienmat.Columns[4].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
                col5.FieldName = "CDP"; col5.Caption = "Tổng Phí CĐ"; GVTienmat.Columns.Add(col5); GVTienmat.Columns[5].Visible = true;
                GVTienmat.Columns[5].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[5].DisplayFormat.FormatString = "N0";
                GVTienmat.Columns[5].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                DevExpress.XtraGrid.Columns.GridColumn col6 = new DevExpress.XtraGrid.Columns.GridColumn();
                col6.FieldName = "DVP"; col6.Caption = "Tổng Phí DV"; GVTienmat.Columns.Add(col6); GVTienmat.Columns[6].Visible = true;                
                GVTienmat.Columns[6].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[6].DisplayFormat.FormatString = "N0";
                GVTienmat.Columns[6].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                

                DevExpress.XtraGrid.Columns.GridColumn col7 = new DevExpress.XtraGrid.Columns.GridColumn();
                col7.FieldName = "TONGTIENDN"; col7.Caption = "Tổng Tiền"; GVTienmat.Columns.Add(col7); GVTienmat.Columns[7].Visible = true;
                GVTienmat.Columns[7].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[7].DisplayFormat.FormatString = "N0";
                GVTienmat.Columns[7].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                DevExpress.XtraGrid.Columns.GridColumn col8 = new DevExpress.XtraGrid.Columns.GridColumn();
                col8.FieldName = "Note"; col8.Caption = "Nội dung"; GVTienmat.Columns.Add(col8); GVTienmat.Columns[8].Visible = true;

            }
            else
            {
                //lấy dữ liệu từ thu tiền mặt
                _tienmatBussiness = new TienmatBussiness();
                gridTienmat.DataSource = _tienmatBussiness.GetByThangNam(thang, nam);
                GVTienmat.Columns.Clear();
                DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "PHIEUTHU"; col.Caption = "Phiếu thu"; GVTienmat.Columns.Add(col); GVTienmat.Columns[0].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                col1.FieldName = "MADV"; col1.Caption = "Mã Đơn Vị"; GVTienmat.Columns.Add(col1); GVTienmat.Columns[1].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
                col2.FieldName = "SOHD"; col2.Caption = "Số Hóa Đơn"; GVTienmat.Columns.Add(col2); GVTienmat.Columns[2].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();                               
                col3.FieldName = "TTN"; col3.Caption = "Tổng Thuế"; GVTienmat.Columns.Add(col3); GVTienmat.Columns[3].Visible = true;                
                GVTienmat.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[3].DisplayFormat.FormatString = "N0";

                DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
                col4.FieldName = "BHXH_YT"; col4.Caption = "Tổng BHXH"; GVTienmat.Columns.Add(col4); GVTienmat.Columns[4].Visible = true;
                GVTienmat.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[4].DisplayFormat.FormatString = "N0";

                DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
                col5.FieldName = "BHTN"; col5.Caption = "Tổng BHTN"; GVTienmat.Columns.Add(col5); GVTienmat.Columns[5].Visible = true;
                GVTienmat.Columns[5].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[5].DisplayFormat.FormatString = "N0";

                DevExpress.XtraGrid.Columns.GridColumn col6 = new DevExpress.XtraGrid.Columns.GridColumn();
                col6.FieldName = "LUONG"; col6.Caption = "Tổng Lương"; GVTienmat.Columns.Add(col6); GVTienmat.Columns[6].Visible = true;
                GVTienmat.Columns[6].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[6].DisplayFormat.FormatString = "N0";

                DevExpress.XtraGrid.Columns.GridColumn col7 = new DevExpress.XtraGrid.Columns.GridColumn();
                col7.FieldName = "CDP"; col7.Caption = "Tổng Phí CĐ"; GVTienmat.Columns.Add(col7); GVTienmat.Columns[7].Visible = true;                
                GVTienmat.Columns[7].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[7].DisplayFormat.FormatString = "N0";

               

                DevExpress.XtraGrid.Columns.GridColumn col8 = new DevExpress.XtraGrid.Columns.GridColumn();
                col8.FieldName = "DVP"; col8.Caption = "Tổng Phí DV"; GVTienmat.Columns.Add(col8); GVTienmat.Columns[8].Visible = true;                
                GVTienmat.Columns[8].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[8].DisplayFormat.FormatString = "N0";

               

                DevExpress.XtraGrid.Columns.GridColumn col9 = new DevExpress.XtraGrid.Columns.GridColumn();
                col9.FieldName = "T_LUONG"; col9.Caption = "Tổng Cộng"; GVTienmat.Columns.Add(col9); GVTienmat.Columns[9].Visible = true;
                GVTienmat.Columns[9].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GVTienmat.Columns[9].DisplayFormat.FormatString = "N0";

                DevExpress.XtraGrid.Columns.GridColumn col10 = new DevExpress.XtraGrid.Columns.GridColumn();
                col10.FieldName = "NOIDUNGTRA"; col10.Caption = "Nội dung"; GVTienmat.Columns.Add(col10); GVTienmat.Columns[10].Visible = true;
            }

        }
        private void cmbThang_SelectedIndexChanged(object sender, EventArgs e)
        {


            loadGridTienMat(cmbThang.Text, cmbNam.Text);
        }

        private void cmbNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadGridTienMat(cmbThang.Text, cmbNam.Text);
        }
    }
}