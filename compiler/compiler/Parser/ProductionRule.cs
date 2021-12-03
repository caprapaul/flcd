using System.Collections.Generic;

namespace compiler.Grammar
{
    public class ProductionRule
    {
        public List<string> Key { get; set; }
        public List<List<string>> Productions { get; set; }
    }
}
