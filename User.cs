namespace billing_system
{
    internal sealed class User
    {
        public int UserId { get; init; }
        public string Username { get; init; } = "";
        public string FullName { get; init; } = "";
        public string Role { get; init; } = "Cashier";
        public bool IsActive { get; init; } = true;
        public string? LastLogin { get; set; }
    }
}
