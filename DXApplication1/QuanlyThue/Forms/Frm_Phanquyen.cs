using Bussiness;
using DataLayer;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class Frm_Phanquyen : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Phanquyen()
        {
            InitializeComponent();
        }
        bool them, themUser,changepw;
        PhanquyenBussiness _phanquyen = new PhanquyenBussiness();
        ChucnangBussiness _chucnang = new ChucnangBussiness();
        Bussiness.DSNV _nv =new Bussiness.DSNV();
        void ShowHiheControl(bool t)
        {
            cmdAdd.Visible = t;
            cmdExit.Visible = t;
            cmdDel.Visible = t;
            cmdEdit.Visible = t;
            cmdSave.Visible = !t;
            cmdCancel.Visible = !t;

        }
        void ShowHiheControlUser(bool t)
        {
            cmdAddUser.Visible = t;
            cmdResetPassword.Visible = t;
            cmdEditUser.Visible = t;
            cmdSaveUser.Visible = !t;
            cmdCancelUser.Visible = !t;

        }
        void loadUserPhanquyen(string str)
        {
            cmbNguoidung.Properties.DataSource = MyFunction.GetDataTable(str);
            cmbNguoidung.Properties.DisplayMember = "HOTEN";
            cmbNguoidung.Properties.ValueMember = "MANVQL";
        }
        void loadUser(string str)
        {
            gridNguoidung.DataSource = MyFunction.GetDataTable(str);
           //cmbNguoidung.Properties.DisplayMember = "HOTEN";
           // cmbNguoidung.Properties.ValueMember = "MANVQL";
        }
        private void Frm_Phanquyen_Load(object sender, EventArgs e)
        {
            ShowHiheControl(true); ShowHiheControlUser(true); them = themUser= changepw = false;
            loadUserPhanquyen("select distinct MANVQL,HODEM,TEN,MANVQL+'-'+HODEM+' '+TEN AS HOTEN FROM DSNV,phanquyen where manvql=iduser ORDER BY TEN");
            repositoryItemCheckEdit_ToanQuyen.Click += RepositoryItemCheckEdit_ToanQuyen_Click;
            repositoryItemCheckEdit_Them.Click += RepositoryItemCheckEdit_Them_Click;
            repositoryItemCheckEdit_Xoa.Click += RepositoryItemCheckEdit_Xoa_Click;
            repositoryItemCheckEdit_Sua.Click += RepositoryItemCheckEdit_Sua_Click;
            repositoryItemCheckEdit_Xem.Click += RepositoryItemCheckEdit_Xem_Click;
            loadUser("Select distinct MANVQL, HODEM, TEN, MANHOM AS NHOMFOSCO,(CASE WHEN NHOMSD='LG' THEN N'Lương' else (CASE WHEN NHOMSD='BHXH' THEN N'Bảo hiểm xã hội' ELSE (CASE WHEN NHOMSD='PT' THEN N'Công nợ' ELSE N'Ban giám đốc' END)END) END) as NHOMSD,(case when khoataikhoan='0' then N'Đang hoạt động' else N'Ngưng hoạt động' END) as TRANGTHAI FROM DSNV order by nhomsd,ten");
            loadNhomFC("select distinct tennhom from nhomfosco order by tennhom");
            _Enable(false);txtManv.Enabled=false;
        }

        private void RepositoryItemCheckEdit_Xem_Click(object sender, EventArgs e)
        {
            //Xem
            string ID = ViewPhanquyen.GetRowCellValue(ViewPhanquyen.FocusedRowHandle, "ID").ToString();
            MyFunction.RunSQL("update phanquyen set xem=~xem where id_chucnang='" + ID + "' and iduser='" + cmbNguoidung.EditValue + "'");
            loadPhanquyen("select CHUCNANG.ID,TENCHUCNANG,TOANQUYEN,XEM,XOA,SUA,THEM FROM CHUCNANG,PHANQUYEN WHERE CHUCNANG.ID=PHANQUYEN.ID_CHUCNANG AND PHANQUYEN.IDUSER='" + cmbNguoidung.EditValue + "'");

        }

        private void RepositoryItemCheckEdit_Sua_Click(object sender, EventArgs e)
        {
            //Sửa
            string ID = ViewPhanquyen.GetRowCellValue(ViewPhanquyen.FocusedRowHandle, "ID").ToString();
            MyFunction.RunSQL("update phanquyen set sua=~sua where id_chucnang='" + ID + "' and iduser='" + cmbNguoidung.EditValue + "'");
            loadPhanquyen("select CHUCNANG.ID,TENCHUCNANG,TOANQUYEN,XEM,XOA,SUA,THEM FROM CHUCNANG,PHANQUYEN WHERE CHUCNANG.ID=PHANQUYEN.ID_CHUCNANG AND PHANQUYEN.IDUSER='" + cmbNguoidung.EditValue + "'");

        }

        private void RepositoryItemCheckEdit_Xoa_Click(object sender, EventArgs e)
        {
            //Xóa
            string ID = ViewPhanquyen.GetRowCellValue(ViewPhanquyen.FocusedRowHandle, "ID").ToString();
            MyFunction.RunSQL("update phanquyen set xoa=~xoa where id_chucnang='" + ID + "' and iduser='" + cmbNguoidung.EditValue + "'");
            loadPhanquyen("select CHUCNANG.ID,TENCHUCNANG,TOANQUYEN,XEM,XOA,SUA,THEM FROM CHUCNANG,PHANQUYEN WHERE CHUCNANG.ID=PHANQUYEN.ID_CHUCNANG AND PHANQUYEN.IDUSER='" + cmbNguoidung.EditValue + "'");

        }

        private void RepositoryItemCheckEdit_Them_Click(object sender, EventArgs e)
        {
            //Thêm
            string ID = ViewPhanquyen.GetRowCellValue(ViewPhanquyen.FocusedRowHandle, "ID").ToString();
            MyFunction.RunSQL("update phanquyen set them=~them where id_chucnang='" + ID + "' and iduser='" + cmbNguoidung.EditValue + "'");
            loadPhanquyen("select CHUCNANG.ID,TENCHUCNANG,TOANQUYEN,XEM,XOA,SUA,THEM FROM CHUCNANG,PHANQUYEN WHERE CHUCNANG.ID=PHANQUYEN.ID_CHUCNANG AND PHANQUYEN.IDUSER='" + cmbNguoidung.EditValue + "'");

        }

        private void RepositoryItemCheckEdit_ToanQuyen_Click(object sender, EventArgs e)
        {
            //Toàn Quyền
            string ID = ViewPhanquyen.GetRowCellValue(ViewPhanquyen.FocusedRowHandle, "ID").ToString();
            MyFunction.RunSQL("update phanquyen set toanquyen=~toanquyen where id_chucnang='" + ID + "' and iduser='" + cmbNguoidung.EditValue + "'");
            loadPhanquyen("select CHUCNANG.ID,TENCHUCNANG,TOANQUYEN,XEM,XOA,SUA,THEM FROM CHUCNANG,PHANQUYEN WHERE CHUCNANG.ID=PHANQUYEN.ID_CHUCNANG AND PHANQUYEN.IDUSER='" + cmbNguoidung.EditValue + "'");

        }


        private void cmdAdd_Click(object sender, EventArgs e)
        {
            ShowHiheControl(false); them = true;
            loadUserPhanquyen("select MANVQL, HODEM, TEN, MANVQL + '-' + HODEM + ' ' + TEN AS HOTEN FROM DSNV where manvql not in(select distinct iduser from phanquyen)  ORDER BY TEN");
        }

        private void cmdAddUser_Click(object sender, EventArgs e)
        {
            ShowHiheControlUser(false);
            xoatrang();them = true;_Enable(true);txtManv.Enabled = true;
            txtManv.Focus();
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            ShowHiheControl(false);
        }

        private void cmdEditUser_Click(object sender, EventArgs e)
        {
            ShowHiheControlUser(false);_Enable(true);txtMatkhau.Enabled = txtLaplaiMK.Enabled = false;txtHolot.Focus();
           
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            ShowHiheControl(true);
        }

        private void cmbNguoidung_EditValueChanged(object sender, EventArgs e)
        {
            if (them)
            {
                List<PHANQUYEN> _lstPhanquyen = new List<PHANQUYEN>();
                _lstPhanquyen = _phanquyen.GetAllbyUser(cmbNguoidung.EditValue.ToString());
                if (_lstPhanquyen.Count > 0)
                {
                    MessageBox.Show("Người dùng này đã được phân quyền", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    //Thêm mới phân quyền
                    foreach (var item in _chucnang.GetAll())
                    {
                        PHANQUYEN pHANQUYEN = new PHANQUYEN();
                        pHANQUYEN.ID_CHUCNANG = item.ID;
                        pHANQUYEN.IDUSER = cmbNguoidung.EditValue.ToString();
                        pHANQUYEN.XEM = false;
                        pHANQUYEN.THEM = false;
                        pHANQUYEN.SUA = false;
                        pHANQUYEN.XOA = false;
                        pHANQUYEN.NGAYTAO = DateTime.Now.Date;
                        pHANQUYEN.TOANQUYEN = false;
                        _phanquyen.Insert(pHANQUYEN);


                    }
                    loadPhanquyen("select CHUCNANG.ID,TENCHUCNANG,TOANQUYEN,XEM,XOA,SUA,THEM FROM CHUCNANG,PHANQUYEN WHERE CHUCNANG.ID=PHANQUYEN.ID_CHUCNANG AND PHANQUYEN.IDUSER='" + cmbNguoidung.EditValue + "'");

                }

            }
            else
            {
                loadPhanquyen("select CHUCNANG.ID,TENCHUCNANG,TOANQUYEN,XEM,XOA,SUA,THEM FROM CHUCNANG,PHANQUYEN WHERE CHUCNANG.ID=PHANQUYEN.ID_CHUCNANG AND PHANQUYEN.IDUSER='" + cmbNguoidung.EditValue + "'");

            }
        }
        void loadPhanquyen(string sql)
        {
            gridPhanquyen.DataSource = MyFunction.GetDataTable(sql);
        }

        private void cmdDel_Click(object sender, EventArgs e)
        {
            //Xóa phân quyền
            if (cmbNguoidung.EditValue != null)
                if (MessageBox.Show("Bạn có chắc chắn xóa phân quyền cho User: " + cmbNguoidung.EditValue.ToString(),"Thông báo",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
                {
                    MyFunction.RunSQL("delete phanquyen where iduser='" + cmbNguoidung.EditValue + "'");
                    MessageBox.Show("Đã xóa xong!", "Thông bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadUserPhanquyen("select distinct MANVQL,HODEM,TEN,MANVQL+'-'+HODEM+' '+TEN AS HOTEN FROM DSNV,phanquyen where manvql=iduser ORDER BY TEN");

                }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            them = false; ShowHiheControlUser(true);
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            them = false; ShowHiheControlUser(true);this.Close();   
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            gangiatri();
        }
        void gangiatri()
        {
            if (gridView1.RowCount > 0 && gridView1.FocusedRowHandle >= 0)
            {
                try
                {
                    txtManv.Text = gridView1.GetFocusedRowCellValue("MANVQL").ToString();
                    txtHolot.Text = gridView1.GetFocusedRowCellValue("HODEM").ToString();
                    txtTen.Text = gridView1.GetFocusedRowCellValue("TEN").ToString();
                    cmbNhomsd.Text = gridView1.GetFocusedRowCellValue("NHOMSD").ToString();
                    cmbNhomFC.Text = gridView1.GetFocusedRowCellValue("NHOMFOSCO").ToString();
                    cmbTrangthai.Text = gridView1.GetFocusedRowCellValue("TRANGTHAI").ToString();

                }

                catch { }
            }
        
    }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gangiatri();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridView1.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
            {
                if (e.Info.IsRowIndicator) //Nếu là dòng Indicator
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1;//Không hiển thị hình
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();//Số thứ tự tăng dần

                    }
                    SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);//Lấy kích thước của vùng hiển thị Text
                    Int32 _with = Convert.ToInt32(_size.Width + 20);
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, gridView1); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, gridView1); }));//Tăng kích thước nếu text vượt quá

            }
        }
        bool cal(Int32 _with, GridView _view)
        {
            _view.IndicatorWidth = _view.IndicatorWidth < _with ? _with : _view.IndicatorWidth;
            return true;
        }

        private void cmdCancelUser_Click(object sender, EventArgs e)
        {
            them = false; ShowHiheControlUser(true);_Enable(false);txtManv.Enabled = false;
        }
        void loadNhomFC(string sql)
        {
            cmbNhomFC.DataSource=MyFunction.GetDataTable(sql);
            cmbNhomFC.DisplayMember = "tennhom";
            cmbNhomFC.ValueMember = "tennhom";
        }

        private void cmdSaveUser_Click(object sender, EventArgs e)
        {
            if(them)
            { 
                if(string.IsNullOrEmpty(txtManv.Text) || string.IsNullOrEmpty(txtHolot.Text) || string.IsNullOrEmpty(txtTen.Text) || string.IsNullOrEmpty(cmbNhomsd.Text) || string.IsNullOrEmpty(cmbNhomFC.Text))
                {
                    MessageBox.Show("Bạn nhập chưa đầy đủ thông tin, vui lòng kiểm tra lại!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                else if(_nv.FindUser(txtManv.Text)!=null) 
                {
                    MessageBox.Show("Mã nhân viên này đã có, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (string.IsNullOrEmpty(txtMatkhau.Text) || string.IsNullOrEmpty(txtLaplaiMK.Text) || txtLaplaiMK.Text!=txtMatkhau.Text)
                {
                    MessageBox.Show("Mật khẩu chưa nhập hoặc chưa trùng khớp, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    //bắt đ8ầu thêm nhân viên
                   DataLayer.DSNV sNV = new DataLayer.DSNV();
                    sNV.MANVQL = txtManv.Text;
                    sNV.HODEM=txtHolot.Text;
                    sNV.TEN = txtTen.Text;
                    sNV.MANHOM = cmbNhomFC.Text;
                    sNV.NHOMSD = cmbNhomsd.Text;
                    sNV.KHOATAIKHOAN = 0;
                    sNV.Khoasolieu = 0;
                    sNV.Chinhsua= cmbNhomsd.Text=="Lương"?1:0;
                    sNV.InSL = cmbNhomsd.Text == "Lương" ? 1 : 0;
                    sNV.Xem = cmbNhomsd.Text == "Lương" ? 0 : 1;
                    sNV.MATKHAU = txtMatkhau.Text;                    
                    _nv.AddUser(sNV);
                }
            }
            else if(changepw)
            {
                if (string.IsNullOrEmpty(txtMatkhau.Text) || string.IsNullOrEmpty(txtLaplaiMK.Text) || txtLaplaiMK.Text != txtMatkhau.Text)
                {
                    MessageBox.Show("Mật khẩu chưa nhập hoặc chưa trùng khớp, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else //Bắt đầu đổi mật khẩu
                {
                    if(_nv.ChangePW(txtManv.Text, txtMatkhau.Text)==1)
                        MessageBox.Show("Đã cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Đã có lỗi khi cập nhật, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            else //Update
            {
                DataLayer.DSNV _edit = new DataLayer.DSNV();
                _edit.MANVQL = txtManv.Text;
                _edit.HODEM = txtHolot.Text;
                _edit.TEN = txtTen.Text;
                _edit.MANHOM = cmbNhomFC.Text;
                _edit.NHOMSD = cmbNhomsd.Text;
                _edit.KHOATAIKHOAN = cmbTrangthai.Text == "Đang hoạt động" ? 0 : 1;
                _edit.Khoasolieu = 0;
                _edit.Chinhsua = cmbNhomsd.Text == "Lương" ? 1 : 0;
                _edit.InSL = cmbNhomsd.Text == "Lương" ? 1 : 0;
                _edit.Xem = cmbNhomsd.Text == "Lương" ? 0 : 1;
                _edit.MATKHAU = txtMatkhau.Text;
                if(_nv.EditUser(_edit)==1)
                    MessageBox.Show("Đã cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Đã có lỗi khi cập nhật, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            loadUser("Select distinct MANVQL, HODEM, TEN, MANHOM AS NHOMFOSCO,(CASE WHEN NHOMSD='LG' THEN N'Lương' else (CASE WHEN NHOMSD='BHXH' THEN N'Bảo hiểm xã hội' ELSE (CASE WHEN NHOMSD='PT' THEN N'Công nợ' ELSE N'Ban giám đốc' END)END) END) AS NHOMSD,(case when khoataikhoan='0' then N'Đang hoạt động' else N'Ngưng hoạt động' END) as TRANGTHAI FROM DSNV order by nhomsd,ten");
            them = false; ShowHiheControlUser(true);_Enable(false);txtManv.Enabled = false;

        }

        void xoatrang()
        {
            _Enable(true);
            txtManv.Text = null;
            txtHolot.Text = null;
            txtTen.Text = null;
            cmbNhomsd.Text = null;
            cmbNhomFC.Text = null;
            cmbTrangthai.Text = "Đang hoạt động";
        }

        private void cmdResetPassword_Click(object sender, EventArgs e)
        {
            ShowHiheControlUser(false);txtLaplaiMK.Enabled = txtMatkhau.Enabled = changepw= true;
        }

        void _Enable(bool enable)
        {
            txtHolot.Enabled  = txtTen.Enabled = cmbNhomsd.Enabled=cmbNhomFC.Enabled=cmbTrangthai.Enabled=txtMatkhau.Enabled=txtLaplaiMK.Enabled= enable;

        }
    }
}