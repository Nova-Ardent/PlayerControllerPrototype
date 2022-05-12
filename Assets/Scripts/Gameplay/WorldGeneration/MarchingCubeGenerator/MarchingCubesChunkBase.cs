namespace WorldGen
{
    public abstract class MarchingCubesChunkBase
    {
        public abstract float this[int x, int y, int z]
        {
            get;
            set;
        }

        public float[] points;
    }
}
