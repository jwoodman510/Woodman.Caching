using System.Collections.Generic;

namespace Woodman.Caching
{
    public class RedisCacheConfiguration
    {
        public List<string> Endpoints { get; set; } = new List<string>();
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string ConnectionString
        {
            get
            {
                var endpoints = string.Join(",", Endpoints);
                var client = string.IsNullOrEmpty(Username) ? string.Empty : $",name={Username}";
                var password = string.IsNullOrEmpty(Password) ? string.Empty : $",password={Password}";

                return $"{endpoints}{client}{password}";
            }
        }

        public static RedisCacheConfiguration Localhost = new RedisCacheConfiguration
        {
            Endpoints = new List<string> { "localhost:6379" },
            Username = string.Empty,
            Password = string.Empty
        };
    }
}
