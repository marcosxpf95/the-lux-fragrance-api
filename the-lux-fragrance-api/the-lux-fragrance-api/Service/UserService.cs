using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Repository.Interface;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User CriarUser(User user)
    {
        _userRepository.CriarUser(user);
        return user;
    }

    public User? GetByEmail(string email)
    {
        return _userRepository.GetByEmail(email);
    }

    public User? GetById(int id)
    {
        return _userRepository.GetById(id);
    }
}