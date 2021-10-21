using System.Collections.Generic;
using System.Linq;

namespace compiler
{
    public class SymbolTable : ISymbolTable
    {
        private const int InitialCapacity = 16;
        
        public int Capacity { get; private set; }
        public int Length { get; private set; }
        
        private List<string>[] _symbols;

        public SymbolTable()
        {
            Length = 0;
            Capacity = InitialCapacity;
            _symbols = new List<string>[Capacity];
        }

        public Position? Add(string symbol)
        {
            if (Length >= Capacity)
            {
                Grow();
            }
            
            Position? position = Add(ref _symbols, symbol);
            Length++;

            return position;
        }

        private Position? Add(ref List<string>[] symbols, string symbol)
        {
            int hash = Hash(symbol) % Capacity;
            symbols[hash] ??= new List<string>();

            if (symbols[hash].Contains(symbol))
            {
                return null;
            }

            symbols[hash].Add(symbol);

            return new Position
            {
                Hash = hash,
                Index = (symbols[hash].Count - 1)
            };
        }

        public Position? FindPosition(string symbol)
        {
            int hash = Hash(symbol) % Capacity;

            var list = _symbols[hash];
            
            if (list == null)
            {
                return null;
            }

            var index = list.FindIndex(s => s.Equals(symbol));

            if (index == -1)
            {
                return null;
            }

            return new Position
            {
                Hash = hash,
                Index = index
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
            
            foreach (var entry in _symbols)
            {
                foreach (var symbol in entry)
                {
                    Add(ref newSymbols, symbol);
                }
            }

            _symbols = newSymbols;
        }
    }
}