using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanlyThue.Forms
{
    public partial class Frm_TimDonVi : DevExpress.XtraEditors.XtraForm
    {
        string mota = "";
        int _ctnhId = 0;
        public Frm_TimDonVi()
        {
            InitializeComponent();
        }
        public Frm_TimDonVi(int ctnhId, string mota)
        {
            InitializeComponent();
            this.mota = mota;
            this._ctnhId = ctnhId;
            txtMota.Text = mota;
        }

        private void Frm_TimDonVi_Load(object sender, EventArgs e)
        {
            // Thêm dòng "CHUA CO"
                DataTable dt = MyFunction.GetDataTable("SELECT MADV, TENDV,TRANGTHAI FROM Q_DMDV ORDER BY TENDV,TRANGTHAI");
            DataRow row = dt.NewRow();
            row["MADV"] = "?????";
            row["TENDV"] = "CHƯA BIẾT TÊN ĐƠN VỊ";
            row["TRANGTHAI"] = "Đang làm việc";
            dt.Rows.InsertAt(row, 0);   // chèn lên đầu
            //gán giá trị cho SLU

            sluDonVi.Properties.NullText = "-- Chọn đơn vị --";

            sluDonVi.Properties.DataSource =dt;

            sluDonVi.Properties.ValueMember = "MADV";
            sluDonVi.Properties.DisplayMember = "TENDV";

            sluDonVi.Properties.PopupView.Columns["MADV"].Caption = "Mã DV";
            sluDonVi.Properties.PopupView.Columns["TENDV"].Caption = "Tên đơn vị";


            sluDonVi.EditValue = MyFunction.RunSQL_String("SELECT MADV FROM CTNH WHERE ID = " + _ctnhId);
            txtThangNam.Text = MyFunction.RunSQL_String("SELECT THANGNAM_MMYYYY FROM CTNH WHERE ID = " + _ctnhId);
            txtHD1.Text = MyFunction.RunSQL_String("SELECT SOHD1 FROM CTNH WHERE ID = " + _ctnhId);
            txtHD2.Text = MyFunction.RunSQL_String("SELECT SOHD2 FROM CTNH WHERE ID = " + _ctnhId);
            txtHD3.Text = MyFunction.RunSQL_String("SELECT SOHD3 FROM CTNH WHERE ID = " + _ctnhId);
            txtHD4.Text = MyFunction.RunSQL_String("SELECT SOHD4 FROM CTNH WHERE ID = " + _ctnhId);
            txtHD5.Text = MyFunction.RunSQL_String("SELECT SOHD5 FROM CTNH WHERE ID = " + _ctnhId);
            txtHD6.Text = MyFunction.RunSQL_String("SELECT SOHD6 FROM CTNH WHERE ID = " + _ctnhId);
            txtHD7.Text = MyFunction.RunSQL_String("SELECT SOHD7 FROM CTNH WHERE ID = " + _ctnhId);
            txtTienve.EditValue = Convert.ToDecimal(MyFunction.RunSQL_String("SELECT SOTIENCO FROM CTNH WHERE ID = " + _ctnhId));
            txtConlai.EditValue = Convert.ToDecimal(MyFunction.RunSQL_String("SELECT sttemp FROM CTNH WHERE ID = " + _ctnhId));
            txtTienve.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtTienve.Properties.DisplayFormat.FormatString = "n0";
            txtConlai.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtConlai.Properties.DisplayFormat.FormatString = "n0";
           
            loadGiaybao();
            DangKySuKienHoaDon();

        }
        private void DangKySuKienHoaDon()
        {
            TextEdit[] arr =
    {
        txtHD1, txtHD2, txtHD3, txtHD4, txtHD5, txtHD6, txtHD7
    };

            foreach (var txt in arr)
            {
                txt.EditValueChanged += TxtHD_EditValueChanged;
            }
        }

        private void TxtHD_EditValueChanged(object sender, EventArgs e)
        {
            TinhTienChuaPhanTich();
        }

        void loadGiaybao()
        {
            string sql = $@"
        SELECT 
            DV,
            SOHD,TONGBH, TONGTHUE,DNVHG, DVP,TONGTIENDN,NGAYIN, NGAYGIOCHOT,NOTE, CHOTTIENVE,NGAYCHOTTIENVE, ID, NGUOISD, Ghichu,LGTHANG    
        FROM CHOTSOLIEU
        WHERE DV = '{sluDonVi.EditValue}'          
            AND TINHTRANG = 1 and lgthang like '%{txtThangNam.Text}%'
        ORDER BY id desc ";
            gridTimKiem.DataSource = MyFunction.GetDataTable(sql);
            var view = gridViewTimKiem;


            view.OptionsView.ShowFooter = true;
            view.OptionsView.ColumnAutoWidth = false;
            view.OptionsView.ShowAutoFilterRow = true;
            view.BestFitColumns();
            MyFunction.SetCol(view, "DV", "Mã ĐV", 100);
            MyFunction.SetCol(view, "SOHD", "Số HĐ", 100);
            MyFunction.SetMoneyCol(view, "TONGBH", "Tổng BH", 100);
            MyFunction.SetMoneyCol(view, "TONGTHUE", "Tổng Thuế", 100);
            MyFunction.SetMoneyCol(view, "DNVHG", "Lương", 100);
            MyFunction.SetMoneyCol(view, "DVP", "Dịch Vụ Phí", 100);
            MyFunction.SetMoneyCol(view, "TONGTIENDN", "Tổng tiền", 100);
            MyFunction.SetDateCol(view, "NGAYIN", "Ngày In", 100);
            MyFunction.SetDateCol(view, "NOTE", "Ghi Chú", 130);
            MyFunction.SetCol(view, "NGUOISD", "Người Tạo", 100);
            MyFunction.HideCol(view, "ID");
            MyFunction.SetCol(view, "CHOTTIENVE","Đã Chốt Tiền",100);
            MyFunction.SetDateCol(view, "NGAYCHOTTIENVE", "Ngày Chốt Tiền", 100);




        }

        private void cmdLuu_Click(object sender, EventArgs e)
        {
            if (sluDonVi.EditValue == null || sluDonVi.EditValue == "")
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;

            }

            //HỎI XEM CÓ CHẮC CHẮN MUỐN LƯU KHÔNG
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn lưu không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string madv = sluDonVi.EditValue.ToString();

                string sql = @"
        UPDATE CTNH
        SET MADV = N'" + madv.Replace("'", "''") + @"',
            CACHTIM = N'MANUAL',THANGNAM_MMYYYY = N'" + txtThangNam.Text.Replace("'", "''") + @"',
            SOHD1 = N'" + txtHD1.Text.Replace("'", "''") + @"',
            SOHD2 = N'" + txtHD2.Text.Replace("'", "''") + @"',
            SOHD3 = N'" + txtHD3.Text.Replace("'", "''") + @"',
            SOHD4 = N'" + txtHD4.Text.Replace("'", "''") + @"',
            SOHD5 = N'" + txtHD5.Text.Replace("'", "''") + @"',
            SOHD6 = N'" + txtHD6.Text.Replace("'", "''") + @"',
            SOHD7 = N'" + txtHD7.Text.Replace("'", "''") + @"',
            sttemp = " + Convert.ToDecimal(txtConlai.EditValue ?? 0) + @"
        WHERE ID = " + _ctnhId;

                MyFunction.RunSQL(sql);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }


        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtThangNam_Enter(object sender, EventArgs e)
        {
            loadGiaybao();

        }

        private void txtThangNam_KeyPress(object sender, KeyPressEventArgs e)
        {
            //nếu là phím enter thì load lại giấy báo
            if (e.KeyChar == (char)Keys.Enter)
            {
                loadGiaybao();
            }
        }

        private void gridViewTimKiem_DoubleClick(object sender, EventArgs e)
        {
            int rowHandle = gridViewTimKiem.FocusedRowHandle;
            if (rowHandle < 0) return;

            string soHD = gridViewTimKiem.GetRowCellValue(rowHandle, "SOHD")?.ToString();
            string lgthang = gridViewTimKiem.GetRowCellValue(rowHandle, "LGTHANG")?.ToString();

            if (string.IsNullOrEmpty(soHD)) return;

            GanSoHoaDon(soHD, lgthang);
        }

        private void GanSoHoaDon(string soHD,string lgthang)
        {
            //kiểm tra xem số tiền chưa phân tích >0 thì mới cho gán
            if (Convert.ToDecimal(txtConlai.EditValue ?? 0) <= 0)
                return;

            TextEdit[] arr =
            {
                txtHD1, txtHD2, txtHD3, txtHD4, txtHD5, txtHD6, txtHD7
            };

            // kiểm tra trùng
            if (arr.Any(t => t.Text.Trim() == soHD.Trim()))
                return;

            var emptyBox = arr.FirstOrDefault(t => string.IsNullOrWhiteSpace(t.Text));

            if (emptyBox != null)
            {
                emptyBox.Text = soHD;
                emptyBox.Focus();
                if (string.IsNullOrEmpty(txtThangNam.Text))
                {
 
                    txtThangNam.Text = lgthang;
                }

                // 👉 tính lại tiền
                TinhTienChuaPhanTich();
            }
            else
            {
                XtraMessageBox.Show("Đã đủ 7 số hóa đơn");
            }
        }
        private void TinhTienChuaPhanTich()
        {
            TextEdit[] arr =
            {
        txtHD1, txtHD2, txtHD3, txtHD4, txtHD5, txtHD6, txtHD7
    };

            decimal tongTienHoaDon = 0;

            foreach (var txt in arr)
            {
                string soHD = txt.Text.Trim();

                if (!string.IsNullOrEmpty(soHD))
                {
                    decimal tien = Convert.ToDecimal(
                        MyFunction.RunSQL_String(
                            "SELECT ISNULL(SUM(TONGTIENDN),0) FROM CHOTSOLIEU WHERE TINHTRANG =1 AND SOHD='"+soHD+"'")
                        
                    );

                    tongTienHoaDon += tien;
                }
            }

            decimal sotienve = Convert.ToDecimal(txtTienve.EditValue ?? 0);

            txtConlai.EditValue = sotienve - tongTienHoaDon;
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}