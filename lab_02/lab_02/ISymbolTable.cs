namespace lab_02
{
    public interface ISymbolTable
    {
        Position? Add(string symbol);
        Position? FindPosition(string symbol);
    }
}