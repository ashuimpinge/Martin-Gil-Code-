using MartinGilDemo.Shared;
using System.Net;
using System.Runtime.CompilerServices;

namespace MartinGilDemo.Client.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Person>> GetAll();
        
        Task<int> AddUser(Person person);

        Task<IEnumerable<Person>> GetUserById(int Id);
        
        // public void AddUser(User user);
        //  public void UpdateUserDetails(User user);
        // public User GetUserData(int id);
        //  public void DeleteUser(int id);
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
            return await _httpService.Post<int>("/api/WeatherForecast",person);
        }

        public async Task<IEnumerable<Person>> GetUserById(int Id)
        {
            return await _httpService.Get<IEnumerable<Person>>("/api/WeatherForecast");
        }
    }
}
