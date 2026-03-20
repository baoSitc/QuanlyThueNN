using DevExpress.XtraReports.UI;
using QuanlyThue.Forms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QuanlyThue.Report
{
    public partial class R_Giayuyquyen : DevExpress.XtraReports.UI.XtraReport
    {
        public R_Giayuyquyen()
        {
            InitializeComponent();
        }
        frm_ThueNN _thuenn = (frm_ThueNN)Application.OpenForms["frm_ThueNN"];

        private void txt_hoten_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            txt_tendv.Text = MyFunction.RunSQL_String("select tendv from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
        }

        private void ReportHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            txt_hoten_nv.Text = MyFunction.RunSQL_String("select hodem from dsnv where manvql='" + MyFunction._UserName + "'") + " " + MyFunction.RunSQL_String("select ten from dsnv where manvql='" + MyFunction._UserName+ "'");

            txt_cmnd.Text = MyFunction.RunSQL_String("select socmnd from dsnv where manvql='" + MyFunction._UserName + "'");

            //txt_ngaycap.Text = MyFunction.RunSQL_String("select day(ngaycap) from dsnv where manvql='" + MyFunction._UserName + "'") + "/" + MyFunction.RunSQL_String("select month(ngaycap) from dsnv where manvql='" + MyFunction._UserName + "'") + "/" + MyFunction.RunSQL_String("select year(ngaycap) from dsnv where manvql='" + MyFunction._UserName + "'");
            txt_ngaycap.Text = MyFunction.RunSQL_String("select ngaycap from dsnv where manvql='" + MyFunction._UserName + "'");
            //+ "/" + MyFunction.RunSQL_String("select month(ngaycap) from dsnv where manvql='" + MyFunction._UserName + "'") + "/" + MyFunction.RunSQL_String("select year(ngaycap) from dsnv where manvql='" + MyFunction._UserName + "'");

            txt_noicap.Text = MyFunction.RunSQL_String("select noicap from dsnv where manvql='" + MyFunction._UserName + "'");
            txt.Text = MyFunction.RunSQL_String("select hodem from dsnv where manvql='" + MyFunction._UserName+ "'").ToUpper() + " " + MyFunction.RunSQL_String("select ten from dsnv where manvql='" + MyFunction._UserName + "'").ToUpper();

        }
    }
}
