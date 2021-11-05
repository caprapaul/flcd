using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace compiler
{
    class Program
    {
        private static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();
            
            ServiceProvider services = new ServiceCollection()
                .AddSingleton(configuration)
                .AddSingleton<SymbolTable>()
                .AddSingleton<ProgramInternalForm>()
                .AddSingleton<Scanner>()
                .AddSingleton<FiniteAutomata>()
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
            
            var fa = services.GetRequiredService<FiniteAutomata>();
            fa.LoadData("FA.json");
            Console.WriteLine($"FA is deterministic: {fa.IsDeterministic}");
            Console.WriteLine($"Alphabet: \n{fa.Data.AlphabetString()}");
            Console.WriteLine($"States: \n{fa.Data.StatesString()}");
            Console.WriteLine($"Final States: \n{fa.Data.FinalStatesString()}");
            Console.WriteLine($"Transitions: \n{fa.Data.TransitionsString()}");
            Console.WriteLine($"Check 'qwwq': \n{fa.Check("qwwq")}");
            Console.WriteLine($"Check 'qwe': \n{fa.Check("qwe")}");
        }
    }
}