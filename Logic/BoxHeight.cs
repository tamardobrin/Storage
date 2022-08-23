using System;



namespace Logic
{
    internal class BoxHeight : IComparable<BoxHeight>
    {
        public double Y { get; set; }
        public DateTime lastTimeSold { get; set; }
        public DateTime supllyDate { get; set; }
        public int Amount { get; set; }
       
        public int CompareTo(BoxHeight other)
        {
            if (this.Y > other.Y)
                return 1;
            if (this.Y == other.Y)
                return 0;
            return -1;
        }
    }
}
