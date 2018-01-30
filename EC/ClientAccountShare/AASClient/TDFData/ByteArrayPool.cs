using System.Collections.Generic;

namespace AASClient.TDFData
{
    public class ByteArrayPool
    {
        private int _maxCount;
        private Dictionary<int, List<byte[]>> _pool;

        public ByteArrayPool()
        {
            _maxCount = 1000;
            _pool = new Dictionary<int, List<byte[]>>();
        }

        public byte[] Malloc(int size)
        {
            lock (_pool)
            {
                if (_pool.ContainsKey(size) && _pool[size].Count > 0)
                {
                    byte[] tmp = _pool[size][0];
                    tmp.Initialize();
                    _pool[size].RemoveAt(0);
                    return tmp;
                }
                else
                {
                    if (_pool.ContainsKey(size) == false)
                    {
                        _pool.Add(size, new List<byte[]>());
                    }

                    _pool[size].Add(new byte[size]);

                    return new byte[size];
                }
            }
        }

        public void Free(byte[] tmp)
        {
            lock (_pool)
            {
                if (_pool.ContainsKey(tmp.Length))
                {
                    _pool[tmp.Length].Add(tmp);
                }
                else
                {
                    if (_pool.ContainsKey(tmp.Length) == false)
                    {
                        _pool.Add(tmp.Length, new List<byte[]>());
                    }

                    if (_pool[tmp.Length].Count < _maxCount)
                    {
                        _pool[tmp.Length].Add(tmp);
                    }
                }
            }
        }
    }
}
