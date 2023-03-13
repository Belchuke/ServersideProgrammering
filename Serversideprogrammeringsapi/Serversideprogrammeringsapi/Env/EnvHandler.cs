using Serversideprogrammeringsapi.Identity.Properties;
using Serversideprogrammeringsapi.Models;

namespace Serversideprogrammeringsapi.Env
{
    public class EnvHandler
    {
        public static string GETOTPKey()
        {
            DotNetEnv.Env.Load(".env");
            return DotNetEnv.Env.GetString("OTP_KEY", "+sHNeEAP7icPOsIN3O1nxUiZ3Qqy3NieBWOjViFP4owafWXZX9CeEsVfGfpDI6U6i3jx3IkIlDPc/T5t8HDm0A==");
        }

        public static string GetAdminPas()
        {
            DotNetEnv.Env.Load(".env");

            return DotNetEnv.Env.GetString("ADMIN_PASS", "12345678910");
        }


        public static string UserDBString()
        {
            DotNetEnv.Env.Load(".env");

            return $"Server={DotNetEnv.Env.GetString("DB_USER_SERVER")},{DotNetEnv.Env.GetInt("DB_USER_PORT")}; Database={DotNetEnv.Env.GetString("DB_USER_DATABASE")}; TrustServerCertificate=True; User ID={DotNetEnv.Env.GetString("DB_USER_USER")}; Password={DotNetEnv.Env.GetString("DB_USER_PASS")};";
        }

        public static string ToDoDBString()
        {
            DotNetEnv.Env.Load(".env");
            return $"Server={DotNetEnv.Env.GetString("DB_TODO_SERVER")},{DotNetEnv.Env.GetInt("DB_TODO_PORT")}; Database={DotNetEnv.Env.GetString("DB_TODO_DATABASE")}; TrustServerCertificate=True; User ID={DotNetEnv.Env.GetString("DB_TODO_USER")}; Password={DotNetEnv.Env.GetString("DB_TODO_PASS")};";
        }

        public static JwtIssuerOptions GetJWTOptions()
        {
            DotNetEnv.Env.Load(".env");
            string audience = DotNetEnv.Env.GetString("JWT_AUDIENCE", "https://localhost:7228/");
            string issuer = DotNetEnv.Env.GetString("JWT_ISSUER", "data");
            string key = DotNetEnv.Env.GetString(
                "JWT_KEY",
                ""
                );

            return new JwtIssuerOptions()
            {
                Audience = audience,
                Issuer = issuer,
                Key = key
            };
        }

        public static SimpleSmtpOptions GetSmtpOptions()
        {
            DotNetEnv.Env.Load(".env");
            string host = DotNetEnv.Env.GetString("SMTP_HOST", "(missing)");
            int port = DotNetEnv.Env.GetInt("SMTP_PORT", 80);
            string defaultFromEmail = DotNetEnv.Env.GetString("SMTP_DEFAULT_FROM", "");
            string defaultFromName = DotNetEnv.Env.GetString("SMTP_DEFAULT_NAME", "");
            string userName = DotNetEnv.Env.GetString("SMTP_USERNAME", "");
            string password = DotNetEnv.Env.GetString("SMTP_PASS", "");

            return new SimpleSmtpOptions()
            {
                Host = host,
                Port = port,
                DefaultFromEmail = defaultFromEmail,
                DefaultFromName = defaultFromName,
                Username = userName,
                Password = password,
            };
        }
    }
}
