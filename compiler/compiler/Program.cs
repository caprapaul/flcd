using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();
            
            ServiceProvider services = new ServiceCollection()
                .AddSingleton(configuration)
                .AddSingleton<SymbolTable>()
                .AddSingleton<ProgramInternalForm>()
                .AddSingleton<Scanner>()
                .BuildServiceProvider();

            var scanner = services.GetRequiredService<Scanner>();
            try
            {
                scanner.Scan(configuration["File"]);
                
                var st = services.GetRequiredService<SymbolTable>();
                var pif = services.GetRequiredService<ProgramInternalForm>();

                using var stWriter = new StreamWriter("ST.out");
                using var pifWriter = new StreamWriter("PIF.out");
            
                stWriter.Write(st.ToString());
                pifWriter.Write(pif.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}