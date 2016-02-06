using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using amLibrary.Helpers;
using System.Data;

namespace amLibrary
{
    public class MessageCollection : DBObject, IEnumerable<Message>
    {
        #region Members
        private List<Message> _messages;
        #endregion

        #region Properties
        public int Count
        { get { return _messages.Count; } }

        public bool IsLoaded { get; private set; }
        #endregion
        public MessageCollection(DBObject parent)
            :base(parent)
        {          
            _messages = new List<Message>();
           // _messages.Clear();
        }

        public Message Add()
        {
            Message u = new Message(this);            
            _messages.Add(u);
            return u;
        }

        public IEnumerator<Message> GetEnumerator()
        {
           
            foreach (var item in _messages)
            yield return item;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Load()
        {
            _messages.Clear();
            string sql = "select * from [message]";

            DataTable table = new DataTable();
            ExecuteQuery(sql, table);

            foreach (DataRow row in table.Rows)
            {
                    Message u=Add();
                    u.Id = row["id"].ToString();
                    u.Ids= row["ids"].ToString();
                    u.Numbers = row["numbers"].ToString();
                    u.Messages = row["messages"].ToString();
                    u.Sent = row["sent"].ToString();
                    u.Dor = row["dor"].ToString();
                   
                   
              
            }
            IsLoaded = true;
        }

        public void Refresh()
        {
            _messages.Clear();
            Load();
        }
    }
}
