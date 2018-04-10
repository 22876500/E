using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.Model
{
    public class UserClient : AbstractModel
    {
        private string _username;
        private DateTime _updateTime;
        private List<string> _subcodes;

        public string Username
        {
            get {
                return _username;
            }
            set {
                _username = value;
            }
        }

        public int SubCount
        {
            get {
                return _subcodes.Count;
            }
        }

        public DateTime UpdateTime
        {
            get {
                return _updateTime;
            }
            set {
                _updateTime = value;
            }
        }

        public List<string> SubCodes
        {
            get {
                return _subcodes;
            }
            set {
                _subcodes = value;
            }
        }

        public UserClient()
        {
            _subcodes = new List<string>();
            _updateTime = DateTime.Now;
        }

        public void AddSubCodes(List<string> codes)
        { 
            foreach (string code in codes)
            {
                if (_subcodes.Contains(code) == false)
                {
                    _subcodes.Add(code);
                }
            }

            _updateTime = DateTime.Now;
        }

        public void RemoveSubCodes(List<string> codes)
        {
            foreach (string code in codes)
            {
                if (_subcodes.Contains(code) == true)
                {
                    _subcodes.Remove(code);
                }
            }

            _updateTime = DateTime.Now;
        }
    }
}
