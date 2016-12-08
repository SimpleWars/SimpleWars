namespace SimpleWars.Comms
{
    using System;

    using SimpleWars.Utils;

    public class MessageReader : IDisposable
    {
        private readonly Buffers buffers;

        public MessageReader(int bytesToRead, Buffers buffers)
        {
            this.buffers = buffers;
            this.DataBuffer = this.buffers.Take(bytesToRead);
        }

        private bool Disposed { get; set; }

        public byte[] DataBuffer { get; }

        public void Dispose()
        {
            if (this.Disposed) return;

            this.buffers.Return(this.DataBuffer);
            this.Disposed = true;
        }
    }
}