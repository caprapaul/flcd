namespace lab_02
{
    public struct Position
    {
        public int Hash { get; set; }
        public int Index { get; set; }
        
        public override string ToString()
        {
            return $"{{ Hash: {Hash}, Index: {Index} }}";
        }
    }
}