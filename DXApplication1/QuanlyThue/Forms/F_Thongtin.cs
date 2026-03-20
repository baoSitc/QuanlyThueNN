using Bussiness;
using DataLayer;
using DevExpress.CodeParser;
using DevExpress.DashboardCommon;
using DevExpress.Text.Fonts;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.Import.Html;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static DevExpress.XtraEditors.Mask.MaskSettings;
using Series = DevExpress.XtraCharts.Series;

namespace QuanlyThue.Forms
{
    public partial class F_Thongtin : DevExpress.XtraEditors.XtraForm
    {
        public F_Thongtin()
        {
            InitializeComponent();
        }
        public class loaiBC
        {
            public string ID_PARENT { set; get; }
            public string ID { set; get; }
            public string NAME { set; get; }
        }
        bool load = false; DonviBussiness _DV;double tong = 0;
        string ID = "";

        private void F_Thongtin_Load(object sender, EventArgs e)
        {

            cmbTuthang.Text = "01";
            cmbTunam.Text = DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4);
            cmbDenthang.Text = "0" + DateTime.Now.Month.ToString().Substring(0, 1);
            cmbDennam.Text = DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4);

            int sodv = int.Parse(MyFunction.RunSQL_String("select count(distinct MADV) from Q_HSC where q_hsc.TenTrangthaiLamViec=N'Đang làm việc' and CD='X'"));
            int tsnv = int.Parse(MyFunction.RunSQL_String("select count(MANV) from Q_HSC where q_hsc.TenTrangthaiLamViec=N'Đang làm việc' and CD='X'"));
            DataTable dt = MyFunction.GetDataTable("select GT,count(MANV) as TSNV from Q_HSC where q_hsc.TenTrangthaiLamViec=N'Đang làm việc' and CD='X' group by GT");


            gridBaocao.Visible = false;   
            chart2.Visible = false;
            load = true;
            //Load TreeView
            var brands = new List<loaiBC>() {
            new loaiBC{ID_PARENT = "-1", ID = "0",NAME="Danh Sách Báo Cáo"},
            new loaiBC{ID_PARENT = "0", ID = "NG",NAME="NGOẠI GIAO"},            
             new loaiBC{ID_PARENT = "NG", ID = "NGNG",NAME="Ngoại Giao"},
              new loaiBC{ID_PARENT = "NG", ID = "NGPCP",NAME="Phi Chính Phủ"},
               new loaiBC{ID_PARENT = "NG", ID = "NGQT",NAME="Tổ Chức Quốc Tế"},                

             new loaiBC{ID_PARENT = "0", ID = "KT",NAME="KINH TẾ"},             
             new loaiBC{ID_PARENT = "KT", ID = "KTKT",NAME="Kinh Tế"},              
                new loaiBC{ID_PARENT = "KT", ID = "KTVP",NAME="Văn Phòng Đại Diện"},
                 new loaiBC{ID_PARENT = "KT", ID = "KTCT",NAME="Công Ty"},
                  new loaiBC{ID_PARENT = "KT", ID = "KTHH",NAME="Hiệp Hội"},
             new loaiBC{ID_PARENT = "0", ID = "TNN",NAME="THUẾ NƯỚC NGOÀI"},
                new loaiBC{ID_PARENT = "TNN", ID = "NVTNN",NAME="Nhân Viên Thuế Nước Ngoài"},
                  new loaiBC{ID_PARENT = "0", ID = "BD",NAME="BÁO CÁO BIẾN ĐỘNG"},
                  new loaiBC{ID_PARENT = "BD", ID = "BDCD",NAME="Công Đoàn Viên"},
                  new loaiBC{ID_PARENT = "BD", ID = "BDNV",NAME="Theo Nhân Viên"},
                  new loaiBC{ID_PARENT = "BD", ID = "BDDV",NAME="Theo Đơn Vị"},
                  new loaiBC{ID_PARENT = "BD", ID = "BDDT",NAME="Theo Doanh Thu"},

            };
            treeList1.ParentFieldName = "ID_PARENT";
            treeList1.KeyFieldName = "ID";
            treeList1.Appearance.FocusedCell.BackColor = System.Drawing.SystemColors.Highlight;
            treeList1.Appearance.FocusedCell.ForeColor = System.Drawing.SystemColors.HighlightText;
            treeList1.OptionsBehavior.Editable = false;
            treeList1.ViewStyle = DevExpress.XtraTreeList.TreeListViewStyle.TreeView;
            treeList1.DataSource = brands;

        }

        void thietlapcot()
        {
            DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.FieldName = "MADV"; col.Caption = "Mã Đơn Vị"; ViewNV.Columns.Add(col); ViewNV.Columns[0].Visible = true;

            DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
            col1.FieldName = "TENDV"; col1.Caption = "Tên Đơn Vị"; ViewNV.Columns.Add(col1); ViewNV.Columns[1].Visible = true;

            DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
            col2.FieldName = "TSNV"; col2.Caption = "Tổng số NV"; ViewNV.Columns.Add(col2); ViewNV.Columns[2].Visible = true;
            ViewNV.Columns[2].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            ViewNV.Columns[2].SummaryItem.DisplayFormat = "Total:{0:n0}";


            DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
            col3.FieldName = "NHOMFC"; col3.Caption = "Nhóm FC"; ViewNV.Columns.Add(col3); ViewNV.Columns[3].Visible = true;
        }
      
        void loadChart()
        {
            string nams, thangs, LGTHANG, str, STR1, STR2, TANG, GIAM, DVP;
            TANG = GIAM = DVP = "0";
            double tongdvp = 0;
            DateTime tungay, denngay;
            chart2.Visible = true; chart2.Dock = DockStyle.Fill;gridBaocao.Visible = false;
            chart2.Series.Clear(); chart2.Legends.Clear(); chart2.ChartAreas.Clear();
            if (load == false) { return; }
            if (ID=="BDNV")
            {
                try
                {
                    chart2.Titles["Title1"].Text = "Báo Cáo Tăng Giảm Nhân Viên Từ Tháng:" + cmbTuthang.Text + "/" + cmbTunam.Text + " Đến Tháng:" + cmbDenthang.Text + "/" + cmbDennam.Text;

                    chart2.Series.Add("Tang");
                    chart2.Series["Tang"].Color = Color.Blue; chart2.Series["Tang"].IsValueShownAsLabel = true;
                    chart2.Series.Add("Giam"); chart2.Series["Giam"].Color = Color.Red; chart2.Series["Giam"].IsValueShownAsLabel = true;
                    chart2.Legends.Add("NV");


                    // Assign the legend to Series1.
                    chart2.Series["Tang"].Legend = "NV";
                    chart2.Series["Tang"].IsVisibleInLegend = true;
                    chart2.Series["Tang"].LegendText = "Tăng nhân viên";
                    chart2.Series["Giam"].Legend = "NV";
                    chart2.Series["Giam"].LegendText = "Giảm nhân viên";
                    chart2.Series["Giam"].IsVisibleInLegend = true;
                    chart2.ChartAreas.Add("NV");
                    //Số tháng cần quét
                    int I = 1;
                    nams = cmbTunam.Text;
                    thangs = cmbTuthang.Text;
                    do
                    {
                        LGTHANG = thangs + nams;
                        //str = "D" + thangs + nams.Substring(2);
                        //Chuan hoa lai so lieu
                        TANG = MyFunction.RunSQL_String("select COUNT(MaNhanSu) as tang from NhanSu where month(NGAYTAO)='" + thangs + "' and YEAR(ngaytao)='" + nams + "'");

                        chart2.Series["Tang"].Points.AddXY(thangs + "/" + nams, double.Parse(TANG));
                        GIAM = MyFunction.RunSQL_String("select COUNT(MaNhanSu) as tang from NhanSu where month(NgayNghiViecMoiNhat)='" + thangs + "' and YEAR(NgayNghiViecMoiNhat)='" + nams + "'");

                        chart2.Series["Giam"].Points.AddXY(thangs + "/" + nams, double.Parse(GIAM));
                        I = I + 1;
                        STR1 = thangs + nams;
                        STR2 = cmbDenthang.Text + cmbDennam.Text;
                        if (String.Compare(STR1, STR2, true) == 0)
                            I = 13;
                        if (thangs == "12")
                            thangs = "01";
                        else if (Int32.Parse(thangs) + 1 < 10)
                            thangs = "0" + (Int32.Parse(thangs) + 1).ToString();
                        else
                            thangs = (Int32.Parse(thangs) + 1).ToString();
                        if (thangs == "01")
                            nams = (Int32.Parse(nams) + 1).ToString();
                        else
                            nams = (Int32.Parse(nams)).ToString();

                    } while (I <= 12);
                    tungay = new DateTime(int.Parse(cmbTunam.Text), int.Parse(cmbTuthang.Text), 1);
                    denngay = new DateTime(int.Parse(cmbDennam.Text), int.Parse(cmbDenthang.Text), 1);
                    denngay = denngay.AddMonths(1);
                    denngay = denngay.AddDays(-(denngay.Day));

                    str = "select nhansu.MaDonVi as MADV,manhansu as MANV,holotnhansu+' '+TenNhanSu as HOTEN,DonVi.TenNhomFosco as tennhomfosco,NhanSu.NGAYTAO as NGAY, N'Tăng' as LOAI " +
                                                                " from NhanSu, DonVi where NhanSu.MaDonVi = DonVi.MaDonVi and nhansu.ngaytao >= '" + tungay.ToString("yyyy/MM/dd") + "' and nhansu.ngaytao <='" + denngay.ToString("yyyy/MM/dd") + "' " +
                                                                " union " +
                                                                "select nhansu.MaDonVi as MADV, manhansu as MANV, holotnhansu + ' ' + TenNhanSu as HOTEN, DonVi.TenNhomFosco as NHOMFC, NhanSu.NgayNghiViecMoiNhat as NGAY, N'Giảm' as LOAI" +
                                                                " from NhanSu, DonVi where NhanSu.MaDonVi = DonVi.MaDonVi" +
                                                                " and nhansu.NgayNghiViecMoiNhat  >= '" + tungay.ToString("yyyy/MM/dd") + "' and nhansu.NgayNghiViecMoiNhat <= '" + denngay.ToString("yyyy/MM/dd") + "'" +
                                                                " order by LOAI";
                    gridNV.DataSource = MyFunction.GetDataTable(str);
                    ViewNV.Columns.Clear();
                    ViewNV.OptionsView.ColumnAutoWidth = true;
                    ViewNV.OptionsView.ShowFooter = true;
                    DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                    col.FieldName = "MADV"; col.Caption = "Mã đơn vị"; ViewNV.Columns.Add(col); ViewNV.Columns[0].Visible = true;
                    DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col1.FieldName = "MANV"; col1.Caption = "Mã nhân viên"; ViewNV.Columns.Add(col1); ViewNV.Columns[1].Visible = true;
                    ViewNV.Columns[1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                    ViewNV.Columns[1].SummaryItem.DisplayFormat = "Total:{0:n0}";

                    DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col2.FieldName = "HOTEN"; col2.Caption = "Họ tên"; ViewNV.Columns.Add(col2); ViewNV.Columns[2].Visible = true;
                    DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col3.FieldName = "tennhomfosco"; col3.Caption = "Nhóm FC"; ViewNV.Columns.Add(col3); ViewNV.Columns[3].Visible = true;
                    DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col4.FieldName = "NGAY"; col4.Caption = "Ngày"; ViewNV.Columns.Add(col4); ViewNV.Columns[4].Visible = true;
                    DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col5.FieldName = "LOAI"; col5.Caption = "Loại"; ViewNV.Columns.Add(col5); ViewNV.Columns[5].Visible = true;
                }
                catch { }
            }
            else if (ID == "BDDT")
            {
                chart2.Titles["Title1"].Text = null;
                if (MyFunction.RunSQL_String("select xem from phanquyen where id_chucnang='7' and iduser='" + MyFunction._UserName + "'") == "False" || MyFunction.RunSQL_String("select xem from phanquyen where id_chucnang='7' and iduser='" + MyFunction._UserName + "'") == "") return; 
               
                try
                {
                    tongdvp = 0;
                    chart2.Series.Add("Doanhthu"); chart2.Legends.Add("DT"); chart2.Series["Doanhthu"].IsValueShownAsLabel = true;
                    chart2.ChartAreas.Add("DT");
                    chart2.Series["Doanhthu"].LegendText = "Doanh thu";

                    //Số tháng cần quét
                    int I = 1;
                    //nams = cmbDennam.Text;
                    //thangs = cmbDenthang.Text;
                    nams = cmbTunam.Text;
                    thangs = cmbTuthang.Text;

                    do
                    {
                        tungay = new DateTime(int.Parse(nams), int.Parse(thangs), 1);
                        denngay = new DateTime(int.Parse(nams), int.Parse(thangs), 1);
                        denngay = denngay.AddMonths(1);
                        denngay = denngay.AddDays(-(denngay.Day));

                        LGTHANG = thangs + nams;
                        //str = "D" + thangs + nams.Substring(2);
                        //Chuan hoa lai so lieu
                        DVP = MyFunction.RunSQL_String("select SUM(dvp) as DVP from CHOTSOLIEU where DACHOT=1 and tinhtrang=1 and ngaygiochot between '" + tungay.ToString("yyyy/MM/dd") + "' and '" + denngay.ToString("yyyy/MM/dd") + "' and dvp>0");
                        DVP = (double.Parse(DVP) - (double.Parse(DVP) * 0.08)).ToString();
                        tongdvp += double.Parse(DVP);
                        chart2.Series["Doanhthu"].Points.AddXY(thangs + "/" + nams, double.Parse(DVP) / 1000);
                        chart2.Series["Doanhthu"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
                        chart2.Series["Doanhthu"].LabelFormat = "N0"; chart2.Series["Doanhthu"].LabelBackColor = Color.DimGray; chart2.Series["Doanhthu"].LabelForeColor = Color.White;

                        I = I + 1;
                        STR1 = thangs + nams;
                        STR2 = cmbDenthang.Text + cmbDennam.Text;
                        if (String.Compare(STR1, STR2, true) == 0)
                            I = 13;
                        if (thangs == "12")
                            thangs = "01";
                        else if (Int32.Parse(thangs) + 1 < 10)
                            thangs = "0" + (Int32.Parse(thangs) + 1).ToString();
                        else
                            thangs = (Int32.Parse(thangs) + 1).ToString();
                        if (thangs == "01")
                            nams = (Int32.Parse(nams) + 1).ToString();
                        else
                            nams = (Int32.Parse(nams)).ToString();

                    } while (I <= 12);
                    chart2.Titles["Title1"].Text = "Báo Cáo Doanh Thu Từ Tháng:" + cmbTuthang.Text + "/" + cmbTunam.Text + " Đến Tháng:" + cmbDenthang.Text + "/" + cmbDennam.Text + ", TỔNG CỘNG:" + Math.Round(tongdvp/ 1000, 0).ToString("N0");
                    tungay = new DateTime(int.Parse(cmbTunam.Text), int.Parse(cmbTuthang.Text), 1);
                    denngay = new DateTime(int.Parse(cmbDennam.Text), int.Parse(cmbDenthang.Text), 1);
                    denngay = denngay.AddMonths(1);
                    denngay = denngay.AddDays(-(denngay.Day));

                    str = "select DV as MADV,SOHD, SUM(dvp)/1000 as DVP,ngaygiochot,note from CHOTSOLIEU where DACHOT=1 and tinhtrang=1 and ngaygiochot between '" + tungay.ToString("yyyy/MM/dd") + "' and '" + denngay.ToString("yyyy/MM/dd") + "' and dvp>0 group by DV,SOHD,ngaygiochot,note";
                    gridNV.DataSource = MyFunction.GetDataTable(str);
                    ViewNV.Columns.Clear();
                    ViewNV.OptionsView.ColumnAutoWidth = false;
                    ViewNV.OptionsView.ShowFooter = true;
                    DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                    col.FieldName = "MADV"; col.Caption = "Mã đơn vị"; ViewNV.Columns.Add(col); ViewNV.Columns[0].Visible = true;
                    col.Width = 55;
                    //
                    DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col1.FieldName = "SOHD"; col1.Caption = "Số hóa đơn"; ViewNV.Columns.Add(col1); ViewNV.Columns[1].Visible = true;
                    col1.Width = 100;
                    //col1.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                    //
                    DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col2.FieldName = "ngaygiochot"; col2.Caption = "Ngày chốt"; ViewNV.Columns.Add(col2); ViewNV.Columns[2].Visible = true;
                    //
                    DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col3.FieldName = "DVP"; col3.Caption = "Tiền DVP"; ViewNV.Columns.Add(col3); ViewNV.Columns[3].Visible = true;
                    ViewNV.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    ViewNV.Columns[3].DisplayFormat.FormatString = "N0";
                    ViewNV.Columns[3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    ViewNV.Columns[3].SummaryItem.DisplayFormat = "Total: {0:n0}";
                    DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col4.FieldName = "note"; col4.Caption = "Ghi chú"; ViewNV.Columns.Add(col4); ViewNV.Columns[4].Visible = true;
                    col4.Width = 150;
                }
                catch { }

            }
            else if (ID == "BDDV")
            {
                try
                {

                    chart2.Titles["Title1"].Text = "Báo Cáo Tăng Giảm Theo Đơn Vị Từ Tháng:" + cmbTuthang.Text + "/" + cmbTunam.Text + " Đến Tháng:" + cmbDenthang.Text + "/" + cmbDennam.Text;
                    groupBox2.Text= "Báo Cáo Tăng Giảm Theo Đơn Vị Từ Tháng:" + cmbTuthang.Text + "/" + cmbTunam.Text + " Đến Tháng:" + cmbDenthang.Text + "/" + cmbDennam.Text;
                    chart2.Series.Add("TangDV");
                    chart2.Series["TangDV"].Color = Color.Blue; chart2.Series["TangDV"].IsValueShownAsLabel = true;
                    chart2.Series.Add("GiamDV"); chart2.Series["GiamDV"].Color = Color.Red; chart2.Series["GiamDV"].IsValueShownAsLabel = true;
                    chart2.Legends.Add("DV");
                    chart2.Series["TangDV"].Legend = "DV";
                    chart2.Series["TangDV"].LegendText = "Tăng mới đơn vị";
                    chart2.Series["TangDV"].IsVisibleInLegend = true;
                    chart2.Series["GiamDV"].Legend = "DV";
                    chart2.Series["GiamDV"].LegendText = "Ngưng dịch vụ";
                    chart2.Series["GiamDV"].IsVisibleInLegend = true;
                    chart2.ChartAreas.Add("DV");
                    //Số tháng cần quét
                    int I = 1;
                    //nams = cmbDennam.Text;
                    //thangs = cmbDenthang.Text;
                    nams = cmbTunam.Text;
                    thangs = cmbTuthang.Text;

                    do
                    {
                        LGTHANG = thangs + nams;
                        //str = "D" + thangs + nams.Substring(2);
                        //Chuan hoa lai so lieu
                        TANG = MyFunction.RunSQL_String("select count(madonvi)  from donvi where month(NGAY_VAO)='" + thangs + "' and YEAR(NGAY_VAO)='" + nams + "'");
                        chart2.Series["TangDV"].Points.AddXY(thangs + "/" + nams, double.Parse(TANG));
                        chart2.Series["TangDV"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
                        //chart2.Series["DV"].LabelFormat = "N0"; chart2.Series["Doanhthu"].LabelBackColor = Color.DimGray; chart2.Series["Doanhthu"].LabelForeColor = Color.White;
                        //
                        GIAM = MyFunction.RunSQL_String("select count(madonvi)  from donvi where month(NgayKetThucHopDong)='" + thangs + "' and YEAR(NgayKetThucHopDong)='" + nams + "'");

                        chart2.Series["GiamDV"].Points.AddXY(thangs + "/" + nams, double.Parse(GIAM));
                        chart2.Series["GiamDV"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                        I = I + 1;
                        STR1 = thangs + nams;
                        STR2 = cmbDenthang.Text + cmbDennam.Text;
                        if (String.Compare(STR1, STR2, true) == 0)
                            I = 13;
                        if (thangs == "12")
                            thangs = "01";
                        else if (Int32.Parse(thangs) + 1 < 10)
                            thangs = "0" + (Int32.Parse(thangs) + 1).ToString();
                        else
                            thangs = (Int32.Parse(thangs) + 1).ToString();
                        if (thangs == "01")
                            nams = (Int32.Parse(nams) + 1).ToString();
                        else
                            nams = (Int32.Parse(nams)).ToString();

                    } while (I <= 12);

                    tungay = new DateTime(int.Parse(cmbTunam.Text), int.Parse(cmbTuthang.Text), 1);
                    denngay = new DateTime(int.Parse(cmbDennam.Text), int.Parse(cmbDenthang.Text), 1);
                    denngay = denngay.AddMonths(1);
                    denngay = denngay.AddDays(-(denngay.Day));
                    str = "select madonvi as MADV,tendonvi,tennhomfosco,ngaykyhopdong,N'Tăng mới đơn vị' as Loai from donvi where ngay_vao between '" + tungay.ToString("yyyy/MM/dd") + "' and '" + denngay.ToString("yyyy/MM/dd") + "' " +
                                                                " union " +
                                                                "select madonvi,tendonvi,tennhomfosco,ngayketthuchopdong,N'Ngưng dịch vụ' as Loai from donvi where ngayketthuchopdong between '" + tungay.ToString("yyyy/MM/dd") + "' and '" + denngay.ToString("yyyy/MM/dd") + "' " +
                                                                " order by LOAI,madonvi";
                    gridNV.DataSource = MyFunction.GetDataTable(str);
                    ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;
                    ViewNV.OptionsView.ShowFooter = true;

                    DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                    col.FieldName = "MADV"; col.Caption = "Mã đơn vị"; ViewNV.Columns.Add(col); ViewNV.Columns[0].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col1.FieldName = "tendonvi"; col1.Caption = "Tên đơn vị"; ViewNV.Columns.Add(col1); ViewNV.Columns[1].Visible = true;

                    ViewNV.Columns[1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                    DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();

                    col2.FieldName = "tennhomfosco"; col2.Caption = "Nhóm FC"; ViewNV.Columns.Add(col2); ViewNV.Columns[2].Visible = true;
                    DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();

                    col3.FieldName = "ngaykyhopdong"; col3.Caption = "Ngày"; ViewNV.Columns.Add(col3); ViewNV.Columns[3].Visible = true;
                    DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();

                    col4.FieldName = "Loai"; col4.Caption = "Loại"; ViewNV.Columns.Add(col4); ViewNV.Columns[4].Visible = true;
                    DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
                    //col5.FieldName = "LOAI"; col4.Caption = "Loại"; ViewNV.Columns.Add(col5); ViewNV.Columns[5].Visible = true;
                }
                catch { }

            }
        }

        private void ViewNV_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!ViewNV.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
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
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, ViewNV); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, ViewNV); }));//Tăng kích thước nếu text vượt quá

            }
        }
        bool cal(Int32 _with, GridView _view)
        {
            _view.IndicatorWidth = _view.IndicatorWidth < _with ? _with : _view.IndicatorWidth;
            return true;
        }

        private void cmdExcelNS_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewNV.RowCount > 0)
                {
                    SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                    saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                    if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                    {
                        string exportFilePath = saveFileDialogExcel.FileName;
                        //gridControl1.DataSource = selectgridvalues();
                        ViewNV.ExportToXlsx(exportFilePath);
                        Process.Start(exportFilePath);
                    }
                }
                else
                    MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch { }

        }

        private void cmbTuthang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(load)
            if (double.Parse(cmbDennam.Text + cmbDenthang.Text) < double.Parse(cmbTunam.Text + cmbTuthang.Text) || DateTime.Parse("01/" + cmbDenthang.Text + "/" + cmbDennam.Text) > DateTime.Now.Date)
            {
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                loadChart();
        }

        private void cmbTunam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                if (double.Parse(cmbDennam.Text + cmbDenthang.Text) < double.Parse(cmbTunam.Text + cmbTuthang.Text) || DateTime.Parse("01/" + cmbDenthang.Text + "/" + cmbDennam.Text) > DateTime.Now.Date)
            {
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                loadChart();
        }

        private void cmbDenthang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                if (double.Parse(cmbDennam.Text + cmbDenthang.Text) < double.Parse(cmbTunam.Text + cmbTuthang.Text) || DateTime.Parse("01/" + cmbDenthang.Text + "/" + cmbDennam.Text) > DateTime.Now.Date)
            {
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                loadChart();
        }

        private void cmbDennam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                if (double.Parse(cmbDennam.Text + cmbDenthang.Text) < double.Parse(cmbTunam.Text + cmbTuthang.Text) || DateTime.Parse("01/" + cmbDenthang.Text + "/" + cmbDennam.Text) > DateTime.Now.Date)
            {
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                loadChart();
        }

        private void cmbTuthang_Leave(object sender, EventArgs e)
        {


        }

        private void cmbTunam_Leave(object sender, EventArgs e)
        {


        }

        private void F_Thongtin_Shown(object sender, EventArgs e)
        {
            cmbTuthang.Text = "01";
            cmbTunam.Text = DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4);
            cmbDenthang.Text = ("0" + DateTime.Now.Month.ToString()).Substring(0, 2);
            cmbDennam.Text = DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4);

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void ViewNV_DoubleClick(object sender, EventArgs e)
        {

        }

        private void ViewNV_Click(object sender, EventArgs e)
        {
            groupThongtin.Visible = true; groupThongtin.Dock = DockStyle.Top;
            chart2.Dock = DockStyle.Fill;
            if (ViewNV.RowCount > 0 && ViewNV.FocusedRowHandle >= 0)
            //if (radioDV.Checked)
            {
                try
                {

                    string madv = ViewNV.GetFocusedRowCellValue("MADV").ToString();
                    string tennhom = MyFunction.RunSQL_String("select nhomfc from q_dmdv where madv='" + madv + "'");

                    groupThongtin.Text = "Thông tin về đơn vị: " + madv + " - Chuyên viên tiền lương:" + MyFunction.RunSQL_String("select hodem+' '+ ten as hoten from dsnv where manhom='" + tennhom + "'");
                    _DV = new DonviBussiness();
                    List<DonVi> _dv = _DV.ListGetbyMadv(madv);
                    gridDanhsach.DataSource = _dv;
                    ViewDanhsach.Columns.Clear();
                    DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                    col.FieldName = "MaDonVi"; col.Caption = "Mã đơn vị"; ViewDanhsach.Columns.Add(col); ViewDanhsach.Columns[0].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col1.FieldName = "TenDonVi"; col1.Caption = "Tên đơn vị"; ViewDanhsach.Columns.Add(col1); ViewDanhsach.Columns[1].Visible = true;
                    // col1.Width = 150;
                    DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col2.FieldName = "DiaChiDonVi"; col2.Caption = "Địa chỉ"; ViewDanhsach.Columns.Add(col2); ViewDanhsach.Columns[2].Visible = true;
                    //col2.Width = 150;
                    DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col3.FieldName = "NgayKyHopDong"; col3.Caption = "Ngày vao"; ViewDanhsach.Columns.Add(col3); ViewDanhsach.Columns[3].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col4.FieldName = "NgayKetThucHopDong"; col4.Caption = "Ngày Ra"; ViewDanhsach.Columns.Add(col4); ViewDanhsach.Columns[4].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col5.FieldName = "DichVuPhi"; col5.Caption = "DVP"; ViewDanhsach.Columns.Add(col5); ViewDanhsach.Columns[5].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col6 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col6.FieldName = "MaSoThue"; col6.Caption = "MST"; ViewDanhsach.Columns.Add(col6); ViewDanhsach.Columns[6].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col7 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col7.FieldName = "MaBHXH"; col7.Caption = "Ma BHXH"; ViewDanhsach.Columns.Add(col7); ViewDanhsach.Columns[7].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col8 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col8.FieldName = "MaBHXH_NN"; col8.Caption = "Ma BHXH NN"; ViewDanhsach.Columns.Add(col8); ViewDanhsach.Columns[8].Visible = true;
                    DevExpress.XtraGrid.Columns.GridColumn col9 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col9.FieldName = "LoaiLuong"; col9.Caption = "Loại Lương"; ViewDanhsach.Columns.Add(col9); ViewDanhsach.Columns[9].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col10 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col10.FieldName = "GhiChu"; col9.Caption = "Ghi chú"; ViewDanhsach.Columns.Add(col10); ViewDanhsach.Columns[10].Visible = true;

                    ViewDanhsach.OptionsView.ColumnAutoWidth = true;
                }
                catch { }
            }
        }

       

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            DonviBussiness donviBussiness = new DonviBussiness();
            var id = treeList1.FocusedNode.GetValue("ID");
            ID = id.ToString();
           if(id.ToString() == "NG")
            {
                tong = 0;
                    this.Cursor = Cursors.WaitCursor;
                    List<Q_DMDV> _lstQ_dmdv = new List<Q_DMDV>();
                    _lstQ_dmdv = donviBussiness.ListQ_DMDV(id.ToString());
                    foreach (var item in _lstQ_dmdv)
                    {
                        _lstQ_dmdv.Where(x => x.MADV == item.MADV).FirstOrDefault().TSNV = double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "'" +
                                  "and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));
                    tong += double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "' and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));

                }

                    gridNV.DataSource = _lstQ_dmdv;
                    ViewNV.OptionsView.ShowFooter = true;
                    ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;                   
                //groupBox2.Text = "Tổng số Đơn vị Ngoại Giao:" + MyFunction.RunSQL_String("select count(madonvi) from donvi where left(loaidonvi,2)='NG' and trangthai=N'Đang làm việc'") + ", Tổng số nhân viên:" + MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from NhanSu,DonVi where NhanSu.MaDonVi = DonVi.MaDonVi and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc' and left(donvi.loaidonvi,2)='NG'");
                groupBox2.Text = "Ngoại Giao:" + _lstQ_dmdv.Count + ", Nhân viên:" + tong.ToString("N0");
                thietlapcot();
                    this.Cursor = Cursors.Default;
            }
           else if (id.ToString() == "NGNG")
            {
                tong = 0;
                this.Cursor = Cursors.WaitCursor;
                List<Q_DMDV> _lstQ_dmdv = new List<Q_DMDV>();
                _lstQ_dmdv = donviBussiness.ListQ_DMDV(id.ToString());
                foreach (var item in _lstQ_dmdv)
                {
                    _lstQ_dmdv.Where(x => x.MADV == item.MADV).FirstOrDefault().TSNV = double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "'" +
                              "and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));
                    tong += double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "' and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));

                }

                gridNV.DataSource = _lstQ_dmdv;
                ViewNV.OptionsView.ShowFooter = true;
                ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;

                groupBox2.Text = "Ngoại Giao khối Ngoại Giao:" +_lstQ_dmdv.Count + ", Nhân viên:" +tong.ToString("N0") ;
                thietlapcot();
                this.Cursor = Cursors.Default;
            }
            else if (id.ToString() == "NGPCP")
            {
                tong = 0;
                this.Cursor = Cursors.WaitCursor;
                List<Q_DMDV> _lstQ_dmdv = new List<Q_DMDV>();
                _lstQ_dmdv = donviBussiness.ListQ_DMDV(id.ToString());
                foreach (var item in _lstQ_dmdv)
                {
                    _lstQ_dmdv.Where(x => x.MADV == item.MADV).FirstOrDefault().TSNV = double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "'" +
                              "and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));
                    tong += double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "' and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));

                }

                gridNV.DataSource = _lstQ_dmdv;
                ViewNV.OptionsView.ShowFooter = true;
                ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;

                groupBox2.Text = "Ngoại Giao - Phi Chính Phủ:" + _lstQ_dmdv.Count + ", Nhân viên:" + tong.ToString("N0");
                thietlapcot();
                this.Cursor = Cursors.Default;
            }
            else if (id.ToString() == "NGQT")
            {
                tong = 0;
                this.Cursor = Cursors.WaitCursor;
                List<Q_DMDV> _lstQ_dmdv = new List<Q_DMDV>();
                _lstQ_dmdv = donviBussiness.ListQ_DMDV(id.ToString());
                foreach (var item in _lstQ_dmdv)
                {
                    _lstQ_dmdv.Where(x => x.MADV == item.MADV).FirstOrDefault().TSNV = double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "'" +
                              "and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));
                    tong += double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "' and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));

                }

                gridNV.DataSource = _lstQ_dmdv;
                ViewNV.OptionsView.ShowFooter = true;
                ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;

                groupBox2.Text = "Ngoại Giao-Tổ chức Quốc Tế:" + _lstQ_dmdv.Count + ", Nhân viên:" + tong.ToString("N0");
                thietlapcot();
                this.Cursor = Cursors.Default;
            }
            else if (id.ToString() == "KT")
            {
                tong = 0;
                this.Cursor = Cursors.WaitCursor;
                List<Q_DMDV> _lstQ_dmdv = new List<Q_DMDV>();
                _lstQ_dmdv = donviBussiness.ListQ_DMDV(id.ToString());
                foreach (var item in _lstQ_dmdv)
                {
                    _lstQ_dmdv.Where(x => x.MADV == item.MADV).FirstOrDefault().TSNV = double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "'" +
                              "and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));
                    tong += double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "' and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));

                }
                gridNV.DataSource = _lstQ_dmdv;
                ViewNV.OptionsView.ShowFooter = true;
                ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;
                groupBox2.Text = "Kinh Tế:" + _lstQ_dmdv.Count + ", Nhân viên:" + tong.ToString("N0");
                thietlapcot();
                this.Cursor = Cursors.Default;
            }
            else if (id.ToString() == "KTKT")
            {
                tong = 0;
                this.Cursor = Cursors.WaitCursor;
                List<Q_DMDV> _lstQ_dmdv = new List<Q_DMDV>();
                _lstQ_dmdv = donviBussiness.ListQ_DMDV(id.ToString());
                foreach (var item in _lstQ_dmdv)
                {
                    _lstQ_dmdv.Where(x => x.MADV == item.MADV).FirstOrDefault().TSNV = double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "'" +
                              "and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));
                    tong += double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "' and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));

                }
                gridNV.DataSource = _lstQ_dmdv;
                ViewNV.OptionsView.ShowFooter = true;
                ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;
                groupBox2.Text = "Kinh Tế khối Kinh Tế:" + _lstQ_dmdv.Count + ", Nhân viên:" + tong.ToString("N0");
                thietlapcot();
                this.Cursor = Cursors.Default;
            }
            else if (id.ToString() == "KTVP")
            {
                tong = 0;
                this.Cursor = Cursors.WaitCursor;
                List<Q_DMDV> _lstQ_dmdv = new List<Q_DMDV>();
                _lstQ_dmdv = donviBussiness.ListQ_DMDV(id.ToString());
                foreach (var item in _lstQ_dmdv)
                {
                    _lstQ_dmdv.Where(x => x.MADV == item.MADV).FirstOrDefault().TSNV = double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "'" +
                              "and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));
                    tong += double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "' and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));

                }
                gridNV.DataSource = _lstQ_dmdv;
                ViewNV.OptionsView.ShowFooter = true;
                ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;
                groupBox2.Text = "Kinh Tế-VPĐD:" + _lstQ_dmdv.Count + ", Nhân viên:" + tong.ToString("N0");
                thietlapcot();
                this.Cursor = Cursors.Default;
            }
            else if (id.ToString() == "KTHH")
            {
                tong = 0;
                this.Cursor = Cursors.WaitCursor;
                List<Q_DMDV> _lstQ_dmdv = new List<Q_DMDV>();
                _lstQ_dmdv = donviBussiness.ListQ_DMDV(id.ToString());
                foreach (var item in _lstQ_dmdv)
                {
                    _lstQ_dmdv.Where(x => x.MADV == item.MADV).FirstOrDefault().TSNV = double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "'" +
                              "and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));
                    tong += double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "' and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));

                }
                gridNV.DataSource = _lstQ_dmdv;
                ViewNV.OptionsView.ShowFooter = true;
                ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;
                groupBox2.Text = "Kinh Tế-Hiệp Hội:" + _lstQ_dmdv.Count + ", Nhân viên:" + tong.ToString("N0");
                thietlapcot();
                this.Cursor = Cursors.Default;
            }
            else if (id.ToString() == "KTCT")
            {
                tong = 0;
                this.Cursor = Cursors.WaitCursor;
                List<Q_DMDV> _lstQ_dmdv = new List<Q_DMDV>();
                _lstQ_dmdv = donviBussiness.ListQ_DMDV(id.ToString());
                foreach (var item in _lstQ_dmdv)
                {
                    _lstQ_dmdv.Where(x => x.MADV == item.MADV).FirstOrDefault().TSNV = double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "'" +
                              "and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));
                    tong += double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "' and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));

                }
                gridNV.DataSource = _lstQ_dmdv;
                ViewNV.OptionsView.ShowFooter = true;
                ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;
                groupBox2.Text = "Kinh Tế-Hiệp Hội:" + _lstQ_dmdv.Count + ", Nhân viên:" + tong.ToString("N0");
                thietlapcot();
                this.Cursor = Cursors.Default;
            }
            else if (id.ToString() == "BDCD")
            {
               
                this.Cursor = Cursors.WaitCursor;               
                gridNV.DataSource = MyFunction.GetDataTable("SELECT Q_HSC.MADV,Q_DMDV.TENDV,COUNT(Q_HSC.MANV) AS TSNV,Q_DMDV.NHOMFC FROM Q_HSC,Q_DMDV WHERE Q_HSC.MADV=Q_DMDV.MADV AND Q_HSC.TenTrangthaiLamViec=N'Đang làm việc' and q_hsc.CD='X' group by Q_HSC.MADV,Q_DMDV.TENDV,Q_DMDV.NHOMFC ");
                ViewNV.OptionsView.ShowFooter = true;
                ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;
                groupBox2.Text = "Tổng số công Đoàn viên:" + int.Parse(MyFunction.RunSQL_String("select count(MANV) from Q_HSC where q_hsc.TenTrangthaiLamViec=N'Đang làm việc' and CD='X'")) + ", Tổ Công Đoàn:" + ViewNV.RowCount ;
                thietlapcot();
                this.Cursor = Cursors.Default;
            }
            else if (id.ToString() == "BDNV" || id.ToString() == "BDDV" || id.ToString() == "BDDT")
            {
                

                    this.Cursor = Cursors.WaitCursor;
               
                loadChart();
                this.Cursor = Cursors.Default;
            }
            else if (id.ToString() == "NVTNN")
            {
                tong = 0;
                this.Cursor = Cursors.WaitCursor;
                List<Q_DMDV> _lstQ_dmdv = new List<Q_DMDV>();
                _lstQ_dmdv = donviBussiness.ListQ_DMDV(id.ToString());
                foreach (var item in _lstQ_dmdv)
                {
                    _lstQ_dmdv.Where(x => x.MADV == item.MADV).FirstOrDefault().TSNV = double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "'" +
                              "and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));
                    tong += double.Parse(MyFunction.RunSQL_String("select COUNT(nhansu.MaNhanSu) as tsnv from nhansu where NhanSu.MaDonVi ='" + item.MADV + "' and NhanSu.ID_TrangThaiLamViec = N'Đang làm việc'"));

                }
                gridNV.DataSource = _lstQ_dmdv;
                ViewNV.OptionsView.ShowFooter = true;
                ViewNV.Columns.Clear(); ViewNV.OptionsView.ColumnAutoWidth = true;
                groupBox2.Text = "Tổng số Đơn Vị Thuế Nước Ngoài:" + _lstQ_dmdv.Count + ", Nhân viên:" + tong.ToString("N0");
                thietlapcot();
                this.Cursor = Cursors.Default;
            }

        }
    }
}