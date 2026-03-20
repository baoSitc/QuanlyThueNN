using Bussiness;
using DataLayer;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanlyThue.Forms
{
    public partial class Frm_Baocao : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Baocao()
        {
            InitializeComponent();
        }
        frm_ThueNN _thuenn = (frm_ThueNN)Application.OpenForms["frm_ThueNN"];
        FrmXacnhanthue _xacnhanthue= (FrmXacnhanthue)Application.OpenForms["FrmXacnhanthue"];
        ChotsolieuBussiness _chotsl;
        private void Frm_Baocao_Load(object sender, EventArgs e)
        {
          
            //String sql = "SELECT DISTINCT TROCAPTS.DV, TROCAPTS.MANV, TROCAPTS.HODEM, TROCAPTS.TEN, TROCAPTS.SOSOBHXH, TROCAPTS.DKTH, TROCAPTS.TTLV, TROCAPTS.THOIDIEM, TROCAPTS.TUNGAY, TROCAPTS.DENNGAY,TROCAPTS.LUYKE, TROCAPTS.NOIDUNG, TROCAPTS.DOT, TROCAPTS.QUY, TROCAPTS.NGAY_VAO, Q_DMDV.TENDV AS TENDV, Q_DMDV.SOSOBHXH AS SOBHXH_DV, TROCAPTS.THANG6,TROCAPTS.THANG, TROCAPTS.NAM, TROCAPTS.NTK, TROCAPTS.ID FROM TROCAPTS INNER JOIN Q_DMDV ON TROCAPTS.DV = Q_DMDV.MADV";
            _chotsl = new ChotsolieuBussiness();
            String sql = "";
            XtraReport rpt = new XtraReport();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (MyFunction._frm == "frm_ThueNN")
                {
                    if (_thuenn.radioInGiayBao.Checked && String.IsNullOrEmpty(_thuenn.txtSohd.Text))
                    {
                        MessageBox.Show("Không có dữ liệu để báo cáo, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (_thuenn.radioInGiayBao.Checked)
                    {
                        KTchotsolieu();

                        sql = "select * from t_thuenn where sohd='" + _thuenn.txtSohd.Text + "' and madv='" + _thuenn.searchMadonvi.EditValue.ToString().Substring(0, 5) + "'";
                        rpt = new Report.Report_Giaybao();
                    }

                    else if (_thuenn.radioGiaybaoMauK.Checked)
                    {

                        KTchotsolieu();

                        sql = "select * from t_thuenn where sohd='" + _thuenn.txtSohd.Text + "' and madv='" + _thuenn.searchMadonvi.EditValue.ToString().Substring(0, 5) + "'";
                        rpt = new Report.Report_Giaybao_MauK();
                    }
                    else if (_thuenn.radioInToKhaiThue.Checked)
                    {
                        sql = "select * from q_hsc where manv='" + _thuenn.searchTennhanvien.EditValue + "'";
                        rpt = new Report.R_Tokhaithue();
                    }
                    else if (_thuenn.radioInGiayUYQuyen.Checked)
                    {
                        sql = "select * from q_hsc where manv='" + _thuenn.searchTennhanvien.EditValue + "'";
                        rpt = new Report.R_Giayuyquyen();
                    }
                    else if (_thuenn.radioInGiayDeNghiTCKT.Checked)
                    {
                        sql = "SELECT madv,manv,ten,sohd,diengiai,tongtien_dn from T_thuenn where madv='" + _thuenn.searchMadonvi.EditValue.ToString().Substring(0, 5) + "' AND sohd ='" + _thuenn.txtSohd.Text + "'";
                        rpt = new Report.R_DenghiTCKH();
                    }
                    else if (_thuenn.radioInToKhaiTNCN.Checked)
                    {
                        sql = "select * from q_hsc where manv='" + _thuenn.searchTennhanvien.EditValue + "'";
                        rpt = new Report.R_TokhaiDangky_TNCN();
                    }
                }
                //IN thu tien mat
                else if (MyFunction._frm == "Frm_Thutienmat")
                {
                    sql = "select * from t_tienmat where ID='" + MyFunction._temp + "'";
                    rpt = new Report.R_Thutienmat();
                }
                //IN bảng kê thu nhập
                else if (MyFunction._frm == "R_Bangkethunhap")
                {
                    sql = "SELECT MANV, HODEM, TEN, LG_THANG, SUM(LCB) AS LCB, SUM(PC_TTHUE) AS THUONG, SUM(TC_NV) AS TC_NV, SUM(SOPT) AS SOPT, SUM(TNCN) AS TNCN, SUM(TTN) AS TTN,CONVERT(int, RIGHT(LG_THANG, 4)) AS nam, CONVERT(int, LEFT(LG_THANG, 2)) AS thang, SUM(LGTNCN) AS LGTNCN, DV, NGUOISD, LoaiLG FROM dbo.Q_LUONG WHERE (LoaiLG <> 'N') and NGUOISD='" + MyFunction._UserName + "' and MANV='" +_xacnhanthue._manv  + "' GROUP BY MANV, HODEM, TEN, LG_THANG, CONVERT(int, LEFT(LG_THANG, 2)), CONVERT(int, RIGHT(LG_THANG, 4)), DV, NGUOISD, LoaiLG ORDER BY TEN";
                    rpt = new Report.R_Bangkethunhap();
                    //rpt.ExportToDocx("C:\\Test.docx");
                }


                rpt.DataSource = MyFunction.GetDataTable(sql);
                documentViewer1.PrintingSystem = rpt.PrintingSystem;
                rpt.CreateDocument();
                this.Cursor = Cursors.Arrow;
            }
            catch { }
        }
        void KTchotsolieu()
        {
            if (_thuenn.chkChotGB.Checked)
            {
                if (MessageBox.Show("Bạn có chắc chắn chốt giấy báo này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //if(MyFunction.RunSQL_String("select sohd from chotsolieu where dv='" + _thuenn.searchMadonvi.EditValue+ "' and sohd='" + _thuenn.txtSohd.Text + "' and tinhtrang=1") =="")
                    if (_chotsl.GetBySohd(_thuenn.txtSohd.Text, _thuenn.searchMadonvi.EditValue.ToString()) == 0)
                    {
                        //them vao chot so lieu
                        CHOTSOLIEU _chot = new CHOTSOLIEU();
                        _chot.DV = _thuenn.searchMadonvi.EditValue.ToString();
                        _chot.SOHD = _thuenn.txtSohd.Text;
                        //_chot.TONGBH = double.Parse(_thuenn.txtBHNV.Text);
                        _chot.TONGBH = double.Parse(MyFunction.RunSQL_String("select sum(NVBHXH) from T_THUENN where sohd='"+_chot.SOHD+"' and madv='"+_chot.DV.Substring(0,5)+"'"));
                        //_chot.DNVHG = double.Parse(_thuenn.txtDNVHG.Text);
                        _chot.DNVHG = double.Parse(MyFunction.RunSQL_String("select sum(DNVHG) from T_THUENN where sohd='" + _chot.SOHD + "' and madv='" + _chot.DV.Substring(0,5) + "'"));
                        _chot.DVP = double.Parse(MyFunction.RunSQL_String("select sum(PDV) from T_THUENN where sohd='" + _chot.SOHD + "' and madv='" + _chot.DV.Substring(0, 5) + "'"));
                        _chot.TONGTHUE = double.Parse(MyFunction.RunSQL_String("select sum(TTN) from T_THUENN where sohd='" + _chot.SOHD + "' and madv='" + _chot.DV.Substring(0, 5) + "'"));
                        _chot.TONGTIENDN = double.Parse(MyFunction.RunSQL_String("select sum(TONGTIEN_DN) from T_THUENN where sohd='" + _chot.SOHD + "' and madv='" + _chot.DV.Substring(0, 5)   + "'"));
                        _chot.Note = _thuenn.cmbDiengiai.Text+"-"+_chot.DV.Substring(0,5)+"-"+_thuenn.searchTennhanvien.EditValue.ToString() + "-" + _thuenn.searchTennhanvien.Text;
                        _chot.CDP = 0;
                        _chot.tinhtrang = true;
                        _chot.DACHOT = false;
                        _chot.NGUOISD = MyFunction._UserName;
                        _chot.NGAYIN = DateTime.Now.Date;
                        _chot.LGTHANG=_thuenn.cmbThang.Text+_thuenn.cmbNam.Text;
                        _chot.LOAIGB = "TNN";
                        _chotsl.Insert(_chot);

                    }
                    else
                    {
                        _chotsl.edit(_thuenn.txtSohd.Text, _thuenn.searchMadonvi.EditValue.ToString());
                    }

                }
                else
                {
                    _thuenn.chkChotGB.Checked = false;

                }
            }
        }
    }
}