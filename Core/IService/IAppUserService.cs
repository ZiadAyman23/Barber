namespace Core.IService
{
    public interface IAppUserService
    {
        Task<Tokens> Login(Login login);
        Task<Tokens> RefreshToken(string refreshToken);
        public Task CheckSecreatKey (CheckAccess secreatKey);
        public Task Register(RegisterD registerD);
    }
}
