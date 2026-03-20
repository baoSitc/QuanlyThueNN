using DevExpress.XtraReports.UI;
using QuanlyThue.Forms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QuanlyThue.Report
{
    public partial class Report_Giaybao : DevExpress.XtraReports.UI.XtraReport
    {
        public Report_Giaybao()
        {
            InitializeComponent();
        }

        frm_ThueNN _thuenn = (frm_ThueNN)Application.OpenForms["frm_ThueNN"];

        private void ReportHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            label1.Text = _thuenn.chkChotGB.Checked ? "GIẤY BÁO THANH TOÁN\n NOTIFICATION OF PAYMENT" : "XEM TRƯỚC KHI IN - KHÔNG CÓ GIÁ TRỊ";
            txt_tendv.Text = MyFunction.RunSQL_String("select tendv from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            txt_diachi.Text = MyFunction.RunSQL_String("select diachi from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
        }

        private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (_thuenn.chkIntheohethong.Checked == false || MyFunction.RunSQL_String("select tinhtrang from chotsolieu where sohd='" + _thuenn.txtSohd.Text + "' and dachot='1' and tinhtrang='1'") == "True")
                txt_ngaythang.Text = "TP. Hồ Chí Minh, Ngày ...... Tháng ...... Năm " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4);
            else if (_thuenn.chkIntheohethong.Checked)
                txt_ngaythang.Text = "TP. Hồ Chí Minh, Ngày " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(0, 2) + " Tháng " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2) + " Năm " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4);
            
                

            txt_nguoilapbang.Text = MyFunction.RunSQL_String("select hodem from dsnv where manvql='" + MyFunction._UserName + "'") + " " + MyFunction.RunSQL_String("select ten from dsnv where manvql='" + MyFunction._UserName + "'");
           
                txt_chuky.Text = MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='CK_NN'").ToUpper();
                txt_kyten.Text = MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='TUQ_VN'") + "\n" + MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='TUQ_EN'") + "\n" + MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='CV_NN_VN'") + "\n" + MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='CV_NN_EN'");
            
        }
    }
}
