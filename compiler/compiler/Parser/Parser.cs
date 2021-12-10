using System;
using System.Collections.Generic;
using System.Linq;

namespace compiler.Parser
{
    public class Parser
    {
        private readonly Grammar _grammar;
        private ParserState _state = ParserState.Normal;
        private int _position = 0;
        private readonly Stack<(string value, int index)> _workingStack = new();
        private readonly Stack<string> _inputStack = new();
        
        private int _errorPosition = 0;
        private string _errorSymbol = "";

        public Parser(Grammar grammar)
        {
            _grammar = grammar;
            _inputStack.Push(grammar.Data.StartingSymbol);
        }

        public Stack<(string value, int index)> WorkingStack => _workingStack;

        public Stack<string> InputStack => _inputStack;
        
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

        public ParserOutput Run(List<string> input)
        {
            while (_state != ParserState.Final && _state != ParserState.Error)
            {
                switch (_state)
                {
                    case ParserState.Normal:
                        HandleNormal(input);
                        break;
                    case ParserState.Back:
                        HandleBack();
                        break;
                    case ParserState.Final:
                        break;
                    case ParserState.Error:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (_state == ParserState.Error)
            {
                throw new ApplicationException("Parsing error.");
            }

            return new ParserOutput(_workingStack, _grammar);
        }
        
        private void HandleNormal(IReadOnlyList<string> input)
        {
            if (_position == input.Count 
                && (_inputStack.Count == 0))
            {
                Success();
                return;
            }

            if (_inputStack.Count == 0)
            {
                MomentaryInsuccess();
                return;
            }
            
            string symbol = _inputStack.Peek();
            
            if (symbol == "E")
            {
                _inputStack.Pop();
                return;
            }

            if (_grammar.CheckIfTerminal(symbol))
            {
                if (_position < input.Count && symbol == input[_position])
                {
                    Advance();
                }
                else
                {
                    MomentaryInsuccess();
                }
            }
            else
            {
                Expand();
            }
        }
        
        private void HandleBack()
        {
            (string lastSymbol, int index) = _workingStack.Peek();

            if (_grammar.CheckIfTerminal(lastSymbol))
            {
                Back();
            }
            else
            {
                AnotherTry();
            }
        }

        public void Expand()
        {
            string nonTerminal = _inputStack.Pop();
            List<string> production = _grammar.GetProductions(nonTerminal)[0];

            for (int i = production.Count - 1; i >= 0; i--)
            {
                _inputStack.Push(production[i]);
            }
            
            _workingStack.Push((nonTerminal, 0));
        }
        
        public void Advance()
        {
            string terminal = _inputStack.Pop();

            _position++;
            _workingStack.Push((terminal, -1));
        }
        
        public void MomentaryInsuccess()
        {
            // _errorPosition = _position;
            // _workingStack.TryPeek(out (string value, int index) entry);
            // _errorSymbol = entry.value;
            
            _state = ParserState.Back;
        }
        
        public void Back()
        {
            (string terminal, _) = _workingStack.Pop();
            
            _inputStack.Push(terminal);
            _position--;
        }

        public void AnotherTry()
        {
            (string nonTerminal, int index) = _workingStack.Pop();

            List<List<string>> productions = _grammar.GetProductions(nonTerminal);
            List<string> production = productions[index];
            
            for (int i = 0; i < production.Count; i++)
            {
                if (production[i] == "E")
                {
                    continue;
                }
                
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
                
                _workingStack.Push((nonTerminal, index));
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
