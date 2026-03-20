using DataLayer;
using DevExpress.Charts.Native;
using DevExpress.CodeParser;
using DevExpress.DataAccess.Sql;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.Export;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using ExcelDataReader;
using Microsoft.Office.Interop.Word;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace QuanlyThue.Forms
{
    public partial class Frm_PhanTichCTNH : DevExpress.XtraEditors.XtraForm
    {
        public Frm_PhanTichCTNH()
        {
            InitializeComponent();
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {


            DataSet ds = new DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                //thiết lập lại từ ngày và đến ngày về mặc định
                    txtTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    txtDenNgay.Value = DateTime.Now;

                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel Workbook 97-2003|*.xls", ValidateNames = true })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                        {
                            IExcelDataReader reader;
                            if (ofd.FilterIndex == 2)
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(stream);
                            }
                            else
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            }

                            ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });

                            dt = ds.Tables[0];

                           
                            reader.Close();
                            //xóa dữ liệu cũ
                            MyFunction.RunSQL("delete from CTNH");
                            //Dùng dữ liệu từ DataSet để thêm mới công nợ
                            using (SqlConnection conn = new SqlConnection(MyFunction.str_con))
                            {
                                conn.Open();

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow row = dt.Rows[i];
                                    DateTime ngayGD;
                                    var s = row["NGAYGIAODICH"]?.ToString();




                                    using (SqlCommand cmd = new SqlCommand(@"
            INSERT INTO CTNH
            (NGAYGIAODICH,SOTHAMCHIEU,SOTIENCO,MOTA,STTEMP)
            VALUES
            (@NGAYGIAODICH, @SOTHAMCHIEU, @SOTIENCO, @MOTA,@SOTIENCO)
        ", conn))
                                    {
                                        if (row["NGAYGIAODICH"] != DBNull.Value)
                                        {
                                            cmd.Parameters.Add("@NGAYGIAODICH", SqlDbType.DateTime)
                                               .Value = Convert.ToDateTime(row["NGAYGIAODICH"]);
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add("@NGAYGIAODICH", SqlDbType.DateTime).Value = DBNull.Value;
                                        }


                                        //cmd.Parameters.AddWithValue("@NGAYGIAODICH", row["NGAYGIAODICH"]?.ToString());
                                        cmd.Parameters.AddWithValue("@SOTHAMCHIEU", row["SOTHAMCHIEU"]?.ToString());
                                        cmd.Parameters.AddWithValue("@SOTIENCO", row["SOTIENCO"]?.ToString());
                                        cmd.Parameters.AddWithValue("@MOTA", row["MOTA"]?.ToString());

                                        //cmd.Parameters.AddWithValue("@DATRA",
                                        //    double.TryParse(row["DATRA"]?.ToString(), out double daTra) ? daTra : 0);
                                        //cmd.Parameters.AddWithValue("@CONLAI",
                                        //    double.TryParse(row["CONLAI"]?.ToString(), out double conLai) ? conLai : 0);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                            MessageBox.Show("Import dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //load dữ liệu lên gridview
                            gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Frm_PhanTichCTNH_Load(object sender, EventArgs e)
        {
            //thiết lập lại từ ngày và đến ngày về mặc định
            txtTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            txtDenNgay.Value = DateTime.Now;

            gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH ORDER BY MADV");
        }

        private void gridViewCTNH_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridViewCTNH.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
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
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, gridViewCTNH); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, gridViewCTNH); }));//Tăng kích thước nếu text vượt quá

            }
        }
        bool cal(Int32 _with, GridView _view)
        {
            _view.IndicatorWidth = _view.IndicatorWidth < _with ? _with : _view.IndicatorWidth;
            return true;
        }

        private void cmdDel_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = txtTuNgay.Value.Date;
            DateTime denNgay = txtDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
            if (tuNgay > denNgay)
            {
                MessageBox.Show("Từ ngày phải nhỏ hơn hoặc bằng Đến ngày","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            //thiết lập con trỏ chuột dạng chờ
            this.Cursor = Cursors.WaitCursor;


            string sql = @"
                       UPDATE C
            SET C.MADV = D.MADV,C.CACHTIM='MADV'
            FROM CTNH C
            JOIN Q_DMDV D
              ON C.MOTA LIKE '%' + D.MADV + '%'
            WHERE C.MADV IS NULL";
            MyFunction.RunSQL_String(sql);

            for (int i = 30; i >= 10; i -= 2)
            {
                sql = @"
                UPDATE C
                SET 
                    C.MADV = D.MADV,
                    C.CACHTIM = 'TENDV ' + CAST(" + i + @" AS VARCHAR(10))
                FROM CTNH C
                JOIN Q_DMDV D
                    ON C.MOTA LIKE '%' + LEFT(D.TENRUTGON, " + i + @") + '%'
                WHERE C.MADV IS NULL ";

                MyFunction.RunSQL_String(sql);
            }
            //Thông báo cập nhật thành công
            this.Cursor = Cursors.Default;
            MyFunction.RunSQL("sp_AutoMap_SOHD_CTNH;");

            MessageBox.Show("Cập nhật mã đơn vị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //load lại dữ liệu
            gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH");
            //refresh lại gridview
            gridCTNH.Refresh();
        }

        private void gridViewCTNH_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;

            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            string cachTim = view.GetRowCellValue(e.RowHandle, "CACHTIM")?.ToString();

            if (!string.IsNullOrEmpty(cachTim) && cachTim.ToUpper().Contains("TENDV"))
            {
                e.Appearance.BackColor = Color.LightPink;   // nền đỏ nhạt
                e.Appearance.ForeColor = Color.DarkRed;     // chữ đỏ đậm
                e.HighPriority = true;
            }
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            //Tìm số hóa đơn.
            //thiết lập con trỏ chuột dạng chờ
            this.Cursor = Cursors.WaitCursor;

            MyFunction.RunSQL("sp_AutoMap_SOHD_CTNH;");


            this.Cursor = Cursors.Default;
            //Thông báo cập nhật thành công
            MessageBox.Show("Cập nhật số hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //load lại dữ liệu
            gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH");

        }

        private void gridViewCTNH_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            LoadChotSoLieu_MultiSOHD();
        }

        private void LoadChotSoLieu_MultiSOHD()
        {
            if (gridViewCTNH.FocusedRowHandle < 0) return;
            gridHoaDon.DataSource = null;
            //xóa dữ liệu cũ
            gridViewHoaDon .Columns.Clear();     // QUAN TRỌNG
            gridViewHoaDon .RefreshData();       // QUAN TRỌNG

            string madv = gridViewCTNH.GetFocusedRowCellValue("MADV")?.ToString();
            string sohd1 = gridViewCTNH.GetFocusedRowCellValue("SOHD1")?.ToString();
            string sohd2 = gridViewCTNH.GetFocusedRowCellValue("SOHD2")?.ToString();
            string sohd3 = gridViewCTNH.GetFocusedRowCellValue("SOHD3")?.ToString();
            string sohd4 = gridViewCTNH.GetFocusedRowCellValue("SOHD4")?.ToString();
            string sohd5 = gridViewCTNH.GetFocusedRowCellValue("SOHD5")?.ToString();
            string sohd6 = gridViewCTNH.GetFocusedRowCellValue("SOHD6")?.ToString();
            string sohd7 = gridViewCTNH.GetFocusedRowCellValue("SOHD7")?.ToString();

            List<string> listSoHD = new List<string>();
            if (!string.IsNullOrWhiteSpace(sohd1)) listSoHD.Add(sohd1);
            if (!string.IsNullOrWhiteSpace(sohd2)) listSoHD.Add(sohd2);
            if (!string.IsNullOrWhiteSpace(sohd3)) listSoHD.Add(sohd3);
            if (!string.IsNullOrWhiteSpace(sohd4)) listSoHD.Add(sohd4);
            if (!string.IsNullOrWhiteSpace(sohd5)) listSoHD.Add(sohd5);
            if (!string.IsNullOrWhiteSpace(sohd6)) listSoHD.Add(sohd6);
            if (!string.IsNullOrWhiteSpace(sohd7)) listSoHD.Add(sohd7);

            if (string.IsNullOrWhiteSpace(madv) || listSoHD.Count == 0)
            {
                gridHoaDon.DataSource = null;
                return;
            }

            // Escape dấu ' cho an toàn
            string inList = string.Join(",",
                listSoHD.Select(x => "'" + x.Replace("'", "''") + "'")
            );

            string sql = $@"
        SELECT 
            DV,
            SOHD,TONGBH, TONGTHUE,DNVHG, DVP,TONGTIENDN,NGAYIN, NGAYGIOCHOT,NOTE, CHOTTIENVE, ID, NGUOISD, Ghichu 
        FROM CHOTSOLIEU
        WHERE DV = '{madv.Replace("'", "''")}'
          AND SOHD IN ({inList})
            AND TINHTRANG = 1
        ORDER BY NGAYGIOCHOT ";

            System.Data.DataTable dt = MyFunction.GetDataTable(sql);
            gridHoaDon.DataSource = dt;
            SetupGridChotSoLieuColumns();
        }

        private void SetupGridChotSoLieuColumns()
        {
            var view = gridViewHoaDon;
           

            view.OptionsView.ShowFooter = true;
            view.OptionsView.ColumnAutoWidth = false;
            view.OptionsView.ShowAutoFilterRow = true;
            view.BestFitColumns();

            // SOHD
           MyFunction. SetCol(view, "SOHD", "Số HĐ", 150, HorzAlignment.Center);

            // DVP - Tiền phải trả
            MyFunction.SetMoneyCol(view, "DVP", "Dịch vụ phí", 120);
            // DVP - Tổng tiền chuyển FOSCO
            MyFunction.SetMoneyCol(view, "TONGTIENDN", "Tổng tiền", 120);

            // TONGBH
            MyFunction.SetMoneyCol(view, "TONGBH", "Tổng BH", 120);

            // TONGTHUE
             MyFunction.SetMoneyCol(view, "TONGTHUE", "Tổng thuế", 120);

            // NGAYIN
            MyFunction.SetDateCol(view, "NGAYIN", "Ngày in", 100);

            // NGAYGIOCHOT
            MyFunction.SetDateCol(view, "NGAYGIOCHOT", "Ngày chốt", 110);

            // CHOTTIENVE
            MyFunction.SetBoolCol(view, "CHOTTIENVE", "Đã chốt tiền", 90);
            // NGUOISD
            MyFunction.SetCol(view, "NGUOISD", "Người tạo", 120, HorzAlignment.Center);

            // Ẩn các cột không cần
            MyFunction.HideCol(view, "ID");
            MyFunction.HideCol(view, "DV");            
            MyFunction.HideCol(view, "Ghichu");
        }
        

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Phân tích tiền về theo CTNH
            gridHoaDon.DataSource = null;
            var view = gridViewHoaDon;
            view.Columns.Clear();     // QUAN TRỌNG
            view.RefreshData();       // QUAN TRỌNG
            view.OptionsView.ShowFooter = true;
            view.OptionsView.ColumnAutoWidth = false;
            view.OptionsView.ShowAutoFilterRow = true;
            view.BestFitColumns();
            groupPanel2.Text = "Phân tích chứng từ ngân hàng";

            //load dữ liệu phân tích CTNH
            gridHoaDon.DataSource = MyFunction.GetDataTable(@"SELECT
    C.NGAYGIAODICH,
    SUM(C.SOTIENCO) AS SOTIENCO,
    C.MADV,
    MAX(D.TENDV) AS TENDV,

    SUM(A.SUM_DNVHG) AS SUM_DNVHG,
    0 AS SUM_TONGBH,
    0 AS SUM_BHTN,
    0 AS SUM_CDP,
    0 AS SUM_TONGTHUE,
    0 AS SUM_THUENN,

    SUM(A.SUM_DVP) AS SUM_DVP,

    SUM(C.SOTIENCO) 
        - SUM(A.SUM_DNVHG) 
        - SUM(A.SUM_DVP) AS THUA,

    MAX(A.GHICHU) AS GHICHU
FROM CTNH C
LEFT JOIN Q_DMDV D
ON C.MADV = D.MADV
OUTER APPLY
(
    SELECT
        SUM(H.DNVHG) AS SUM_DNVHG,
        SUM(H.DVP) AS SUM_DVP,

        CASE
            WHEN MAX(RIGHT(H.SOHD,2)) IN ('KD','DV')
                THEN MAX(H.Note)

            WHEN MIN(H.LGTHANG) = MAX(H.LGTHANG)
                THEN N'THÁNG '
                     + LEFT(MIN(H.LGTHANG),2) + '/'
                     + RIGHT(MIN(H.LGTHANG),4)

            ELSE N'THÁNG '
                 + LEFT(MIN(H.LGTHANG),2) + '/'
                 + RIGHT(MIN(H.LGTHANG),4)
                 + N' --> '
                 + LEFT(MAX(H.LGTHANG),2) + '/'
                 + RIGHT(MAX(H.LGTHANG),4)
        END AS GHICHU

    FROM
    (
        SELECT SOHD
        FROM (VALUES
            (C.SOHD1),
            (C.SOHD2),
            (C.SOHD3),
            (C.SOHD4),
            (C.SOHD5),
            (C.SOHD6),
            (C.SOHD7)
        ) V(SOHD)
        WHERE SOHD IS NOT NULL
    ) X

    JOIN CHOTSOLIEU H
        ON H.SOHD = X.SOHD
        AND H.DV = C.MADV
        AND H.TINHTRANG = 1

) A

GROUP BY
    C.NGAYGIAODICH,
    C.MADV

ORDER BY
    C.NGAYGIAODICH,
    SOTIENCO ");       

            //thiết lập các Col            

            MyFunction.SetDateCol(view, "NGAYGIAODICH", "NGAY", 120);
            MyFunction.SetMoneyCol(view, "SOTIENCO", "SO TIEN", 150);
            MyFunction.SetCol(view, "MADV", "MA DV", 100, HorzAlignment.Center);
            MyFunction.SetCol(view, "TENDV", "TEN DV", 200, HorzAlignment.Default);
            MyFunction.SetMoneyCol(view, "SUM_DNVHG", "LG+PC", 100);
            MyFunction.SetMoneyCol(view, "SUM_TONGBH", "BHXH+YT", 100);
            MyFunction.SetMoneyCol(view, "SUM_TONGTHUE", "TTN", 100);
            MyFunction.SetMoneyCol(view, "SUM_BHTN", "BHTN", 100);
            MyFunction.SetMoneyCol(view, "SUM_CDP", "CDP", 100);
            MyFunction.SetMoneyCol(view, "THUA", "THUA", 100);
            MyFunction.SetMoneyCol(view, "SUM_THUENN", "THUE NN/DV K/GPLD", 100);
            MyFunction.SetMoneyCol(view, "SUM_DVP", "PHI DV", 150);
            MyFunction.SetCol(view, "GHICHU", "GHICHU", 200);

           // MyFunction.HideCol(view, "SUM_TONGTIENDN");
            //Ẩn các cột không cần thiết

        }

        private void gridViewHoaDon_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridViewHoaDon.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
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
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, gridViewHoaDon); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, gridViewHoaDon); }));//Tăng kích thước nếu text vượt quá

            }

        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            //tự động chốt  tiền về
            string sql= @"UPDATE H
                SET 
                    H.chottienve = 1,
                    H.ngaychottienve = GETDATE(),
                    H.nguoichottienve = @user
                FROM CHOTSOLIEU H
                INNER JOIN CTNH C
                    ON H.DV = C.MADV
                   AND H.SOHD IN (C.SOHD1,C.SOHD2,C.SOHD3,C.SOHD4,C.SOHD5,C.SOHD6,C.SOHD7)
                    AND H.TINHTRANG = 1 AND H.CHOTTIENVE=0
                WHERE C.stTemp = 0";
            var prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@user", MyFunction._UserName+ "- Auto")); // user đang đăng nhập
            MyFunction.RunSQL(sql, prms.ToArray());
            //thông báo chốt tiền thành công
            MessageBox.Show("Chốt tiền về thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void gridViewCTNH_DoubleClick(object sender, EventArgs e)
        {
            var view = sender as GridView;
            if (view.FocusedRowHandle < 0) return;

            string mota = view.GetFocusedRowCellValue("MOTA")?.ToString();
            string madv = view.GetFocusedRowCellValue("MADV")?.ToString();
            int id = Convert.ToInt32(view.GetFocusedRowCellValue("ID"));

            Frm_TimDonVi frm = new Frm_TimDonVi(id, mota);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //load lại form
                gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH ORDER BY MADV");
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //export excel
            //gọi hàm phân tích tiền về theo CTNH trước
             toolStripButton1_Click(sender, e);
            if (gridViewHoaDon.RowCount > 0)
            {
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    PrintingSystem ps = new PrintingSystem();
                    PrintableComponentLink link = new PrintableComponentLink(ps);

                    //float pageWidth = gridViewHoaDon.ViewRect.Width;
                    // QUAN TRỌNG khi export
                    gridViewHoaDon.AppearancePrint.Row.Font = new System.Drawing.Font("Times New Roman", 10);
                    gridViewHoaDon.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Times New Roman", 13, FontStyle.Bold);
                    gridViewHoaDon.AppearancePrint.FooterPanel.Font = new System.Drawing.Font("Times New Roman", 13, FontStyle.Bold);

                    //warp text trong ô
                    gridViewHoaDon.AppearancePrint.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    gridViewHoaDon.AppearancePrint.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    gridViewHoaDon.AppearancePrint.FooterPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    //tăng chiều cao cho HeaderPanel
                    gridViewHoaDon.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
                    gridViewHoaDon.AppearancePrint.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gridViewHoaDon.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;


                    gridViewHoaDon.AppearancePrint.Row.Options.UseTextOptions = true;

                    gridViewHoaDon.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
                    gridViewHoaDon.AppearancePrint.FooterPanel.Options.UseTextOptions = true;
                    gridViewHoaDon.AppearancePrint.Row.Options.UseFont = true;
                    gridViewHoaDon.AppearancePrint.HeaderPanel.Options.UseFont = true;
                    gridViewHoaDon.AppearancePrint.FooterPanel.Options.UseFont = true;
                    gridViewHoaDon.AppearancePrint.Row.Options.UseBackColor = true;
                    gridViewHoaDon.AppearancePrint.HeaderPanel.Options.UseBackColor = true;
                    gridViewHoaDon.AppearancePrint.FooterPanel.Options.UseBackColor = true;
                    gridViewHoaDon.AppearancePrint.Row.BackColor = Color.White;
                    gridViewHoaDon.AppearancePrint.HeaderPanel.BackColor = Color.LightGray;
                    gridViewHoaDon.AppearancePrint.FooterPanel.BackColor = Color.LightGray;




                    gridViewHoaDon.OptionsPrint.AutoWidth = true;                   

                    link.Component = gridHoaDon;                   

                    // 👉 Thêm tiêu đề phía trên
                    link.CreateMarginalHeaderArea += (s, eArgs2) =>
                    {
                        float pageWidth = eArgs2.Graph.ClientPageSize.Width;
                        string title = "BẢNG PHÂN TÍCH CHỨNG TỪ NGÂN HÀNG (VND)";
                        string dateRange = $"Từ ngày: {txtTuNgay.Value:dd/MM/yyyy} đến ngày: {txtDenNgay.Value:dd/MM/yyyy}";

                        // 3. Thiết lập Font và Căn lề (Căn giữa tiêu đề)
                        eArgs2.Graph.Font = new System.Drawing.Font("Times New Roman", 14, FontStyle.Bold);
                        eArgs2.Graph.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Center);

                        // Vẽ tiêu đề (Dòng 1)
                        eArgs2.Graph.DrawString(title, Color.Black, new RectangleF(0, 0, pageWidth, 30), DevExpress.XtraPrinting.BorderSide.None);


                       // eArgs2.Graph.DrawString(title, new RectangleF(0, 0, eArgs2.Graph.ClientPageSize.Width, 30));
                        eArgs2.Graph.DrawString(dateRange,Color.Black,  new RectangleF(0, 30, pageWidth, 20), DevExpress.XtraPrinting.BorderSide.None);
                    };
                    link.CreateReportFooterArea += (s, eArgs2) =>
                    {
                        // bỏ màu nền xám
                        eArgs2.Graph.BackColor = Color.White;

                        float pageWidth = eArgs2.Graph.ClientPageSize.Width;

                        // ===== Ngày xuất báo cáo (góc phải) =====
                        string exportTime = "Ngày " + DateTime.Now.ToString("dd") +
                                            " Tháng " + DateTime.Now.ToString("MM") +
                                            " Năm " + DateTime.Now.ToString("yyyy");

                        eArgs2.Graph.Font = new System.Drawing.Font("Times New Roman", 13, FontStyle.Italic);
                        eArgs2.Graph.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Far);

                        eArgs2.Graph.DrawString(exportTime, Color.Black,
                            new RectangleF(0, 10, pageWidth-30 , 20),
                            DevExpress.XtraPrinting.BorderSide.None);

                       
                        // ===== Khu vực chữ ký =====
                        float colWidth = pageWidth / 3;

                        eArgs2.Graph.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Center);
                        eArgs2.Graph.Font = new System.Drawing.Font("Times New Roman", 13, FontStyle.Bold);

                        eArgs2.Graph.DrawString("P.TCKT", Color.Black,
                            new RectangleF(0, 30, colWidth, 20),
                            DevExpress.XtraPrinting.BorderSide.None);

                        eArgs2.Graph.DrawString("Người lập bảng", Color.Black,
                            new RectangleF(colWidth, 30, colWidth, 20),
                            DevExpress.XtraPrinting.BorderSide.None);

                        eArgs2.Graph.DrawString("P.Giám đốc TTCULD", Color.Black,
                            new RectangleF(colWidth * 2, 30, colWidth, 20),
                            DevExpress.XtraPrinting.BorderSide.None);

                        // dòng ghi chú ký
                        eArgs2.Graph.Font = new System.Drawing.Font("Times New Roman", 9, FontStyle.Italic);

                        eArgs2.Graph.DrawString("(Ký, ghi rõ họ tên)", Color.Black,
                            new RectangleF(0, 50, colWidth, 20),
                            DevExpress.XtraPrinting.BorderSide.None);

                        eArgs2.Graph.DrawString("(Ký, ghi rõ họ tên)", Color.Black,
                            new RectangleF(colWidth, 50, colWidth, 20),
                            DevExpress.XtraPrinting.BorderSide.None);

                        eArgs2.Graph.DrawString("(Ký, ghi rõ họ tên)", Color.Black,
                            new RectangleF(colWidth * 2, 50, colWidth, 20),
                            DevExpress.XtraPrinting.BorderSide.None);
                    };                                      

                    link.PaperKind = System.Drawing.Printing.PaperKind.A3;
                    link.Landscape = true;
                    link.Margins = new System.Drawing.Printing.Margins(50, 50, 80, 80);


                    gridViewHoaDon.BestFitColumns();

                    link.CreateDocument();

                    XlsxExportOptionsEx opt = new XlsxExportOptionsEx();
                    
                    opt.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                    opt.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;

                    link.ExportToXlsx(exportFilePath, opt);

                    //gridViewHoaDon.ExportToXlsx();
                    Process.Start(exportFilePath);
                }
            }
            else
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void Options_CustomizeSheetHeader(ContextEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTuNgay_ValueChanged(object sender, EventArgs e)
        {
            if (txtTuNgay.IsEmpty || txtDenNgay.IsEmpty)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Từ ngày và Đến ngày", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            XoavaLoadCTNH();
        }

        private void txtDenNgay_ValueChanged(object sender, EventArgs e)
        {
            if (txtTuNgay.IsEmpty || txtDenNgay.IsEmpty)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Từ ngày và Đến ngày", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            XoavaLoadCTNH();
        }
        void XoavaLoadCTNH()
        {
            DateTime tuNgay = txtTuNgay.Value.Date;
            DateTime denNgay = txtDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
            if (tuNgay > denNgay)
            {
                MessageBox.Show("Từ ngày phải nhỏ hơn hoặc bằng Đến ngày", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Xóa ngày CTNH
            string sql = @"DELETE FROM CTNH
               WHERE NGAYGIAODICH < @TuNgay 
               OR NGAYGIAODICH > @DenNgay";
            var prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@TuNgay", tuNgay));
            prms.Add(new SqlParameter("@DenNgay", denNgay));
            MyFunction.RunSQL(sql, prms.ToArray());
            //load lại dữ liệu
            gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH");

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //export Misa
            //Phân tích tiền về theo CTNH
            gridHoaDon.DataSource = null;
            var view = gridViewHoaDon;
            view.Columns.Clear();     // QUAN TRỌNG
            view.RefreshData();       // QUAN TRỌNG
            view.OptionsView.ShowFooter = true;
            view.OptionsView.ColumnAutoWidth = false;
            view.OptionsView.ShowAutoFilterRow = true;
            view.BestFitColumns();
            groupPanel2.Text = "Export Misa";

            //load dữ liệu phân tích CTNH
            gridHoaDon.DataSource = MyFunction.GetDataTable(@" WITH A AS
(
    -- QUERY GỐC CỦA BẠN
    SELECT
        C.NGAYGIAODICH,
        SUM(C.SOTIENCO) AS SOTIENCO,
        C.MADV,
        '' AS TenDV,
        SUM(SUM_DVP) AS SUM_DVP,
        SUM(SUM_DNVHG) AS SUM_DNVHG,
        MAX(A.GHICHU) AS GHICHU,
        MAX(SOTHAMCHIEU) AS SOTHAMCHIEU
    FROM CTNH C
    LEFT JOIN Q_DMDV D ON C.MADV = D.MADV
   OUTER APPLY
(
    SELECT
        ISNULL(SUM(H.DNVHG),0) AS SUM_DNVHG,
        ISNULL(SUM(H.DVP),0) AS SUM_DVP,

        CASE 
            WHEN MAX(H.Note) IS NULL OR MAX(H.Note)='' 
            THEN N'DỊCH VỤ PHÍ THÁNG '+LEFT(MAX(H.LGTHANG),2)+'/'+RIGHT(MAX(H.LGTHANG),4)
            ELSE MAX(H.Note)
        END AS GHICHU

    FROM CHOTSOLIEU H
    WHERE H.DV = C.MADV
    AND H.TINHTRANG = 1
    AND H.SOHD IN (C.SOHD1,C.SOHD2,C.SOHD3,C.SOHD4,C.SOHD5,C.SOHD6,C.SOHD7)

) A
    GROUP BY C.MADV,C.NGAYGIAODICH
	
)

------------------------------------------------
-- DÒNG DNVHG
------------------------------------------------
SELECT
'' AS [Hiển thị trên sổ],
NGAYGIAODICH AS [Ngày hạch toán (*)],
NGAYGIAODICH AS [Ngày chứng từ (*)],
SOTHAMCHIEU AS [Số chứng từ (*)],
MADV AS [Mã đối tượng],
TenDV AS [Tên đối tượng],
'CULD' AS [Địa chỉ],
'007.1.00.4735213' AS [Nộp vào TK],
N'Ngân hàng TMCP Ngoại thương Việt Nam' AS [Mở tại NH],
N'34' AS [Lý do thu],
N'THU LƯƠNG+PC,'+GHICHU AS [Diễn giải lý do thu],
'' AS [Nhân viên thu],
'VND' AS [Loại tiền],
'' AS [Tỷ giá],
GHICHU AS [Diễn giải],
'1121B' AS [TK Nợ (*)],
'13885L' AS [TK Có (*)],
SUM_DNVHG AS [Số tiền],
SUM_DNVHG AS [Số tiền quy đổi],
MADV AS [Đối tượng],
'' AS [Khoản mục CP],
'' AS [Đơn vị],
'' AS [Đối tượng THCP],
'' AS [Công trình],
'' AS [Đơn đặt hàng],
'' AS [Hợp đồng mua],
'' AS [Hợp đồng bán],
'' AS [Mã thống kê]

FROM A
WHERE SUM_DNVHG > 0 

UNION ALL

------------------------------------------------
-- DÒNG DVP
------------------------------------------------

SELECT
'' ,
NGAYGIAODICH ,
NGAYGIAODICH ,
SOTHAMCHIEU ,
MADV ,
TenDV ,
'CULD' AS [Địa chỉ],
'007.1.00.4735213' AS [Nộp vào TK],
N'Ngân hàng TMCP Ngoại thương Việt Nam' AS [Mở tại NH],
N'34' AS [Lý do thu],
N'THU  '+GHICHU AS [Diễn giải lý do thu],
'' AS [Nhân viên thu],
'VND' AS [Loại tiền],
'' AS [Tỷ giá],
GHICHU AS [Diễn giải],
'1121B' AS [TK Nợ (*)],
'13885P' AS [TK Có (*)],
SUM_DVP ,
SUM_DVP ,
MADV ,
'' ,
'' ,
'' ,
'' ,
'' ,
'' ,
'' ,
''

FROM A
WHERE SUM_DVP > 0 ");           

            if (gridViewHoaDon.RowCount > 0)
            {
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;

                    gridViewHoaDon.ExportToXlsx(exportFilePath);
                    //gridViewHoaDon.ExportToXlsx();
                    Process.Start(exportFilePath);
                }
            }
            else
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }
    }



    }