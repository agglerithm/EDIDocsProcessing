using System.Collections;
using Infrastructure;


namespace AFPST.Common.Infrastructure.impl
{
    public class ObjectCache : IObjectCache
    {
        private readonly object lockObject = new object();  
        private readonly Hashtable _hashtable = new Hashtable();
        public static readonly IObjectCache Instance;

        static ObjectCache()
        {
            Instance = new ObjectCache();
        }

        public T Get<T>(string key) where T : class
        {
            lock (lockObject)
            {
                if (_hashtable.Contains(key) == false)
                {
                    //ToDo: Log Cache Miss
                    return null;
                }
                object data = _hashtable[key];
                return (T) data;
            }
        }

        public void Insert(string key, object data)
        {
            lock (lockObject)
            {
                if (string.IsNullOrEmpty(key)) return;
                if (data == null) return;
                if (_hashtable.ContainsKey(key))
                {
                    _hashtable.Remove(key);
                }
                _hashtable.Add(key,data);
            }
        }

        public int CountOfObjectsInCache //this is for testing
        {
            get { return _hashtable.Count; }
        }

        public void Flush()
        {
            lock (lockObject)
            {
                _hashtable.Clear();
            }
        }
    }
}