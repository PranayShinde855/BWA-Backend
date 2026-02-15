using BWA.ServiceEntities;

namespace BWA.Services.interfaces
{
    public interface IAuthService
    {
        Task<LoginDetails> LoginAsync(LoginDto requestDto);
        Task<bool> LogOutAsync(LogOutDto requestDto);
    }
}
