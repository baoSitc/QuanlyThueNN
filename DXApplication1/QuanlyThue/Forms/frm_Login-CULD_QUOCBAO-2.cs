using Bussiness;
using DataLayer;
using DevExpress.XtraEditors;
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
    public partial class frm_Login : Form
    {
        Bussiness.DSNV _dsnv;
        public frm_Login()
        {
            InitializeComponent();
        }

        private void cmdLogin_Click(object sender, EventArgs e)
        {
            _dsnv = new Bussiness.DSNV();
            DataLayer.DSNV nv = new DataLayer.DSNV();
            nv = _dsnv.GetBYUserPW(txtUser.Text, txtPassword.Text);
            if (nv != null)
            {
                MessageBox.Show("Chúc bạn làm việc vui vẽ");
               
                frm_Main frm = new frm_Main();
                frm.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Bạn đăng nhập sai, vui lòng kiểm tra lại!");
            }
                
        }

        private void frm_Login_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(495, 183);

        }

        private void cmdSave_Click(object sender, EventArgs e)
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

        private void cmd_Config_Click(object sender, EventArgs e)
        {
            groupConfig.Visible =true ;            
                
                this.Size = new System.Drawing.Size(495, 486);
            
            
            
           
        }

        private void frm_Login_SizeChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Chiều rộng:"+this.Size.Width.ToString()+", chiều cao:"+ this.Size.Width.ToString());
        }

        private void frm_Login_ResizeEnd(object sender, EventArgs e)
        {
            MessageBox.Show("Chiều rộng:" + this.Size.Width.ToString() + ", chiều cao:" + this.Size.Height.ToString());
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmdQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}