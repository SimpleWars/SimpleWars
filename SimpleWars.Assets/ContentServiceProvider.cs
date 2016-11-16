namespace SimpleWars.Assets
{
    using System;

    public static class ContentServiceProvider
    {
        public static IServiceProvider ContentService { get; set; }

        public static string RootDirectory { get; set; }
    }
}