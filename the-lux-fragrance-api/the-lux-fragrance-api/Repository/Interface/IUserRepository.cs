using the_lux_fragrance_api.Models;

namespace the_lux_fragrance_api.Repository.Interface;

public interface IUserRepository
{
    public User CriarUser(User user);
    public User? GetByEmail(string email);
    public User? GetById(int id);
}