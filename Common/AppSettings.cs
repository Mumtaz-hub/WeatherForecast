namespace Common
{
    public class AppSettings
    {
        public const string Key = "AppSettings";

        public string Environment { get; set; }
        public bool UseInMemoryDatabase { get; set; }
    }
}
