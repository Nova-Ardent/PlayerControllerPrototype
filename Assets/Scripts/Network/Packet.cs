
namespace Network
{
    public class Packet
    {
        public const long NetworkFlagLength = 1;

        public enum OPCodes
        {
            Connecting,
            Connected,
            Ping,
        }

        long readIndex;
        byte[] data;

        public Packet(byte[] data)
        {
            this.readIndex = 0;
            this.data = data;
        }

        public Packet(OPCodes code)
        {
            this.data = new byte[]
            {
                (byte)code
            };
        }

        public OPCodes GetOpCode()
        {
            return (OPCodes)this.data[readIndex++];
        }

        public byte[] GetBytes()
        {
            return data;
        }

        public int Size()
        {
            return data.Length;
        }
    }
}
