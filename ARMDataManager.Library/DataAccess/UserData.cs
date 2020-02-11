using System.Collections.Generic;
using ARMDataManager.Library.Internal.DataAccess;
using ARMDataManager.Library.Models;

namespace ARMDataManager.Library.DataAccess
{
    /// <summary>
    ///  Get information about the User table
    /// </summary>
    public class UserData
    {
        public List<UserModel> GetUserById(string id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new
            {
                Id = id
            };

            var output =
                sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");

            return output;
        }
    }
}