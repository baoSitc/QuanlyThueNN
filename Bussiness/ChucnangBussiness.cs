using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class ChucnangBussiness
    {
        Entities db;
        public ChucnangBussiness()
        {
            db = Entities.CreateEntities();
        }
        public List<CHUCNANG> GetAll()
        {
            return db.CHUCNANGs.ToList();
        }
        void Insert(CHUCNANG _chucnang)
        {
            try
            {
                db.CHUCNANGs.Add(_chucnang);
                db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);

            }

        }
    }
}
