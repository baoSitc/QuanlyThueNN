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
    public partial class TreeList : DevExpress.XtraEditors.XtraForm
    {
        public TreeList()
        {
            InitializeComponent();
        }
        public class loaiBC
        {
            public string ID_PARENT { set; get; }
            public string ID { set; get; }
            public string NAME { set; get; }
        }

        private void TreeList_Load(object sender, EventArgs e)
        {
            var brands = new List<loaiBC>() {
            new loaiBC{ID_PARENT = "-1", ID = "0",NAME="Danh sách loại đơn vị"},
            new loaiBC{ID_PARENT = "0", ID = "1",NAME="NGOẠI GIAO"},
             new loaiBC{ID_PARENT = "1", ID = "2",NAME="Ngoại Giao"},
              new loaiBC{ID_PARENT = "1", ID = "3",NAME="Phi Chính Phủ"},
               new loaiBC{ID_PARENT = "1", ID = "4",NAME="Tổ Chức Quốc Tế"},
                new loaiBC{ID_PARENT = "1", ID = "5",NAME="Vă  Phòng Đại Diện"},
                 new loaiBC{ID_PARENT = "1", ID = "6",NAME="Công Ty"},
                  new loaiBC{ID_PARENT = "1", ID = "7",NAME="Hiệp Hội"},

             new loaiBC{ID_PARENT = "0", ID = "8",NAME="KINH TẾ"},
             new loaiBC{ID_PARENT = "8", ID = "9",NAME="Kinh Tế"},
              new loaiBC{ID_PARENT = "8", ID = "10",NAME="NGO"},
               new loaiBC{ID_PARENT = "8", ID = "11",NAME="QT"},
                new loaiBC{ID_PARENT = "8", ID = "12",NAME="VPDD"},
                 new loaiBC{ID_PARENT = "8", ID = "13",NAME="CTY"},
                  new loaiBC{ID_PARENT = "8", ID = "14",NAME="HH"},

            };
            treeList1.ParentFieldName = "ID_PARENT";
            treeList1.KeyFieldName = "ID";
            treeList1.Appearance.FocusedCell.BackColor = System.Drawing.SystemColors.Highlight;
            treeList1.Appearance.FocusedCell.ForeColor=System.Drawing.SystemColors.HighlightText;
            treeList1.OptionsBehavior.Editable = false;
            treeList1.ViewStyle = DevExpress.XtraTreeList.TreeListViewStyle.TreeView;

            treeList1.DataSource = brands;

        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            var name = treeList1.FocusedNode.GetValue("NAME");
            var id = treeList1.FocusedNode.GetValue("ID");
            var id_parent = treeList1.FocusedNode.GetValue("ID_PARENT");
            MessageBox.Show($"id:{id} - id_parent:{id_parent} - Name:{name}");
        }
    }
}