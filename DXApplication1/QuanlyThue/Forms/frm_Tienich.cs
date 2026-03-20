using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;

namespace QuanlyThue.Forms
{
    public partial class frm_Tienich : DevExpress.XtraEditors.XtraForm
    {
        public frm_Tienich()
        {
            InitializeComponent();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void RenameFilesToUpperCase(string folderPath,String _chuoi,int _opt)
        {
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Thư mục không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] files = Directory.GetFiles(folderPath);

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                string extension = Path.GetExtension(filePath);
                string nameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                // Xử lý: thay "-" bằng "_", bỏ khoảng trắng, viết hoa
                string newName = nameWithoutExtension;
                if (_opt == 2)
                {
                    newName += _chuoi;
                }
                else if (_opt == 3)
                {
                    newName = newName.Substring(0, 5) + _chuoi;
                }
                newName = Regex.Replace(newName, @"\s+", "_");

                newName = newName.Replace("-", "_")
                                    .ToUpper();
                newName = Regex.Replace(newName, @"_+", "_");

               


                string newFileName = newName + extension.ToUpper(); // nếu muốn giữ đuôi thường thì bỏ ToUpper()
                string newFilePath = Path.Combine(folderPath, newFileName);

                // Nếu tên mới khác tên cũ thì mới rename
                if (!filePath.Equals(newFilePath))
                {
                    try
                    {
                        File.Move(filePath, newFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi đổi tên file {fileName}: {ex.Message}");
                    }
                }
            }

            MessageBox.Show("Đổi tên file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmdRenameFile_Click(object sender, EventArgs e)
        {

        }

        private void radioOp1_CheckedChanged(object sender, EventArgs e)
        {
            txtChuoi.Enabled = !radioOp1.Checked;
        }
        void DoiTenFileHoaDon()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {


                MessageBox.Show("Bạn chọn file Excel chứa tên tập tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            if (dt.Columns[0].ColumnName != "STT" || dt.Columns[1].ColumnName != "TENFILE")

                            {
                                MessageBox.Show("Định dạng file excel không đúng, vui lòng kiểm tra lại tên cột Excel theo thứ tự sau:\n1)STT; 2)TENFILE","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Bạn chọn Folder chứa các File cần đổi tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //bắt đầu vào chương trình
                                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                                {
                                    if (fbd.ShowDialog() == DialogResult.OK)
                                    {

                                        if (MessageBox.Show("Bạn có chắc chắn thực hiện đổi tên file?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        {
                                            for(int i = 0; i < dt.Rows.Count; i++)
                                            {
                                               int vitri= dt.Rows[i][1].ToString().IndexOf("_");
                                                DoiFileHoaDon(fbd.SelectedPath, dt.Rows[i][1].ToString().Substring(0, vitri), dt.Rows[i][1].ToString());
                                            }

                                            MessageBox.Show("Đã đổi xong " + Directory.GetFiles(fbd.SelectedPath).Count() + " File", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        }
                                    }
                                }



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

        private void cmdThuchien_Click(object sender, EventArgs e)
        {
            if (radioOp1.Checked)
            {

                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {

                        if (MessageBox.Show("Bạn có chắc chắn thực hiện đổi tên file?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            RenameFilesToUpperCase(fbd.SelectedPath, null, 1);
                        }
                    }
                }
            }
            else if (radioOp2.Checked)
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {

                        if (MessageBox.Show("Bạn có chắc chắn thực hiện đổi tên file?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            RenameFilesToUpperCase(fbd.SelectedPath, txtChuoi.Text, 2);
                        }
                    }
                }
            }
            else if (radioOp3.Checked)
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {

                        if (MessageBox.Show("Bạn có chắc chắn thực hiện đổi tên file?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            RenameFilesToUpperCase(fbd.SelectedPath, txtChuoi.Text, 3);
                        }
                    }
                }
            }
            else if (radioOp4.Checked) 
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {

                        if (MessageBox.Show("Bạn có chắc chắn thực hiện đổi tên file?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataTable dt= new DataTable();
                            dt = MyFunction.GetDataTable("select * from Appointments");
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr[15].ToString() == "N")
                                {
                                    timfile(fbd.SelectedPath, dr[17].ToString(), dr[19].ToString(), dr[15].ToString(), dr[16].ToString(), dr[0].ToString());
                                }
                                else {
                                    timfile(fbd.SelectedPath, dr[17].ToString(), dr[19].ToString(), "", dr[16].ToString(), dr[0].ToString());
                                }
                                
                            }
                            MessageBox.Show("Da hoan thanh");
                            
                        }
                    }
                }
            }
            //Đổi tên file hóa đơn hàng loạt
            else if (radioDoiTenHoaDon.Checked)
            {
                //Doi tên hàng loạt file Hóa Đơn
               DoiTenFileHoaDon();

                
            }
        }
        void timfile(string _folder, string _keyword1, string _keyword2, string _keyword3,string _tenfile,string _id)
        {
            //string folderPath = @"C:\YourFolderPath"; // Thay đổi đường dẫn theo ý bạn
            //string keyword1 = "ABC"; // Ký tự đầu cần tìm
            //string keyword2 = "ABC"; // Ký tự đầu cần tìm
            if (Directory.Exists(_folder))
            {
                var matchingFiles = Directory.GetFiles(_folder)
                .Where(file =>
                {
                    string fileName = Path.GetFileName(file).ToLower();
                    return fileName.Contains(_keyword1.ToLower()) && fileName.Contains(_keyword2.ToLower()) && fileName.Contains(_keyword3.ToLower());
                })
                    .ToList();

                foreach (var file in matchingFiles)
                {
                    // Nếu tên mới khác tên cũ thì mới rename
                    string _newfile = "";


                        try
                        {
                        if (_keyword3 == "N")
                        {

                             _newfile = "E:\\HopDong\\" + _keyword2 + "NN_" + _tenfile + ".PDF";
                        }
                        else
                        {
                             _newfile = "E:\\HopDong\\" + _keyword2 + "_" + _tenfile + ".PDF";
                        }
                            File.Copy(file , _newfile);
                        MyFunction.RunSQL("update Appointments set type='1' where UniqueID='" + _id + "'");

                        }
                        catch (Exception ex)
                        {
                        MyFunction.RunSQL("update Appointments set type='0' where UniqueID='" + _id + "'");
                        // MessageBox.Show($"Lỗi đổi tên file {file}: {ex.Message}");
                    }
                    

                   // MessageBox.Show("Ten file:" + file);
                }

                if (!matchingFiles.Any())
                {
                    Console.WriteLine("Không tìm thấy file nào bắt đầu bằng '" + _keyword1 + "'");
                }
            }
            else
            {
                Console.WriteLine("Thư mục không tồn tại.");
            }
        }
        void DoiFileHoaDon(string _folder, string _keyword1, string _tenfile)
        {
            //string folderPath = @"C:\YourFolderPath"; // Thay đổi đường dẫn theo ý bạn
            //string keyword1 = "ABC"; // Ký tự đầu cần tìm
            //string keyword2 = "ABC"; // Ký tự đầu cần tìm
            if (Directory.Exists(_folder))
            {
                var matchingFiles = Directory.GetFiles(_folder)
                .Where(file =>
                {
                    string fileName = Path.GetFileName(file).ToLower();
                    return fileName.Contains(_keyword1.ToLower());
                }).FirstOrDefault();

                if (matchingFiles != null) {
                    try
                    {
                        string _newfile = _folder + "\\" + _tenfile + ".pdf";

                        File.Move(matchingFiles, _newfile);
                        //MyFunction.RunSQL("update Appointments set type='1' where UniqueID='" + _id + "'");

                    }
                    catch (Exception ex)
                    {
                        
                    }
                }

                else {
                    MessageBox.Show("Không tìm thấy file nào bắt đầu bằng '" + _tenfile + "'");
                }
                 
                }

                
        }
    }
}