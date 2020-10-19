using AzsunaBOT.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace AzsunaBOT.Data
{
    static class MVPTimerList
    {
        public static List<MVPTimer> _list;
        private static MVPTimer timer;

        static MVPTimerList()
        {
            _list = new List<MVPTimer>();
        }

        public static void Add(MVPTimer timer)
        {
            _list.Add(timer);
        }

        public static void Remove(MVPTimer timer)
        {
            _list.Remove(timer);
        }

        public static bool Check(string name)
        {
            bool listChecked = false;

            listChecked = _list.Any(l => l.Name.ToUpper() == name);

            return listChecked;
        }

        public static MVPTimer GetTimerObject(string name)
            => timer = _list.SingleOrDefault(t => t.Name.ToUpper() == name);
    }
}
