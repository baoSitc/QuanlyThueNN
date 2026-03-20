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
using DataLayer;
using Bussiness;

namespace QuanlyThue.Forms
{
    public partial class frm_Nguoidung : Form
    {
        Bussiness.DSNV _dsnv;
        public frm_Nguoidung()
        {
            InitializeComponent();
        }

        private void frm_Nguoidung_Load(object sender, EventArgs e)
        {
            _dsnv = new Bussiness.DSNV();
            gridControl1.DataSource = _dsnv.Getall();
        }
    }
}