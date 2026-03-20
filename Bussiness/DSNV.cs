using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace Bussiness
{
   public  class DSNV
    {
        Entities db;
        public DSNV()
        {
            db = Entities.CreateEntities();
        }
        public List<DataLayer.DSNV>Getall()
        {
            return db.DSNVs.ToList();
        }
        public DataLayer.DSNV FindUser(string user)
        {
            return db.DSNVs.FirstOrDefault(x=>x.MANVQL==user);
        }
        public DataLayer.DSNV GetBYUserPW(string User,string PW)
        {

            return db.DSNVs.FirstOrDefault(x => x.MANVQL == User && x.MATKHAU==PW && x.KHOATAIKHOAN==0);
            
        }
        public int ChangePW(string User, string PWN)
        {
            DataLayer.DSNV _nv= new DataLayer.DSNV();

            _nv= db.DSNVs.FirstOrDefault(x => x.MANVQL == User);
            if( _nv!=null )
            {
                _nv.MATKHAU = PWN;
                db.SaveChanges();
                return 1;
            }
            else
            { return 0; }

        }
        public int AddUser(DataLayer.DSNV NV)
        {
            try
            {
                db.DSNVs.Add(NV);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);

            }
        }
        public int EditUser(DataLayer.DSNV NV)
        {
            try
            {

                DataLayer.DSNV _nv = new DataLayer.DSNV();

                _nv = db.DSNVs.FirstOrDefault(x => x.MANVQL == NV.MANVQL);
                if (_nv != null)
                {
                    _nv.HODEM = NV.HODEM;
                    _nv.TEN=NV.TEN;
                    _nv.MANHOM = NV.MANHOM;
                    _nv.NHOMSD = NV.NHOMSD;
                    _nv.KHOATAIKHOAN = NV.KHOATAIKHOAN;
                    _nv.Khoasolieu = 0;
                    _nv.Chinhsua = NV.Chinhsua;
                    _nv.InSL = NV.InSL;
                    _nv.Xem = NV.Xem;
                    db.SaveChanges();
                    return 1;
                }
                else
                { return 0; }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);

            }
        }


    }
}
