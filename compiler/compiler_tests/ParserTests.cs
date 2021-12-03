using System.Collections.Generic;
using compiler.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace compiler_tests
{
    [TestClass]
    public class ParserTests
    {
        private Parser _parser;

        [TestInitialize]
        public void Startup()
        {
            var grammar = new Grammar();
            grammar.LoadData("g2.json");
            _parser = new Parser(grammar, new List<string>());
        }
        
        [TestMethod]
        public void TestExpand()
        {
            _parser.Expand();
            (string value, int index) = _parser.WorkingStack.Peek();
            string valueInput = _parser.InputStack.Peek();
            
            Assert.AreEqual("program", value);
            Assert.AreEqual("statement_list", valueInput);
        }
        
        [TestMethod]
        public void TestAdvance()
        {
            _parser.InputStack.Push("while");
            _parser.Advance();
            
            (string value, int index) = _parser.WorkingStack.Peek();
            string valueInput = _parser.InputStack.Peek();
            
            Assert.AreEqual(1, _parser.Position);
            Assert.AreEqual("while", value);
        }
        
        [TestMethod]
        public void TestMomentaryInsuccess()
        {
            _parser.MomentaryInsuccess();
            Assert.AreEqual(ParserState.Back, _parser.State);
        }
        
        [TestMethod]
        public void TestBack()
        {
            _parser.WorkingStack.Push(("while", 0));
            _parser.Position = 1;
            _parser.Back();
            
            string valueInput = _parser.InputStack.Peek();
            
            Assert.AreEqual(0, _parser.Position);
            Assert.AreEqual("while", valueInput);
        }
        
        [TestMethod]
        public void TestAnotherTryExists()
        {
            _parser.WorkingStack.Push(("statement", 0));
            _parser.InputStack.Push("declare_statement");
            _parser.AnotherTry();
            
            (string workingValue, int index) = _parser.WorkingStack.Peek();
            string inputValue = _parser.InputStack.Peek();
            
            Assert.AreEqual("expression_statement", inputValue);
            Assert.AreEqual("statement", workingValue);
            Assert.AreEqual(1, index);
            Assert.AreEqual(ParserState.Normal, _parser.State);
        }
        
        [TestMethod]
        public void TestAnotherTryNotExists()
        {
            _parser.WorkingStack.Push(("statement", 2));
            _parser.InputStack.Push(";");
            _parser.Position = 1;
            _parser.State = ParserState.Back;
            _parser.AnotherTry();
            
            string inputValue = _parser.InputStack.Peek();
            
            Assert.AreEqual("statement", inputValue);
            Assert.AreEqual(ParserState.Back, _parser.State);
        }
        
        [TestMethod]
        public void TestAnotherTryError()
        {
            _parser.WorkingStack.Push(("program", 0));
            _parser.InputStack.Push("statement_list");
            _parser.Position = 0;
            _parser.State = ParserState.Back;
            _parser.AnotherTry();
            
            Assert.AreEqual(ParserState.Error, _parser.State);
        }
        
        [TestMethod]
        public void TestMomentarySuccess()
        {
            _parser.Success();
            Assert.AreEqual(ParserState.Final, _parser.State);
        }
    }
}
