using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Bussiness
{
    public  class DonviBussiness
    {
        Entities db;
        public DonviBussiness() 
        {
            db = Entities.CreateEntities();
            
        }
        public List<DonVi> GetAll()
        {
            return db.DonVis.ToList();
        }
       
        public DataLayer.DonVi GetbyMadv(string madv)
        {
            return db.DonVis.FirstOrDefault(x => x.MaDonVi == madv);
        }
        public List<DataLayer.DonVi> ListGetbyMadv(string madv)
        {
            return db.DonVis.Where(x => x.MaDonVi == madv).ToList();
        }
        public List<DonVi> ListGetByNhom(string nhom)
        {
            return db.DonVis.Where(x => x.TenNhomFosco == nhom && x.TrangThai == "Đang làm việc").ToList();
        }

        public List<Q_DMDV> ListQ_DMDV(string loaidv)
        {
            List<Q_DMDV> _lstQ_DMDV = new List<Q_DMDV>();            
            List<DonVi> _dv = new List<DonVi>();
            if (loaidv == "NG")
            {
                _dv = db.DonVis.Where(x => x.TrangThai == "Đang làm việc" && x.LoaiDonVi.Substring(0, 2) == "NG" && x.MaDonVi.Length == 5).OrderBy(x=>x.MaDonVi).ToList();
            }            
            else if (loaidv == "NGNG")
            {
                _dv = db.DonVis.Where(x => x.TrangThai == "Đang làm việc" && x.LoaiDonVi == "NG-NG" && x.MaDonVi.Length==5).OrderBy(x => x.MaDonVi).ToList();
            }
            
            else if (loaidv == "NGPCP")
            {
                _dv = db.DonVis.Where(x => x.TrangThai == "Đang làm việc" && x.LoaiDonVi == "NG-NGO" && x.MaDonVi.Length == 5).OrderBy(x => x.MaDonVi).ToList();
            }
            else if (loaidv == "NGQT")
            {
                _dv = db.DonVis.Where(x => x.TrangThai == "Đang làm việc" && x.LoaiDonVi == "NG-QT" && x.MaDonVi.Length == 5).OrderBy(x => x.MaDonVi).ToList();
            }
            else if (loaidv == "KT")
            {
                _dv = db.DonVis.Where(x => x.TrangThai == "Đang làm việc" && x.LoaiDonVi.Substring(0, 2) == "KT" && x.MaDonVi.Length == 5).OrderBy(x => x.MaDonVi).ToList();
            }
            else if (loaidv == "KTKT")
            {
                _dv = db.DonVis.Where(x => x.TrangThai == "Đang làm việc" && x.LoaiDonVi == "KT-KT" && x.MaDonVi.Length == 5).OrderBy(x => x.MaDonVi).ToList();
            }
            else if (loaidv == "KTVP")
            {
                _dv = db.DonVis.Where(x => x.TrangThai == "Đang làm việc" && x.LoaiDonVi == "KT-VPDD" && x.MaDonVi.Length == 5).OrderBy(x => x.MaDonVi).ToList();
            }
            else if (loaidv == "KTHH")
            {
                _dv = db.DonVis.Where(x => x.TrangThai == "Đang làm việc" && x.LoaiDonVi == "KT-HH" && x.MaDonVi.Length == 5).OrderBy(x => x.MaDonVi).ToList();
            }
            else if (loaidv == "KTCT")
            {
                _dv = db.DonVis.Where(x => x.TrangThai == "Đang làm việc" && x.LoaiDonVi == "KT-CT" && x.MaDonVi.Length == 5).OrderBy(x => x.MaDonVi).ToList();
            }
            else if (loaidv == "NVTNN")
            {
                _dv = db.DonVis.Where(x => x.TrangThai == "Đang làm việc" && x.MaDonVi.Length==7).OrderBy(x => x.MaDonVi).ToList();
            }



            foreach (var item in _dv)
            {
                Q_DMDV _q_dmdv = new Q_DMDV();
                _q_dmdv.MADV = item.MaDonVi;
                _q_dmdv.NHOMFC = item.TenNhomFosco;
                _q_dmdv.TENDV = item.TenDonVi;
                _q_dmdv.TrangThai = item.TrangThai;
                _q_dmdv.TSNV = 0;
                _lstQ_DMDV.Add(_q_dmdv);
            }
            return _lstQ_DMDV;
        }
    }
}
