using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace compiler
{
    public class FiniteAutomata
    {
        public FiniteAutomataData Data { get; private set; }
        public bool IsDeterministic
        {
            get
            {
                // Check if all states have unique transition labels
                return Data.Transitions.Values.All(transitions => transitions.Count == transitions
                    .Select(t => t.Label)
                    .Distinct()
                    .Count()
                );
            }
        }

        public void LoadData(string filePath)
        {
            using var file = new StreamReader(filePath);
            string json = file.ReadToEnd();
            Data = JsonSerializer.Deserialize<FiniteAutomataData>(json);
        }

        public bool Check(string sequence)
        {
            List<char> chars = sequence.ToList();
            string currentState = Data.InitialState;

            for (int index = sequence.Length - 1; index >= 0; index--)
            {
                chars.RemoveAt(index);
                char c = sequence[index];
                string nextState = GetNextState(currentState, c.ToString());
                
                if (nextState == null)
                {
                    break;
                }

                currentState = nextState;
            }

            return Data.FinalStates.Contains(currentState);
        }

        private string GetNextState(string currentState, string c)
        {
            if (!Data.Transitions.ContainsKey(currentState))
            {
                return null;
            }
            
            List<Transition> transitions = Data.Transitions[currentState];

            return transitions.Where(transition => transition.Label == c)
                .Select(transition => transition.ToState)
                .FirstOrDefault();
        }
    }
}
