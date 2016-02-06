using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using amLibrary.Helpers;
using System.Data.SqlServerCe;

namespace amLibrary
{
    public class Prefix : DBObject
    {
        public Prefix(DBObject parent)
            : base(parent)
        {
            Id = Guid.NewGuid().ToString();

        }
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _provider;

        public string Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        private string _pre;

        public string Pre
        {
            get { return _pre; }
            set { _pre = value; }
        }

        public override void Save()
        {
            if (Provider == " ")
            {
                throw new Exception("Empty fields");
            }

            else
            {
                SqlCeCommand cmd = con.CreateCommand();

                cmd.CommandText = "INSERT INTO [prefix](id,provider,pre)VALUES(@id,@provider,@pre)";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@provider", Provider);
                cmd.Parameters.AddWithValue("@pre", Pre);

                ExecuteNonQuery(cmd);
                //System.Diagnostics.Debug.WriteLine(cmd);

            }
        }
        // _prefix.Update(Id, fprovider.Text,lprovider.Text,dor.Text,dob.Text, height.Text,weight.Text,phone.Text,email.Text,region.Text,"");

        public void Update(string id, string sent)
        {
            SqlCeCommand cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE [prefix] SET sent='" + sent + "' WHERE id = '" + id + "'";

            ExecuteNonQuery(cmd);

        }

        public bool Delete(string Id)
        {
            System.Diagnostics.Debug.Write(Id + "|");
            SqlCeCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM [prefix] WHERE id ='" + Id + "'";
            try
            {

                ExecuteNonQuery(cmd);
            }
            catch (SqlCeException ex)
            {
                throw ex;
            }
            finally
            {

            }
            return true;

        }
        public bool Deleteall()
        {
            SqlCeCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM [prefix]";
            try
            {

                ExecuteNonQuery(cmd);
            }
            catch (SqlCeException ex)
            {
                throw ex;
            }
            finally
            {

            }
            return true;

        }



    }
}
