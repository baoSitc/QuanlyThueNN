using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DataLayer;

namespace Bussiness
{
    
    public class ThietBiBussiness

    {
        Entities db;
        public ThietBiBussiness()
        {
            db = Entities.CreateEntities();

        }
        public List<ThietBi> Getall()
        {
            return db.ThietBis.Where(x=>x.Enable==true).ToList();
        }
        public int InsertThietBi(ThietBi thietBi) 
        {
            try
            {
                db.ThietBis.Add(thietBi);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);

            }
        }
        public int UpdateThietBi(ThietBi _thietbi)
        {
            ThietBi thietBi = db.ThietBis.FirstOrDefault(x => x.ID == _thietbi.ID);
            //thietBi.MaThietBi = _thietbi.MaThietBi;
            thietBi.LoaiThietBi=_thietbi.LoaiThietBi;
            thietBi.TenThietBi=_thietbi.TenThietBi;
            thietBi.NhomThietBi= _thietbi.NhomThietBi;
            thietBi.GhiChu= _thietbi.GhiChu;
            thietBi.NgayMua= _thietbi.NgayMua;
            thietBi.NguoiSuDung=_thietbi.NguoiSuDung;
            thietBi.NgaySuDung = _thietbi.NgaySuDung;
            thietBi.NgayThanhLy= _thietbi.NgayThanhLy;
            thietBi.Tinhtrang = _thietbi.Tinhtrang;
            thietBi.NgayKiemKe = _thietbi.NgayKiemKe;
            thietBi.DONGIA= _thietbi.DONGIA;

            try
            {
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);

            }

        }
        public int DeleteThietBi(ThietBi _thietbi)
        {
            ThietBi thietBi = db.ThietBis.FirstOrDefault(x => x.ID == _thietbi.ID);
            

            try
            {
                db.ThietBis.Remove(thietBi);
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
