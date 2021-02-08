using AzsunaBOT.Data;
using AzsunaBOT.Enums;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers.Processes
{
    public static class RoleProcessor
    {
        public static async Task SortAllAsync(List<AttendanceModel> list)
        {
            await SortFirstBatchAsync(list);
            await SortSecondBatchAsync(list);
            await SortThirdBatchAsync(list);
        }

        private static async Task<List<AttendanceModel>> SortFirstBatchAsync(List<AttendanceModel> list)
        {
            return await Task.Run(() =>
            {
                foreach (var item in list)
                {
                    if (Enum.IsDefined(typeof(FirstPostRoles), item.Role.ToUpper()))
                    {
                        RoleLists.FirstBatch.Add(item);
                    }
                }
                var sorted = RoleLists.FirstBatch.OrderByDescending(x => x.Role).ThenByDescending(x => x.Sign).ToList();

                return sorted;
            });
        }

        private static async Task<List<AttendanceModel>> SortSecondBatchAsync(List<AttendanceModel> list)
        {
            return await Task.Run(() =>
            {
                foreach (var item in list)
                {
                    if (Enum.IsDefined(typeof(SecondPostRoles), item.Role.ToUpper()))
                    {
                        RoleLists.SecondBatch.Add(item);
                    }
                }
                var sorted = RoleLists.SecondBatch.OrderByDescending(x => x.Role).ThenByDescending(x => x.Sign).ToList();

                return sorted;
            });
        }

        private static async Task<List<AttendanceModel>> SortThirdBatchAsync(List<AttendanceModel> list)
        {
            return await Task.Run(() =>
            {
                foreach (var item in list)
                {
                    if (Enum.IsDefined(typeof(ThirdPostRoles), item.Role.ToUpper()))
                    {
                        RoleLists.ThirdBatch.Add(item);
                    }
                }
                var sorted = RoleLists.ThirdBatch.OrderByDescending(x => x.Role).ThenByDescending(x => x.Sign).ToList();

                return sorted;
            });
        }
    }
}
