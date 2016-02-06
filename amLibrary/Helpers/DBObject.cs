using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace amLibrary.Helpers
{
    public abstract class DBObject : DBHelper
    {
        public DBObject Parent { get; private set; }
        public DBObject(DBObject parent):base() { Parent = parent; }
        public virtual void Save() { }
        public virtual void Delete() { }
        public virtual void Edit() { }
    }
}
