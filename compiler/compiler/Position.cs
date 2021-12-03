namespace compiler
{
    public struct Position
    {
        public static Position None => new()
        {
            Hash = -1,
            Index = -1
        };

        public int Hash { get; set; }
        public int Index { get; set; }

        public override string ToString()
        {
            return $"({Hash}, {Index})";
        }
    }
}
