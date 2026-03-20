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
    public partial class frm_Login : DevExpress.XtraEditors.XtraForm
    {
        public frm_Login()
        {
            InitializeComponent();
        }

        private void cmd_Config_Click(object sender, EventArgs e)
        {
            groupConfig.Visible = !groupConfig.Visible;
            this.AutoSize = true;
            
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
        }
    }
}