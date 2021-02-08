namespace AzsunaBOT.Helpers
{
    public static class AttendanceHelper
    {
        //private static List<AttendanceModel> _attendanceList;



        //public static async void RefreshRoster(CommandContext context, IDataAccess _data, IConfiguration _config)
        //{
        //    try
        //    {
        //        await RoleLists.ClearLists();

        //        _attendanceList = _data.LoadData<AttendanceModel, dynamic>("sp_adminLookUp", new { @tblName = CurrentWoeDate }, _config.GetConnectionString("AzsunaBOT")).Result;

        //        await RosterMessager.FirstRoleMessage(context, await RoleProcessor.SortFirstBatchAsync(_attendanceList));
        //        await RosterMessager.SecondRoleMessage(context, await RoleProcessor.SortSecondBatchAsync(_attendanceList));
        //        await RosterMessager.ThirdRoleMessage(context, await RoleProcessor.SortThirdBatchAsync(_attendanceList));

        //    }
        //    catch (Exception e) { Console.WriteLine(e.Message); }
        //}
    }
}
