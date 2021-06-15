namespace F1Pitwall.Processor.Models
{
    public abstract class WSCarData
    {
        public string Type { get; set; }
        public uint FrameId { get; set; }
        public int CarId { get; set; }
    }
}
