using DevExpress.XtraBars;
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
    public partial class F_Doipass : DevExpress.XtraEditors.XtraForm
    {
        public F_Doipass()
        {
            InitializeComponent();
        }
        Bussiness.DSNV _ns;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtmk1, "");
            errorProvider1.SetError(txtmkMoi1, "");
            errorProvider1.SetError(txtmkMoi2, "");

            if (txtmk1.Text == "" || txtmk1.Text!=MyFunction._Password)
            {
                errorProvider1.SetError(txtmk1, "Mật khẩu củ không đúng");
                return;
            }
            else if (txtmkMoi1.Text == "" || txtmkMoi1.Text!=txtmkMoi2.Text)
            {
                errorProvider1.SetError(txtmkMoi1, "Mật khẩu mới không trùng khớp");
                return;
            }
            else
            {
                if (_ns.ChangePW(MyFunction._UserName, txtmkMoi1.Text) == 1)
                    MessageBox.Show("bạn đã thây đổi mật khẩu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Đã có lỗi khi thay đổi mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private void F_Doipass_Load(object sender, EventArgs e)
        {
            _ns = new Bussiness.DSNV();
        }
    }
}