using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Import.Html;
using QuanlyThue.Forms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QuanlyThue.Report
{
    public partial class R_Bangkethunhap : DevExpress.XtraReports.UI.XtraReport
    {
        FrmXacnhanthue _xacnhanthue = (FrmXacnhanthue)Application.OpenForms["FrmXacnhanthue"];
        public R_Bangkethunhap()
        {
            InitializeComponent();
        }

        private void ReportHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            txtnoidung1.Text = "   Công ty TNHH MTV Dịch vụ cơ quan nước ngoài (FOSCO) thực hiện tính lương và các khoản trích theo lương cho văn phòng đại diện:" + _xacnhanthue.cmbDonvi.Text + " \r\n   Căn cứ giấy báo hàng tháng do công ty Fosco phát hành, thông tin về tiền lương và các khoản trích theo lương trong năm "+_xacnhanthue.cmbTunam.Text+" của:";
            txtSocmnd.Text += MyFunction.RunSQL_String("select socmnd from q_hsc where manv='"+_xacnhanthue._manv+"'");
            txtMstNV.Text += MyFunction.RunSQL_String("select MST from q_hsc where manv='" + _xacnhanthue._manv + "'");
            txtTendv.Text += _xacnhanthue.cmbDonvi.Text +" - Mã số thuế VP:"+ MyFunction.RunSQL_String("select MST from q_dmdv where madv='" + _xacnhanthue.cmbDonvi.EditValue + "'");
            txtTieude.Text = "BẢNG KÊ THU NHẬP THƯỜNG XUYÊN \r\n ĐÃ THÔNG BÁO QUA FOSCO \r\nNĂM " + _xacnhanthue.cmbTunam.Text;
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            txt_ngaythang.Text = "TP. Hồ Chí Minh, Ngày " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(0, 2) + " Tháng " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2) + " Năm " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4);
            txt_nguoilapbang.Text = MyFunction.RunSQL_String("select hodem from dsnv where manvql='" + MyFunction._UserName + "'") + " " + MyFunction.RunSQL_String("select ten from dsnv where manvql='" + MyFunction._UserName + "'");

            txt_chuky.Text = MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='CK_VN_B'").ToUpper();
            txt_kyten.Text = MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='TUQ_VN'") + "\n" + MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='TUQ_EN'") + "\n" + MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='CV_VN_B'") + "\n" + MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='CV_EN_B'");
        }
    }
}
