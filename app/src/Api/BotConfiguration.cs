namespace Api;

public class BotConfiguration
{
    public string Token { get; set; } = null!;
    public string HostAddress { get; set; } = null!;
    public string WebhookToken { get; init; } = null!;
}
