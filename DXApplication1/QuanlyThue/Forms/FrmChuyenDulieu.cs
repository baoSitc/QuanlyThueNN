using Bussiness;
using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Xpo.DB.DataStoreLongrunnersWatch;

namespace QuanlyThue.Forms
{
    public partial class FrmChuyenDulieu : DevExpress.XtraEditors.XtraForm
    {
        DonviBussiness _DV;
        public FrmChuyenDulieu()
        {
            InitializeComponent();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void loadMadv(string tuthang, string tunam)
        {
            string TenData = "D" + tuthang + tunam.Substring(2, 2);
            cmbMadv.Properties.DataSource = MyFunction.GetDataTable("select DISTINCT MADV,MADV+'-'+TENDV AS TENDV FROM Q_DMDV," + TenData + " WHERE Q_DMDV.MADV=" + TenData + ".DV");
            cmbMadv.Properties.DisplayMember = "TENDV";
            cmbMadv.Properties.ValueMember = "MADV";

        }
        void loadMadvDen(string sql)
        {
            
            cmbMadvDen.Properties.DataSource = MyFunction.GetDataTable(sql);
            cmbMadvDen.Properties.DisplayMember = "TENDV";
            cmbMadvDen.Properties.ValueMember = "MADV";

        }

        void loadSOHD(string TENTB, string MADV)
        {
            if (kiemtra() == 1)
            {
                cmbSohd.DataSource = MyFunction.GetDataTable("select DISTINCT SOHD FROM " + TENTB + " WHERE DV='" + cmbMadv.EditValue + "'");
                cmbSohd.DisplayMember = "SOHD";
                cmbSohd.ValueMember = "SOHD";
            }

        }

        private void cmbTuthang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kiemtra() == 1)
                loadMadv(cmbTuthang.Text, cmbTunam.Text);
            else
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

       

        private void cmbTunam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kiemtra()==1)
            loadMadv(cmbTuthang.Text, cmbTunam.Text);
            else
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void cmdChuyenGB_Click(object sender, EventArgs e)
        {
            string tentbTU = "D" + cmbTuthang.Text + cmbTunam.Text.Substring(2, 2);
            string tentbDen = "D" + cmbDenthang.Text + cmbDennam.Text.Substring(2, 2);


            if (cmbTuthang.Text + cmbTunam.Text == cmbDenthang.Text + cmbDennam.Text || double.Parse(cmbDenthang.Text + cmbDennam.Text) > double.Parse(DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy")))
            {
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                //bat dau chuyen du lieu
                if (MessageBox.Show("Bạn có chắc chắn chuyển dữ liệu: " + cmbSohd.Text + " từ tháng: " + cmbTuthang.Text + "/" + cmbTunam.Text + " --> " + cmbDenthang.Text + "/" + cmbDennam.Text + "?", "Thông báo", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "INSERT INTO " + tentbDen + "(LG_THANG,DV,MANV,HODEM,TEN,LOAINV,L_HDLD,LOAILG,LCV,DLCV,THUONG,LTLD,DVT,SOPT,DVP,CD,LCB, TIENPC,DVTPC, AN,DT, CT, TP,KHAC,PC_TIENNHA,PC_TTKHNHA,HD_NHA, THUE_NHA, PC_TBH,PC_TTHUE, TONGLUONG,LGTNCN,LGBHXH,DV_BHXH,DV_BHYT,DV_BHTN,TC_DN,NVBHXH,NVBHYT,NVBHTN, TC_NV, Giam_TBT, ST_NGPT,TNCN,TTN,PHI_CD,DNVHG,TIEN_DVP,SOHD,TGLG, TGBHXH,LGBHTN,TONGTIEN_DN,inhd,NGUOISD,TTN_TEMP,TNCN_TEMP,SOCV,SOTK,NGANHANG,TINHTP,THANGCN,GHICHU_HOANTHUE) " +
                    "SELECT '" + cmbDenthang.Text + cmbDennam.Text + "',DV,MANV,HODEM,TEN,LOAINV,L_HDLD,LOAILG,LCV,DLCV,THUONG,LTLD,DVT,SOPT,DVP,CD,LCB, TIENPC,DVTPC, AN,DT, CT, TP,KHAC,PC_TIENNHA,PC_TTKHNHA,HD_NHA, THUE_NHA, PC_TBH,PC_TTHUE, TONGLUONG,LGTNCN,LGBHXH,DV_BHXH,DV_BHYT,DV_BHTN,TC_DN,NVBHXH,NVBHYT,NVBHTN, TC_NV, Giam_TBT, ST_NGPT,TNCN,TTN,PHI_CD,DNVHG,TIEN_DVP,SOHD,TGLG, TGBHXH,LGBHTN,TONGTIEN_DN,inhd,NGUOISD,TTN_TEMP,TNCN_TEMP,SOCV,SOTK,NGANHANG,TINHTP,'" + cmbTuthang.Text + cmbTunam.Text + "',N'CHUYEN DU LIEU TU THANG:" + cmbTuthang.Text + cmbTunam.Text + "-" + DateTime.Now.ToString() + "' FROM " + tentbTU + " WHERE DV='" + cmbMadv.EditValue + "' AND SOHD='" + cmbSohd.Text + "'";
                    MyFunction.RunSQL(sql);

                    MyFunction.RunSQL("delete "+tentbTU+" where dv='"+cmbMadv.EditValue + "' and sohd='"+cmbSohd.SelectedValue+"'");

                    sql = "update chotsolieu set lgthang='" + cmbDenthang.Text + cmbDennam.Text + "',GHICHU='CHUYEN DU LIEU TU THANG:" + cmbDenthang.Text + cmbDennam.Text + " - " + DateTime.Now.ToString() + "'  where sohd='" + cmbSohd.Text + "' and tinhtrang=1 and lgthang='" + cmbTuthang.Text + cmbTunam.Text +"'";
                    MyFunction.RunSQL(sql);  
                    MessageBox.Show("Đã chuyển xong dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void cmbMadv_EditValueChanged(object sender, EventArgs e)
        {
            string TenData = "D" + cmbTuthang.Text + cmbTunam.Text.Substring(2, 2);
            if (cmbMadv.EditValue != null)
                loadSOHD(TenData, cmbMadv.EditValue.ToString());

        }
        int kiemtra()
        {

            if (double.Parse(cmbDennam.Text + cmbDenthang.Text) > double.Parse(DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM")) || double.Parse(cmbTunam.Text + cmbTuthang.Text) > double.Parse(DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM")))
            
                //MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            
            else return 1;
        }

        private void cmbDenthang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kiemtra() == 0)
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void cmbDennam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kiemtra() == 0)
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        void loadDanhsachNV()
        {
            gridNhanvien.DataSource = MyFunction.GetDataTable("select MADV,MANV,HODEM,TEN,GT,NGAYSINH,DONVICONGTAC,SOCMND,NGAYCAPCMND,SOCCCD,NGAYCAPCCCD,MST,TENTRANGTHAILAMVIEC,GHICHU FROM Q_HSC ORDER BY MADV,TEN ");
            ViewNV.Columns.Clear();
            DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.FieldName = "MADV"; col.Caption = "Mã đơn vị"; ViewNV.Columns.Add(col); ViewNV.Columns[0].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
            col1.FieldName = "DONVICONGTAC"; col1.Caption = "Tên đơn vị"; ViewNV.Columns.Add(col1); ViewNV.Columns[1].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
            col2.FieldName = "MANV"; col2.Caption = "Mã nhân viên"; ViewNV.Columns.Add(col2); ViewNV.Columns[2].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
            col3.FieldName = "HODEM"; col3.Caption = "Họ đệm"; ViewNV.Columns.Add(col3); ViewNV.Columns[3].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
            col4.FieldName = "TEN"; col4.Caption = "Tên"; ViewNV.Columns.Add(col4); ViewNV.Columns[4].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
            col5.FieldName = "GT"; col5.Caption = "Giới tính"; ViewNV.Columns.Add(col5); ViewNV.Columns[5].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col6 = new DevExpress.XtraGrid.Columns.GridColumn();
            col6.FieldName = "NGAYSINH"; col6.Caption = "Ngày sinh"; ViewNV.Columns.Add(col6); ViewNV.Columns[6].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col7 = new DevExpress.XtraGrid.Columns.GridColumn();
            col7.FieldName = "MST"; col7.Caption = "Mã số thuế"; ViewNV.Columns.Add(col7); ViewNV.Columns[7].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col8 = new DevExpress.XtraGrid.Columns.GridColumn();
            col8.FieldName = "SOCCCD"; col8.Caption = "Số CCCD"; ViewNV.Columns.Add(col8); ViewNV.Columns[8].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col9 = new DevExpress.XtraGrid.Columns.GridColumn();
            col9.FieldName = "TENTRANGTHAILAMVIEC"; col9.Caption = "Trạng thái"; ViewNV.Columns.Add(col9); ViewNV.Columns[9].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col10 = new DevExpress.XtraGrid.Columns.GridColumn();
            col10.FieldName = "GHICHU"; col10.Caption = "Ghi chú"; ViewNV.Columns.Add(col10); ViewNV.Columns[10].Visible = true;
        }

        private void FrmChuyenDulieu_Load(object sender, EventArgs e)
        {
            loadDanhsachNV();loadMadvDen("select DISTINCT MADV,MADV+'-'+TENDV AS TENDV FROM Q_DMDV WHERE TRANGTHAI=N'Đang làm việc' order by MADV");
            cmbTuthang.Text = DateTime.Now.ToString("MM");
            cmbDenthang.SelectedIndex = int.Parse(DateTime.Now.ToString("MM"))-2;

        }

        private void ViewNV_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {

            if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header)
            {
                e.Info.Appearance.BackColor = Color.White;
                e.Info.Appearance.ForeColor = Color.Black;
                e.Info.Appearance.DrawString(e.Cache, "STT", e.Bounds);
                e.Handled = true;
            }
            if (!ViewNV.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
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
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, ViewNV); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, ViewNV); }));//Tăng kích thước nếu text vượt quá

            }


        }
        bool cal(Int32 _with, GridView _view)
        {
            _view.IndicatorWidth = _view.IndicatorWidth < _with ? _with : _view.IndicatorWidth;
            return true;
        }
        void gangiatri()
        {
            if (ViewNV.RowCount > 0 && ViewNV.FocusedRowHandle >= 0)
            {
                try
                {
                    txtTendv.Text = ViewNV.GetFocusedRowCellValue("MADV").ToString()+"-"+ ViewNV.GetFocusedRowCellValue("DONVICONGTAC").ToString();

                    loadMadvDen("select DISTINCT MADV,MADV+'-'+TENDV AS TENDV FROM Q_DMDV WHERE TRANGTHAI=N'Đang làm việc' and madv <>'"+ ViewNV.GetFocusedRowCellValue("MADV").ToString()+"'  order by MADV");

                    txtManv.Text = ViewNV.GetFocusedRowCellValue("MANV").ToString();
                    txtHotennv.Text = ViewNV.GetFocusedRowCellValue("HODEM").ToString()+" "+ ViewNV.GetFocusedRowCellValue("TEN").ToString();
                    txtTrangthai.Text= ViewNV.GetFocusedRowCellValue("TENTRANGTHAILAMVIEC").ToString();
                    txtGhichu.Text = ViewNV.GetFocusedRowCellValue("GHICHU").ToString();
                }
                catch { }
            }
        }

        private void ViewNV_Click(object sender, EventArgs e)
        {
            txtTendv.Text = txtManv.Text = txtHotennv.Text = txtTrangthai.Text = null;
            gangiatri();
        }

        private void ViewNV_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            txtTendv.Text = txtManv.Text = txtHotennv.Text = txtTrangthai.Text = null;
            gangiatri();
        }

        private void cmdChuyenNV_Click(object sender, EventArgs e)
        {
            string sql, str;
        if( txtManv.Text ==null  || txtTrangthai.Text=="Đang làm việc" || cmbMadvDen.EditValue==null)
            {
                MessageBox.Show("Bạn phải chọn đơn vị chuyển đến, chọn mã nhân viên và nhân viên này phải nghỉ việc !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        else
            {
                if(MessageBox.Show("Bạn có chắc chắn chuyển nhân viên có mã: "+txtManv.Text+" đến đơn vị: "+cmbMadvDen.Text,"Thông báo",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes )
                {
                    str = "Chuyển từ " + ViewNV.GetFocusedRowCellValue("MADV").ToString() + " đến: " + cmbMadvDen.EditValue + " Ngày:" + DateTime.Now.ToString();
                    sql = "update nhansu set ghichu=(case when ghichu is null then N'" + str + "' else ghichu+'- '+N'" + str + "' end) ,MaDonVi='" + cmbMadvDen.EditValue + "' where madonvi='" + ViewNV.GetFocusedRowCellValue("MADV").ToString() + "' and MaNhansu='" + txtManv.Text + "'";
                    MyFunction.RunSQL(sql);
                    str = txtManv.Text + " Chuyển từ: " + ViewNV.GetFocusedRowCellValue("MADV").ToString() + " đến: " + cmbMadvDen.EditValue + " ngày:" + DateTime.Now.ToString("dd/MM/yyyy");
                    sql = "Insert into Nhatky(nguoidung,thoigian,Hanhdong,Giatri,ThucHienHanhDongThanhCong) values('" + MyFunction._UserName.ToUpper() + "','" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "','Chuyen Don Vi',N'" + str + "','1')";
                    MyFunction.RunSQL(sql);
                    //kIEM TRA VA THEM VAO HSDV, NEU CHUA CO THI SE THEM VAO DON VI CU VA DON VI MOI
                    sql = "Insert into HSDV(MANV,NGAYC,DV) VALUES('" + txtManv.Text + "','" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "','" + cmbMadvDen.EditValue + "')";
                    MyFunction.RunSQL(sql);
                    MessageBox.Show("Đã chuyển đổi xong mã đơn vị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTendv.Text = txtManv.Text = txtHotennv.Text = txtTrangthai.Text =cmbMadvDen.Text= null;
                }
            }
    
  
        }
    }
}