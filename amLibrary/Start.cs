using amLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amLibrary
{
    public class Start : DBObject
    {
        public Start()
            : base(null)
        {
            _messages = new MessageCollection(this);
            _networks = new NetworkCollection(this);
            _prefixs = new PrefixCollection(this);
          

        }

        private MessageCollection _messages;
        public MessageCollection Messages
        {
            get
            {
                if (!_messages.IsLoaded)
                    _messages.Refresh();
                _messages.Load();
                return _messages;
            }
        }

        private NetworkCollection _networks;
        public NetworkCollection Networks
        {
            get
            {
                if (!_networks.IsLoaded)
                    _networks.Refresh();
                _networks.Load();
                return _networks;
            }
        }


        private PrefixCollection _prefixs;
        public PrefixCollection Prefixs
        {
            get
            {
                if (!_prefixs.IsLoaded)
                    _prefixs.Refresh();
                _prefixs.Load();
                return _prefixs;
            }
        }



       

    }
}
