using System;
using System.IO;
using System.Linq;
using compiler.FA;
using compiler.Parser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace compiler
{
    internal class Program
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
                .AddTransient<Scanner.Scanner>()
                .AddTransient<FiniteAutomata>()
                .AddSingleton<Parser.Grammar>()
                .AddTransient<Parser.Parser>()
                .BuildServiceProvider();

            var scanner = services.GetRequiredService<Scanner.Scanner>();
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

            // Grammar(services);
            Parse(services);
        }

        private static void FA(ServiceProvider services)
        {
            var fa = services.GetRequiredService<FiniteAutomata>();
            fa.LoadData("FA.json");
            Console.WriteLine($"FA is deterministic: {fa.IsDeterministic}");
            Console.WriteLine($"Alphabet: \n{fa.Data.AlphabetString()}");
            Console.WriteLine($"States: \n{fa.Data.StatesString()}");
            Console.WriteLine($"Final States: \n{fa.Data.FinalStatesString()}");
            Console.WriteLine($"Initial State: \n{fa.Data.InitialState}");
            Console.WriteLine($"Transitions: \n{fa.Data.TransitionsString()}");
            Console.WriteLine($"Check '1011000': \n{fa.Check("1011000")}");
            Console.WriteLine($"Check '1011': \n{fa.Check("1011")}");
            Console.WriteLine($"Check '1010': \n{fa.Check("1010")}");
            Console.WriteLine($"Check '0': \n{fa.Check("0")}");
            Console.WriteLine($"Check '1': \n{fa.Check("1")}");
            Console.WriteLine($"Check '': \n{fa.Check("")}");
            Console.WriteLine($"Check '02': \n{fa.Check("02")}");
        }

        private static void Grammar(ServiceProvider services)
        {
            var grammar = services.GetRequiredService<Parser.Grammar>();
            grammar.LoadData("g2.json");
            Console.WriteLine($"{grammar.Data}");
            Console.WriteLine($"Is context free: {grammar.IsContextFree}");
            Console.WriteLine(
                $"Productions for 'expression': {grammar.GetProductions("expression").Select(p => string.Join(" ", p)).Aggregate((s1, s2) => $"{s1} | {s2}")}");
            Console.WriteLine(
                $"Productions for 'while_expression': {grammar.GetProductions("while_expression").Select(p => string.Join(" ", p)).Aggregate((s1, s2) => $"{s1} | {s2}")}");
        }

        private static void Parse(ServiceProvider services)
        {
            var grammar = services.GetRequiredService<Parser.Grammar>();
            var pif = services.GetRequiredService<ProgramInternalForm>();

            try
            {
                grammar.LoadData("g1.json");
                var parser = services.GetRequiredService<Parser.Parser>();
                var input = "( a + a ) * ( a + a )";
                
                Console.WriteLine($"Parsing '{input}' with grammar 'g1.json'...");
                ParserOutput output = parser.Run(input.Split(" ").ToList());
                Console.WriteLine("Done");
                Console.WriteLine(output);
                using var writer = new StreamWriter("parser_out1.txt");
                writer.Write(output.ToString());
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
            
            try
            {
                grammar.LoadData("g2.json");
                Console.WriteLine(grammar.Data);
                var parser = services.GetRequiredService<Parser.Parser>();
                
                Console.WriteLine("Parsing 'p1.txt' with grammar 'g2.json'...");
                ParserOutput output = parser.Run(pif.Items.Select((entry) => entry.Item1).ToList());
                Console.WriteLine("Done");
                
                using var writer = new StreamWriter("parser_out2.txt");

                writer.Write(output.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
