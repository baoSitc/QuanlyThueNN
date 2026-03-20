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
    }
}
