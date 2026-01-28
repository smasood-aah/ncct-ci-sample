using Ncct.Blazor.Sample.Shared.Interfaces;
using Ncct.Blazor.Sample.Shared.Models;

namespace Ncct.Blazor.Sample.Services;

public class UserService
{
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;
    
    public UserService(IUserRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }
    
    public User GetUser(int id) => _repository.GetById(id);
    
    public void CreateUserAndNotify(User user)
    {
        _repository.Save(user);
        _emailService.SendEmail(user.Email, "Welcome", $"Welcome {user.Name}!");
    }
}