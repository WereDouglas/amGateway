using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using amLibrary.Helpers;
using System.Data;

namespace amLibrary
{
    public class NetworkCollection : DBObject, IEnumerable<Network>
    {
        #region Members
        private List<Network> _networks;
        #endregion

        #region Properties
        public int Count
        { get { return _networks.Count; } }

        public bool IsLoaded { get; private set; }
        #endregion
        public NetworkCollection(DBObject parent)
            : base(parent)
        {
            _networks = new List<Network>();
            // _networks.Clear();
        }

        public Network Add()
        {
            Network u = new Network(this);
            _networks.Add(u);
            return u;
        }

        public IEnumerator<Network> GetEnumerator()
        {
            foreach (var item in _networks)
                yield return item;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Load()
        {
            _networks.Clear();
            string sql = "select * from [network]";

            DataTable table = new DataTable();
            ExecuteQuery(sql, table);

            foreach (DataRow row in table.Rows)
            {
                Network u = Add();
                u.Id = row["id"].ToString();
                u.Names = row["names"].ToString();
                u.Comm = row["comm"].ToString();
                u.Status = row["status"].ToString();

            }

            IsLoaded = true;
        }

        public void Refresh()
        {
            _networks.Clear();
            Load();
        }
    }
}
