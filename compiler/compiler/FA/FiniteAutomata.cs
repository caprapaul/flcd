using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace compiler.FA
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
            Data = JsonConvert.DeserializeObject<FiniteAutomataData>(json);
        }

        public bool Check(string sequence)
        {
            List<char> chars = sequence.ToList();
            string currentState = Data.InitialState;

            foreach (char c in sequence)
            {
                if (!Data.Transitions.ContainsKey(currentState))
                {
                    return Data.FinalStates.Contains(currentState) && chars.Count == 0;
                }

                string nextState = Data.Transitions[currentState]
                    .Where(transition => transition.Label == c.ToString())
                    .Select(transition => transition.ToState)
                    .FirstOrDefault();

                if (nextState == null)
                {
                    return Data.FinalStates.Contains(currentState) && chars.Count == 0;
                }

                chars.RemoveAt(0);
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


            return transitions
                .Select(transition => transition.ToState)
                .FirstOrDefault();
        }
    }
}
