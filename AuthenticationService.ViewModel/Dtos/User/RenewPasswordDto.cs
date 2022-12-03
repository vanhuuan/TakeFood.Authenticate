namespace AuthenticationService.ViewModel.Dtos.User;

public class RenewPasswordDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
}
