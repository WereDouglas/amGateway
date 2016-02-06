using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using amLibrary.Helpers;
using System.Data.SqlServerCe;

namespace amLibrary
{
    public class Network : DBObject
    {
        public Network(DBObject parent)
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
        private string _names;

        public string Names
        {
            get { return _names; }
            set { _names = value; }
        }

        private string _comm;

        public string Comm
        {
            get { return _comm; }
            set { _comm = value; }
        }


        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }


        public override void Save()
        {
            if (Names == " ")
            {
                throw new Exception("Empty fields");
            }

            else
            {
                SqlCeCommand cmd = con.CreateCommand();

                cmd.CommandText = "INSERT INTO [network](id,names,comm,status)VALUES(@id,@names,@comm,@status)";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@names", Names);
                cmd.Parameters.AddWithValue("@comm", Comm);
                cmd.Parameters.AddWithValue("@status", Status);

                ExecuteNonQuery(cmd);
                //System.Diagnostics.Debug.WriteLine(cmd);

            }
        }
        // _network.Update(Id, fnames.Text,lnames.Text,dor.Text,dob.Text, height.Text,weight.Text,phone.Text,email.Text,region.Text,"");

        public void Update(string names, string comm)
        {
            SqlCeCommand cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE [network] SET comm='" + comm + "' WHERE names = '" + names + "'";

            ExecuteNonQuery(cmd);

        }
        public void UpdateStatus(string names, string status)
        {
            SqlCeCommand cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE [network] SET status='" + status + "' WHERE names = '" + names + "'";
            ExecuteNonQuery(cmd);

        }
        public bool Delete(string Id)
        {
            System.Diagnostics.Debug.Write(Id + "|");
            SqlCeCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM [network] WHERE id ='" + Id + "'";
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
            cmd.CommandText = "DELETE FROM [network]";
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
