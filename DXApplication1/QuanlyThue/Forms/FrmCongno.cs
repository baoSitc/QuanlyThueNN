using Bussiness;
using DataLayer;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using ExcelDataReader;
using Microsoft.Office.Interop.Word;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LicenseContext = System.ComponentModel.LicenseContext;

namespace QuanlyThue.Forms
{
    public partial class FrmCongno : DevExpress.XtraEditors.XtraForm
    {
        public FrmCongno()
        {
            InitializeComponent();
        }
        CongnoBussiness _congno = new CongnoBussiness();
        DonviBussiness _donvi = new DonviBussiness();

        private void cmdChontatca_Click(object sender, EventArgs e)
        {
            //

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //đường dẫn file word
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "word files (*.doc)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFileword.Text = openFileDialog.FileName;
            }
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            // Chọn folder chứa kết quả
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtKetqua.Text = folderBrowserDialog.SelectedPath.ToString();
        }
        private void FrmCongno_Load(object sender, EventArgs e)
        {
            MyFunction.RunSQL("update congno set trangthai=0 where nguoisd='"+ MyFunction._UserName+"'");
            List<CONGNO> _lstCongno = _congno.Getall(MyFunction._UserName);           
            gridCongno.DataSource = _lstCongno.OrderBy(x => x.MADV);
            ViewCongno.OptionsSelection.MultiSelect = true;

            //load máy in vào combobox
           // using System.Drawing.Printing;

            comboBoxPrinters.Items.Clear();

            // lấy máy in mặc định
            string defaultPrinter = new PrinterSettings().PrinterName;

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                comboBoxPrinters.Items.Add(printer);
            }

            // tự động chọn máy in mặc định
            if (comboBoxPrinters.Items.Contains(defaultPrinter))
            {
                comboBoxPrinters.SelectedItem = defaultPrinter;
            }



        }

        private void ViewCongno_Click(object sender, EventArgs e)
        {
            //if (ViewCongno.RowCount > 0)
            //    MyFunction.RunSQL("update congno set trangthai=~trangthai where id='"+ ViewCongno.GetFocusedRowCellValue("ID").ToString()+"'");


        }
        
        

        private void cmdLuuKetQua_Click(object sender, EventArgs e)
        {
            var application = new Microsoft.Office.Interop.Word.Application();
            object oMissing = System.Reflection.Missing.Value;
            string temp = "";
            errorProvider1.SetError(txtFileword, null);
            errorProvider1.SetError(txtKetqua, null);

            if (String.IsNullOrEmpty(txtFileword.Text))
            {
                errorProvider1.SetError(txtFileword, "Bạn chưa chọn file Word!"); return;
            }
            else if (String.IsNullOrEmpty(txtKetqua.Text))
            {
                errorProvider1.SetError(txtKetqua, "Bạn chưa chọn thư mục chứa kết quả!"); return;
            }
            if (ViewCongno.FocusedRowHandle < 0 || ViewCongno.SelectedRowsCount <= 0)
            {
                MessageBox.Show("Bạn chưa chọn đơn vị. Vui lòng chọn đơn vị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            Int32[] selectedRowHandles = ViewCongno.GetSelectedRows();
            for (int i = 0; i < selectedRowHandles.Length; i++)
            {
                int selectedRowHandle = selectedRowHandles[i];
                if (selectedRowHandle >= 0)
                    
                temp = temp + ViewCongno.GetRowCellDisplayText(selectedRowHandle, "ID") + ",";
            }


            if (temp != "")
                temp = temp.Substring(0, temp.Length - 1);

            MyFunction.RunSQL("update congno set trangthai=1 where id in(" + temp + ")");

            foreach (var item in _congno.GetallbyTrangthai(MyFunction._UserName))
            {
                var document = application.Documents.Add(Template: txtFileword.Text);
                // application.Visible = true;
                foreach (Microsoft.Office.Interop.Word.Field field in document.Fields)
                {
                    if (field.Code.Text.Contains("TENDV"))
                    {
                        field.Select();
                        application.Selection.TypeText(item.TENDV);
                    }
                    else if (field.Code.Text.Contains("DATRA"))
                    {
                        field.Select();
                        application.Selection.TypeText(
                        item.DATRA?.ToString("N0") ?? ""
                        );
                    }
                    else if (field.Code.Text.Contains("CONLAI"))
                    {
                        field.Select();
                        application.Selection.TypeText(
                        item.CONLAI?.ToString("N0") ?? ""
                            );
                    }
                    else if (field.Code.Text.Contains("DIACHI"))
                    {
                        field.Select();
                        application.Selection.TypeText(item.DIACHI);
                    }
                    else if (field.Code.Text.Contains("MADV"))
                    {
                        field.Select();
                        application.Selection.TypeText(item.MADV);
                    }
                }
                // document.PrintOut(oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, false, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

                document.SaveAs(FileName: @txtKetqua.Text+"\\" + item.MADV);
                document.Close();
            }
            MessageBox.Show("Đã thực hiện xong!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmdInKetQua_Click(object sender, EventArgs e)
        {
            var application = new Microsoft.Office.Interop.Word.Application();
            object oMissing = System.Reflection.Missing.Value;
            string temp = "";
            errorProvider1.SetError(txtFileword, null);
            errorProvider1.SetError(txtKetqua, null);

            if (String.IsNullOrEmpty(txtFileword.Text))
            {
                errorProvider1.SetError(txtFileword, "Bạn chưa chọn file Word!"); return;
            }
           
            if (ViewCongno.FocusedRowHandle < 0 || ViewCongno.SelectedRowsCount <= 0)
            {
                MessageBox.Show("Bạn chưa chọn đơn vị. Vui lòng chọn đơn vị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Int32[] selectedRowHandles = ViewCongno.GetSelectedRows();
            for (int i = 0; i < selectedRowHandles.Length; i++)
            {
                int selectedRowHandle = selectedRowHandles[i];
                if (selectedRowHandle >= 0)

                    temp = temp + ViewCongno.GetRowCellDisplayText(selectedRowHandle, "ID") + ",";
            }


            if (temp != "")
                temp = temp.Substring(0, temp.Length - 1);

            MyFunction.RunSQL("update congno set trangthai=1 where id in(" + temp + ")");
            MyFunction.RunSQL("update congno set trangthai=0 where nguoisd='"+ MyFunction._UserName +"' and  id not in(" + temp + ")");
     


            foreach (var item in _congno.GetallbyTrangthai(MyFunction._UserName))
            {
                var document = application.Documents.Add(Template: txtFileword.Text);
                // application.Visible = true;
                foreach (Microsoft.Office.Interop.Word.Field field in document.Fields)
                {
                    if (field.Code.Text.Contains("TENDV"))
                    {
                        field.Select();
                        application.Selection.TypeText(item.TENDV);
                    }
                    else if (field.Code.Text.Contains("DATRA"))
                    {
                        field.Select();
                        application.Selection.TypeText(
                         item.DATRA?.ToString("N0") ?? ""
                             );
                    }
                    else if (field.Code.Text.Contains("CONLAI"))
                    {
                        field.Select();
                        application.Selection.TypeText(
                         item.CONLAI?.ToString("N0") ?? ""
                             );
                    }
                    else if (field.Code.Text.Contains("DIACHI"))
                    {
                        field.Select();
                        application.Selection.TypeText(item.DIACHI);
                    }
                    else if (field.Code.Text.Contains("MADV"))
                    {
                        field.Select();
                        application.Selection.TypeText(item.MADV);
                    }
                }
                //document.PrintOut(Copies: 3, Background: false);
                application.ActivePrinter = comboBoxPrinters.SelectedItem.ToString();
                int soLanIn = Convert.ToInt32(spinSolanin.Value);

                document.PrintOut(
                    Copies: soLanIn,
                    Background: false
                );

                // document.PrintOut(oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, false, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

            }
            MessageBox.Show("Đã thực hiện xong!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        bool cal(Int32 _with, GridView _view)
        {
            _view.IndicatorWidth = _view.IndicatorWidth < _with ? _with : _view.IndicatorWidth;
            return true;
        }
        private void ViewCongno_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!ViewCongno.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
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
                    BeginInvoke(new MethodInvoker(delegate { cal(_with, ViewCongno); }));//Tăng kích thước nếu text vượt quá

                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));//Nhận -1 để đánh lại số thứ tự tăng dần
                SizeF _size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _with = Convert.ToInt32(_size.Width + 20);
                BeginInvoke(new MethodInvoker(delegate { cal(_with, ViewCongno); }));//Tăng kích thước nếu text vượt quá

            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
        ImportCongNoFromExcel();
        MessageBox.Show("Import công nợ thành công!");


        }
        public void ImportCongNoFromExcel()
        {
            DataSet ds = new DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {


                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel Workbook 97-2003|*.xls", ValidateNames = true })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                        {
                            IExcelDataReader reader;
                            if (ofd.FilterIndex == 2)
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(stream);
                            }
                            else
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            }

                            ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });

                            dt = ds.Tables[0];

                            //Kiểm tra xem phải định dạng excel của Tiền về
                            if (dt.Columns[0].ColumnName != "MADV" || dt.Columns[1].ColumnName != "TENDV" || dt.Columns[2].ColumnName != "DIACHIDV")

                            {
                                MessageBox.Show("Định dạng file excel không đúng, vui lòng kiểm tra lại tên cột Excel theo thứ tự sau:\n1)MAKH; 2)MANHA; 3)TUNGAY\n4)DENNGAY; 5)MASODH", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                reader.Close();
                                //xóa dữ liệu cũ
                                MyFunction.RunSQL("delete from CONGNO where NGUOISD='" + MyFunction._UserName + "'");
                                //Dùng dữ liệu từ DataSet để thêm mới công nợ
                                foreach (DataRow row in dt.Rows)
                                {
                                    // Thêm dữ liệu vào bảng CONGNO
                                    using (SqlConnection conn = new SqlConnection(MyFunction.str_con))
                                    {
                                        conn.Open();
                                        using (SqlCommand cmd = new SqlCommand(@"
                                        INSERT INTO CONGNO
                                        (MADV, TENDV, DIACHI, DATRA, CONLAI, GHICHU,NGUOISD)
                                        VALUES
                                        (@MADV, @TENDV, @DIACHIDV, @DATRA, @CONLAI, @GHICHU,@NGUOISD)
                                    ", conn))
                                        {
                                            cmd.Parameters.AddWithValue("@MADV", row["MADV"].ToString());
                                            cmd.Parameters.AddWithValue("@TENDV", row["TENDV"].ToString());
                                            cmd.Parameters.AddWithValue("@DIACHIDV", row["DIACHIDV"].ToString());

                                            // DATRA
                                            cmd.Parameters.AddWithValue("@DATRA",
                                                double.TryParse(row["DATRA"]?.ToString(), out double daTra)
                                                    ? daTra
                                                    : 0
                                                );
                                            // CONLAI
                                            cmd.Parameters.AddWithValue("@CONLAI",
                                                double.TryParse(row["CONLAI"].ToString(), out double conLai)
                                                ? conLai
                                                : 0
                                            );
                                            cmd.Parameters.AddWithValue("@GHICHU", row["GHICHU"].ToString());
                                            
                                            // Thêm NGUOISD
                                            cmd.Parameters.AddWithValue("@NGUOISD", MyFunction._UserName);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                //Reload lại dữ liệu
                                FrmCongno_Load(null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            
        }
    }
}