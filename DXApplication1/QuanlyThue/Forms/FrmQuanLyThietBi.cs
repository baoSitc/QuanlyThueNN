using Bussiness;
using DataLayer;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanlyThue.Forms
{
    public partial class FrmQuanLyThietBi : DevExpress.XtraEditors.XtraForm
    {
        ThietBiBussiness _thietbi_Buss;
        bool them = false;
        public FrmQuanLyThietBi()
        {
            InitializeComponent();
        }

        private void FrmQuanLyThietBi_Load(object sender, EventArgs e)
        {
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.IsSplitterFixed = false;

            _thietbi_Buss = new ThietBiBussiness();
            gridThietBi.DataSource = _thietbi_Buss.Getall();
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

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            them = true; xoatrang();
            ShowHiheControl(false);
            groupBox1.Enabled = true; txtMaTB.Enabled = false;

        }
        void xoatrang()
        {
            dtNgayThanhLy.Text = dtNgaySuDung.Text = dtNgayMua.Text = txtNguoiSuDung.Text = txtTenThietBi.Text = txtGhichu.Text = null;

        }

        bool cal(Int32 _with, GridView _view)
        {
            _view.IndicatorWidth = _view.IndicatorWidth < _with ? _with : _view.IndicatorWidth;
            return true;
        }

        private void ViewThietBi_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!ViewThietBi.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
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
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, ViewThietBi); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, ViewThietBi); }));//Tăng kích thước nếu text vượt quá

            }
        }
        void gangiatri()
        {
            if (ViewThietBi.RowCount > 0 && ViewThietBi.FocusedRowHandle >= 0)
            {
                try
                {

                    txtMaTB.Text = ViewThietBi.GetFocusedRowCellValue("MaThietBi").ToString();
                    txtTenThietBi.Text = ViewThietBi.GetFocusedRowCellValue("TenThietBi").ToString();

                    object ngayKiemKeObj = ViewThietBi.GetFocusedRowCellValue("NgayKiemKe");

                    if (ngayKiemKeObj == null || ngayKiemKeObj == DBNull.Value)
                    {
                        dtNgayKiemKe.EditValue = null;
                    }
                    else
                    {
                        dtNgayKiemKe.EditValue = Convert.ToDateTime(ngayKiemKeObj);
                    }

                    
                    object ngayMuaObj = ViewThietBi.GetFocusedRowCellValue("NgayMua");

                    if (ngayMuaObj == null || ngayMuaObj == DBNull.Value)
                    {
                        dtNgayMua.EditValue = null;
                    }
                    else
                    {
                        dtNgayMua.EditValue = Convert.ToDateTime(ngayMuaObj);
                    }

                    object ngaySuDungObj = ViewThietBi.GetFocusedRowCellValue("NgaySuDung");

                    if (ngaySuDungObj == null || ngaySuDungObj == DBNull.Value)
                    {
                        dtNgaySuDung.EditValue = null;
                    }
                    else
                    {
                        dtNgaySuDung.EditValue = Convert.ToDateTime(ngaySuDungObj);
                    }

                    object ngayThanhLyObj = ViewThietBi.GetFocusedRowCellValue("NgayThanhLy");

                    if (ngayThanhLyObj == null || ngayThanhLyObj == DBNull.Value)
                    {
                        dtNgayThanhLy.EditValue = null;
                    }
                    else
                    {
                        dtNgayThanhLy.EditValue = Convert.ToDateTime(ngayThanhLyObj);
                    }
                    
                    txtNguoiSuDung.Text= ViewThietBi.GetFocusedRowCellValue("NguoiSuDung").ToString();
                    cmbLoaiThietBi.Text= ViewThietBi.GetFocusedRowCellValue("LoaiThietBi").ToString();
                    cmbNhomThietBi.Text= ViewThietBi.GetFocusedRowCellValue("NhomThietBi").ToString();
                    cmbTinhTrang.Text= ViewThietBi.GetFocusedRowCellValue("Tinhtrang").ToString();
                    txtDonGia.Text= double.Parse(ViewThietBi.GetFocusedRowCellValue("DONGIA").ToString()).ToString("N0");
                    //double.Parse(txtKhac.Text, new CultureInfo("vi-VN"));

                }
                catch { }
            }

        }

        private void ViewThietBi_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gangiatri();
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            if (ViewThietBi.RowCount > 0)
            {
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;

                    ViewThietBi.ExportToXlsx(exportFilePath);
                    Process.Start(exportFilePath);
                }
            }
            else
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            ShowHiheControl(true);  them = false;
            groupBox1.Enabled = false; 
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            ThietBi _thietbi = new ThietBi();
            _thietbi.ID= int.Parse(ViewThietBi.GetFocusedRowCellValue("ID").ToString());

            _thietbi.MaThietBi = txtMaTB.Text;
            _thietbi.NgayThanhLy = (Convert.ToDateTime(dtNgayThanhLy.EditValue).Year<1900)? (DateTime?)null : Convert.ToDateTime(dtNgayThanhLy.EditValue);
            _thietbi.NgaySuDung= (Convert.ToDateTime(dtNgaySuDung.EditValue).Year < 1900) ? (DateTime?)null : Convert.ToDateTime(dtNgaySuDung.EditValue);
            _thietbi.NgayMua=(Convert.ToDateTime(dtNgayMua.EditValue).Year < 1900) ? (DateTime?)null : Convert.ToDateTime(dtNgayMua.EditValue);
            _thietbi.NgayKiemKe = (Convert.ToDateTime(dtNgayKiemKe.EditValue).Year < 1900) ? (DateTime?)null :Convert.ToDateTime(dtNgayKiemKe.EditValue);
            _thietbi.TenThietBi = txtTenThietBi.Text;
            _thietbi.LoaiThietBi = cmbLoaiThietBi.Text;
            _thietbi.NhomThietBi=cmbNhomThietBi.Text;
            _thietbi.GhiChu=txtGhichu.Text;
            _thietbi.NguoiSuDung=txtNguoiSuDung.Text;
            _thietbi.Tinhtrang=cmbTinhTrang.Text;
            _thietbi.Enable = true;
            _thietbi.DONGIA=double.Parse(txtDonGia.Text);


            if (them) //Thêm mới thiết bị
            {
                if (MessageBox.Show("Bạn có chắc chắn thêm dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    if (_thietbi_Buss.InsertThietBi(_thietbi) == 1)
                    {
                        MessageBox.Show("Đã thêm thành công dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //gridControlDanhsachGB.DataSource = _thuenn.Getbykey(searchMadonvi.EditValue.ToString().Substring(0, 5), null);
                        ShowHiheControl(true);them = false;
                    }
                    else
                        MessageBox.Show("Đã có lỗi khi thêm dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else //Cập nhật lại Thiết bị
            {
                if (ViewThietBi.RowCount > 0)
                    if (MessageBox.Show("Bạn có chắc chắn cập nhật lại dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        //_t_thuenn.Id = int.Parse(gridView1.GetFocusedRowCellValue("Id").ToString());
                        if (_thietbi_Buss.UpdateThietBi(_thietbi) == 1)
                        {
                            MessageBox.Show("Đã cập nhật thành công dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowHiheControl(true); them = false;
                            groupBox1.Enabled = false;

                        }
                        else
                            MessageBox.Show("Đã có lỗi khi cập nhật dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

            }
            gridThietBi.DataSource = _thietbi_Buss.Getall();
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            them = false;
            ShowHiheControl(false);
            groupBox1.Enabled = true;txtMaTB.Enabled = false;
        }

       
        private void ExportTemplate()
        {
            //Export file template excel
            // Set License trước khi dùng EPPlus
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            
            //using (SaveFileDialog sfd = new SaveFileDialog())
            //{
            //    sfd.Filter = "Excel Files|*.xlsx";
            //    sfd.FileName = "FileMau.xlsx";
            //    if (sfd.ShowDialog() == DialogResult.OK)
            //    {
            //        FileInfo newfile = new FileInfo(sfd.FileName);
            //        using (var package = new ExcelPackage(newfile))
            //        {
            //            var ws = package.Workbook.Worksheets.Add("Template");

            //            // Tạo tiêu đề cột
            //            ws.Cells[1, 1].Value = "Họ Tên";
            //            ws.Cells[1, 2].Value = "Ngày Sinh";
            //            ws.Cells[1, 3].Value = "Địa Chỉ";
            //            ws.Cells[1, 4].Value = "Số Điện Thoại";

            //            ws.Cells[1, 1, 1, 4].Style.Font.Bold = true; // In đậm tiêu đề
            //            File.WriteAllBytes(sfd.FileName, package.GetAsByteArray());
            //            MessageBox.Show("Xuất file mẫu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //        }

                    
            //    }
            //}


            
        }

        private void cmdExportTemplate_Click(object sender, EventArgs e)
        {
            ExportTemplate();
        }
    }
}