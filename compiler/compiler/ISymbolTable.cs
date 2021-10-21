namespace compiler
{
    public interface ISymbolTable
    {
        Position? Add(string symbol);
        Position? FindPosition(string symbol);
    }
}