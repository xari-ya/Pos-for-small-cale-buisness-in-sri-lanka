namespace billing_system
{
    internal static class AppConfig
    {
        public const string DbPath = "bims.db";
        public const int Pbkdf2Iter = 100_000;
        public const int SaltSize = 16;
        public const int HashSize = 32;
    }
}
