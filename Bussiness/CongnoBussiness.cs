using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Bussiness
{
    public class CongnoBussiness
    {
        Entities db;
        public CongnoBussiness()
        {
            db = Entities.CreateEntities();
        }
        public List<CONGNO> Getall(string nguoisd) 
        {
            return db.CONGNOes.Where(x=>x.NGUOISD==nguoisd) .OrderByDescending(x => x.NGAYCN).ToList();
        }
        public List<CONGNO> GetallbyTrangthai(String nguoisd)
        {
            return db.CONGNOes.Where(x=>x.TRANGTHAI==true && x.NGUOISD==nguoisd).ToList();
        }
        //Thêm mới công nợ
        public void ThemCongno(CONGNO cn)
        {
            db.CONGNOes.Add(cn);
            db.SaveChanges();
        }


    }
}
