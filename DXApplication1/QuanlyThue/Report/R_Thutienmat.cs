using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QuanlyThue.Report
{
    public partial class R_Thutienmat : DevExpress.XtraReports.UI.XtraReport
    {
        public R_Thutienmat()
        {
            InitializeComponent();
        }

        private void ReportHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            txt_ngaythang.Text = "TP. Hồ Chí Minh, Ngày " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(0, 2) + " Tháng " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2)
                + " Năm " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4) + "\n Người đề xuất";
            //  txt_ngaythang.Text = "TP. Hồ Chí Minh, Ngày 30 Tháng 01 Năm " + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(6, 4) + "\n Người đề xuất";

        }

        private void txt_tc_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            txt_tienchu.Text = "Bằng chữ: " + MyFunction.NumberToTextVN(decimal.Parse(txt_tc.Text));
        }
    }
}
