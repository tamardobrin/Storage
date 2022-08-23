

namespace Logic
{
    internal class BoxDateDlete
    {
        public BoxBase BoxBasepProp { get; set; }
        public BoxHeight BoxHeightProp { get; set; }
        public BoxDateDlete(BoxBase BoxBaseX, BoxHeight BoxHeightY)
        {
            BoxBasepProp = BoxBaseX;
            BoxHeightProp = BoxHeightY;
        }
    }
}
