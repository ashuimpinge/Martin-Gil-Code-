using MartinGilDemo.Shared;
using System.Net;
using System.Runtime.CompilerServices;

namespace MartinGilDemo.Client.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Person>> GetAll();

        Task<int> AddUser(Person person);

        Task<Person> GetUserById(int Id);

        Task<int> UpdateUserDetails(Person person);
        Task<int> DeleteUser(int id);

        //public void DeleteUser(int id);
    }

    public class UserService : IUserService
    {
        private IHttpService _httpService;

        public UserService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _httpService.Get<IEnumerable<Person>>("/api/WeatherForecast");
        }

        public async Task<int> AddUser(Person person)
        {
            return await _httpService.Post<int>("/api/WeatherForecast", person);
        }

        public async Task<Person> GetUserById(int Id)
        {
            return await _httpService.Get<Person>("/api/WeatherForecast/" + Id);
        }

        //Need to check 
        public async Task<int> UpdateUserDetails(Person person)
        {
            return await _httpService.Put<int>("/api/WeatherForecast/" + person.Id, person);
        }

        public async Task<int> DeleteUser(int Id)
        {
            return await _httpService.Delete<int>("/api/WeatherForecast/" + Id);
        }
    }
}
