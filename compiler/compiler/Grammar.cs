using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace compiler
{
    public class Grammar
    {        
        public GrammarData Data { get; private set; }
        
        public bool IsContextFree
        {
            get
            {
                return Data.ProductionRules.Keys.All(key => key.Count == 1 && Data.NonTerminals.Contains(key[0]));
            }
        }

        public void LoadData(string filePath)
        {
            using var file = new StreamReader(filePath);
            string json = file.ReadToEnd();
            Data = JsonConvert.DeserializeObject<GrammarData>(json);
            Data?.Init();
        }
    }
}
