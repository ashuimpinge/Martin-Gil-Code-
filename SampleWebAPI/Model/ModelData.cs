using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace MartinGilDemoAPI.Model
{
    public class ModelData
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MobileNumber { get; set; }
        public string Email { get; set; }

        internal AppDb Db { get; set; }
        public ModelData()
        {
        }
        internal ModelData(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Person` (`FirstName`, `LastName`,`MobileNumber`,`Email`) VALUES (@FirstName, @LastName,@MobileNumber,@Email);";
            //cmd.CommandText = @"";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Person` SET `FirstName` = @FirstName, `LastName` = @LastName, `MobileNumber` = @MobileNumber,`Email` = @Email WHERE `Id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Person` WHERE `Id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FirstName",
                DbType = DbType.String,
                Value = FirstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LastName",
                DbType = DbType.String,
                Value = LastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@MobileNumber",
                DbType = DbType.Int16,
                Value = MobileNumber,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Email",
                DbType = DbType.String,
                Value = Email,
            });
        }
    }
}
