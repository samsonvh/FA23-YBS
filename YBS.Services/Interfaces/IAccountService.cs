namespace YBS.Services.Interfaces

{
    public interface IAccountService 
    {
        Task Login (string username , string password);
        Task GoogleLogin (string idToken);
    }
}