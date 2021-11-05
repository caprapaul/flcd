using System.Collections.Generic;
using System.Linq;

namespace compiler
{
    public class FiniteAutomataData
    {
        public List<string> States { get; set; }
        public List<string> Alphabet { get; set; }
        public string InitialState { get; set; }
        public List<string> FinalStates { get; set; }
        public Dictionary<string, List<Transition>> Transitions { get; set; }

        public string StatesString()
        {
            return string.Join(",", States);
        }        
        
        public string FinalStatesString()
        {
            return string.Join(",", FinalStates);
        }
        
        public string AlphabetString()
        {
            return string.Join(",", Alphabet);
        }
        
        public string TransitionsString()
        {
            var result = "";

            foreach ((string key, List<Transition> transitions) in Transitions)
            {
                foreach (Transition transition in transitions)
                {
                    result += $"({key}, '{transition.Label}') = {transition.ToState}\n";
                }
            }
            
            return result;
        }
    }
}