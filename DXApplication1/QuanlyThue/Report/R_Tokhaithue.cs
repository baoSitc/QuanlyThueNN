using DevExpress.XtraReports.UI;
using QuanlyThue.Forms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QuanlyThue.Report
{
    public partial class R_Tokhaithue : DevExpress.XtraReports.UI.XtraReport
    {
        public R_Tokhaithue()
        {
            InitializeComponent();
        }
        frm_ThueNN _thuenn = (frm_ThueNN)Application.OpenForms["frm_ThueNN"];

        private void txtManv_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            txtHochieu.Text = MyFunction.RunSQL_String("select sohochieu from hochieu where manhansu='" + txtManv.Text + "'");
            txtNgaycapHC.Text = MyFunction.RunSQL_String("select ngaycaphochieu from hochieu where manhansu='" + txtManv.Text + "'")==""?null: DateTime.Parse(MyFunction.RunSQL_String("select ngaycaphochieu from hochieu where manhansu='" + txtManv.Text + "'")).ToString("dd/MM/yyyy");
            txtNoicapHC.Text = MyFunction.RunSQL_String("select noicaphochieu from hochieu where manhansu='" + txtManv.Text + "'");
            txtQT.Text = MyFunction.RunSQL_String("select Tenquocgia from quocgia where maquocgia='" + MyFunction.RunSQL_String("select QT from q_hsc where manv='" + txtManv.Text + "'") + "'");
            txtDiachiCty.Text = MyFunction.RunSQL_String("select diachi from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue+ "'");
            txtQuanhuyen.Text = MyFunction.RunSQL_String("select TenQuanHuyen from QuanHuyenVietNam where MaQuanHuyen='" + MyFunction.RunSQL_String("select QuanHuyenDonVi from DonVi where MADONVI='" + _thuenn.searchMadonvi.EditValue + "'") + "'");
            txtTinhTP.Text = MyFunction.RunSQL_String("select tentinhthanh from TinhThanhVietNam where MaTinhThanh='" + MyFunction.RunSQL_String("select TinhThanhPhoDonVi from DonVi where MADONVI='" + _thuenn.searchMadonvi.EditValue + "'") + "'");
            DTDD.Text = MyFunction.RunSQL_String("select DTDD from Q_HSC where manv='" + txtManv.Text + "'");

            txtLuong.Text = double.Parse(MyFunction.RunSQL_String("select LCV_HSC from Q_HSC where manv='" + txtManv.Text + "'") is null ? "0" : MyFunction.RunSQL_String("select LCV_HSC from Q_HSC where manv='" + txtManv.Text + "'")).ToString("N0");

            

            txtThuong.Text = double.Parse(MyFunction.RunSQL_String("select LTLD from Q_HSC where manv='" + txtManv.Text + "'") is null ? "0" : MyFunction.RunSQL_String("select LTLD from Q_HSC where manv='" + txtManv.Text + "'")).ToString("N0");
           

            txtKhac.Text = double.Parse(MyFunction.RunSQL_String("select khac from Q_HSC where manv='" + txtManv.Text + "'") == "" ? "0" : MyFunction.RunSQL_String("select khac from Q_HSC where manv='" + txtManv.Text + "'")).ToString("N0");
            
        }

        private void txt_tendv_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            txt_tendv.Text = MyFunction.RunSQL_String("select tendv from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue+ "'") + "\n MSTVP: " + MyFunction.RunSQL_String("select MST from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");


            txt_qt.Text = MyFunction.RunSQL_String("select QT from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            txt_diachi.Text = MyFunction.RunSQL_String("select diachi from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            txt_sogp.Text = MyFunction.RunSQL_String("select SOGP from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            txt_ngaygp.Text = MyFunction.RunSQL_String("select NGAYGIAYPHEP from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
            txt_tungay.Text = MyFunction.RunSQL_String("select NGAYCHINHTHUC from q_dmdv where madv='" + _thuenn.searchMadonvi.EditValue + "'");
        }
    }
}
