using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace Bussiness
{
    
    public  class ThueNNBussiness
    {
        Entities db;
        public ThueNNBussiness() 
        {
            db = Entities.CreateEntities();
        }
        public List<T_THUENN> GetAll()
        {
            return db.T_THUENN.ToList();
        }
        public List<T_THUENN> Getbykey(string MADV,string MANV)
        {
            if (MANV ==null )
            {
                return db.T_THUENN.Where(x => x.MADV == MADV).OrderByDescending(x=>x.Id).ToList();
            }
            else 
                return db.T_THUENN.Where(x => x.MADV == MADV && x.MANV == MANV).OrderByDescending(x=>x.Id).ToList();
        }
        public int Update(T_THUENN THUENN)
        {
            T_THUENN _thuenn = db.T_THUENN.FirstOrDefault(x => x.Id == THUENN.Id);
            _thuenn.DNVHG= THUENN.DNVHG;
            _thuenn.TNCN = THUENN.TNCN;
            _thuenn.LGTNCN=THUENN.LGTNCN;
            _thuenn.TTN=THUENN.TTN;
            _thuenn.DIENGIAI=THUENN.DIENGIAI;
            _thuenn.DISCRIPTION=    THUENN.DISCRIPTION;
            _thuenn.DLCV= THUENN.DLCV;
            _thuenn.PDV= THUENN.PDV;
            _thuenn.GT_BANTHAN= THUENN.GT_BANTHAN;
            _thuenn.GT_PHUTHUOC = THUENN.GT_PHUTHUOC;
            _thuenn.LGTHANG= THUENN.LGTHANG;
            _thuenn.PHUCAP = THUENN.PHUCAP;
            _thuenn.THUONG= THUENN.THUONG;
            _thuenn.TIENNHA= THUENN.TIENNHA;
            _thuenn.KHAC= THUENN.KHAC;
            _thuenn.SOHD=THUENN.SOHD;
            _thuenn.TONGTIEN_DN = THUENN.TONGTIEN_DN;
            _thuenn.KYTHUE=THUENN.KYTHUE;
            _thuenn.LCV_USD = THUENN.LCV_USD;
            _thuenn.NVBHXH = THUENN.NVBHXH;

            try
            {
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            { throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);
                
            }

        }
        public int DeleteByID(T_THUENN THUENN)
        {
            T_THUENN _thuenn = db.T_THUENN.FirstOrDefault(x => x.Id == THUENN.Id);            

            try
            {
                db.T_THUENN.Remove(_thuenn);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);

            }

        }
        public int Insert(T_THUENN THUENN)
        {
            
            try
            {
                db.T_THUENN.Add(THUENN);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);

            }

        }

    }
}
