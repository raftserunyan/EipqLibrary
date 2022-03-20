using System;

namespace EipqLibrary.Shared.SharedSettings.Interfaces
{
    public interface IAppConfig
    {
        JwtSettings JwtSettings { get; set; }
    }
}
