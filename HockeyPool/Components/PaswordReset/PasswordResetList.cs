namespace HockeyPool.Components.PaswordReset;

public static class PasswordResetList
{
    public static List<PasswordToResetRequest> Requests = new List<PasswordToResetRequest>();
}

public class PasswordToResetRequest
{
    public Guid Code { get; set; }
    public string Email { get; set; }
    public DateTime RequestedAt { get; set; }
}
