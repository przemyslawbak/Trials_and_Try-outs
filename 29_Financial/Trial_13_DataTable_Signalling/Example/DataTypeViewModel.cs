namespace Example
{
    internal class DataTypeViewModel
    {
        public string DataType { get; set; }
        public string DataValue { get; set; }
        public bool IsSelected { get; set; }
        public int Interval { get; set; }
        public int Multiplier { get; set; }
        public int[] MultipliersArray { get; set; } = new int[] { 1, -1 };
        public int[] IntervalsArray { get; set; } = new int[] { 2, 3 };
    }
}
