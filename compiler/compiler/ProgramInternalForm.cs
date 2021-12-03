using System.Collections.Generic;
using System.Linq;

namespace compiler
{
    public class ProgramInternalForm
    {
        private readonly List<(string, Position)> _items = new();

        public void Add(string token, Position position)
        {
            _items.Add((token, position));
        }

        public override string ToString()
        {
            return _items.Select(entry => $"{entry.Item2} -> {entry.Item1}")
                .Aggregate((s1, s2) => $"{s1}\n{s2}");
        }
    }
}
