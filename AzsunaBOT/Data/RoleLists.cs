using AzsunaBOT.Enums;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static async Task FillLists(List<AttendanceModel> list)
        {
            await ClearLists();

            FirstBatch = await FillFirstBatchAsync(list);
            SecondBatch = await FillSecondBatchAsync(list);
            ThirdBatch = await FillThirdBatchAsync(list);
        }

        private static async Task<List<AttendanceModel>> FillFirstBatchAsync(List<AttendanceModel> list)
        {
            return await Task.Run(() =>
            {
                foreach (var item in list)
                {
                    if (Enum.IsDefined(typeof(FirstPostRoles), item.Role.ToUpper()))
                        FirstBatch.Add(item);
                }
                var sorted = FirstBatch.OrderBy(x => Enum.Parse(typeof(RolesAll), x.Role)).ThenBy(x => Enum.Parse(typeof(Signs), x.Sign)).ToList();

                return sorted;
            });
        }

        private static async Task<List<AttendanceModel>> FillSecondBatchAsync(List<AttendanceModel> list)
        {
            return await Task.Run(() =>
            {
                foreach (var item in list)
                {
                    if (Enum.IsDefined(typeof(SecondPostRoles), item.Role.ToUpper()))
                        SecondBatch.Add(item);
                }
                var sorted = SecondBatch.OrderBy(x => Enum.Parse(typeof(RolesAll), x.Role)).ThenBy(x => Enum.Parse(typeof(Signs), x.Sign)).ToList();

                return sorted;
            });
        }

        private static async Task<List<AttendanceModel>> FillThirdBatchAsync(List<AttendanceModel> list)
        {
            return await Task.Run(() =>
            {
                foreach (var item in list)
                {
                    if (Enum.IsDefined(typeof(ThirdPostRoles), item.Role.ToUpper()))
                        ThirdBatch.Add(item);
                }
                var sorted = ThirdBatch.OrderBy(x => Enum.Parse(typeof(RolesAll), x.Role)).ThenBy(x => Enum.Parse(typeof(Signs), x.Sign)).ToList();

                return sorted;
            });
        }
    }
}
