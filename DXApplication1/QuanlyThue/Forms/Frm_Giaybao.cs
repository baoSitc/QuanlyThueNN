using Bussiness;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.Import.Html;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using GridRow = DevComponents.DotNetBar.SuperGrid.GridRow;
using Rectangle = System.Drawing.Rectangle;

namespace QuanlyThue.Forms
{
    public partial class Frm_Giaybao : DevExpress.XtraEditors.XtraForm
    {

        public Frm_Giaybao()
        {
            InitializeComponent();
        }
        ChotsolieuBussiness _chotsl; bool loi, huygb = false;
        RepositoryItemCheckEdit RepositoryItemCheckEdit1 = new RepositoryItemCheckEdit();

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Frm_Giaybao_Load(object sender, EventArgs e)
        {
            txtTungay.Value = DateTime.Parse("01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());
            txtDenngay.Value = DateTime.Now;
            loi = false;
            if (MyFunction.RunSQL_String("select toanquyen from phanquyen where id_chucnang=8 and iduser='" + MyFunction._UserName + "'") != "True")
                ViewGiaybao.OptionsBehavior.ReadOnly = true;

            superGridControl1.CellValueChanged += SuperGridControl1_CellValueChanged;
            superGridControl1.PreRenderCell += SuperGridControl1_PreRenderCell;
            superGridControl1.PreRenderRow += SuperGridControl1_PreRenderRow;   

        }

        private void SuperGridControl1_PreRenderRow(object sender, GridPreRenderRowEventArgs e)
        {
            // Render the row whitespace if the row is selected

            if (e.RenderParts == RenderParts.Whitespace)
            {
                if (e.GridRow.IsSelected == true)
                {
                    RenderRect(e.Graphics, e.Bounds);
                    e.Cancel = true;
                }
            }
        }

        private void SuperGridControl1_PreRenderCell(object sender, GridPreRenderCellEventArgs e)
        {
            if (e.RenderParts == RenderParts.Background)
            {
                // Render the cell background if the cell is selected

                if (e.GridCell.IsSelected == true)
                {
                    RenderRect(e.Graphics, e.Bounds);
                    e.Cancel = true;
                }
            }
            else if (e.RenderParts == RenderParts.Border)
            {
                // Don't render the border if the entire row is selected

                if (e.GridCell.GridRow.IsSelected == true)
                    e.Cancel = true;
            }
        }

        private void SuperGridControl1_CellValueChanged(object sender, GridCellValueChangedEventArgs e)
        {
            GridCell cell = e.GridCell;

            // If the cell changing value is in the "Power State" column
            // then adjust the row "Start/Stop" cell appropriately

            if (cell.GridColumn.Name.Equals("Hủy chốt") == true)
            {
                GridRow row = cell.GridRow;

                // Hide the cell if the switch button is off
                // and reset the cell's value back to the "Start" state

                //row.Cells["Start/Stop"].Visible = (bool)e.NewValue;
                //row.Cells["Start/Stop"].Value = (bool)e.NewValue ? "Start" : "Hidden";
               
                if (MessageBox.Show("Bạn có chắc chắn hủy chốt giấy báo: " + row.Cells["Số hóa đơn"].Value.ToString() + " ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    MyFunction.RunSQL("update chotsolieu set tinhtrang=0,scan='" + MyFunction._UserName + "; " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where ID='" + row.Cells["ID"].Value.ToString() + "'");
                    MessageBox.Show("Đã yêu cầu hủy chốt giấy báo:" + row.Cells["Số hóa đơn"].Value.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    

                }

            }
        }

        private void RepositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show(RepositoryItemCheckEdit1.ValueChecked.ToString());
            if (MyFunction.RunSQL_String("select dachot from chotsolieu where id='" + ViewGiaybao.GetFocusedRowCellValue("ID").ToString() + "' and tinhtrang='True'") == "True")
            {
                MessageBox.Show("Số giấy báo: " + ViewGiaybao.GetFocusedRowCellValue("SOHD").ToString() + " đã chốt rùi!", "Thông báo", MessageBoxButtons.OK);
                RepositoryItemCheckEdit1.ValueUnchecked = true;
                return;
            }
            else if (MyFunction.RunSQL_String("select dachot from chotsolieu where id='" + ViewGiaybao.GetFocusedRowCellValue("ID").ToString() + "' and tinhtrang='True'") == "False")
            {
                if (MessageBox.Show("Bạn có chắc chắn chốt giấy báo: " + ViewGiaybao.GetFocusedRowCellValue("SOHD").ToString() + " ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _chotsl.UpdateByID(MyFunction._UserName.ToUpper(), int.Parse(ViewGiaybao.GetFocusedRowCellValue("ID").ToString()));
                    cmdFilterGB_Click(sender, e);
                }
                else
                RepositoryItemCheckEdit1.ValueUnchecked = true;
            }
            else
                RepositoryItemCheckEdit1.ValueUnchecked = true;



        }

        private void cmdFilterGB_Click(object sender, EventArgs e)
        {
            
            _chotsl = new ChotsolieuBussiness();
            string sql = "D" + txtTungay.Value.ToString("MM") + txtTungay.Value.ToString("yy");
            if (radioDain.Checked && loi == false)
            {
                groupGiaybao.Text = "Danh sách giấy báo đã in tháng: " + txtTungay.Value.ToString("MM") + "/" + txtTungay.Value.ToString("yyyy"); ;
                gridGiaybao.DataSource = _chotsl.GetbyNgay(txtTungay.Value, txtDenngay.Value);
                ViewGiaybao.Columns.Clear(); ViewGiaybao.OptionsView.ShowFooter = true;
                //Thiết lập cột
                DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "LGTHANG"; col.Caption = "Tháng"; ViewGiaybao.Columns.Add(col); ViewGiaybao.Columns[0].Visible = true;
                ViewGiaybao.Columns[0].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                ViewGiaybao.Columns[0].SummaryItem.DisplayFormat = "Tổng Cộng:{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                col1.FieldName = "DV"; col1.Caption = "Mã Đơn Vị"; ViewGiaybao.Columns.Add(col1); ViewGiaybao.Columns[1].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
                col2.FieldName = "SOHD"; col2.Caption = "Số Hóa Đơn"; ViewGiaybao.Columns.Add(col2); ViewGiaybao.Columns[2].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
                col3.FieldName = "TONGBH"; col3.Caption = "Tổng Bảo Hiểm"; ViewGiaybao.Columns.Add(col3); ViewGiaybao.Columns[3].Visible = true;
                ViewGiaybao.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[3].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[3].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
                col4.FieldName = "TONGTHUE"; col4.Caption = "Tổng Thuế"; ViewGiaybao.Columns.Add(col4); ViewGiaybao.Columns[4].Visible = true;
                ViewGiaybao.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[4].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[4].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[4].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
                col5.FieldName = "DNVHG"; col5.Caption = "Lương NV"; ViewGiaybao.Columns.Add(col5); ViewGiaybao.Columns[5].Visible = true;
                ViewGiaybao.Columns[5].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[5].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[5].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[5].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col6 = new DevExpress.XtraGrid.Columns.GridColumn();
                col6.FieldName = "DVP"; col6.Caption = "Dịch Vụ Phí"; ViewGiaybao.Columns.Add(col6); ViewGiaybao.Columns[6].Visible = true;
                ViewGiaybao.Columns[6].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[6].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[6].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[6].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col7 = new DevExpress.XtraGrid.Columns.GridColumn();
                col7.FieldName = "CDP"; col7.Caption = "Phí CĐ"; ViewGiaybao.Columns.Add(col7); ViewGiaybao.Columns[7].Visible = true;
                ViewGiaybao.Columns[7].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[7].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[7].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[7].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col8 = new DevExpress.XtraGrid.Columns.GridColumn();
                col8.FieldName = "CDP138"; col8.Caption = "Phí CĐ 138"; ViewGiaybao.Columns.Add(col8); ViewGiaybao.Columns[8].Visible = true;
                ViewGiaybao.Columns[8].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[8].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[8].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[8].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col9 = new DevExpress.XtraGrid.Columns.GridColumn();
                col9.FieldName = "TONGTIENDN"; col9.Caption = "Tổng Tiền FOSCO"; ViewGiaybao.Columns.Add(col9); ViewGiaybao.Columns[9].Visible = true;
                ViewGiaybao.Columns[9].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[9].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[9].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[9].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col10 = new DevExpress.XtraGrid.Columns.GridColumn();
                col10.FieldName = "Note"; col10.Caption = "Ghi Chú"; ViewGiaybao.Columns.Add(col10); ViewGiaybao.Columns[10].Visible = true;
                DevExpress.XtraGrid.Columns.GridColumn col11 = new DevExpress.XtraGrid.Columns.GridColumn();
                col11.FieldName = "NGAYIN"; col11.Caption = "Ngày in"; ViewGiaybao.Columns.Add(col11); ViewGiaybao.Columns[11].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col12 = new DevExpress.XtraGrid.Columns.GridColumn();
                col12.FieldName = "tinhtrang"; col12.Caption = "Tình Trạng"; ViewGiaybao.Columns.Add(col12); ViewGiaybao.Columns[12].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col13 = new DevExpress.XtraGrid.Columns.GridColumn();
                col13.FieldName = "DACHOT"; col13.Caption = "Đã Chốt"; ViewGiaybao.Columns.Add(col13);
                gridGiaybao.RepositoryItems.Add(RepositoryItemCheckEdit1);
                ViewGiaybao.Columns["DACHOT"].ColumnEdit = RepositoryItemCheckEdit1;
                ViewGiaybao.Columns[13].Visible = true;
                RepositoryItemCheckEdit1.CheckedChanged += RepositoryItemCheckEdit1_CheckedChanged;


                DevExpress.XtraGrid.Columns.GridColumn col14 = new DevExpress.XtraGrid.Columns.GridColumn();
                col14.FieldName = "NGAYGIOCHOT"; col14.Caption = "Ngày Chốt"; ViewGiaybao.Columns.Add(col14); ViewGiaybao.Columns[14].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col15 = new DevExpress.XtraGrid.Columns.GridColumn();
                col15.FieldName = "NGAYYC_HUY"; col15.Caption = "Ngày Hủy"; ViewGiaybao.Columns.Add(col15); ViewGiaybao.Columns[15].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col16 = new DevExpress.XtraGrid.Columns.GridColumn();
                col16.FieldName = "Ghichu"; col16.Caption = "Nội dung hủy"; ViewGiaybao.Columns.Add(col16); ViewGiaybao.Columns[16].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col17 = new DevExpress.XtraGrid.Columns.GridColumn();
                col17.FieldName = "ID"; col17.Caption = "ID"; ViewGiaybao.Columns.Add(col17); ViewGiaybao.Columns[17].Visible = false;

                DevExpress.XtraGrid.Columns.GridColumn col18 = new DevExpress.XtraGrid.Columns.GridColumn();
                col18.FieldName = "NGUOISD"; col18.Caption = "Người SD"; ViewGiaybao.Columns.Add(col18); ViewGiaybao.Columns[18].Visible = true;
                

            }
            else if (radioChuain.Checked && loi == false)
            {

                groupGiaybao.Text = "Danh sách giấy báo chưa in tháng: " + txtTungay.Value.ToString("MM") + "/" + txtTungay.Value.ToString("yyyy");
                gridGiaybao.DataSource = MyFunction.GetDataTable("select LG_THANG,DV,SOHD,SUM(TC_DN+TC_NV) AS TONGBH,SUM(TTN) AS TONGTHUE,SUM(TIEN_DVP) AS TONGDVP,SUM(PHI_CD) AS TONGCD,SUM(DNVHG) AS TONGDNVHG,SUM(TONGTIEN_DN) AS TONGTIEN,UPPER(DSNV.MANVQL) as NGUOISD from " + sql + ",DSNV,Q_DMDV where Q_DMDV.MADV=DV AND Q_DMDV.NHOMFC=DSNV.MANHOM AND SOHD not in(select SOHD from CHOTSOLIEU where tinhtrang='1') GROUP BY LG_THANG,DV,SOHD,DSNV.MANVQL order by DV");
                ViewGiaybao.Columns.Clear(); ViewGiaybao.OptionsView.ShowFooter = true;
                DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "LG_THANG"; col.Caption = "Tháng"; ViewGiaybao.Columns.Add(col); ViewGiaybao.Columns[0].Visible = true;
                ViewGiaybao.Columns[0].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                ViewGiaybao.Columns[0].SummaryItem.DisplayFormat = "Tổng Cộng:{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                col1.FieldName = "DV"; col1.Caption = "Mã Đơn Vị"; ViewGiaybao.Columns.Add(col1); ViewGiaybao.Columns[1].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
                col2.FieldName = "SOHD"; col2.Caption = "Số Hóa Đơn"; ViewGiaybao.Columns.Add(col2); ViewGiaybao.Columns[2].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
                col3.FieldName = "TONGBH"; col3.Caption = "Tổng Bảo Hiểm"; ViewGiaybao.Columns.Add(col3); ViewGiaybao.Columns[3].Visible = true;
                ViewGiaybao.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[3].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[3].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
                col4.FieldName = "TONGTHUE"; col4.Caption = "Tổng Thuế"; ViewGiaybao.Columns.Add(col4); ViewGiaybao.Columns[4].Visible = true;
                ViewGiaybao.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[4].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[4].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[4].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
                col5.FieldName = "TONGDVP"; col5.Caption = "Tổng DVP"; ViewGiaybao.Columns.Add(col5); ViewGiaybao.Columns[5].Visible = true;
                ViewGiaybao.Columns[5].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[5].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[5].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[5].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col6 = new DevExpress.XtraGrid.Columns.GridColumn();
                col6.FieldName = "TONGCD"; col6.Caption = "Tổng Phí CĐ"; ViewGiaybao.Columns.Add(col6); ViewGiaybao.Columns[6].Visible = true;
                ViewGiaybao.Columns[6].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[6].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[6].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[6].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col7 = new DevExpress.XtraGrid.Columns.GridColumn();
                col7.FieldName = "TONGDNVHG"; col7.Caption = "Tổng lƯƠNG NV"; ViewGiaybao.Columns.Add(col7); ViewGiaybao.Columns[7].Visible = true;
                ViewGiaybao.Columns[7].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[7].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[7].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[7].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col8 = new DevExpress.XtraGrid.Columns.GridColumn();
                col8.FieldName = "TONGTIEN"; col8.Caption = "Tổng TIỀN FOSCO"; ViewGiaybao.Columns.Add(col8); ViewGiaybao.Columns[8].Visible = true;
                ViewGiaybao.Columns[8].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                ViewGiaybao.Columns[8].DisplayFormat.FormatString = "N0";
                ViewGiaybao.Columns[8].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                ViewGiaybao.Columns[8].SummaryItem.DisplayFormat = "{0:n0}";

                DevExpress.XtraGrid.Columns.GridColumn col9 = new DevExpress.XtraGrid.Columns.GridColumn();
                col9.FieldName = "NGUOISD"; col9.Caption = "Người Phụ Trách"; ViewGiaybao.Columns.Add(col9); ViewGiaybao.Columns[9].Visible = true;
                ViewGiaybao.OptionsBehavior.ReadOnly = true;
            }

        }

        private void ViewGiaybao_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {



        }
        bool cal(Int32 _with, DevExpress.XtraGrid.Views.Grid.GridView _view)
        {
            _view.IndicatorWidth = _view.IndicatorWidth < _with ? _with : _view.IndicatorWidth;
            return true;
        }
        int kiemtra()
        {


            if (txtTungay.Value > txtDenngay.Value || txtDenngay.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Bạn chọn sai dữ liệu, vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
            else return 0;

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Thời gian, kiểm tra xem có giấy báo nào yêu câu hủy
            _chotsl = new ChotsolieuBussiness();
            GridPanel panel = superGridControl1.PrimaryGrid;
            panel.Rows.Clear();
            if (_chotsl.Yeucauhuy().Count > 0 && MyFunction._NhomSD == "PT")
            {
                foreach (var item in _chotsl.Yeucauhuy())
                {
                    GridRow row = new GridRow(item.LGTHANG,item.DV,item.SOHD,item.TONGBH,item.TONGTHUE,item.DNVHG,item.DVP,item.CDP,item.CDP138,item.TONGTIENDN,item.NGUOISD,item.NGAYIN,item.NGAYYC_HUY,item.Ghichu,item.Note,true,item.ID);//
                    panel.Rows.Add(row);

                }

                //groupGiaybao.AppearanceCaption.ForeColor = Color.Red;
                //groupGiaybao.Text = "Danh sách giấy báo yêu cầu hủy";
                //gridGiaybao.DataSource = _chotsl.Yeucauhuy();
                //ViewGiaybao.Columns.Clear(); ViewGiaybao.OptionsView.ShowFooter = true;
                //DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                //col.FieldName = "LGTHANG"; col.Caption = "Tháng"; ViewGiaybao.Columns.Add(col); ViewGiaybao.Columns[0].Visible = true;
                //ViewGiaybao.Columns[0].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                //ViewGiaybao.Columns[0].SummaryItem.DisplayFormat = "Tổng cộng:{0:n0}";

                //DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col1.FieldName = "DV"; col1.Caption = "Mã Đơn Vị"; ViewGiaybao.Columns.Add(col1); ViewGiaybao.Columns[1].Visible = true;

                //DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col2.FieldName = "SOHD"; col2.Caption = "Số Hóa Đơn"; ViewGiaybao.Columns.Add(col2); ViewGiaybao.Columns[2].Visible = true;

                //DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col3.FieldName = "TONGBH"; col3.Caption = "Tổng Bảo Hiểm"; ViewGiaybao.Columns.Add(col3); ViewGiaybao.Columns[3].Visible = true;
                //ViewGiaybao.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //ViewGiaybao.Columns[3].DisplayFormat.FormatString = "N0";
                //ViewGiaybao.Columns[3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //ViewGiaybao.Columns[3].SummaryItem.DisplayFormat = "{0:n0}";

                //DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col4.FieldName = "TONGTHUE"; col4.Caption = "Tổng Thuế"; ViewGiaybao.Columns.Add(col4); ViewGiaybao.Columns[4].Visible = true;
                //ViewGiaybao.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //ViewGiaybao.Columns[4].DisplayFormat.FormatString = "N0";
                //ViewGiaybao.Columns[4].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //ViewGiaybao.Columns[4].SummaryItem.DisplayFormat = "{0:n0}";

                //DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col5.FieldName = "DNVHG"; col5.Caption = "Lương NV"; ViewGiaybao.Columns.Add(col5); ViewGiaybao.Columns[5].Visible = true;
                //ViewGiaybao.Columns[5].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //ViewGiaybao.Columns[5].DisplayFormat.FormatString = "N0";
                //ViewGiaybao.Columns[5].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //ViewGiaybao.Columns[5].SummaryItem.DisplayFormat = "{0:n0}";

                //DevExpress.XtraGrid.Columns.GridColumn col6 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col6.FieldName = "DVP"; col6.Caption = "Dịch Vụ Phí"; ViewGiaybao.Columns.Add(col6); ViewGiaybao.Columns[6].Visible = true;
                //ViewGiaybao.Columns[6].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //ViewGiaybao.Columns[6].DisplayFormat.FormatString = "N0";
                //ViewGiaybao.Columns[6].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //ViewGiaybao.Columns[6].SummaryItem.DisplayFormat = "{0:n0}";

                //DevExpress.XtraGrid.Columns.GridColumn col7 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col7.FieldName = "CDP"; col7.Caption = "Phí CĐ"; ViewGiaybao.Columns.Add(col7); ViewGiaybao.Columns[7].Visible = true;
                //ViewGiaybao.Columns[7].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //ViewGiaybao.Columns[7].DisplayFormat.FormatString = "N0";
                //ViewGiaybao.Columns[7].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //ViewGiaybao.Columns[7].SummaryItem.DisplayFormat = "{0:n0}";

                //DevExpress.XtraGrid.Columns.GridColumn col8 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col8.FieldName = "CDP138"; col8.Caption = "Phí CĐ 138"; ViewGiaybao.Columns.Add(col8); ViewGiaybao.Columns[8].Visible = true;
                //ViewGiaybao.Columns[8].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //ViewGiaybao.Columns[8].DisplayFormat.FormatString = "N0";
                //ViewGiaybao.Columns[8].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //ViewGiaybao.Columns[8].SummaryItem.DisplayFormat = "{0:n0}";

                //DevExpress.XtraGrid.Columns.GridColumn col9 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col9.FieldName = "TONGTIENDN"; col9.Caption = "Tổng Tiền FOSCO"; ViewGiaybao.Columns.Add(col9); ViewGiaybao.Columns[9].Visible = true;
                //ViewGiaybao.Columns[9].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //ViewGiaybao.Columns[9].DisplayFormat.FormatString = "N0";
                //ViewGiaybao.Columns[9].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //ViewGiaybao.Columns[9].SummaryItem.DisplayFormat = "{0:n0}";

                //DevExpress.XtraGrid.Columns.GridColumn col10 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col10.FieldName = "Note"; col10.Caption = "Ghi Chú"; ViewGiaybao.Columns.Add(col10); ViewGiaybao.Columns[10].Visible = true;

                //DevExpress.XtraGrid.Columns.GridColumn col11 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col11.FieldName = "NGAYIN"; col11.Caption = "Ngày in"; ViewGiaybao.Columns.Add(col11); ViewGiaybao.Columns[11].Visible = true;

                //DevExpress.XtraGrid.Columns.GridColumn col12 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col12.FieldName = "tinhtrang"; col12.Caption = "Tình Trạng"; ViewGiaybao.Columns.Add(col12); ViewGiaybao.Columns[12].Visible = true;

                //DevExpress.XtraGrid.Columns.GridColumn col13 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col13.FieldName = "DACHOT"; col13.Caption = "Đã Chốt"; ViewGiaybao.Columns.Add(col13); ViewGiaybao.Columns[13].Visible = true;

                //DevExpress.XtraGrid.Columns.GridColumn col14 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col14.FieldName = "NGAYGIOCHOT"; col14.Caption = "Ngày Chốt"; ViewGiaybao.Columns.Add(col14); ViewGiaybao.Columns[14].Visible = true;

                //DevExpress.XtraGrid.Columns.GridColumn col15 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col15.FieldName = "NGAYYC_HUY"; col15.Caption = "Ngày Hủy"; ViewGiaybao.Columns.Add(col15); ViewGiaybao.Columns[15].Visible = true;

                //DevExpress.XtraGrid.Columns.GridColumn col16 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col16.FieldName = "Ghichu"; col16.Caption = "Nội dung hủy"; ViewGiaybao.Columns.Add(col16); ViewGiaybao.Columns[16].Visible = true;

                //DevExpress.XtraGrid.Columns.GridColumn col17 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col17.FieldName = "ID"; col17.Caption = "ID"; ViewGiaybao.Columns.Add(col17); ViewGiaybao.Columns[17].Visible = false;

                //DevExpress.XtraGrid.Columns.GridColumn col18 = new DevExpress.XtraGrid.Columns.GridColumn();
                //col18.FieldName = "NGUOISD"; col18.Caption = "Người SD"; ViewGiaybao.Columns.Add(col18); ViewGiaybao.Columns[18].Visible = true;
                //ViewGiaybao.OptionsBehavior.ReadOnly = true;
                huygb = true;
                // groupGiaybao.AppearanceCaption.ForeColor = Color.Black;
            }
            else
            {
                huygb = false;
                if (MyFunction.RunSQL_String("select toanquyen from phanquyen where id_chucnang=8 and iduser='" + MyFunction._UserName + "'") == "True")
                    ViewGiaybao.OptionsBehavior.ReadOnly = false;
            }
        }



        private void txtTungay_ValueChanged(object sender, EventArgs e)
        {
            if (kiemtra() == 0)
            {
                cmdFilterGB_Click(sender, e);
                loi = false;
            }
            else loi = true;
        }

        private void txtDenngay_ValueChanged(object sender, EventArgs e)
        {
            if (kiemtra() == 0)
            {
                cmdFilterGB_Click(sender, e);
                loi = false;
            }
            else loi = true;

        }

        private void radioDain_CheckedChanged(object sender, EventArgs e)
        {

            cmdFilterGB_Click(sender, e);
        }

        private void radioChuain_CheckedChanged(object sender, EventArgs e)
        {

            cmdFilterGB_Click(sender, e);
        }

        private void txtDenngay_Validating(object sender, CancelEventArgs e)
        {
            if (loi)
            {

                e.Cancel = true;
            }
            else
            {

                e.Cancel = false;
            }
        }

        private void cmdCloseGB_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exportTCKTTheongay_Click(object sender, EventArgs e)
        {

            gridGiaybao.DataSource = MyFunction.GetDataTable("SELECT '' AS STT, CONVERT(varchar, NGAYGIOCHOT, 103) AS NgayHachToan, CONVERT(varchar, NGAYIN, 103) AS NgayChungTu, DV AS MaKhachHang, 'GB' + SOHD AS SoChungTu,N'GIẤY BÁO THÁNG ' + LEFT(LGTHANG, 2) + N'/' + RIGHT(LGTHANG, 4) + (CASE WHEN LEN(NOTE) > 0 THEN ' - ' + NOTE ELSE '' END) AS DienGiaiChung, DVP, CDP, DNVHG AS LUONG, TONGBH AS BH,TONGTHUE AS THUE, TONGTIENDN AS TONG FROM CHOTSOLIEU where NGAYGIOCHOT between '" + txtTungay.Value.ToString("yyyy/MM/dd") + "' and '" + txtDenngay.Value.ToString("yyyy/MM/dd") + "' and tinhtrang=1 order by ngaygiochot desc");

            if (ViewGiaybao.RowCount > 0)
            {
                ViewGiaybao.Columns.Clear();
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                    col.FieldName = "STT"; col.Caption = "STT"; ViewGiaybao.Columns.Add(col); ViewGiaybao.Columns[0].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col1.FieldName = "NgayHachToan"; col1.Caption = "NgayHachToan"; ViewGiaybao.Columns.Add(col1); ViewGiaybao.Columns[1].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col2.FieldName = "NgayChungTu"; col2.Caption = "NgayChungTu"; ViewGiaybao.Columns.Add(col2); ViewGiaybao.Columns[2].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col3.FieldName = "MaKhachHang"; col3.Caption = "MaKhachHang"; ViewGiaybao.Columns.Add(col3); ViewGiaybao.Columns[3].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col4.FieldName = "SoChungTu"; col4.Caption = "SoChungTu"; ViewGiaybao.Columns.Add(col4); ViewGiaybao.Columns[4].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col5.FieldName = "DienGiaiChung"; col5.Caption = "DienGiaiChung"; ViewGiaybao.Columns.Add(col5); ViewGiaybao.Columns[5].Visible = true;

                    DevExpress.XtraGrid.Columns.GridColumn col6 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col6.FieldName = "DVP"; col6.Caption = "DVP"; ViewGiaybao.Columns.Add(col6); ViewGiaybao.Columns[6].Visible = true;
                    ViewGiaybao.Columns[6].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    ViewGiaybao.Columns[6].DisplayFormat.FormatString = "N0";

                    DevExpress.XtraGrid.Columns.GridColumn col7 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col7.FieldName = "CDP"; col7.Caption = "CDP"; ViewGiaybao.Columns.Add(col7); ViewGiaybao.Columns[7].Visible = true;
                    ViewGiaybao.Columns[7].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    ViewGiaybao.Columns[7].DisplayFormat.FormatString = "N0";

                    DevExpress.XtraGrid.Columns.GridColumn col8 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col8.FieldName = "LUONG"; col8.Caption = "LUONG"; ViewGiaybao.Columns.Add(col8); ViewGiaybao.Columns[8].Visible = true;
                    ViewGiaybao.Columns[8].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    ViewGiaybao.Columns[8].DisplayFormat.FormatString = "N0";

                    DevExpress.XtraGrid.Columns.GridColumn col9 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col9.FieldName = "BH"; col9.Caption = "BH"; ViewGiaybao.Columns.Add(col9); ViewGiaybao.Columns[9].Visible = true;
                    ViewGiaybao.Columns[9].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    ViewGiaybao.Columns[9].DisplayFormat.FormatString = "N0";

                    DevExpress.XtraGrid.Columns.GridColumn col10 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col10.FieldName = "THUE"; col10.Caption = "THUE"; ViewGiaybao.Columns.Add(col10); ViewGiaybao.Columns[10].Visible = true;
                    ViewGiaybao.Columns[10].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    ViewGiaybao.Columns[10].DisplayFormat.FormatString = "N0";

                    DevExpress.XtraGrid.Columns.GridColumn col11 = new DevExpress.XtraGrid.Columns.GridColumn();
                    col11.FieldName = "TONG"; col11.Caption = "TONG"; ViewGiaybao.Columns.Add(col11); ViewGiaybao.Columns[11].Visible = true;
                    ViewGiaybao.Columns[11].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    ViewGiaybao.Columns[11].DisplayFormat.FormatString = "N0";

                    gridGiaybao.ExportToXlsx(exportFilePath);

                    Process.Start(exportFilePath);
                }
            }
        }
        private void exportTCKTTheobang_Click(object sender, EventArgs e)
        {
            if (ViewGiaybao.RowCount > 0)
            {
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    ViewGiaybao.ExportToXlsx(exportFilePath);
                    Process.Start(exportFilePath);
                }
            }
            else
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        private void exportExcelMisaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Export Misa
            MyFunction.RunSQL("delete chotsolieu_excel");
            MyFunction.RunSQL("INSERT INTO CHOTSOLIEU_EXCEL(LGTHANG, SOHD,TONGTIENDN,DV, NGAYIN, NGAYGIOCHOT,MANHOMFC,GHICHU,NOTE) " +
                "SELECT LGTHANG, SOHD,(CASE WHEN RIGHT(DV,1)='@' AND TONGBH IS NOT NULL THEN ROUND(TONGBH*0.9375,0) ELSE (CASE WHEN TONGBH IS NOT NULL THEN TONGBH ELSE 0 END)END)," +
                "DV,NGAYIN, NGAYGIOCHOT,(CASE WHEN RIGHT(DV,1)='@' THEN '13885BHXH' ELSE '138BH' END),(CASE WHEN RIGHT(DV,1)='@' THEN '3383BHXH' ELSE '338BH' END),NOTE " +
                "FROM CHOTSOLIEU WHERE NGAYGIOCHOT between '" + txtTungay.Value.ToString("yyyy/MM/dd") + "' and '" + txtDenngay.Value.ToString("yyyy/MM/dd") + "' and tinhtrang=1 order by ngaygiochot desc");

            MyFunction.RunSQL("INSERT INTO CHOTSOLIEU_EXCEL(LGTHANG, SOHD,TONGTIENDN,DV, NGAYIN, NGAYGIOCHOT,MANHOMFC,GHICHU,NOTE)" +
                "SELECT LGTHANG, SOHD,(CASE WHEN RIGHT(DV,1)='@' AND TONGBH IS NOT NULL THEN ROUND(TONGBH*0.0625,0) ELSE TONGBH END),DV,NGAYIN, NGAYGIOCHOT,'13885BHTN','3386BHTN',NOTE FROM CHOTSOLIEU " +
                "WHERE NGAYGIOCHOT between '" + txtTungay.Value.ToString("yyyy/MM/dd") + "' and '" + txtDenngay.Value.ToString("yyyy/MM/dd") + "' and tinhtrang=1 and RIGHT(DV,1)='@' order by ngaygiochot desc");

            MyFunction.RunSQL("INSERT INTO CHOTSOLIEU_EXCEL(LGTHANG, SOHD,TONGTIENDN,DV, NGAYIN, NGAYGIOCHOT,MANHOMFC,GHICHU,NOTE) " +
                "SELECT LGTHANG, SOHD,(CASE WHEN TONGTHUE IS NOT NULL THEN TONGTHUE ELSE 0 END),DV,NGAYIN, NGAYGIOCHOT,(CASE WHEN RIGHT(DV,1)='@' THEN '13885T' ELSE '138T' END),(CASE WHEN RIGHT(DV,1)='@' THEN '3388T2' ELSE '338T' END),NOTE " +
                "FROM CHOTSOLIEU WHERE NGAYGIOCHOT between '" + txtTungay.Value.ToString("yyyy/MM/dd") + "' and '" + txtDenngay.Value.ToString("yyyy/MM/dd") + "' and tinhtrang=1 order by ngaygiochot desc");
            MyFunction.RunSQL("INSERT INTO CHOTSOLIEU_EXCEL(LGTHANG, SOHD,TONGTIENDN,DV, NGAYIN, NGAYGIOCHOT,MANHOMFC,GHICHU,NOTE) " +
                "SELECT LGTHANG, SOHD,(CASE WHEN DV='CHKDB' OR DV='CSG2L' Or DV='CSGAT' Or DV='CSGUM' Or DV='CNHYX' THEN CDP138 ELSE CDP END),DV,NGAYIN, NGAYGIOCHOT,(CASE WHEN DV='CHKDB' OR DV='CSG2L' Or DV='CSGAT' Or DV='CSGUM' Or DV='CNHYX' THEN '138CD' ELSE '13885CD' END),(CASE WHEN DV='CHKDB' OR DV='CSG2L' Or DV='CSGAT' Or DV='CSGUM' Or DV='CNHYX' THEN '338PCD' ELSE '3388PCD' END),NOTE " +
                "FROM CHOTSOLIEU WHERE NGAYGIOCHOT between '" + txtTungay.Value.ToString("yyyy/MM/dd") + "' and '" + txtDenngay.Value.ToString("yyyy/MM/dd") + "' and tinhtrang=1 order by ngaygiochot desc");

            MyFunction.RunSQL("INSERT INTO CHOTSOLIEU_EXCEL(LGTHANG, SOHD,TONGTIENDN,DV, NGAYIN, NGAYGIOCHOT,MANHOMFC,GHICHU,NOTE) " +
                "SELECT LGTHANG, SOHD,(CASE WHEN DNVHG IS NOT NULL THEN DNVHG ELSE 0 END),DV,NGAYIN, NGAYGIOCHOT,'13885L','3388L',NOTE " +
                "FROM CHOTSOLIEU WHERE NGAYGIOCHOT between '" + txtTungay.Value.ToString("yyyy/MM/dd") + "' and '" + txtDenngay.Value.ToString("yyyy/MM/dd") + "' and tinhtrang=1 order by ngaygiochot desc");

            MyFunction.RunSQL("INSERT INTO CHOTSOLIEU_EXCEL(LGTHANG, SOHD,TONGTIENDN,DV, NGAYIN, NGAYGIOCHOT,MANHOMFC,GHICHU,NOTE) " +
                "SELECT LGTHANG, SOHD,(CASE WHEN DVP IS NOT NULL THEN DVP ELSE 0 END),DV,NGAYIN, NGAYGIOCHOT,'13885P','1316',NOTE " +
                "FROM CHOTSOLIEU WHERE NGAYGIOCHOT between '" + txtTungay.Value.ToString("yyyy/MM/dd") + "' and '" + txtDenngay.Value.ToString("yyyy/MM/dd") + "' and tinhtrang=1 order by ngaygiochot desc");

            gridGiaybao.DataSource = MyFunction.GetDataTable("SELECT TOP 100 PERCENT '' AS HTTS, CONVERT(varchar, NGAYIN, 103) AS NGAYCT, CONVERT(varchar, NGAYGIOCHOT, 103) AS NGAYHT, 'GB' + SOHD AS SOCT," +
                "N'GIẤY BÁO THÁNG ' + LEFT(LGTHANG, 2) + N'/' + RIGHT(LGTHANG, 4) + (CASE WHEN LEN(NOTE) > 0 THEN ' - ' + NOTE ELSE '' END) AS DIENGIAI, (CASE WHEN DAY(NGAYGIOCHOT) < 25 THEN '25/' + LEFT(LGTHANG, 2) + '/' + RIGHT(LGTHANG, 4) ELSE CONVERT(VARCHAR, DATEADD(DAY, 5, NGAYGIOCHOT), 103) END) AS HANTT," +
                " '' AS LOAITIEN, '' AS TG,(CASE WHEN MANHOMFC = '13885P' THEN N'DỊCH VỤ PHÍ THÁNG ' ELSE (CASE WHEN MANHOMFC = '13885CD' OR MANHOMFC = '138CD' THEN N'CÔNG ĐOÀN PHÍ THÁNG ' ELSE (CASE WHEN MANHOMFC = '13885L' THEN N'LƯƠNG THÁNG ' ELSE (CASE WHEN MANHOMFC = '138BH' THEN N'BHXH THÁNG ' ELSE (CASE WHEN MANHOMFC = '13885BHXH' THEN N'BHXH+BHYT THÁNG ' ELSE (CASE WHEN MANHOMFC = '13885BHTN' THEN N'BHTN THÁNG ' ELSE N'THUẾ THÁNG ' END) END) END) END) END) END)+ LEFT(LGTHANG, 2) + '/' + RIGHT(LGTHANG, 4) AS DIENGIAI," +
                "MANHOMFC AS TKNO, Ghichu AS TKCO, TONGTIENDN AS SOTIEN, TONGTIENDN AS SOTIENQD, DV AS DTNO, DV AS DTCO, (CASE WHEN LEN(DV) = '7' THEN 'ZZ' + LEFT(DV, 5) ELSE DV END) AS DT " +
                "FROM CHOTSOLIEU_EXCEL");
            if (ViewGiaybao.RowCount > 0)
            {
                ViewGiaybao.Columns.Clear();
                DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "HTTS"; col.Caption = "Hiển thị trên sổ"; ViewGiaybao.Columns.Add(col); ViewGiaybao.Columns[0].Visible = true;
                //ViewGiaybao.Columns[11].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //ViewGiaybao.Columns[11].DisplayFormat.FormatString = "N0";
                DevExpress.XtraGrid.Columns.GridColumn col1 = new DevExpress.XtraGrid.Columns.GridColumn();
                col1.FieldName = "NGAYCT"; col1.Caption = "Ngày chứng từ (*)"; ViewGiaybao.Columns.Add(col1); ViewGiaybao.Columns[1].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col2 = new DevExpress.XtraGrid.Columns.GridColumn();
                col2.FieldName = "NGAYHT"; col2.Caption = "Ngày hạch toán (*)"; ViewGiaybao.Columns.Add(col2); ViewGiaybao.Columns[2].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col3 = new DevExpress.XtraGrid.Columns.GridColumn();
                col3.FieldName = "SOCT"; col3.Caption = "Số chứng từ (*)"; ViewGiaybao.Columns.Add(col3); ViewGiaybao.Columns[3].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col4 = new DevExpress.XtraGrid.Columns.GridColumn();
                col4.FieldName = "DIENGIAI"; col4.Caption = "Diễn giải"; ViewGiaybao.Columns.Add(col4); ViewGiaybao.Columns[4].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col5 = new DevExpress.XtraGrid.Columns.GridColumn();
                col5.FieldName = "HANTT"; col5.Caption = "Hạn thanh toán"; ViewGiaybao.Columns.Add(col5); ViewGiaybao.Columns[5].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col6 = new DevExpress.XtraGrid.Columns.GridColumn();
                col6.FieldName = "LOAITIEN"; col6.Caption = "Loại tiền"; ViewGiaybao.Columns.Add(col6); ViewGiaybao.Columns[6].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col7 = new DevExpress.XtraGrid.Columns.GridColumn();
                col7.FieldName = "TG"; col7.Caption = "Tỷ giá"; ViewGiaybao.Columns.Add(col7); ViewGiaybao.Columns[7].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col8 = new DevExpress.XtraGrid.Columns.GridColumn();
                col8.FieldName = "DIENGIAI"; col8.Caption = "Diễn giải (Hạch toán)"; ViewGiaybao.Columns.Add(col8); ViewGiaybao.Columns[8].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col9 = new DevExpress.XtraGrid.Columns.GridColumn();
                col9.FieldName = "TKNO"; col9.Caption = "TK Nợ (*)"; ViewGiaybao.Columns.Add(col9); ViewGiaybao.Columns[9].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col10 = new DevExpress.XtraGrid.Columns.GridColumn();
                col10.FieldName = "TKCO"; col10.Caption = "TK Có (*)"; ViewGiaybao.Columns.Add(col10); ViewGiaybao.Columns[10].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col11 = new DevExpress.XtraGrid.Columns.GridColumn();
                col11.FieldName = "SOTIEN"; col11.Caption = "Số tiền"; ViewGiaybao.Columns.Add(col11); ViewGiaybao.Columns[11].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col12 = new DevExpress.XtraGrid.Columns.GridColumn();
                col12.FieldName = "SOTIENQD"; col12.Caption = "Số tiền quy đổi"; ViewGiaybao.Columns.Add(col12); ViewGiaybao.Columns[12].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col13 = new DevExpress.XtraGrid.Columns.GridColumn();
                col13.FieldName = "DTNO"; col13.Caption = "Đối tượng Nợ"; ViewGiaybao.Columns.Add(col13); ViewGiaybao.Columns[13].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col14 = new DevExpress.XtraGrid.Columns.GridColumn();
                col14.FieldName = "DTCO"; col14.Caption = "Đối tượng Có"; ViewGiaybao.Columns.Add(col14); ViewGiaybao.Columns[14].Visible = true;

                DevExpress.XtraGrid.Columns.GridColumn col15 = new DevExpress.XtraGrid.Columns.GridColumn();
                col15.FieldName = "DT"; col15.Caption = "DT"; ViewGiaybao.Columns.Add(col15); ViewGiaybao.Columns[15].Visible = true;

                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    ViewGiaybao.ExportToXlsx(exportFilePath);
                    Process.Start(exportFilePath);
                }
            }
            else
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void cmdChuyendl_Click(object sender, EventArgs e)
        {
            FrmChuyenDulieu frm = new FrmChuyenDulieu();
            frm.Show();
        }

        private void ViewGiaybao_CustomDrawRowIndicator_1(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header)
            {
                e.Info.Appearance.BackColor = Color.White;
                e.Info.Appearance.ForeColor = Color.Black;
                e.Info.Appearance.DrawString(e.Cache, "STT", e.Bounds);
                e.Handled = true;
            }
            if (!ViewGiaybao.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
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
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, ViewGiaybao); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, ViewGiaybao); }));//Tăng kích thước nếu text vượt quá

            }

        }

        private void ViewGiaybao_DoubleClick(object sender, EventArgs e)
        {

            if (huygb)
            {
                if (MessageBox.Show("Bạn có chắc chắn hủy chốt giấy báo: " + ViewGiaybao.GetFocusedRowCellValue("SOHD").ToString() + " ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    MyFunction.RunSQL("update chotsolieu set tinhtrang=0,scan='" + MyFunction._UserName + "; " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where ID='" + ViewGiaybao.GetFocusedRowCellValue("ID").ToString() + "'");
                    MessageBox.Show("Đã yêu cầu hủy chốt giấy báo:" + ViewGiaybao.GetFocusedRowCellValue("SOHD").ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cmdFilterGB_Click(sender,e) ;

                }
            }
            else
            {
                MessageBox.Show(ViewGiaybao.GetFocusedRowCellValue("ID").ToString());
            }

        }

        private void txtTungay_Validating(object sender, CancelEventArgs e)
        {
            if (loi)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }

        }
        #region cbxRenderBack_CheckedChanged

        /// <summary>
        /// Handles change requests for rendering the row background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void cbxRenderBack_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (cbxRenderBack.Checked == true)
        //    {
        //        superGridControl1.PreRenderCell += superGridControl1_PreRenderCell;
        //        superGridControl1.PreRenderRow += superGridControl1_PreRenderRow;
        //    }
        //    else
        //    {
        //        superGridControl1.PreRenderCell -= superGridControl1_PreRenderCell;
        //        superGridControl1.PreRenderRow -= superGridControl1_PreRenderRow;
        //    }

        //    // Refresh the display

        //    superGridControl1.Invalidate();
        //}

        #region superGridControl1_PreRenderCell

        /// <summary>
        /// Pre-renders the given cell RenderPart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void superGridControl1_PreRenderCell(object sender, GridPreRenderCellEventArgs e)
        {
            
        }

        #endregion

        #region superGridControl1_PreRenderRow

        /// <summary>
        /// Renders the row whitespace if the row is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void superGridControl1_PreRenderRow(object sender, GridPreRenderRowEventArgs e)
        {
           
        }

        #endregion

        #region RenderRect

        /// <summary>
        /// Renders the background for the given rectangle
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="bounds">Bounding rectangle</param>
        private void RenderRect(Graphics g, System.Drawing.Rectangle bounds)
        {
            Color topStart = Color.FromArgb(0xff, 0xfb, 0xdb, 0xb5);
            Color topEnd = Color.FromArgb(0xff, 0xfe, 0xc7, 0x78);
            Color bottomStart = Color.FromArgb(0xff, 0xfe, 0xb4, 0x56);
            Color bottomEnd = Color.FromArgb(0xff, 0xfd, 0xeb, 0x9f);

            using (LinearGradientBrush lb = new LinearGradientBrush(bounds, topStart, bottomEnd, 90f))
            {
                ColorBlend cb = new ColorBlend(4);

                cb.Colors = new Color[] { topStart, topEnd, bottomStart, bottomEnd };
                cb.Positions = new float[] { 0, 0.35f, 0.35f, 1f };

                lb.InterpolationColors = cb;

                g.FillRectangle(lb, bounds);
            }

            // Add a little hilight to the bottom of the area

            using (GraphicsPath path = new GraphicsPath())
            {
                int splitHeight = (int)(bounds.Height * 0.35f);

                Rectangle r = new Rectangle(bounds.X, bounds.Bottom - splitHeight, bounds.Width, splitHeight);
                Rectangle ellipse = new Rectangle(r.X, r.Y - 2, r.Width, bounds.Height + 4);

                path.AddEllipse(ellipse);

                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(0xff, 0xfd, 0xeb, 0x9f);
                    brush.SurroundColors = new Color[] { Color.Transparent };
                    brush.CenterPoint = new PointF(ellipse.X + ellipse.Width / 2, bounds.Bottom);

                    Blend blend = new Blend();
                    blend.Factors = new float[] { 0f, .5f, .6f };
                    blend.Positions = new float[] { .0f, .4f, 1f };

                    brush.Blend = blend;

                    g.FillRectangle(brush, r);
                }
            }
        }

        #endregion

        #endregion

    }
}