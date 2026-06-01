namespace PocketGrail.Infrastructure.Configuration;

public class EmailConfiguration
{
    public string SenderAddress { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SmtpUsername { get; set; } = string.Empty;
    public string VerificationCodeTemplatePath { get; set; } = string.Empty;
}
