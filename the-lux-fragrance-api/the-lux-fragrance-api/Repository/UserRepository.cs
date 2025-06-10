using the_lux_fragrance_api.Data;
using the_lux_fragrance_api.Models;
using the_lux_fragrance_api.Repository.Interface;

namespace the_lux_fragrance_api.Repository;

public class UserRepository(CatalogoContext context) : IUserRepository
{
    public User CriarUser(User user)
    {
        context.Add(user);
        user.Id = context.SaveChanges();
        return user;
    }

    public User? GetByEmail(string email)
    {
        var user = context.Users.SingleOrDefault(x => x.Email == email);
        return user;
    }

    public User? GetById(int id)
    {
        var user = context.Users.SingleOrDefault(x => x.Id == id);
        return user;
    }
}