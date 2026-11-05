namespace PriceForgeClicker.Network
{
    public sealed record AuthRequest(string Email, string Password)
    {
        public string Email { get; } = Email;
        public string Password { get; } = Password;
    }
}