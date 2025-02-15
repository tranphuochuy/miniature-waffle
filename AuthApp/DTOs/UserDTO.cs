public class UserDTO
{
    
    public string Username { get; set; } = string.Empty;

    public string Passwordhash { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = null!;
}