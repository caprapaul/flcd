using System.Collections.Generic;

namespace compiler.Parser
{
    public class Parser
    {
        private readonly Grammar _grammar;
        private ParserState _state = ParserState.Normal;
        private int _position = 0;
        private readonly Stack<(string value, int index)> _workingStack = new();
        private readonly Stack<string> _inputStack = new();
        private readonly List<string> _input;

        public Parser(Grammar grammar, List<string> input)
        {
            _grammar = grammar;
            _input = input;
            _inputStack.Push(grammar.Data.StartingSymbol);
        }

        public Stack<(string value, int index)> WorkingStack => _workingStack;

        public Stack<string> InputStack => _inputStack;

        public List<string> Input => _input;

        public int Position
        {
            get => _position;
            set => _position = value;
        }

        public ParserState State
        {
            get => _state;
            set => _state = value;
        }

        public void Expand()
        {
            string nonTerminal = _inputStack.Pop();
            List<string> production = _grammar.GetProductions(nonTerminal)[0];

            for (int i = production.Count - 1; i >= 0; i--)
            {
                _inputStack.Push(production[i]);
            }
            
            WorkingStack.Push((nonTerminal, 0));
        }
        
        public void Advance()
        {
            string terminal = _inputStack.Pop();

            _position++;
            WorkingStack.Push((terminal, 0));
        }
        
        public void MomentaryInsuccess()
        {
            _state = ParserState.Back;
        }
        
        public void Back()
        {
            (string terminal, _) = WorkingStack.Pop();
            
            _inputStack.Push(terminal);
            _position--;
        }

        public void AnotherTry()
        {
            (string nonTerminal, int index) = WorkingStack.Pop();

            List<List<string>> productions = _grammar.GetProductions(nonTerminal);
            List<string> production = productions[index];
            
            for (int i = 0; i < production.Count; i++)
            {
                _inputStack.Pop();
            }

            if (index + 1 < productions.Count)
            {
                index++;
                production = productions[index];

                for (int i = production.Count - 1; i >= 0; i--)
                {
                    _inputStack.Push(production[i]);
                }
                
                WorkingStack.Push((nonTerminal, index));
                _state = ParserState.Normal;
            }
            else if (_position == 0 && nonTerminal == _grammar.Data.StartingSymbol)
            {
                _state = ParserState.Error;
            }
            else
            {
                _inputStack.Push(nonTerminal);
            }
        }

        public void Success()
        {
            _state = ParserState.Final;
        }
    }
}
