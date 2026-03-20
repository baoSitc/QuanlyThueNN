using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public  class ChotsolieuBussiness
    {
        Entities db;
        public ChotsolieuBussiness()
        {
            db = Entities.CreateEntities();
        }
        public List<CHOTSOLIEU> GetAll()
        {
            return db.CHOTSOLIEUx.ToList();
        }
        public int GetBySohd(string sohd,string madv) 
        {
            CHOTSOLIEU _chotsl;
            _chotsl=db.CHOTSOLIEUx.FirstOrDefault(x=>x.SOHD==sohd && x.DV==madv && x.tinhtrang==true);
            if (_chotsl == null)
                return 0;
            else return 1;
        }
        public CHOTSOLIEU GetByMADV(string madv,string thangnam)
        {
            
            return db.CHOTSOLIEUx.FirstOrDefault(x=>x.DV == madv && x.LGTHANG == thangnam && x.tinhtrang == true);
            
        }

        public List<CHOTSOLIEU> GetByThangNam(string thang, string nam)
        {
            
            return db.CHOTSOLIEUx.Where(x=>x.LGTHANG==thang+nam).ToList();
            
        }
        public List<CHOTSOLIEU> GetbyNgay(DateTime tungay, DateTime denngay)
        {

            return db.CHOTSOLIEUx.Where(x => x.NGAYIN >= tungay && x.NGAYIN <= denngay).OrderByDescending(x => x.ID).ToList();

        }
        public List<CHOTSOLIEU> Yeucauhuy()
        {

            return db.CHOTSOLIEUx.Where(x => x.tinhtrang==true && x.NGAYYC_HUY != null).OrderByDescending(x => x.NGAYYC_HUY).ToList();

        }


        public int Insert(CHOTSOLIEU _chotsl)
        {

            try
            {
                db.CHOTSOLIEUx.Add(_chotsl);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);

            }

        }
        public void edit(string sohd, string madv)
        {
            CHOTSOLIEU _chotsl;
            _chotsl = db.CHOTSOLIEUx.FirstOrDefault(x => x.SOHD == sohd && x.DV == madv && x.tinhtrang == true);
            _chotsl.Ghichu=_chotsl.Ghichu+ ",In lại ngày "+DateTime.Now;
            db.SaveChanges();
        }
        public void UpdateByID(string nguoichot, int id)
        {
            CHOTSOLIEU _chotsl;
            _chotsl = db.CHOTSOLIEUx.FirstOrDefault(x => x.ID == id);
            _chotsl.NGUOICHOT = nguoichot;
            _chotsl.NGAYGIOCHOT=DateTime.Now;
            _chotsl.DACHOT = true;
            db.SaveChanges();
        }

    }
}
