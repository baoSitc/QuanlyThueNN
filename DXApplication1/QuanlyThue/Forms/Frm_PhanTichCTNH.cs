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
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
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
                            //MyFunction.RunSQL("delete from CTNH WHERE KHONGXOA IS NULL");
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
            (NGAYGIAODICH,SOTHAMCHIEU,SOTIENCO,MOTA,STTEMP,KHONGXOA)
            SELECT
            @NGAYGIAODICH, @SOTHAMCHIEU, @SOTIENCO, @MOTA,@SOTIENCO,@KHONGXOA
        WHERE NOT EXISTS (
        SELECT 1 FROM CTNH WHERE ISNULL(SOTHAMCHIEU,'') = ISNULL(@SOTHAMCHIEU,''))", conn))
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
                                        cmd.Parameters.AddWithValue("@KHONGXOA", 0);                                      

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
            catch (SqlException ex)
            {

                MessageBox.Show("Lỗi SQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Frm_PhanTichCTNH_Load(object sender, EventArgs e)
        {
            //thiết lập lại từ ngày và đến ngày về mặc định
            txtTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            txtDenNgay.Value = DateTime.Now;
            //tHỰC HIỆN MAP MÃ ĐƠN VỊ TỰ ĐỘNG
            DateTime tuNgay = txtTuNgay.Value.Date;
            DateTime denNgay = txtDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
            var prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@TuNgay", tuNgay));
            prms.Add(new SqlParameter("@DenNgay", denNgay));

            gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH WHERE NGAYGIAODICH >=@TuNgay and ngaygiaodich <= @DenNgay ORDER BY MADV",prms.ToArray());
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
            //tHỰC HIỆN MAP MÃ ĐƠN VỊ TỰ ĐỘNG
            DateTime tuNgay = txtTuNgay.Value.Date;
            DateTime denNgay = txtDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
            var prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@TuNgay", tuNgay));
            prms.Add(new SqlParameter("@DenNgay", denNgay));


            if (tuNgay > denNgay)
            {
                MessageBox.Show("Từ ngày phải nhỏ hơn hoặc bằng Đến ngày", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH WHERE NGAYGIAODICH>=@TuNgay and ngaygiaodich <@DenNgay",prms.ToArray());
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
            //XÓA CTNH
            XoavaLoadCTNH();

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
            gridViewHoaDon.Columns.Clear();     // QUAN TRỌNG
            gridViewHoaDon.RefreshData();       // QUAN TRỌNG

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
            MyFunction.SetCol(view, "SOHD", "Số HĐ", 150, HorzAlignment.Center);

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
            DateTime tuNgay = txtTuNgay.Value.Date;
            DateTime denNgay = txtDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
            var prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@TuNgay", tuNgay));
            prms.Add(new SqlParameter("@DenNgay", denNgay));
           


            //load dữ liệu phân tích CTNH
            gridHoaDon.DataSource = MyFunction.GetDataTable(@"SELECT
    C.NGAYGIAODICH,
    SUM(C.SOTIENCO) AS SOTIENCO,
    LEFT(C.MADV,5) AS MADV,
    MAX(D.TENDV) AS TENDV,

    -- 🔥 nếu có trả hộ → tính khác
    SUM(
        CASE 
            WHEN E.IsTraHo = 1 THEN A.SUM_DNVHG
            ELSE A.SUM_DNVHG
        END
    ) AS SUM_DNVHG,

    0 AS SUM_TONGBH,
    0 AS SUM_BHTN,
    0 AS SUM_CDP,
    0 AS SUM_TONGTHUE,
    0 AS SUM_THUENN,

    SUM(A.SUM_DVP) AS SUM_DVP,

    SUM(CASE WHEN C.STTEMP > 0 THEN C.STTEMP ELSE 0 END) AS THUA,
    '' as PHILOAIK,'' AS MANN,'' AS LUONGNN,'' AS THUENN,'' AS DVPNN,  

    MAX(A.GHICHU) AS GHICHU

FROM CTNH C

LEFT JOIN Q_DMDV D
    ON C.MADV = D.MADV

-- 🔥 xác định có phải dòng ""trả hộ""
CROSS APPLY
(
    SELECT 
        CASE 
            WHEN EXISTS
            (
                SELECT 1
                FROM (VALUES
                    (C.SOHD1),
                    (C.SOHD2),
                    (C.SOHD3),
                    (C.SOHD4),
                    (C.SOHD5),
                    (C.SOHD6),
                    (C.SOHD7)
                ) V(SOHD)
                WHERE LEN(SOHD) > 5
                AND LEFT(SOHD,5) <> LEFT(C.MADV,5)
            )
            THEN 1 ELSE 0
        END AS IsTraHo
) E

OUTER APPLY
(
    SELECT
        SUM(H.DNVHG) AS SUM_DNVHG,
        SUM(H.DVP) AS SUM_DVP,

        CASE
            WHEN (MAX(RIGHT(H.SOHD,2)) IN ('KD','DV')) 
                 OR (MAX(H.SOHD) LIKE '%PDV%')
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
        WHERE LEN(SOHD) > 5
    ) X

    JOIN CHOTSOLIEU H
        ON H.SOHD = X.SOHD   -- 🔥 CHỈ JOIN THEO SOHD
        AND H.TINHTRANG = 1

) A

WHERE 
    C.NGAYGIAODICH >= @TuNgay
    AND C.NGAYGIAODICH <@DenNgay

GROUP BY
    C.NGAYGIAODICH,
    C.MADV,
    SOTHAMCHIEU

ORDER BY
    C.NGAYGIAODICH,
    SOTIENCO
 ", prms.ToArray());

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
            MyFunction.SetCol(view, "PHILOAIK", "PHÍ LOẠI K", 100);
            MyFunction.SetCol(view, "MANN", "MÃ NN", 100);
            MyFunction.SetCol(view, "LUONGNN", "LƯƠNG NN", 100);
            MyFunction.SetCol(view, "THUENN", "THUẾ NN", 100);
            MyFunction.SetCol(view, "DVPNN", "DVP NN", 100);
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
            string sql = @"UPDATE H
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
            prms.Add(new SqlParameter("@user", MyFunction._UserName + "- Auto")); // user đang đăng nhập
            MyFunction.RunSQL(sql, prms.ToArray());
            //thông báo chốt tiền thành công
            MessageBox.Show("Chốt tiền về thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void gridViewCTNH_DoubleClick(object sender, EventArgs e)
        {
            DateTime tuNgay = txtTuNgay.Value.Date;
            DateTime denNgay = txtDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
            var prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@TuNgay", tuNgay));
            prms.Add(new SqlParameter("@DenNgay", denNgay));


            var view = sender as GridView;
            if (view.FocusedRowHandle < 0) return;

            string mota = view.GetFocusedRowCellValue("MOTA")?.ToString();
            string madv = view.GetFocusedRowCellValue("MADV")?.ToString();
            int id = Convert.ToInt32(view.GetFocusedRowCellValue("ID"));

            Frm_TimDonVi frm = new Frm_TimDonVi(id, mota);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //load lại form
                gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH WHERE NGAYGIAODICH>=@TuNgay and NGAYGIAODICH<@DenNgay ORDER BY MADV",prms.ToArray());
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
                try
                {
                    if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                    {
                        string exportFilePath = saveFileDialogExcel.FileName;
                        PrintingSystem ps = new PrintingSystem();
                        PrintableComponentLink link = new PrintableComponentLink(ps);

                        //float pageWidth = gridViewHoaDon.ViewRect.Width;
                        // QUAN TRỌNG khi export
                        gridViewHoaDon.AppearancePrint.Row.Font = new System.Drawing.Font("Times New Roman", 13);
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
                            eArgs2.Graph.DrawString(dateRange, Color.Black, new RectangleF(0, 30, pageWidth, 20), DevExpress.XtraPrinting.BorderSide.None);
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
                                new RectangleF(0, 10, pageWidth - 30, 20),
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
                        //KIEM TRA XEM FILE ĐANG MỞ HAY KHÔNG TRƯỚC KHI EXPORT


                        link.ExportToXlsx(exportFilePath, opt);

                        //gridViewHoaDon.ExportToXlsx();
                        Process.Start(exportFilePath);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            DateTime tuNgay = txtTuNgay.Value.Date;
            DateTime denNgay = txtDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
            var prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@TuNgay", tuNgay));
            prms.Add(new SqlParameter("@DenNgay", denNgay));

            if (txtTuNgay.IsEmpty || txtDenNgay.IsEmpty)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Từ ngày và Đến ngày", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH WHERE NGAYGIAODICH >=@TuNgay and NGAYGIAODICH<=@DenNgay", prms.ToArray());
        }

        private void txtDenNgay_ValueChanged(object sender, EventArgs e)
        {
            DateTime tuNgay = txtTuNgay.Value.Date;
            DateTime denNgay = txtDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
            var prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@TuNgay", tuNgay));
            prms.Add(new SqlParameter("@DenNgay", denNgay));

            if (txtTuNgay.IsEmpty || txtDenNgay.IsEmpty)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Từ ngày và Đến ngày", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH WHERE NGAYGIAODICH >=@TuNgay and NGAYGIAODICH<=@DenNgay", prms.ToArray());
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
            //hỏi nguoi dùng có chắc chắn muốn xóa không
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu từ ngày " + tuNgay.ToString("dd/MM/yyyy") + " đến ngày " + denNgay.ToString("dd/MM/yyyy") + " không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = @"DELETE FROM CTNH
               WHERE NGAYGIAODICH >= @TuNgay 
               AND NGAYGIAODICH <= @DenNgay";
                var prms = new List<SqlParameter>();
                prms.Add(new SqlParameter("@TuNgay", tuNgay));
                prms.Add(new SqlParameter("@DenNgay", denNgay));
                MyFunction.RunSQL(sql, prms.ToArray());
                //load lại dữ liệu
                gridCTNH.DataSource = MyFunction.GetDataTable("select * from CTNH");
            }


        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //export Misa
            //Phân tích tiền về theo CTNH
            DateTime tuNgay = txtTuNgay.Value.Date;
            DateTime denNgay = txtDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
            var prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@TuNgay", tuNgay));
            prms.Add(new SqlParameter("@DenNgay", denNgay));
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
            gridHoaDon.DataSource = MyFunction.GetDataTable(@"WITH A AS
(
    SELECT
        C.NGAYGIAODICH,
        C.SOTHAMCHIEU,

        -- 🔥 dùng đơn vị con nếu có
        --X.MADV_CON AS MADV,
        C.MADV,

        '' AS TenDV,

        SUM(X.DNVHG) AS SUM_DNVHG,
        SUM(X.DVP) AS SUM_DVP,

        SUM(CASE WHEN C.STTEMP > 0 THEN C.STTEMP ELSE 0 END) AS STTEMP,

        CASE 
            WHEN MAX(RIGHT(X.SOHD,2)) IN ('KD','DV') 
                 OR MAX(X.SOHD) LIKE '%PDV%'
                THEN MAX(X.Note)
            ELSE
                N'DỊCH VỤ PHÍ THÁNG '
                + LEFT(MAX(X.LGTHANG),2) + '/'
                + RIGHT(MAX(X.LGTHANG),4)
        END AS GHICHU

    FROM CTNH C

    CROSS APPLY
    (
        SELECT 
            H.SOHD,
            LEFT(H.DV,5) AS MADV_CON,
            H.DNVHG,
            H.DVP,
            H.LGTHANG,
            H.Note
        FROM (VALUES
            (C.SOHD1),
            (C.SOHD2),
            (C.SOHD3),
            (C.SOHD4),
            (C.SOHD5),
            (C.SOHD6),
            (C.SOHD7)
        ) V(SOHD)
        JOIN CHOTSOLIEU H
            ON H.SOHD = V.SOHD
            AND H.TINHTRANG = 1
        WHERE LEN(V.SOHD) > 5
    ) X

    LEFT JOIN Q_DMDV D 
        ON D.MADV = X.MADV_CON   -- 🔥 lấy tên đơn vị con

    WHERE 
        C.NGAYGIAODICH >= @TuNgay
        AND C.NGAYGIAODICH < @DenNgay

    GROUP BY
        C.NGAYGIAODICH,
        C.SOTHAMCHIEU,
        C.MADV
)

------------------------------------------------
-- DÒNG DNVHG
------------------------------------------------
SELECT
'' AS [Hiển thị trên sổ],
NGAYGIAODICH AS [Ngày hạch toán (*)],
NGAYGIAODICH AS [Ngày chứng từ (*)],
SOTHAMCHIEU AS [Số chứng từ (*)],
LEFT(MADV,5) AS [Mã đối tượng],
TenDV AS [Tên đối tượng],
'CULD' AS [Địa chỉ],
'007.1.00.4735213' AS [Nộp vào TK],
N'Ngân hàng TMCP Ngoại thương Việt Nam' AS [Mở tại NH],
N'34' AS [Lý do thu],
N'THU LƯƠNG+PC '+ REPLACE(GHICHU,N'DỊCH VỤ PHÍ','') AS [Diễn giải lý do thu],
'' AS [Nhân viên thu],
'VND' AS [Loại tiền],
'' AS [Tỷ giá],
N'THU LƯƠNG+PC '+ REPLACE(GHICHU,N'DỊCH VỤ PHÍ','') AS [Diễn giải],
'1121B' AS [TK Nợ (*)],
'13885L' AS [TK Có (*)],
SUM_DNVHG AS [Số tiền],
SUM_DNVHG AS [Số tiền quy đổi],
LEFT(MADV,5) AS [Đối tượng],
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
LEFT(MADV,5) ,
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
WHERE SUM_DVP > 0 
UNION ALL

------------------------------------------------
-- DÒNG STTEMP (THỪA CHƯA PHÂN TÍCH)
------------------------------------------------
SELECT
'' ,
NGAYGIAODICH ,
NGAYGIAODICH ,
SOTHAMCHIEU ,
LEFT(MADV,5) ,
TenDV ,
'CULD' ,
'007.1.00.4735213' ,
N'Ngân hàng TMCP Ngoại thương Việt Nam' ,
N'34' ,
N'THỪA CHƯA PHÂN TÍCH' ,
'' ,
'VND' ,
'' ,
N'THỪA CHƯA PHÂN TÍCH' AS [Diễn giải],   -- yêu cầu của bạn
'1121B' ,
'33886' ,  -- bạn có thể đổi TK này nếu cần
STTEMP ,
STTEMP ,
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
WHERE STTEMP > 0

", prms.ToArray());

            if (gridViewHoaDon.RowCount > 0)
            {
                try
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
                catch (IOException)
                {
                    MessageBox.Show("File Excel đang mở. Vui lòng đóng file trước khi xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }



}