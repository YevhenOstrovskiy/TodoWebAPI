namespace WebAPI.Controllers
{
    public record RegisterUserRequest(string UserName, string Email, string Password)
    {
    }
}
