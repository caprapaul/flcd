using System.Collections.Generic;
using System.Linq;

namespace compiler.Parser
{
    public class ParserOutput
    {
        public struct Node
        {
            public string Info { get; set; }
            public int Parent { get; set; }
            public int RightSibling { get; set; }
        }

        private List<Node> _nodes;

        public ParserOutput(Stack<(string value, int index)> workingStack, Grammar grammar)
        {
            _nodes = new List<Node>(workingStack.Count);
            var newWorkingStack = new Stack<(string symbol, int index)>(workingStack);
            
            
            ConstructTree(newWorkingStack, grammar, -1,-1);
        }

        private void ConstructTree(Stack<(string value, int index)> workingStack, Grammar grammar, int currentParentIndex, int currentSiblingIndex)
        {
            (string symbol, int index) = workingStack.Pop();
            int currentIndex = _nodes.Count;
            _nodes.Add(new Node
            {
                Info = symbol,
                Parent = currentParentIndex,
                RightSibling = currentSiblingIndex
            });

            List<string> production = grammar.GetProductions(symbol)[index];

            int siblingIndex = -1;
            
            for (int i = 0; i < production.Count; i++)
            {
                string productionSymbol = production[i];
                
                if (productionSymbol == "E")
                {
                    continue;
                }
                
                (_, int productionIndex) = workingStack.Peek();

                if (productionIndex == -1)
                {
                    workingStack.Pop();
                    
                    _nodes.Add(new Node
                    {
                        Info = productionSymbol,
                        Parent = currentIndex,
                        RightSibling = siblingIndex
                    });
                    
                    siblingIndex = _nodes.Count - 1;
                }
                else
                {
                    int count = _nodes.Count;
                    ConstructTree(workingStack, grammar, currentIndex, siblingIndex);
                    siblingIndex = count;
                }
            }
        }

        public override string ToString()
        {
            return _nodes.Select((node, index) => $"{index, -8}{node.Info, -40}{node.Parent, -8}{node.RightSibling, -8}").Aggregate((n1, n2) => $"{n1}\n{n2}");
        }
    }
}
