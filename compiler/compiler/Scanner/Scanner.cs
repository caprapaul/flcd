using System;
using System.IO;
using System.Text.RegularExpressions;

namespace compiler.Scanner
{
    public class Scanner
    {
        private const string Separators = @"\[\]{}():; +\-*/%<>|&""";
        private const string Operators = @"+-*/%<>|&";
        private const string Alphabet = @"a-zA-Z0-9_\[\]{}%():; +-*/=<>&|!";

        private static readonly Func<string, (string, string)>[] Patterns =
        {
            // Keywords
            line =>
            {
                string token = Regex.Match(line,
                        $@"^(let|const|if|else|while|i32|bool|char|print|read)(?=[{Separators}]|$)")
                    .Value;
                return (token, token);
            },
            // Int constant
            line =>
            {
                string token = Regex.Match(line, $@"^([0-9]+)(?=[{Separators}]|$)").Value;
                return ("const", token);
            },
            // Composed operators
            line =>
            {
                string token = Regex.Match(line, @"^(<=|==|>=|&&|\|\|)").Value;
                return (token, token);
            },
            // Operators
            line =>
            {
                string token = Regex.Match(line, @"^(=|\+|-|\*|/|<|>|!|%)").Value;
                return (token, token);
            },
            // Separators
            line =>
            {
                string token = Regex.Match(line, @"^(\(|\)|\[|\]|\{|\}|\:|\;)").Value;
                return (token, token);
            },
            // String constant
            line =>
            {
                string token = Regex.Match(line, @"^(""[a-zA-Z0-9 _]*"")").Value;
                return ("const", token);
            },
            // Char constant
            line =>
            {
                string token = Regex.Match(line, @"^('[a-zA-Z0-9 _]')").Value;
                return ("const", token);
            },
            // Bool constant
            line =>
            {
                string token = Regex.Match(line, $@"^(true|false)(?=[{Separators}]|$)").Value;
                return ("const", token);
            },
            // Identifier
            line =>
            {
                string token = Regex.Match(line, $@"^([a-zA-Z_][a-zA-Z0-9_]*)(?=[{Separators}]|$)").Value;
                return ("id", token);
            }
        };

        private readonly ProgramInternalForm _programInternalForm;

        private readonly SymbolTable _symbolTable;

        public Scanner(SymbolTable symbolTable, ProgramInternalForm programInternalForm)
        {
            _symbolTable = symbolTable;
            _programInternalForm = programInternalForm;
        }

        public void Scan(string filePath)
        {
            using var file = new StreamReader(filePath);
            var counter = 0;
            string line;

            while ((line = file.ReadLine()) != null)
            {
                counter++;
                while (!string.IsNullOrEmpty(line))
                {
                    line = line.TrimStart();
                    var forward = 0;
                    var token = "";
                    var tokenType = "";

                    foreach (Func<string, (string, string)> pattern in Patterns)
                    {
                        (tokenType, token) = pattern(line);

                        if (token.Length <= 0)
                        {
                            continue;
                        }

                        //Console.WriteLine(token);
                        forward = token.Length;
                        break;
                    }

                    if (forward == 0)
                    {
                        throw new Exception($"({counter}): '{line}'");
                    }

                    switch (tokenType)
                    {
                        case "const" or "id":
                            Position position = _symbolTable.Position(token);
                            _programInternalForm.Add(tokenType, position);
                            break;

                        default:
                            _programInternalForm.Add(token, Position.None);
                            break;
                    }

                    line = line[forward..];
                }
            }

            file.Close();
        }
    }
}
