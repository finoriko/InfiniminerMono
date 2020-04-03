using Microsoft.Xna.Framework;

namespace InfiniminerMono.States
{
    public class ClickRegion
    {
        public Rectangle Rectangle;
        public string Tag;

        public ClickRegion(Rectangle rectangle, string tag)
        {
            Rectangle = rectangle;
            Tag = tag;
        }

        /// <summary>
        /// Returns the tag, if any, of the region that contains point.
        /// </summary>
        public static string HitTest(ClickRegion[] regionList, Point point)
        {
            foreach (ClickRegion r in regionList)
            {
                if (r.Rectangle.Contains(point))
                    return r.Tag;
            }
            return null;
        }
    }
}