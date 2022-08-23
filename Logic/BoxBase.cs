using System;
using System.Collections.Generic;



namespace Logic
{
    internal class BoxBase : IComparable<BoxBase>
    {
        public double X { get; set; }
        public LinkedList<BoxHeight> boxHeightslist;
        public BoxBase(double boxLength)
        {
           X = boxLength;
            boxHeightslist = new LinkedList<BoxHeight>();
        }
       

        public int CompareTo(BoxBase other)
        {
            if (X == other.X)
                return 0;
            if (this.X > other.X)
                return 1;
            return -1;
        }
    }
}
