using System.Collections.Generic;
using System.Linq;

namespace compiler
{
    public class SymbolTable
    {
        private const int InitialCapacity = 1009;

        private List<string>[] _symbols;

        public SymbolTable()
        {
            Length = 0;
            Capacity = InitialCapacity;
            _symbols = new List<string>[Capacity];
        }

        public int Capacity { get; private set; }
        public int Length { get; private set; }

        public Position Position(string symbol)
        {
            // if (Length >= Capacity)
            // {
            //     Grow();
            // }

            Position position = Add(ref _symbols, symbol);
            Length++;

            return position;
        }

        private Position Add(ref List<string>[] symbols, string symbol)
        {
            int hash = Hash(symbol) % Capacity;
            symbols[hash] ??= new List<string>();
            List<string> list = symbols[hash];

            int index = list.FindIndex(s => s.Equals(symbol));

            if (index != -1)
            {
                return new Position
                {
                    Hash = hash,
                    Index = index
                };
            }

            symbols[hash].Add(symbol);

            return new Position
            {
                Hash = hash,
                Index = symbols[hash].Count - 1
            };
        }

        private int Hash(string symbol)
        {
            return symbol.Select(c => (int) c).Sum();
        }

        private void Grow()
        {
            Capacity *= 2;
            var newSymbols = new List<string>[Capacity];

            foreach (List<string> entry in _symbols)
            {
                foreach (string symbol in entry)
                {
                    Add(ref newSymbols, symbol);
                }
            }

            _symbols = newSymbols;
        }

        public override string ToString()
        {
            return _symbols.Select((tokens, index) =>
                {
                    if (tokens == null)
                    {
                        return "";
                    }

                    string values = string.Join(",", tokens);
                    return $"{index}: [{values}]";
                })
                .Where(s => !string.IsNullOrEmpty(s))
                .Aggregate((s1, s2) => $"{s1}\n{s2}");
            ;
        }
    }
}
