using EipqLibrary.Shared.SharedSettings.Interfaces;

namespace EipqLibrary.Shared.SharedSettings
{
    public class AppConfig : IAppConfig
    {
        public JwtSettings JwtSettings { get; set; }
    }
}
