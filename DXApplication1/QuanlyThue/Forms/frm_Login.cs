using AutoUpdaterDotNET;
using Bussiness;
using DataLayer;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.XtraEditors.Mask.MaskSettings;

namespace QuanlyThue.Forms
{
    public partial class frm_Login : DevExpress.XtraEditors.XtraForm
    {
        Bussiness.DSNV _dsnv;
        public frm_Login()
        {
            InitializeComponent(); 
        }

        private void cmdLogin_Click(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtUser, "");
            errorProvider1.SetError(txtPassword, "");
            if (txtUser.Text == "")
            {
                errorProvider1.SetError(txtUser, "User Name không được để trống");
                return;
            }
            else if(txtPassword.Text=="")
            {
                errorProvider1.SetError(txtPassword, "Password không được để trống");
                return;
            }
            _dsnv = new Bussiness.DSNV();
            DataLayer.DSNV nv = new DataLayer.DSNV();
            nv = _dsnv.GetBYUserPW(txtUser.Text, txtPassword.Text);
            if (nv != null)
            {
                //thiết lập lại các thông số của hệ thống
                MyFunction._Nhomfc = MyFunction._NhomSD = MyFunction._Password = MyFunction._temp = MyFunction._UserName = null;

                //MessageBox.Show("Chúc bạn làm việc vui vẽ","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                MyFunction.connect();
               
                frm_Main frm = new frm_Main();
                
                frm.Show();
                this.Hide();
                MyFunction._UserName=txtUser.Text.Trim();
                MyFunction._Nhomfc = MyFunction.RunSQL_String("select manhom from dsnv where manvql='"+txtUser.Text+"'");
                MyFunction._Password=txtPassword.Text.Trim();
                MyFunction._NhomSD = MyFunction.RunSQL_String("select nhomsd from dsnv where manvql='" + txtUser.Text + "'");
            }
            else
            {
                MessageBox.Show("Bạn đăng nhập sai hoặc tài khoản đã ngừng hoạt động, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
                
        }

        private void frm_Login_Load(object sender, EventArgs e)
        {
            //AutoUpdate
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            string version = fvi.FileVersion;
            //label1.Text = "Phiên bản: " + version;
            AutoUpdater.DownloadPath = "update";
            AutoUpdater.Start(@"T:\UPDATE\THUENN\thuenn.xml");
            //AutoUpdater.Start("http://ttldfosco.com.vn/TTCULD/update.xml");
            this.Text = this.Text+"- Version: " + Application.ProductVersion;
          // txtUser.Text = "hoanvy";txtPassword.Text = "123456";
            this.Size = new System.Drawing.Size(495, 195);
            //DOc file Connect
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open("connectdb.dba", FileMode.Open, FileAccess.Read);
            connect cp = (connect)bf.Deserialize(fs);
            fs.Close();
            //Decrypt noidung
            if (cp != null)
            {

                txtServer.Text = Encryptor.Decrypt(cp.servername, "quocbao", true);
                txtUserSa.Text = Encryptor.Decrypt(cp.username, "quocbao", true);
                txtPasswordSa.Text = Encryptor.Decrypt(cp.passwd, "quocbao", true);
                txtDatabase.Text = Encryptor.Decrypt(cp.database, "quocbao", true);
            }

        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtServer, "");
            errorProvider1.SetError(txtUserSa, "");
            errorProvider1.SetError(txtPasswordSa, "");
            errorProvider1.SetError(txtDatabase, "");
            if (txtServer.Text == "")
            {
                errorProvider1.SetError(txtServer, "Server không được để trống");
                return;
            }
            else if (txtUserSa.Text == "")
            {
                errorProvider1.SetError(txtUserSa, "User sa không được để trống");
                return;
            }
            else if (txtPasswordSa.Text == "")
            {
                errorProvider1.SetError(txtPasswordSa, "Password sa không được để trống");
                return;
            }
            else if (txtDatabase.Text == "")
            {
                errorProvider1.SetError(txtDatabase, "Database không được để trống");
                return;
            }
            if (txtServer.Text != "" && txtUserSa.Text != "" && txtPasswordSa.Text != "" && txtDatabase.Text != "")
            {
                string enCryptServ = Encryptor.Encrypt(txtServer.Text, "quocbao", true);
                string enCryptPass = Encryptor.Encrypt(txtPasswordSa.Text, "quocbao", true);
                string enCryptData = Encryptor.Encrypt(txtDatabase.Text, "quocbao", true);
                string enCryptUser = Encryptor.Encrypt(txtUserSa.Text, "quocbao", true);
                connect cn = new connect(enCryptServ, enCryptUser, enCryptPass, enCryptData);
                cn.SaveFile();
                MessageBox.Show("Luu file thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                groupConfig.Visible = false;
                this.Size = new System.Drawing.Size(495, 183);
            }
            else
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private void cmd_Config_Click(object sender, EventArgs e)
        {
            if (groupConfig.Visible)
            {
                groupConfig.Visible = false;
                this.Size = new System.Drawing.Size(495, 200);
            }
            else
            {
                groupConfig.Visible = true;
                this.Size = new System.Drawing.Size(495, 489);
            }
        }

        private void frm_Login_SizeChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Chiều rộng:"+this.Size.Width.ToString()+", chiều cao:"+ this.Size.Width.ToString());
        }

        private void frm_Login_ResizeEnd(object sender, EventArgs e)
        {
            //MessageBox.Show("Chiều rộng:" + this.Size.Width.ToString() + ", chiều cao:" + this.Size.Height.ToString());
        }

        private void cmdQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.IsUpdateAvailable)
            {
                DialogResult dialogResult;
                dialogResult =
                        MessageBox.Show(
                            $@"Bạn ơi, phần mềm của bạn có phiên bản mới {args.CurrentVersion}. Phiên bản bạn đang sử dụng hiện tại  {args.InstalledVersion}. Bạn có muốn cập nhật phần mềm không?", @"Cập nhật phần mềm",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                if (dialogResult.Equals(DialogResult.Yes) || dialogResult.Equals(DialogResult.OK))
                {
                    try
                    {
                        if (AutoUpdater.DownloadUpdate(args))
                        {
                            Application.Exit();
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
               // MessageBox.Show(@"Phiên bản bạn đang sử dụng đã được cập nhật mới nhất.", @"Cập nhật phần mềm",
                 //   MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
           //cmdLogin_Click(sender, e);
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
                cmdLogin_Click(sender, e);
        }
    }
}