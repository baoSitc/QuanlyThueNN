using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class PhanquyenBussiness
    {
        Entities db;
        public PhanquyenBussiness()
        {
            db = Entities.CreateEntities();
        }
        public List<PHANQUYEN> GetAll()
        {
            return db.PHANQUYENs.ToList();
        }
        public List<PHANQUYEN> GetAllbyUser(string iduser)
        {
            return db.PHANQUYENs.Where(x=>x.IDUSER==iduser).ToList();
        }
        public void Insert(PHANQUYEN _phanquyen)
        {
            try
            {
                
                db.PHANQUYENs.Add(_phanquyen);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xãy ra trong quá trình xử lý dữ liệu " + ex.Message);

            }

        }
    }
}
