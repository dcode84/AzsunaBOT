using DataLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzsunaBOT.Data
{
    public static class RoleLists
    {
        public static List<AttendanceModel> FirstBatch = new List<AttendanceModel>();
        public static List<AttendanceModel> SecondBatch = new List<AttendanceModel>();
        public static List<AttendanceModel> ThirdBatch = new List<AttendanceModel>();

        public static Task ClearLists()
        {
            FirstBatch.Clear();
            SecondBatch.Clear();
            ThirdBatch.Clear();

            return Task.CompletedTask;
        }
    }
}
