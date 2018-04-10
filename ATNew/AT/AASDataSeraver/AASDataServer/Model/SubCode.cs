using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.Model
{
    public class SubCode : AbstractModel
    {
        private string _code;
        private string _name;
        private DateTime _subTime;
        private DateTime _flushTime;

        private List<string> _users;

        public string Code
        {
            get {
                return _code;
            }
            set {
                _code = value;
            }
        }

        public string Name
        {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }

        public List<string> Users
        {
            get {
                return _users;
            }
            set {
                _users = value;
            }
        }
		
		public int UserCount
		{
			get {
				return _users.Count;
			}
		}

        public DateTime SubTime
        {
            get {
                return _subTime;
            }
            set {
                _subTime = value;
            }
        }

        public DateTime FlushTime
        {
            get {
                return _flushTime;
            }
            set {
                _flushTime = value;
            }
        }

        public SubCode()
        {
            _users = new List<string>();
            _subTime = DateTime.Now;
            _flushTime = DateTime.Now;
        }

        public void Flush(string username)
        {
            _flushTime = DateTime.Now;
            if (_users.Contains(username) == false)
            {
                _users.Add(username);
            }
        }
    }
}
