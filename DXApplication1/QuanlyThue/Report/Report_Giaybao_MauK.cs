using DevExpress.Utils.MVVM.Services;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Import.Html;
using iText.Signatures.Validation.Report;
using QuanlyThue.Forms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;

namespace QuanlyThue.Report
{
    public partial class Report_Giaybao_MauK : DevExpress.XtraReports.UI.XtraReport
    {
        public Report_Giaybao_MauK()
        {
            InitializeComponent();
        }
        frm_ThueNN _thuenn = (frm_ThueNN)Application.OpenForms["frm_ThueNN"];
        public int STT = 0;


        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (_thuenn.chkIntheohethong.Checked)
                txt_ngaythang.Text = "TP. Hồ Chí Minh, Ngày " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(0, 2) + " Tháng " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2) + " Năm " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4);
            else
                txt_ngaythang.Text = "TP. Hồ Chí Minh, Ngày ...... Tháng ...... Năm " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4);

            txt_nguoilapbang.Text = MyFunction.RunSQL_String("select hodem from dsnv where manvql='" + MyFunction._UserName + "'") + " " + MyFunction.RunSQL_String("select ten from dsnv where manvql='" + MyFunction._UserName + "'");

                txt_chuky.Text = MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='CK_VN_B'").ToUpper();
                txt_kyten.Text = MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='TUQ_VN'") + "\n" + MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='TUQ_EN'") + "\n" + MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='CV_VN_B'") + "\n" + MyFunction.RunSQL_String("select ten_option from tbl_option where ma_option='CV_EN_B'");

            xrLabel38.Text = "- Vào tài khoản (Beneficiary):" + MyFunction.RunSQL_String("select noinopthue from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            xrLabel38.Text += "\n- Mã số thuế(Tax Code):" + MyFunction.RunSQL_String("select mst from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            xrLabel38.Text += "\n- Ngân hàng:" + MyFunction.RunSQL_String("select nganhangnopthue from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'")+
                "\n- Số tài khoản(Account number)"+ MyFunction.RunSQL_String("select sotaikhoannopthue from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            xrLabel38.Text += "\n- Nội dung(Description):" + txt_tendv.Text + " - MST: " + txt_mst.Text + "- " + txtDiengiai.Text + " - Mã chương(Item no.): 557 - Tiểu mục(Subsection): " + txt_machuong.Text;
            xrLabel38.Text += "\n- Lưu ý: Thời hạn chuyển nộp cơ quan thuế: chậm nhất trước ngày 20 tháng liền kề của tháng phát giấy báo(khai thuế tháng) hoặc trước ngày 30 tháng đầu quý sau(khai thuế quý)."
                + "\n-Deadline for tax payment: no later than the 20th of the following month of this notification issuance(for monthly tax declaration) or no later than the 30th of the first month of the next quarter(for quarterly tax declaration).'";

        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            STT++;
            txt_stt.Text = STT.ToString();
        }

        private void txt_manv_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            txt_mst.Text = MyFunction.RunSQL_String("select mst from q_hsc where manv='" + txt_manv.Text + "'");
            txt_machuong.Text=_thuenn.cmbMachuong.Text;
            txtNoinopThue.Text = MyFunction.RunSQL_String("select noinopthue from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            
        }

        private void ReportHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            label1.Text = _thuenn.chkChotGB.Checked ? "GIẤY BÁO THANH TOÁN\n NOTIFICATION OF PAYMENT" : "XEM TRƯỚC KHI IN - KHÔNG CÓ GIÁ TRỊ";
            txt_tendv.Text = MyFunction.RunSQL_String("select tendv from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            txt_diachi.Text = MyFunction.RunSQL_String("select diachi from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            if (_thuenn.cmbKythue.Text == "Tháng")
                txt_thangQuy.Text = "Tháng (Month): " + _thuenn.cmbThang.Text + " Năm (Year): " + _thuenn.cmbNam.Text;
            else
                txt_thangQuy.Text = "Quý (Quarter): " + _thuenn.cmbKythue.Text.Substring(4, 1) + " Năm (Year): " + _thuenn.cmbNam.Text;
           

        }
    }
}
