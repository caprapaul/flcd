using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace compiler
{
    public class GrammarData
    {
        public HashSet<string> NonTerminals { get; set; }
        public HashSet<string> Terminals { get; set; }
        public string StartingSymbol { get; set; }
        [JsonProperty("ProductionRules")] 
        private Dictionary<string, List<string>> _productionRules;
        [JsonIgnore]
        public Dictionary<List<string>, List<List<string>>> ProductionRules { get; set; }

        public void Init()
        {
            ProductionRules = new Dictionary<List<string>, List<List<string>>>();

            foreach ((string key, List<string> rules) in _productionRules)
            {
                var newKey = new List<string>(key.Split(" "));
                List<List<string>> newRules = rules.Select(ruleString => new List<string>(ruleString.Split(" "))).ToList();

                ProductionRules[newKey] = newRules;
            }
        }

        public override string ToString()
        {
            string nonTerminals = string.Join(" ", NonTerminals);
            string terminals = string.Join(" ", Terminals);
            string startingSymbol = StartingSymbol;
            string productionRules = ProductionRules.Select(entry =>
            {
                (List<string> key, List<List<string>> rules) = entry;
                string productions = rules.Select(r => string.Join(" ", r)).Aggregate((p1, p2) => $"{p1} | {p2}");
                return $"{string.Join(" ", key)} -> {productions}";
            }).Aggregate((p1, p2) => p1 + "\n" + p2);

            return $"NonTerminals: {nonTerminals}\n" +
                   $"Terminals: {terminals}\n" +
                   $"Productions:\n{productionRules}";
        }
    }
}
