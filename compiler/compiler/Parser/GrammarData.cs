using System.Collections.Generic;
using System.Linq;
using compiler.Grammar;
using Newtonsoft.Json;

namespace compiler.Parser
{
    public class GrammarData
    {
        [JsonProperty("ProductionRules")] private Dictionary<string, List<string>> _productionRules;

        public HashSet<string> NonTerminals { get; set; }
        public HashSet<string> Terminals { get; set; }
        public string StartingSymbol { get; set; }

        [JsonIgnore]
        public List<ProductionRule> ProductionRules { get; set; }

        public void Init()
        {
            ProductionRules = new List<ProductionRule>();

            foreach ((string key, List<string> rules) in _productionRules)
            {
                var newKey = new List<string>(key.Split(" "));
                List<List<string>> newProductions =
                    rules.Select(ruleString => new List<string>(ruleString.Split(" "))).ToList();

                ProductionRules.Add(new ProductionRule
                {
                    Key = newKey,
                    Productions = newProductions
                });
            }
        }

        public override string ToString()
        {
            string nonTerminals = string.Join(" ", NonTerminals);
            string terminals = string.Join(" ", Terminals);
            string startingSymbol = StartingSymbol;
            string productionRules = ProductionRules.Select(rule =>
            {
                string productions = rule.Productions.Select(r => string.Join(" ", r))
                    .Aggregate((p1, p2) => $"{p1} | {p2}");
                return $"{string.Join(" ", rule.Key)} -> {productions}";
            }).Aggregate((p1, p2) => p1 + "\n" + p2);

            return $"NonTerminals: {nonTerminals}\n" +
                   $"Terminals: {terminals}\n" +
                   $"Productions:\n{productionRules}";
        }
    }
}
