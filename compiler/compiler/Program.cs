using System;

namespace compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var symbols = new SymbolTable();
            Console.WriteLine($"Added {symbols.Add("abcd")}");
            Console.WriteLine($"Added {symbols.Add("abcd")}");
            Console.WriteLine($"Added {symbols.Add("dcba")}");
            Console.WriteLine($"Added {symbols.Add("aaaa")}");
            Console.WriteLine($"Found {symbols.FindPosition("abcd")}");
            Console.WriteLine($"Found {symbols.FindPosition("dcba")}");
            Console.WriteLine($"Found {symbols.FindPosition("dcb")}");
            Console.WriteLine($"Found {symbols.FindPosition("aaaa")}");
        }
    }
}