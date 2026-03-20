using DevComponents.DotNetBar.Metro;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraPrinting.Native;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DataTable = System.Data.DataTable;

namespace QuanlyThue.Forms
{
    public partial class FrmXacnhanthue : MetroForm//DevExpress.XtraEditors.XtraForm
    {
        public string _folder, _pathApp, _manv;
        public FrmXacnhanthue()
        {
            InitializeComponent();
        }

        private void searchLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            locDl();
        }

        private void chkNV_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNV.Checked)
                chkNV.Text = "Theo nhân viên";
            else
                chkNV.Text = "Theo đơn vị";


        }

        private void chkDVKY_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDVKY.Checked)
                chkDVKY.Text = "Đơn vị ký";
            else
                chkDVKY.Text = "Fosco ký";
        }

        private void cmbTuthang_SelectedIndexChanged(object sender, EventArgs e)
        {
            kiemtra();
        }
        void kiemtra()
        {
            if (double.Parse(cmbDennam.Text + cmbDenthang.Text) < double.Parse(cmbTunam.Text + cmbTuthang.Text) || DateTime.Parse("01/" + cmbDenthang.Text + "/" + cmbDennam.Text) > DateTime.Now.Date || DateTime.Parse("01/" + cmbTuthang.Text + "/" + cmbTunam.Text) > DateTime.Now.Date)
            {
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cmbTunam_SelectedIndexChanged(object sender, EventArgs e)
        {
            kiemtra();
        }

        private void cmbDenthang_SelectedIndexChanged(object sender, EventArgs e)
        {
            kiemtra();
        }

        private void cmbDennam_SelectedIndexChanged(object sender, EventArgs e)
        {
            kiemtra();
        }

        private void locDl()
        {

            int I;
            // GTBT = 11000000; GTPT = 4400000;
            String thangs, nams, LGTHANG, str, STR1, STR2;
            if (double.Parse(cmbDennam.Text + cmbDenthang.Text) < double.Parse(cmbTunam.Text + cmbTuthang.Text) || DateTime.Parse("01/" + cmbDenthang.Text + "/" + cmbDennam.Text) > DateTime.Now.Date || DateTime.Parse("01/" + cmbTuthang.Text + "/" + cmbTunam.Text) > DateTime.Now.Date)
            {
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {

                if (cmbDonvi.EditValue == null)
                {
                    MessageBox.Show("Bạn chưa chọn đơn vị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                gridDanhsach.DataSource = null;

                this.Cursor = Cursors.WaitCursor;
                MyFunction.RunSQL("delete from q_luong where nguoisd='" + MyFunction._UserName + "' or nguoisd is null");
                try
                {
                    //Số tháng cần quét
                    I = 100;
                    //nams = DateTime.Now.ToString(" MM/dd/yyyy").Trim().Substring(6, 4);
                    //thangs = DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2);
                    //int.Parse(DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2)) < 10 ? "0" + DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2) : DateTime.Now.ToString(" dd/MM/yyyy").Trim().Substring(3, 2);
                    thangs = cmbDenthang.Text;
                    nams = cmbDennam.Text;
                    do
                    {
                        LGTHANG = thangs + nams;
                        str = "D" + thangs + nams.Substring(2);
                        //Chuan hoa lai so lieu
                        MyFunction.RunSQL("update " + str + " set loainv=null where loainv=''");
                        MyFunction.RunSQL("update " + str + " set L_HDLD=null where L_HDLD=''");
                        //MyFunction.RunSQL("INSERT INTO Q_LUONG(LG_THANG, DV, MANV, HODEM, TEN, L_HDLD, LOAILG, LCV, DLCV, LTLD, DVT, SOPT, DVP, CD, LCB, TIENPC, DVTPC, AN, DT, CT, TP, KHAC, PC_TIENNHA, PC_TTKHNHA, HD_NHA, THUE_NHA,PC_TBH, PC_TTHUE, TONGLUONG, LGTNCN, LGBHXH, DV_BHXH, DV_BHYT, DV_BHTN, TC_DN, NVBHXH, NVBHYT, NVBHTN, TC_NV, Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG, PHI_CD, TIEN_DVP,SOHD, TGLG, TGBHXH, LGBHTN, TONGTIEN_DN,NGUOISD,GHICHU) SELECT LG_THANG, DV, MANV, HODEM, TEN,  L_HDLD, LOAILG, LCV, DLCV, LTLD, DVT, SOPT, DVP, CD, LCB, TIENPC, DVTPC, AN, DT, CT, TP, KHAC, PC_TIENNHA, PC_TTKHNHA, HD_NHA, THUE_NHA,PC_TBH, PC_TTHUE, TONGLUONG, LGTNCN, LGBHXH, DV_BHXH, DV_BHYT, DV_BHTN, TC_DN, NVBHXH, NVBHYT, NVBHTN, TC_NV, Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG, PHI_CD, TIEN_DVP,SOHD , TGLG, TGBHXH, LGBHTN, TONGTIEN_DN,'" + MyFunction._UserName + "',GHICHU FROM " + str + " where dv = '" + searchMADV.EditValue + "' and LOAINV is null AND LOAILG<>'S'");
                        MyFunction.RunSQL("INSERT INTO Q_LUONG(LG_THANG, DV, MANV, HODEM, TEN, L_HDLD, LOAILG, LCV, DLCV, LTLD, DVT, SOPT, DVP, CD, LCB, TIENPC, DVTPC, AN, DT, CT, TP, KHAC, PC_TIENNHA, PC_TTKHNHA, HD_NHA, THUE_NHA,PC_TBH, PC_TTHUE, TONGLUONG, LGTNCN, LGBHXH, DV_BHXH, DV_BHYT, DV_BHTN, TC_DN, NVBHXH, NVBHYT, NVBHTN, TC_NV, Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG, PHI_CD, TIEN_DVP,SOHD, TGLG, TGBHXH, LGBHTN, TONGTIEN_DN,NGUOISD,GHICHU) SELECT LG_THANG, DV, MANV, HODEM, TEN,  L_HDLD, LOAILG, LCV, DLCV, LTLD, DVT, SOPT, DVP, CD, LCB, TIENPC, DVTPC, AN, DT, CT, TP, KHAC, PC_TIENNHA, PC_TTKHNHA, HD_NHA, THUE_NHA,PC_TBH, PC_TTHUE, TONGLUONG, LGTNCN, LGBHXH, DV_BHXH, DV_BHYT, DV_BHTN, TC_DN, NVBHXH, NVBHYT, NVBHTN, TC_NV, Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG, PHI_CD, TIEN_DVP,SOHD , TGLG, TGBHXH, LGBHTN, TONGTIEN_DN,'" + MyFunction._UserName + "',GHICHU FROM " + str + " where dv = '" + cmbDonvi.EditValue + "' AND LOAILG<>'S'");
                        // MyFunction.RunSQL("UPDATE Q_LUONG SET Q_LUONG.MSTNV = Q_HSC.MST, Q_LUONG.SCMT = Q_HSC.SOCMND FROM Q_LUONG JOIN Q_HSC ON Q_LUONG.MANV = Q_HSC.MANV AND Q_LUONG.DV ='" + cmbDonvi.EditValue + "' AND Q_LUONG.NGUOISD='" + MyFunction._UserName + "'");

                        I = I - 1;
                        STR1 = thangs + nams;
                        STR2 = cmbTuthang.Text + cmbTunam.Text;
                        if (String.Compare(STR1, STR2, true) == 0)
                            I = 0;
                        if (thangs == "01")
                            thangs = "12";
                        else if (Int32.Parse(thangs) - 1 < 10)
                            thangs = "0" + (Int32.Parse(thangs) - 1).ToString();
                        else
                            thangs = (Int32.Parse(thangs) - 1).ToString();
                        if (thangs == "12")
                            nams = (Int32.Parse(nams) - 1).ToString();
                        else
                            nams = (Int32.Parse(nams)).ToString();

                    } while (I >= 1);
                    this.Cursor = Cursors.Arrow;
                    str = cmbDennam.Text + cmbDenthang.Text;
                    //MyFunction.RunSQL("delete from q_luong where dv='" + searchMADV.EditValue + "' and nguoisd='" + MyFunction._UserName + "' and convert(float,right(lg_thang,4)+left(LG_THANG,2))>'" + cmbDennam.Text + cmbDenthang.Text + "'");
                    MessageBox.Show("Đã tổng hợp xong dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData("select LG_THANG,DV, MANV, HODEM, TEN, L_HDLD, LOAILG, LCV, DLCV, LTLD, LCB, TIENPC, PC_TBH, PC_TTHUE, TONGLUONG, LGTNCN, LGBHXH, NVBHXH, NVBHYT, NVBHTN, TC_NV, Giam_TBT, ST_NGPT, TNCN, TTN, DNVHG,SOPT, SOHD  from q_luong where dv ='" + cmbDonvi.EditValue + "' and nguoisd='" + MyFunction._UserName + "' order by ten,manv,lg_thang");
                    // gridQTT.DataSource = MyFunction.GetDataTable("Select '" + cmbDennam.Text + "' AS NAM,'" + searchMADV.EditValue + "' AS DV,(CASE WHEN SUM(TC_NV)>0 THEN '' ELSE '1' END) as [10%],'' as QT, hodem AS HO,TEN,count(distinct lg_thang) as SOTHANG,SUM(LGTNCN)-sum(pc_tthue) as LUONG,sum(pc_tthue) as PHUCAP,SUM(NVBHXH) as BHXH_NV, SUM(NVBHYT) AS BHYT_NV, SUM(NVBHTN) AS BHTN_NV, sum(sopt) as SO_PHUTHUOC,sum(sopt)*'" + GTPT + "' + (count(distinct lg_thang)*'" + GTBT + "') as TONGGIAM ,SUM(TNCN) AS TNTT, sum(TTN) as THUE, '" + MyFunction.RunSQL_String("select TOP 1 MST FROM Q_DMDV WHERE MADV='" + searchMADV.EditValue + "'") + "' AS MSTVP,'' as MSCN, MSTNV,SCMT, MANV from q_luong where dv ='" + searchMADV.EditValue + "' and nguoisd='" + MyFunction._UserName + "' group by manv,hodem,ten, MSTNV,SCMT");

                    //showtreview(cmb_madv.Text);
                }
                catch { MessageBox.Show("Không có mã đơn vị này. bạn chọn lại mã đơn vị"); }
            }
        }
        void loadData(string sql)
        {
            gridDanhsach.DataSource = MyFunction.GetDataTable(sql);
            ViewDanhsach.Columns.Clear();
            DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.FieldName = "LG_THANG"; col.Caption = "Lương Tháng"; ViewDanhsach.Columns.Add(col); ViewDanhsach.Columns[0].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
            col1.FieldName = "DV"; col1.Caption = "Mã ĐV"; ViewDanhsach.Columns.Add(col1); ViewDanhsach.Columns[1].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
            col2.FieldName = "MANV"; col2.Caption = "Mã NV"; ViewDanhsach.Columns.Add(col2); ViewDanhsach.Columns[2].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
            col3.FieldName = "HODEM"; col3.Caption = "Họ lót"; ViewDanhsach.Columns.Add(col3); ViewDanhsach.Columns[3].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
            col4.FieldName = "TEN"; col4.Caption = "Tên"; ViewDanhsach.Columns.Add(col4); ViewDanhsach.Columns[4].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
            col5.FieldName = "L_HDLD"; col5.Caption = "Loại HĐLĐ"; ViewDanhsach.Columns.Add(col5); ViewDanhsach.Columns[5].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col6 = new DevExpress.XtraGrid.Columns.GridColumn();
            col6.FieldName = "SOHD"; col6.Caption = "Số HĐ"; ViewDanhsach.Columns.Add(col6); ViewDanhsach.Columns[6].Visible = true;
            DevExpress.XtraGrid.Columns.GridColumn col7 = new DevExpress.XtraGrid.Columns.GridColumn();
            col7.FieldName = "LGTNCN"; col7.Caption = "Lương TNCN"; ViewDanhsach.Columns.Add(col7); ViewDanhsach.Columns[7].Visible = true;
            ViewDanhsach.Columns[7].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            ViewDanhsach.Columns[7].DisplayFormat.FormatString = "N0";

            DevExpress.XtraGrid.Columns.GridColumn col8 = new DevExpress.XtraGrid.Columns.GridColumn();
            col8.FieldName = "TC_NV"; col8.Caption = "Bảo hiểm NV"; ViewDanhsach.Columns.Add(col8); ViewDanhsach.Columns[8].Visible = true;
            ViewDanhsach.Columns[8].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            ViewDanhsach.Columns[8].DisplayFormat.FormatString = "N0";
            DevExpress.XtraGrid.Columns.GridColumn col9 = new DevExpress.XtraGrid.Columns.GridColumn();
            col9.FieldName = "SOPT"; col9.Caption = "Số PT"; ViewDanhsach.Columns.Add(col9); ViewDanhsach.Columns[9].Visible = true;
            ViewDanhsach.Columns[9].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            ViewDanhsach.Columns[9].DisplayFormat.FormatString = "N0";

            DevExpress.XtraGrid.Columns.GridColumn col10 = new DevExpress.XtraGrid.Columns.GridColumn();
            col10.FieldName = "TNCN"; col10.Caption = "TNCN"; ViewDanhsach.Columns.Add(col10); ViewDanhsach.Columns[10].Visible = true;
            ViewDanhsach.Columns[10].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            ViewDanhsach.Columns[10].DisplayFormat.FormatString = "N0";

            DevExpress.XtraGrid.Columns.GridColumn col11 = new DevExpress.XtraGrid.Columns.GridColumn();
            col11.FieldName = "TTN"; col11.Caption = "TTN"; ViewDanhsach.Columns.Add(col11); ViewDanhsach.Columns[11].Visible = true;
            ViewDanhsach.Columns[11].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            ViewDanhsach.Columns[11].DisplayFormat.FormatString = "N0";

            ViewDanhsach.OptionsView.ShowAutoFilterRow = true;
        }
        void loadDV()
        {
            cmbDonvi.Properties.DataSource = MyFunction.GetDataTable("select madv,tendv from q_dmdv where len(madv)=5 order by madv");
            cmbDonvi.Properties.DisplayMember = "tendv";
            cmbDonvi.Properties.ValueMember = "madv";

        }

        private void FrmXacnhanthue_Load(object sender, EventArgs e)
        {
            _folder = null;
            loadDV();
            cmbTuthang.Text = cmbDenthang.Text = DateTime.Now.ToString("MM");
            cmbTunam.Text = cmbDennam.Text = DateTime.Now.ToString("yyyy");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmXacnhanthue_FormClosed(object sender, FormClosedEventArgs e)
        {
            MyFunction.RunSQL("delete q_luong where nguoisd='" + MyFunction._UserName + "' or nguoisd is null");
        }

        private void ViewDanhsach_Click(object sender, EventArgs e)
        {
            if (ViewDanhsach.RowCount > 0 && ViewDanhsach.FocusedRowHandle >= 0)
            {
                txtNhanvien.Text = ViewDanhsach.GetFocusedRowCellValue("HODEM").ToString() + " " + ViewDanhsach.GetFocusedRowCellValue("TEN").ToString();
                _manv = ViewDanhsach.GetFocusedRowCellValue("MANV").ToString();
            }
            else

                txtNhanvien.Text = _manv = null;
        }

        private void ViewDanhsach_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (ViewDanhsach.RowCount > 0 && ViewDanhsach.FocusedRowHandle >= 0)
            {
                txtNhanvien.Text = ViewDanhsach.GetFocusedRowCellValue("HODEM").ToString() + " " + ViewDanhsach.GetFocusedRowCellValue("TEN").ToString();
                _manv = ViewDanhsach.GetFocusedRowCellValue("MANV").ToString();
            }
            else

                txtNhanvien.Text = _manv = null;
        }

        private void txtNhanvien_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmdTangsize_Click(object sender, EventArgs e)
        {
            float fontSize = Math.Min(24, this.Font.Size + 2);
            if (this.Font.Size < fontSize)
              layoutControl1.Font= panel1.Font= this.Font = new System.Drawing.Font(this.Font.FontFamily, fontSize, FontStyle.Regular);

        }

        private void cmdGiamsize_Click(object sender, EventArgs e)
        {
            float fontSize = Math.Max(8.25f, this.Font.Size - 2);
            if (this.Font.Size > fontSize)
                this.Font = new System.Drawing.Font(this.Font.FontFamily, fontSize, FontStyle.Regular);
        }

        private void chkXuatfile_CheckedChanged(object sender, EventArgs e)
        {
            if (chkXuatfile.Checked)
            {
                FolderBrowserDialog folderDlg = new FolderBrowserDialog();
                folderDlg.ShowNewFolderButton = true;
                // Show the FolderBrowserDialog.  
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _folder = folderDlg.SelectedPath;

                }
            }
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            var application = new Microsoft.Office.Interop.Word.Application();

            _pathApp = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath).Trim() + "\\Templace\\bangkethue.docx";
            if (chkXuatfile.Checked && cmbChucnang.Text== "Bảng kê thu nhập thường xuyên")
            {
                //Bat dau xuat file
                if (_folder != null)
                {
                    DataTable dt;
                    var document = application.Documents.Add(Template: _pathApp);
                    DataTable _dsnv = MyFunction.GetDataTable("select distinct MANV FROM dbo.Q_LUONG WHERE NGUOISD='" + MyFunction._UserName + "'");
                    foreach (DataRow nv in _dsnv.Rows)
                    {
                       
                        dt = MyFunction.GetDataTable("SELECT MANV, HODEM, TEN, LG_THANG, SUM(LCB) AS LCB, SUM(PC_TTHUE) AS thuong_pc, SUM(TC_NV) AS TC_NV, SUM(SOPT) AS SOPT, SUM(TNCN) AS TNCN, SUM(TTN) AS TTN,CONVERT(int, RIGHT(LG_THANG, 4)) AS nam, CONVERT(int, LEFT(LG_THANG, 2)) AS thang, SUM(LGTNCN) AS LGTNCN, DV, NGUOISD, LoaiLG FROM dbo.Q_LUONG WHERE (LoaiLG <> 'N') and NGUOISD='" + MyFunction._UserName + "' and MANV='" + nv["MANV"] + "' GROUP BY MANV, HODEM, TEN, LG_THANG, CONVERT(int, LEFT(LG_THANG, 2)), CONVERT(int, RIGHT(LG_THANG, 4)), DV, NGUOISD, LoaiLG ORDER BY TEN");
                        document = application.Documents.Add(Template: _pathApp);
                        foreach (Microsoft.Office.Interop.Word.Field field in document.Fields)
                        {
                            if (field.Code.Text.Contains("txtNgay"))
                            {
                                field.Select();
                                application.Selection.TypeText("Ngày " + DateTime.Now.ToString("dd") + " Tháng " + DateTime.Now.ToString("MM") + " Năm " + DateTime.Now.ToString("yyyy"));
                            }
                            else if (field.Code.Text.Contains("txtTendv"))
                            {
                                field.Select();
                                application.Selection.TypeText(cmbDonvi.Text);
                            }
                            else if (field.Code.Text.Contains("txtTennv"))
                            {
                                field.Select();
                                application.Selection.TypeText(MyFunction.RunSQL_String("select hoten from q_hsc where manv='" + nv["MANV"] + "'"));
                            }
                            else if (field.Code.Text.Contains("txtSocccd"))
                            {
                                field.Select();
                                application.Selection.TypeText(MyFunction.RunSQL_String("select Socmnd from q_hsc where manv='" + nv["MANV"] + "'")==""?" ": MyFunction.RunSQL_String("select Socmnd from q_hsc where manv='" + nv["MANV"] + "'"));
                            }
                            else if (field.Code.Text.Contains("txtMstNV"))
                            {
                                field.Select();
                                application.Selection.TypeText(MyFunction.RunSQL_String("select MST from q_hsc where manv='" + nv["MANV"] + "'")==""?" ": MyFunction.RunSQL_String("select MST from q_hsc where manv='" + nv["MANV"] + "'"));
                            }
                            else if (field.Code.Text.Contains("txtMstVP"))
                            {
                                field.Select();
                                application.Selection.TypeText(MyFunction.RunSQL_String("select MST from q_dmdv where madv='" + cmbDonvi.EditValue + "'"));
                            }
                            else if (field.Code.Text.Contains("_Username"))
                            {
                                field.Select();
                                application.Selection.TypeText(MyFunction.RunSQL_String("select HODEM+' '+TEN from DSNV where MANVQL='" + MyFunction._UserName + "'"));
                            }
                            
                        }
                        Microsoft.Office.Interop.Word.Table _table = document.Tables[1];
                        int i = 1;
                        foreach (DataRow row in dt.Rows)
                        {
                            i++;
                            _table.Cell(i, 1).Range.Text = row["LG_THANG"].ToString();

                            _table.Cell(i, 2).Range.Text = Double.Parse(row["LCB"].ToString()).ToString("N0"); _table.Cell(i, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                            _table.Cell(i, 3).Range.Text = Double.Parse(row["thuong_pc"].ToString()).ToString("N0"); _table.Cell(i, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                            _table.Cell(i, 4).Range.Text = Double.Parse(row["LGTNCN"].ToString()).ToString("N0"); _table.Cell(i, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                            _table.Cell(i, 5).Range.Text = Double.Parse(row["TC_NV"].ToString()).ToString("N0"); _table.Cell(i, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                            _table.Cell(i, 6).Range.Text = Double.Parse(row["SOPT"].ToString()).ToString("N0"); _table.Cell(i, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                            _table.Cell(i, 7).Range.Text = Double.Parse(row["TNCN"].ToString()).ToString("N0"); _table.Cell(i, 7).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                            _table.Cell(i, 8).Range.Text = Double.Parse(row["TTN"].ToString()).ToString("N0"); _table.Cell(i, 8).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                            _table.Rows.Add();

                        }
                        _table.Cell(i + 1, 1).Range.Text = "TC:";
                        _table.Cell(i + 1, 2).AutoSum(); _table.Cell(i + 1, 3).AutoSum(); _table.Cell(i + 1, 4).AutoSum(); _table.Cell(i + 1, 5).AutoSum(); _table.Cell(i + 1, 6).AutoSum(); _table.Cell(i + 1, 7).AutoSum(); _table.Cell(i + 1, 8).AutoSum();
                        _table.Cell(i + 1, 2).Range.Bold = 1; _table.Cell(i + 1, 3).Range.Bold = 1; _table.Cell(i + 1, 4).Range.Bold = 1; _table.Cell(i + 1, 5).Range.Bold = 1; _table.Cell(i + 1, 6).Range.Bold = 1; _table.Cell(i + 1, 7).Range.Bold = 1; _table.Cell(i + 1, 8).Range.Bold = 1;
                        _table.Cell(i + 1, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                        _table.Cell(i + 1, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                        _table.Cell(i + 1, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                        _table.Cell(i + 1, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                        _table.Cell(i + 1, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                        _table.Cell(i + 1, 7).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                        _table.Cell(i + 1, 8).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                        document.SaveAs(FileName: _folder + "\\" + MyFunction.RunSQL_String("select hoten from q_hsc where manv='" + nv["MANV"] + "'") + ".docx");
                        document.Close();

                    }

                    MessageBox.Show("Đã tạo xong ");
                }
                else
                    MessageBox.Show("Bạn chưa chọn Folder lưu file");

            }
            else if(cmbChucnang.Text== "Bảng kê thu nhập thường xuyên")
            {
                if(txtNhanvien.Text!=null && txtNhanvien.Text!="")
                {
                    //bat dau bao cao bang ke thu nhap thuong xuyen của nhân viên
                    MyFunction._frm = "R_Bangkethunhap";
                    Frm_Baocao frm = new Frm_Baocao();
                    frm.Show();

                }
            }
        }
    }
}