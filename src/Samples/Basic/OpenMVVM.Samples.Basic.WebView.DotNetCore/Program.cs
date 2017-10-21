namespace OpenMVVM.Samples.Basic.WebView.DotNetCore
{
    using System;
    using System.IO;

    using Microsoft.AspNetCore.Hosting;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://*:6886")
                .Build();

            host.Run();


            Console.ReadLine();
        }
    }
}
