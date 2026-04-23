using DataLayer;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit.Import.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanlyThue.Forms
{
    public partial class F_QuyetTT : DevExpress.XtraEditors.XtraForm
    {
        public F_QuyetTT()
        {
            InitializeComponent();
        }
        public double GTBT, GTPT = 0;bool load=false;
        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void loadMadonvi(string nhomfc)
        {
            string[] selectedGroups = nhomfc.Split(',');
            string formattedList = string.Join(",", selectedGroups.Select(g => $"'{g}'"));
            string sql = $"SELECT madv,madv+'-'+tendv as tendv,trangthai from q_dmdv WHERE nhomfc IN ({formattedList}) and len(madv)=5 order by trangthai,madv";

            //searchMADV.Properties.DataSource = MyFunction.GetDataTable("select madv,madv+'-'+tendv as tendv,trangthai from q_dmdv where nhomfc like '" + nhomfc + "' order by trangthai,madv");
            searchMADV.Properties.DataSource = MyFunction.GetDataTable(sql);
            searchMADV.Properties.DisplayMember = "tendv";
            searchMADV.Properties.ValueMember = "madv";

        }
        void loadNhanvien(string MADV)
        {
            searchHotenNV.Properties.DataSource = MyFunction.GetDataTable("select manv,hodem,ten,hoten,Tentrangthailamviec from q_hsc where madv='" + MADV + "' order by Tentrangthailamviec,ten");
            searchHotenNV.Properties.DisplayMember = "hoten";
            searchHotenNV.Properties.ValueMember = "manv";
            

        }

        private void F_QuyetTT_Load(object sender, EventArgs e)
        {
            
            searchMADV.Properties.NullText = "Chọn tên đơn vị";
            searchHotenNV.Properties.NullText = "Chọn tên nhân viên";
            loadMadonvi(MyFunction._Nhomfc);
            cmbTuThang.Text = "01";cmbDennam.Text= cmbTuNam.Text=DateTime.Now.Year.ToString();
            cmbDenthang.Text= "0" + DateTime.Now.Month.ToString().Substring(0,1);
            load = true;
        }

        private void searchMADV_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchMADV.EditValue.ToString()))
            {
                cmTonghop_Click(sender, new EventArgs());
                loadNhanvien(searchMADV.EditValue.ToString());
            }
        }

        private void searchHotenNV_EditValueChanged(object sender, EventArgs e)
        {
            gridData.DataSource = MyFunction.GetDataTable("select LG_THANG,DV, MANV, HODEM, TEN, L_HDLD, LOAILG, LCV, DLCV, LTLD, LCB, TIENPC, PC_TBH, PC_TTHUE, TONGLUONG," +
                " LGTNCN, LGBHXH,LGBHTN, NVBHXH, NVBHYT, NVBHTN, TC_NV, Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG, SOPT, SOHD from q_luong" +
                " where dv ='" + searchMADV.EditValue + "' and nguoisd='" + MyFunction._UserName + "' and manv='" + searchHotenNV.EditValue + "' order by lg_thang");

            gridQTT.DataSource = MyFunction.GetDataTable("Select '" + cmbDennam.Text + "' AS NAM,'" + searchMADV.EditValue + "' AS DV," +
                   "(CASE WHEN SUM(TC_NV)>0 THEN '' ELSE '1' END) as [10%],'' as QT, NhanSu.HoLotNhanSu AS HO,NhanSu.TenNhanSu AS TEN,count(distinct lg_thang) as SOTHANG," +
                   "SUM(LGTNCN)-sum(pc_tthue) as LUONG,sum(pc_tthue) as PHUCAP,SUM(NVBHXH) as BHXH_NV, SUM(NVBHYT) AS BHYT_NV, SUM(NVBHTN) AS BHTN_NV," +
                   " sum(sopt) as SO_PHUTHUOC,sum(Giam_TBT)+sum(ST_NGPT) as TONGGIAM ," +
                   "SUM(TNCN) AS TNTT, sum(TTN) as THUE, '" + MyFunction.RunSQL_String("select TOP 1 MST FROM Q_DMDV" +
                   " WHERE MADV='" + searchMADV.EditValue + "'") + "' AS MSTVP,'' as MSCN, MSTNV,SCMT, MANV  from q_luong,NhanSu " +
                   "where Q_LUONG.DV=NhanSu.MaDonVi and Q_LUONG.MANV=NhanSu.MaNhanSu" +
                   " AND dv ='" + searchMADV.EditValue + "' and nguoisd='" + MyFunction._UserName + "' and manv='" + searchHotenNV.EditValue + "'" +
                   " group by manv,NhanSu.HoLotNhanSu ,NhanSu.TenNhanSu, MSTNV,SCMT");


        }

        private void cmdExportData_Click(object sender, EventArgs e)
        {
            if (gridViewData.RowCount > 0)
            {
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    
                    gridViewData.ExportToXlsx(exportFilePath);
                    Process.Start(exportFilePath);
                }
            }
            else
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void cmdExportHTKK_Click(object sender, EventArgs e)
        {
            gridQTT.DataSource = MyFunction.GetDataTable("Select '" + cmbDennam.Text + "' AS NAM,'" + searchMADV.EditValue + "' AS DV,(CASE WHEN SUM(TC_NV)>0 THEN '' ELSE '1' END) as [10%],'' as QT, hodem AS HO,TEN,count(distinct lg_thang) as SOTHANG,SUM(LGTNCN)-sum(pc_tthue) as LUONG,sum(pc_tthue) as PHUCAP,SUM(NVBHXH) as BHXH_NV, SUM(NVBHYT) AS BHYT_NV, SUM(NVBHTN) AS BHTN_NV, sum(sopt) as SO_PHUTHUOC,sum(sopt)*'" + GTPT + "' + (count(distinct lg_thang)*'" + GTBT + "') as TONGGIAM ,SUM(TNCN) AS TNTT, sum(TTN) as THUE, '" + MyFunction.RunSQL_String("select TOP 1 MST FROM Q_DMDV WHERE MADV='" + searchMADV.EditValue + "'") + "' AS MSTVP,'' as MSCN, MSTNV,SCMT, MANV from q_luong where dv ='" + searchMADV.EditValue + "' and nguoisd='" + MyFunction._UserName + "' group by manv,hodem,ten, MSTNV,SCMT");
            if (gridViewQTT.RowCount > 0)
            {
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    (gridQTT.MainView as GridView).OptionsPrint.AutoWidth = true;
                    (gridQTT.MainView as GridView).BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

                    XlsxExportOptionsEx advOptions = new XlsxExportOptionsEx();
                    advOptions.AllowGrouping = DevExpress.Utils.DefaultBoolean.False;
                    advOptions.ShowTotalSummaries = DevExpress.Utils.DefaultBoolean.False;
                    advOptions.ApplyFormattingToEntireColumn = DevExpress.Utils.DefaultBoolean.False;
                    advOptions.LayoutMode = DevExpress.Export.LayoutMode.Table;           
                    advOptions.SheetName = "Exported QTT";
                    gridQTT.ExportToXlsx(exportFilePath, advOptions);
                    Process.Start(exportFilePath);
                }
            }
            else
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void gridViewData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridViewData.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
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
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, gridViewData); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, gridViewData); }));//Tăng kích thước nếu text vượt quá

            }
        }
        bool cal(Int32 _with, GridView _view)
        {
            _view.IndicatorWidth = _view.IndicatorWidth < _with ? _with : _view.IndicatorWidth;
            return true;
        }

        private void cmbDenthang_SelectedIndexChanged(object sender, EventArgs e)
        {
            kiemtra();
        }
        void kiemtra()
        {
            
            if (load)
                if (double.Parse(cmbDennam.Text + cmbDenthang.Text) < double.Parse(cmbTuNam.Text + cmbTuThang.Text) || DateTime.Parse("01/" + cmbDenthang.Text + "/" + cmbDennam.Text) > DateTime.Now.Date || DateTime.Parse("01/" + cmbTuThang.Text + "/" + cmbTuNam.Text) > DateTime.Now.Date)
                {
                    MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

        }

        private void cmbDennam_SelectedIndexChanged(object sender, EventArgs e)
        {
            kiemtra();
        }

        private void cmbTuNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            kiemtra();
        }

        private void cmbTuThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            kiemtra();
        }

        private void gridViewQTT_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridViewQTT.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
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
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, gridViewQTT); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, gridViewQTT); }));//Tăng kích thước nếu text vượt quá

            }
        }

        private void cmTonghop_Click(object sender, EventArgs e)
        {
            int I;
            GTBT = 11000000;GTPT = 4400000;
            String thangs, nams, LGTHANG, str, STR1, STR2;
            if (double.Parse(cmbDennam.Text + cmbDenthang.Text) < double.Parse(cmbTuNam.Text + cmbTuThang.Text) ||
                DateTime.Parse("01/" + cmbDenthang.Text + "/" + cmbDennam.Text) > DateTime.Now.Date || DateTime.Parse("01/" + cmbTuThang.Text + "/" + cmbTuNam.Text) > DateTime.Now.Date)
            {
                MessageBox.Show("Bạn chon sai tháng, vui lòng chọn lại!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            if(searchMADV.EditValue==null)
            {
                MessageBox.Show("Bạn chưa chọn đơn vị.","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            gridData.DataSource = null;

            this.Cursor = Cursors.WaitCursor;
            MyFunction.RunSQL("delete from q_luong where nguoisd='" + MyFunction._UserName + "' or nguoisd is null");
            try
            {
                //Số tháng cần quét
                I = 100;
                //nams = DateTime.Now.ToString(" MM/dd/yyyy").Trim().Substring(6, 4);
                //thangs = DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2);
                //int.Parse(DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2)) < 10 ? "0" + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2) : DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2);
                thangs = cmbDenthang.Text;
                nams = cmbDennam.Text;
                do
                {
                    LGTHANG = thangs + nams;
                    str = "D" + thangs + nams.Substring(2);
                    //Chuan hoa lai so lieu
                    MyFunction.RunSQL("update " + str + " set loainv=null where loainv=''");
                    MyFunction.RunSQL("update " + str + " set L_HDLD=null where L_HDLD=''");
                    //MyFunction.RunSQL("INSERT INTO Q_LUONG(LG_THANG, DV, MANV, HODEM, TEN, L_HDLD, LOAILG, LCV, DLCV, LTLD, DVT, SOPT, DVP, CD, LCB, TIENPC, DVTPC, AN, DT, CT, TP, KHAC, PC_TIENNHA, PC_TTKHNHA, HD_NHA, THUE_NHA,PC_TBH, PC_TTHUE, TONGLUONG, LGTNCN, LGBHXH, DV_BHXH, DV_BHYT, DV_BHTN, TC_DN, NVBHXH, NVBHYT, NVBHTN, TC_NV, Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG, PHI_CD, TIEN_DVP,SOHD, TGLG, TGBHXH, LGBHTN, TONGTIEN_DN,NGUOISD,GHICHU) SELECT LG_THANG, DV, MANV, HODEM, TEN,  L_HDLD, LOAILG, LCV, DLCV, LTLD, DVT, SOPT, DVP, CD, LCB, TIENPC, DVTPC, AN, DT, CT, TP, KHAC, PC_TIENNHA, PC_TTKHNHA, HD_NHA, THUE_NHA,PC_TBH, PC_TTHUE, TONGLUONG, LGTNCN, LGBHXH, DV_BHXH, DV_BHYT, DV_BHTN, TC_DN, NVBHXH, NVBHYT, NVBHTN, TC_NV, Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG, PHI_CD, TIEN_DVP,SOHD , TGLG, TGBHXH, LGBHTN, TONGTIEN_DN,'" + MyFunction._UserName + "',GHICHU FROM " + str + " where dv = '" + searchMADV.EditValue + "' and LOAINV is null AND LOAILG<>'S'");
                    MyFunction.RunSQL("INSERT INTO Q_LUONG(LG_THANG, DV, MANV, HODEM, TEN, L_HDLD, LOAILG, LCV, DLCV, LTLD, DVT," +
                        " SOPT, DVP, CD, LCB, TIENPC, DVTPC, AN, DT, CT, TP, KHAC, PC_TIENNHA, PC_TTKHNHA, HD_NHA, THUE_NHA,PC_TBH," +
                        " PC_TTHUE, TONGLUONG, LGTNCN, LGBHXH, DV_BHXH, DV_BHYT, DV_BHTN, TC_DN, NVBHXH, NVBHYT, NVBHTN, TC_NV," +
                        " Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG, PHI_CD, TIEN_DVP,SOHD, TGLG, TGBHXH, LGBHTN, TONGTIEN_DN,NGUOISD,GHICHU)" +
                        " SELECT LG_THANG, DV, MANV, HODEM, TEN,  L_HDLD, LOAILG, LCV, DLCV, LTLD, DVT," +
                        " SOPT, DVP, CD, LCB, TIENPC, DVTPC, AN, DT, CT, TP, KHAC, PC_TIENNHA, PC_TTKHNHA, HD_NHA, THUE_NHA,PC_TBH," +
                        " PC_TTHUE, TONGLUONG, LGTNCN, LGBHXH, DV_BHXH, DV_BHYT, DV_BHTN, TC_DN, NVBHXH, NVBHYT, NVBHTN, TC_NV," +
                        " Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG, PHI_CD, TIEN_DVP,SOHD , TGLG, TGBHXH, LGBHTN, TONGTIEN_DN," +
                        "'" + MyFunction._UserName + "',GHICHU FROM " + str + " where dv = '" + searchMADV.EditValue + "' AND LOAILG<>'S'");

                    MyFunction.RunSQL("UPDATE Q_LUONG SET Q_LUONG.MSTNV = Q_HSC.MST, Q_LUONG.SCMT = Q_HSC.SOCMND FROM Q_LUONG " +
                        "JOIN Q_HSC ON Q_LUONG.MANV = Q_HSC.MANV AND Q_LUONG.DV ='" + searchMADV.EditValue + "' AND Q_LUONG.NGUOISD='" + MyFunction._UserName + "'");

                    I = I - 1;
                    STR1 = thangs + nams;
                    STR2 = cmbTuThang.Text + cmbTuNam.Text;
                    if (String.Compare(STR1, STR2, true) == 0)
                        I = 0;
                    if (thangs == "01")
                        thangs = "12";
                    else if (Int32.Parse(thangs) - 1 < 10)
                        thangs = "0" + (Int32.Parse(thangs) - 1).ToString();
                    else
                        thangs = (Int32.Parse(thangs) - 1).ToString();
                    if (thangs == "12")
                        nams = (Int32.Parse(nams) - 1).ToString();
                    else
                        nams = (Int32.Parse(nams)).ToString();

                } while (I >= 1);
                this.Cursor = Cursors.Arrow;
                str = cmbDennam.Text + cmbDenthang.Text;
                //MyFunction.RunSQL("delete from q_luong where dv='" + searchMADV.EditValue + "' and nguoisd='" + MyFunction._UserName + "' and convert(float,right(lg_thang,4)+left(LG_THANG,2))>'" + cmbDennam.Text + cmbDenthang.Text + "'");
                MessageBox.Show("Đã tổng hợp xong dữ liệu","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                gridData.DataSource=MyFunction.GetDataTable("select LG_THANG,DV, MANV, HODEM, TEN, L_HDLD, LOAILG, LCV, DLCV, LTLD, LCB, TIENPC, PC_TBH, PC_TTHUE, TONGLUONG," +
                    " LGTNCN, LGBHXH, NVBHXH, NVBHYT, NVBHTN, TC_NV, Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG,SOPT, SOHD" +
                    "  from q_luong where dv ='" + searchMADV.EditValue + "' and nguoisd='" + MyFunction._UserName + "' order by lg_thang");

                gridQTT.DataSource=MyFunction.GetDataTable("Select '" + cmbDennam.Text + "' AS NAM,'" + searchMADV.EditValue + "' AS DV," +
                    "(CASE WHEN SUM(TC_NV)>0 THEN '' ELSE '1' END) as [10%],'' as QT, NhanSu.HoLotNhanSu AS HO,NhanSu.TenNhanSu AS TEN,count(distinct lg_thang) as SOTHANG," +
                    "SUM(LGTNCN)-sum(pc_tthue) as LUONG,sum(pc_tthue) as PHUCAP,SUM(NVBHXH) as BHXH_NV, SUM(NVBHYT) AS BHYT_NV, SUM(NVBHTN) AS BHTN_NV," +
                    " sum(sopt) as SO_PHUTHUOC,sum(Giam_TBT)+sum(ST_NGPT) as TONGGIAM ," +
                    "SUM(TNCN) AS TNTT, sum(TTN) as THUE, '" + MyFunction.RunSQL_String("select TOP 1 MST FROM Q_DMDV" +
                    " WHERE MADV='" + searchMADV.EditValue + "'") + "' AS MSTVP,'' as MSCN, MSTNV,SCMT, MANV  from q_luong,NhanSu " +
                    "where Q_LUONG.DV=NhanSu.MaDonVi and Q_LUONG.MANV=NhanSu.MaNhanSu" +
                    " AND dv ='" + searchMADV.EditValue + "' and nguoisd='" + MyFunction._UserName + "' group by manv,NhanSu.HoLotNhanSu ,NhanSu.TenNhanSu, MSTNV,SCMT");

                //showtreview(cmb_madv.Text);
            }
            catch { MessageBox.Show("Không có mã đơn vị này. bạn chọn lại mã đơn vị"); }
        }
    }
}