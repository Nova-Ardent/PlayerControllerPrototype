namespace WorldGen
{
    public class MarchingCubesZerosChunk : MarchingCubesChunkBase
    {
        public override float this[int x, int y, int z]
        {
            get => 0;
            set { }
        }

        public MarchingCubesZerosChunk()
        {
            points = new float[0];
        }
    }
}
