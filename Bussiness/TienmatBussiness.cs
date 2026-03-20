using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace Bussiness
{
    public class TienmatBussiness
    {
        Entities db;
        public TienmatBussiness()
        {
            db = Entities.CreateEntities();
        }
        public List<T_TIENMAT> GetAll()
        {
            return db.T_TIENMAT.ToList();
        }
        public List<T_TIENMAT> GetByMadv(string Madv)
        {
            return db.T_TIENMAT.Where(x=>x.MADV == Madv).OrderByDescending(x=>x.PHIEUTHU).ToList();
        }
        public List<T_TIENMAT> GetByThangNam(string thang,string nam)
        {
            //DateTime tungay = new DateTime(int.Parse(nam), int.Parse(thang), 1);
            //DateTime Denngay  = tungay.AddMonths(1);
            //            Denngay = tungay.AddDays(-(tungay.Day));          

            return db.T_TIENMAT.Where(x => x.THANGCN == thang+nam).OrderByDescending(x => x.MADV).ToList();
        }

        public int Update(T_TIENMAT tIENMAT)
        {
            T_TIENMAT _tienmat = db.T_TIENMAT.FirstOrDefault(x => x.ID == tIENMAT.ID);
            _tienmat.CDP = tIENMAT.CDP;
            _tienmat.LUONG = tIENMAT.LUONG;
            _tienmat.PHIEUTHU = tIENMAT.PHIEUTHU;
            _tienmat.BHXH_YT = tIENMAT.BHXH_YT;
            _tienmat.BHTN = tIENMAT.BHTN;
            _tienmat.DVP = tIENMAT.DVP;
            _tienmat.NOIDUNGTRA = tIENMAT.NOIDUNGTRA;
            _tienmat.T_LUONG = tIENMAT.T_LUONG;
            _tienmat.SOHD = tIENMAT.SOHD;
            _tienmat.MADV = tIENMAT.MADV;
            _tienmat.TENDV = tIENMAT.TENDV;
            _tienmat.TTN = tIENMAT.TTN;
            _tienmat.PHIEUTHU = tIENMAT.PHIEUTHU;


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
        public int DeleteByID(T_TIENMAT t)
        {
            T_TIENMAT _TIENMAT = db.T_TIENMAT.FirstOrDefault(x => x.ID == t.ID);

            try
            {
                db.T_TIENMAT.Remove(_TIENMAT);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);

            }

        }
        public int Insert(T_TIENMAT t)
        {

            try
            {
                db.T_TIENMAT.Add(t);
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
