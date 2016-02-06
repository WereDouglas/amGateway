using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using amLibrary.Helpers;
using System.Data.SqlServerCe;

namespace amLibrary
{
    public class Message : DBObject
    {
        public Message(DBObject parent)
            : base(parent)
        {
            Ids = Guid.NewGuid().ToString();

        }
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _ids;

        public string Ids
        {
            get { return _ids; }
            set { _ids = value; }
        }
        private string _numbers;
        public string Numbers
        {
            get { return _numbers; }
            set { _numbers = value; }
        }

        private string _messages;
        public string Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }

        private string _sent;
        public string Sent
        {
            get { return _sent; }
            set { _sent = value; }
        }
        private string _dor;
        public string Dor
        {
            get { return _dor; }
            set { _dor = value; }
        }

        public override void Save()
        {
            if (Numbers == " " || Messages == " ")
            {
                throw new Exception("Empty fields");
            }

            else
            {
                SqlCeCommand cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO [message](id,ids,numbers,messages,sent,dor)VALUES(@id,@ids,@numbers,@messages,@sent,@dor)";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@ids", Ids);
                cmd.Parameters.AddWithValue("@numbers", Numbers);
                cmd.Parameters.AddWithValue("@messages", Messages);
                cmd.Parameters.AddWithValue("@sent", Sent);
                cmd.Parameters.AddWithValue("@dor", Dor);
               

                ExecuteNonQuery(cmd);
                //System.Diagnostics.Debug.WriteLine(cmd);

            }
        }
        // _message.Update(Id, fnumbers.Text,lnumbers.Text,dor.Text,dob.Text, height.Text,weight.Text,phone.Text,email.Text,region.Text,"");

        public void Update(string id,string sent)
        {
            SqlCeCommand cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE [message] SET sent='" + sent + "' WHERE id = '" + id + "'";
            
            ExecuteNonQuery(cmd);

        }

        public bool Delete(string Id)
        {
            System.Diagnostics.Debug.Write(Id + "|");
            SqlCeCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM [message] WHERE id ='" + Id + "'";
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
            cmd.CommandText = "DELETE FROM [message]";
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
