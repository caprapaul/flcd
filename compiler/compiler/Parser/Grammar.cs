using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace compiler.Parser
{
    public class Grammar
    {
        public GrammarData Data { get; private set; }

        public bool IsContextFree
        {
            get
            {
                return Data.ProductionRules.All(rule => rule.Key.Count == 1 && Data.NonTerminals.Contains(rule.Key[0]));
            }
        }

        public void LoadData(string filePath)
        {
            using var file = new StreamReader(filePath);
            string json = file.ReadToEnd();
            Data = JsonConvert.DeserializeObject<GrammarData>(json);
            Data?.Init();
        }

        public List<List<string>> GetProductions(string nonTerminal)
        {
            return Data.ProductionRules.Where(rule => rule.Key.Contains(nonTerminal)).Select(rule => rule.Productions)
                .Aggregate((p1, p2) => p1.Union(p2).ToList()).ToList();
        }
    }
}
