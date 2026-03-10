namespace Blast
{
    public class BreakableCubeInfo
    {
        public int healthPoints;
        public ColorInfo colorInfo;

        public BreakableCubeInfo(int health, ColorInfo colorInfo)
        {
            this.healthPoints = health;
            this.colorInfo = colorInfo;
        }
    }
}