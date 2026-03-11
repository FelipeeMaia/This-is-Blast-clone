namespace Blast.Data
{
    public class BlockData : IData
    {
        public int healthPoints;
        public ColorData colorData;

        public BlockData(int health, ColorData colorInfo)
        {
            this.healthPoints = health;
            this.colorData = colorInfo;
        }
    }
}