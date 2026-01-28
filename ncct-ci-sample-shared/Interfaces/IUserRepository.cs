using Ncct.Blazor.Sample.Shared.Models;

namespace Ncct.Blazor.Sample.Shared.Interfaces;

public interface IUserRepository
{
    User GetById(int id);
    void Save(User user);
}
