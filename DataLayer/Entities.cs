using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    [Serializable]
    public partial class Entities
    {
        private Entities(DbConnection connectionString, bool contextOwnsConnection = true) : base(connectionString, contextOwnsConnection) { }
        public static Entities CreateEntities(bool contextOwnsConnection = true)
        {
           
                //DOc file Connect
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open("connectdb.dba", FileMode.Open, FileAccess.Read);
                connect cp = (connect)bf.Deserialize(fs);
                //Decrypt noidung
                string servername = Encryptor.Decrypt(cp.servername, "quocbao", true);
                string username = Encryptor.Decrypt(cp.username, "quocbao", true);
                string pass = Encryptor.Decrypt(cp.passwd, "quocbao", true);
                string database = Encryptor.Decrypt(cp.database, "quocbao", true);

                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
                SqlConnectionStringBuilder sqlConnectBuilder = new SqlConnectionStringBuilder();
                sqlConnectBuilder.DataSource = servername;
                sqlConnectBuilder.InitialCatalog = database;
                sqlConnectBuilder.UserID = username;
                sqlConnectBuilder.Password = pass;
                string sqlConnectionString = sqlConnectBuilder.ConnectionString;
                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                entityBuilder.Provider = "System.Data.SqlClient";
                entityBuilder.ProviderConnectionString = sqlConnectionString;
                entityBuilder.Metadata = @"res://*/Quanlythue.csdl|res://*/Quanlythue.ssdl|res://*/Quanlythue.msl";
                EntityConnection connection = new EntityConnection(entityBuilder.ConnectionString);
                fs.Close();
                return new Entities(connection);
           
        }
    }
}
