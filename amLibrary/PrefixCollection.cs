using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using amLibrary.Helpers;
using System.Data;

namespace amLibrary
{
    public class PrefixCollection : DBObject, IEnumerable<Prefix>
    {
        #region Members
        private List<Prefix> _prefixs;
        #endregion

        #region Properties
        public int Count
        { get { return _prefixs.Count; } }

        public bool IsLoaded { get; private set; }
        #endregion
        public PrefixCollection(DBObject parent)
            : base(parent)
        {
            _prefixs = new List<Prefix>();
            // _prefixs.Clear();
        }

        public Prefix Add()
        {
            Prefix u = new Prefix(this);
            _prefixs.Add(u);
            return u;
        }

        public IEnumerator<Prefix> GetEnumerator()
        {
            foreach (var item in _prefixs)
                yield return item;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Load()
        {
            _prefixs.Clear();
            string sql = "select * from [prefix]";

            DataTable table = new DataTable();
            ExecuteQuery(sql, table);

            foreach (DataRow row in table.Rows)
            {
                Prefix u = Add();
                u.Id = row["id"].ToString();
                u.Pre = row["pre"].ToString();
                u.Provider = row["provider"].ToString();

            }

            IsLoaded = true;
        }

        public void Refresh()
        {
            _prefixs.Clear();
            Load();
        }
    }
}
