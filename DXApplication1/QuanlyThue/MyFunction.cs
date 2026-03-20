using DataLayer;
using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanlyThue

{
    public class MyFunction
    {
        public static string _Nhomfc;
        public static string _NhomSD;
        public static string _UserName;
        public static string _Password;
        public static string _srv;
        public static string _user;
        public static string _pass;
        public static string _db;
        public static string _temp;
        public static string _frm;
      

        public static SqlConnection con = new SqlConnection();
        public static SqlCommand cmm = new SqlCommand();
        static SqlDataAdapter da = new SqlDataAdapter();

        // public static String str_con = "Data Source=115.79.61.102,1433\\sqlexpress;Initial Catalog=QLCF_DD;Persist Security Info=True;User ID=sa;Password=Mclcnnbc@123Encovy";
        public static String str_con;
        // = "Data Source=.\\sqlexpress;Initial Catalog=QLCF_DD;Persist Security Info=True;User ID=sa;Password=Mclcnnbc@123Encovy";

        public static int connect()
        {
            try

            {

                //DOc file Connect
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open("connectdb.dba", FileMode.Open, FileAccess.Read);
                connect cp = (connect)bf.Deserialize(fs);
                //Decrypt noidung
                _srv = Encryptor.Decrypt(cp.servername, "quocbao", true);
                _user = Encryptor.Decrypt(cp.username, "quocbao", true);
                _pass = Encryptor.Decrypt(cp.passwd, "quocbao", true);
                _db = Encryptor.Decrypt(cp.database, "quocbao", true);
                fs.Close();
                str_con = @"Data Source=" + _srv + ";Initial Catalog="
                    + _db + ";Persist Security Info=True;User ID=" + _user + ";Password=" + _pass;

                con = new SqlConnection(str_con);
                con.Open();
                cmm.Connection = con;
                return 1;

            }
            catch
            {
                return 0;
                //MessageBox.Show("Khong the ket noi duoc du lieu");

            }

        }
        public static void Disconnect()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
                con = null;
            }
        }

        public static DataTable GetDataTable(string str)
        {
            DataTable dt = new DataTable();
            cmm.CommandType = CommandType.Text;
            try
            {
                cmm.CommandText = str;
                cmm.Connection = con;
                da.SelectCommand = cmm;
                da.Fill(dt);
                // Disconnect();

            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
            return dt;
        }
        public static void RunSQL(string str)
        {

            cmm.CommandType = CommandType.Text;
            cmm.CommandText = str;
            try
            {


                cmm.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //cmm.Dispose();//Giải phóng bộ nhớ
            //cmm = null;

        }
        public static void RunSQL(string str, SqlParameter[] prms)
        {
            cmm.CommandType = CommandType.Text;
            cmm.CommandText = str;
            cmm.Parameters.Clear();

            if (prms != null)
                cmm.Parameters.AddRange(prms);

            try
            {
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static string RunSQL_String(string str)
        {
            string kq = "";

            cmm.CommandType = CommandType.Text;
            cmm.CommandText = str;
            try
            {

                var result = cmm.ExecuteScalar();

                kq = result == null ? "" : result.ToString();


                // cmm.Dispose();//Giải phóng bộ nhớ
                // cmm = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }

            return kq;

        }
        public static DateTime GetFistDayInmonth(int year, int month)
        {
            return new DateTime(year, month, 1);
        }
        public static String NumberToTextVN(decimal total)
        {
            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();

                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }

                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng";
                else
                    rs += "đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }

                rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười");
                return rs.Trim().Replace(",", "");

            }
            catch
            {
                return "";
            }

        }//ket thuc NumberToTextVN
        //Bắt đầu vùng thiết lập cho GridView
        public  static void SetCol(DevExpress.XtraGrid.Views.Grid.GridView view,
                           string field, string caption, int width,
                           HorzAlignment align = HorzAlignment.Near)
        {
            var col = view.Columns[field];
            if (col == null) return;

            col.Caption = caption;
            col.Width = width;
            col.AppearanceCell.TextOptions.HAlignment = align;
            col.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
        }
        public static void SetMoneyCol(DevExpress.XtraGrid.Views.Grid.GridView view,
                         string field, string caption, int width)
        {
            var col = view.Columns[field];
            if (col == null) return;

            col.Caption = caption;
            col.Width = width;
            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            col.DisplayFormat.FormatString = "n0";
            col.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
            col.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

            // Tổng ở footer
            col.Summary.Clear();
            col.Summary.Add(DevExpress.Data.SummaryItemType.Sum, field, "{0:n0}");
        }
        public static void SetDateCol(DevExpress.XtraGrid.Views.Grid.GridView view,
                         string field, string caption, int width)
        {
            var col = view.Columns[field];
            if (col == null) return;
            col.Caption = caption;
            col.Width = width;
            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            col.DisplayFormat.FormatString = "dd/MM/yyyy";
            col.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            col.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
        }
        public static void SetBoolCol(DevExpress.XtraGrid.Views.Grid.GridView view,
                         string field, string caption, int width)
        {
            var col = view.Columns[field];
            if (col == null) return;
            col.Caption = caption;
            col.Width = width;
            col.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            col.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
        }
        public static void HideCol(DevExpress.XtraGrid.Views.Grid.GridView view,
                         string field)
        {
            var col = view.Columns[field];
            if (col == null) return;
            col.Visible = false;

        }

        //Kết thúc vùng thiết lập cho GridView

    }
}

