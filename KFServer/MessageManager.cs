using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASServer
{
    public class MessageManager
    {
        private MessageManager()
        { }

        static object sync = new object();

        private MessageManager _instance;
        public MessageManager GetInstance {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new MessageManager();
                        }
                    }
                }
                return _instance;
            }
        }



    }
}
