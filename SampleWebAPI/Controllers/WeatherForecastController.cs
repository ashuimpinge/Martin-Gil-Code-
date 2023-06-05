using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MartinGilDemoAPI.Model;
using MartinGilDemoAPI.Services;
using MartinGilDemo.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace MartinGilDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new Query(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOne(int Id)
        {
            await Db.Connection.OpenAsync();
            var query = new Query(Db);
            var result = await query.FindOneAsync(Id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Person model)
        {
            var body = new ModelData();
            body.FirstName = model.FirstName;
            body.LastName = model.LastName;
            body.MobileNumber=model.MobileNumber;
            body.Email = model.Email;

            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return Ok(body.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] ModelData body)
        {
            await Db.Connection.OpenAsync();
            var query = new Query(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();

            result.FirstName = body.FirstName;
            result.LastName = body.LastName;
            result.MobileNumber = body.MobileNumber;
            result.Email = body.Email;

            await result.UpdateAsync();
            return new OkObjectResult(result.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Query(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new Query(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }

        public AppDb Db { get; }
    }
}