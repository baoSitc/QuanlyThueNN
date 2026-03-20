using DataLayer;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using DevExpress.Skins;
using QuanlyThue.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanlyThue
{
    public partial class frm_Main : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frm_Main()
        {
            InitializeComponent();
            DesktopAlert.BeforeAlertDisplayed += DesktopAlert_BeforeAlertDisplayed;

        }
        string text = ""; int _canhbao=0;
        #region TaskDialogInfo
        private void CreateTaskDialogInfo(long alertId)
        {
            TaskDialogInfo info = new TaskDialogInfo("Cảnh báo", (eTaskDialogIcon)Enum.Parse(typeof(eTaskDialogIcon),"BlueFlag" ),
                "Danh sách hóa đơn chưa in chốt", text, eTaskDialogButton.Ok,
                (eTaskDialogBackgroundColor)Enum.Parse(typeof(eTaskDialogBackgroundColor), "Blue"),
                null,null, null, null, null);
            TaskDialog.Show(info);
           // return info;
        }
        #endregion TaskDialogInfo

        private long _RunningAlertId = 0;

        private void DesktopAlert_BeforeAlertDisplayed(object sender, EventArgs e)
        {
            DesktopAlertWindow win=(DesktopAlertWindow)sender;
        }

        void Alert()
        {

            string _nhom2 = MyFunction.RunSQL_String("select ghichu from dsnv where manvql='"+MyFunction._UserName+"'");
            string _nhom1 = MyFunction._Nhomfc;
            try
            {
                if (_canhbao==1)
                {
                    text = "";
                    if (MyFunction._Nhomfc.Substring(0, 2) == "NN")
                    { _nhom1 = _nhom2; _nhom2 = MyFunction._Nhomfc; }


                    DataTable dt = MyFunction.GetDataTable("select DISTINCT DV,SOHD,'1' as LOAI from D" + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yy") + " where SOHD not in(select SOHD from CHOTSOLIEU where tinhtrang='1') AND DV IN(SELECT MADV FROM Q_DMDV WHERE NHOMFC='" + _nhom1 + "') UNION select DISTINCT MADV,SOHD,'2' AS LOAI from T_THUENN where CONVERT(float, right(NGAYTAO,4))>=2024  and SOHD not in(select SOHD from CHOTSOLIEU where tinhtrang='True') AND MADV IN(SELECT left(MADV,5) FROM Q_DMDV WHERE NHOMFC='" + _nhom2 + "') order by LOAI,DV"); ;

                    foreach (DataRow row in dt.Rows)
                    {
                        //MessageBox.Show(row["DV"] + ",  " + row["SHD"]);
                        text += row[1] + "; ";

                    }
                    _RunningAlertId++; eDesktopAlertColor color = eDesktopAlertColor.Default; eAlertPosition position = eAlertPosition.BottomRight;
                    if (string.IsNullOrEmpty(text))
                    {
                        //MessageBoxEx.Show("Bạn đã hết cảnh báo");

                        DesktopAlert.Show("Bạn đã hết cảnh báo", "\uf005", eSymbolSet.Awesome, Color.Empty, eDesktopAlertColor.Default, position, 15, _RunningAlertId, CreateTaskDialogInfo);
                        return;
                    }
                    _RunningAlertId++;
                    //eDesktopAlertColor color = (eDesktopAlertColor)(comboColors.SelectedItem ?? eDesktopAlertColor.Default);

                    // eAlertPosition position = (eAlertPosition)(comboLocations.SelectedItem ?? eAlertPosition.BottomRight);

                    DesktopAlert.Show(text, "\uf005", eSymbolSet.Awesome, Color.Empty, eDesktopAlertColor.DarkRed, position, 15, _RunningAlertId, CreateTaskDialogInfo);
                }
            }
            catch { MessageBox.Show("Bạn không có cảnh báo", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        public Boolean KiemTraTonTai(string Frmname)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Name.Equals(Frmname))
                    return true;
            }
            return false;
        }
        public void CloseForm()
        {
            foreach (Form frm in this.MdiChildren)
            {
                ////if (frm.Name.Equals(Frmname))
                    frm.Close();
            }
           
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Application.Exit();
        }

        private void cmdQuanlyNguoidung_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Forms.frm_Nguoidung nguoidung = new Forms.frm_Nguoidung();

            if (KiemTraTonTai("frm_Nguoidung") == true)
                nguoidung.Activate();
            else
            {
                nguoidung.MdiParent = this;
                nguoidung.Show();
            }
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
           

            F_Thongtin frm = new F_Thongtin();
            if (KiemTraTonTai("F_Thongtin") == true)
            {
                frm.Activate();
                frm.cmbTunam.Text = "2024";
                frm.cmbDennam.Text = "2024";
            }
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
            barButtonItem14.ItemClick += BarButtonItem14_ItemClick;

        }

        private void BarButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Cong no khách hàng
            FrmCongno frm = new FrmCongno();
            if (KiemTraTonTai("FrmCongno") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }


        private void frm_Main_Shown(object sender, EventArgs e)
        {
            cmdUser.Caption = MyFunction.RunSQL_String("Select hodem + ' ' + ten as hoten from dsnv where manvql='" + MyFunction._UserName + "'");
            cmdGroup.Caption = "Nhóm " + MyFunction._Nhomfc;
            cmdVersionNew.Caption = "Version: " + Application.ProductVersion;
            //Phân quyền Cho hàm Main
            //Phân quyền Xem công nợ
            if (MyFunction.RunSQL_String("select xem from phanquyen where id_chucnang='5' and iduser='" + MyFunction._UserName + "'") == "True")
            {
                ribbonCongno.Visible = true;
                
            }
            else ribbonCongno.Visible = false;
            //Phân quyền Xem hệ thống
            if (MyFunction.RunSQL_String("select xem from phanquyen where id_chucnang='6' and iduser='" + MyFunction._UserName + "'") == "True")
            {
                ribbonHethong.Visible = true;
            }
            else ribbonHethong.Visible = false;
            //Phân quyền Xem Thuế
            if (MyFunction.RunSQL_String("select xem from phanquyen where id_chucnang='1' and iduser='" + MyFunction._UserName + "'") == "True")
            {
                ribbonThue.Visible = true;
                //Thuế nước ngoài
                if (MyFunction.RunSQL_String("select xem from phanquyen where id_chucnang='2' and iduser='" + MyFunction._UserName + "'") == "True")
                {
                    barButtonThueNN.Enabled = true;
                }
                else
                {
                    barButtonThueNN.Enabled = false;
                }
                //Quyết toán thuế
                if (MyFunction.RunSQL_String("select xem from phanquyen where id_chucnang='3' and iduser='" + MyFunction._UserName + "'") == "True")
                {
                    barButtonItem4.Enabled = true;
                }
                else
                {
                    barButtonItem4.Enabled = false;
                }
            }
            else ribbonThue.Visible = false;

            //Phân quyền Xem Tiền mặt
            if (MyFunction.RunSQL_String("select xem from phanquyen where id_chucnang='4' and iduser='" + MyFunction._UserName + "'") == "True")
            {
                ribbonThutien.Visible = true;
            }
            else ribbonThutien.Visible = false;
            //Phân quyền Xem Quan ly giay bao
            if (MyFunction.RunSQL_String("select xem from phanquyen where id_chucnang='8' and iduser='" + MyFunction._UserName + "'") == "True")
            {
                ribbonQLGiaybao.Visible = true;
            }
            else ribbonQLGiaybao.Visible = false;
            //Phân quyền Xem Quan trị thiết bị
            if (MyFunction.RunSQL_String("select xem from phanquyen where id_chucnang='9' and iduser='" + MyFunction._UserName + "'") == "True")
            {
                ribbonQuanTri.Visible = true;
            }
            else ribbonQuanTri.Visible = false;





        }

        private void cmdDashboard_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            F_Thongtin frm = new F_Thongtin();
            if (KiemTraTonTai("F_Thongtin") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void cmdThongke_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void frm_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void cmdCNKhachhang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //THuế NN
            Forms.frm_ThueNN thuenn = new Forms.frm_ThueNN();

            if (KiemTraTonTai("frm_ThueNN") == true)
                thuenn.Activate();
            else
            {
                thuenn.MdiParent = this;
                thuenn.Show();
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Quyết Toán Thuế
            F_QuyetTT frm = new F_QuyetTT();
            if (KiemTraTonTai("F_QuyetTT") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {// Thu tiền mặt
            Frm_Thutienmat frm = new Frm_Thutienmat();
            if (KiemTraTonTai("Frm_Thutienmat") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }

        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Đổi mật khẩu
            F_Doipass frm = new F_Doipass();
            frm.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Đăng xuất
            //gọi lại form đăng nhập
            CloseForm();
            this.Hide();

            frm_Login frmlg = new frm_Login();
            frmlg.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //THoát khỏi CT
            Application.Exit();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            F_Thongtin frm = new F_Thongtin();
            if (KiemTraTonTai("F_Thongtin") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeList frm = new TreeList();
            if (KiemTraTonTai("TreeList") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void cmdPhanquyen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Frm_Phanquyen frm = new Frm_Phanquyen();
            if (KiemTraTonTai("Frm_Phanquyen") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void cmdGiaybao_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Frm_Giaybao frm = new Frm_Giaybao();
            if (KiemTraTonTai("Frm_Giaybao") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void cmdXacnhanthue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //FrmXacnhanthue frm = new FrmXacnhanthue();
            //if (KiemTraTonTai("FrmXacnhanthue") == true)
            //    frm.Activate();
            //else
            //{
            //    frm.MdiParent = this;
            //    frm.Show();
            //}

            //FrmTest frm=new FrmTest();
            //frm.Show();
            _canhbao = 1;
            Alert();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (DateTime.Now.ToString("HH:mm") == "8:30" || DateTime.Now.ToString("HH:mm") == "11:00" || DateTime.Now.ToString("HH:mm") == "14:00" || DateTime.Now.ToString("HH:mm") == "16:00")
            //{
            //   // _canhbao++;
            //    //Alert();
            //}
            //else
            //    _canhbao = 0;
        }

        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //quản lý thiết bị
            FrmQuanLyThietBi frm = new FrmQuanLyThietBi();
            if (KiemTraTonTai("FrmQuanLyThietBi") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void cmdTienichHethong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //quản lý tiện ích
            frm_Tienich frm = new frm_Tienich();
            if (KiemTraTonTai("frm_Tienich") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Phân tích tiền về
            Frm_PhanTichCTNH frm = new Frm_PhanTichCTNH();
            if (KiemTraTonTai("Frm_PhanTichCTNH") == true)
                frm.Activate();
            else
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }
    }
}
