using DevExpress.XtraReports.UI;
using QuanlyThue.Forms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QuanlyThue.Report
{
    public partial class R_DenghiTCKH : DevExpress.XtraReports.UI.XtraReport
    {
        public R_DenghiTCKH()
        {
            InitializeComponent();
        }
        frm_ThueNN _thuenn = (frm_ThueNN)Application.OpenForms["frm_ThueNN"];
        private void ReportHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            txt_tienchu.Text = txt_tienchu.Text + MyFunction.NumberToTextVN(decimal.Parse(_thuenn.txtTongtien_DN.Text));
            txt_tendv.Text = MyFunction.RunSQL_String("select tendv from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
        }
    }
}
