using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;
using MartinGilDemoAPI.Model;

namespace MartinGilDemoAPI.Services
{
    public class Query
    {
        public AppDb Db { get; }

        public Query(AppDb db)
        {
            Db = db;
        }


        public async Task<ModelData> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `FirstName`, `LastName`,`MobileNumber`, `Email` FROM `Person` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<ModelData>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `FirstName`, `LastName`,`MobileNumber`, `Email` FROM `Person` ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Person`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<ModelData>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<ModelData>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new ModelData(Db)
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        MobileNumber = reader.GetInt32(3),
                        Email = reader.GetString(4),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
