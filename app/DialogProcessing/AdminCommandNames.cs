namespace DialogProcessing;

public class AdminCommandNames
{
    public const string Close = "/close";
    public const string Ban = "/ban";
    public const string Unban = "/unban";

    public static bool IsAdminCommand(string text)
    {
        return text == Close || text == Ban || text.Trim().StartsWith(Unban + " ");
    }
}
