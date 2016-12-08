namespace Server.CommHandlers
{
    using System;
    using System.Net.Sockets;
    using Server.CommHandlers.Interfaces;

    using SimpleWars.Comms;
    using SimpleWars.ModelDTOs;
    using SimpleWars.Serialization;

    public class DefaultWriter : Writer
    {
        private readonly AsynchronousSocketClient client;

        public DefaultWriter(AsynchronousSocketClient client)
        {
            this.client = client;
        }

        public void Send(Message message)
        {
            Tuple<byte[], int> data = null;
            try
            {
                data = SerManager.SerializeToManagedBufferPrefixed(message, this.client.Buffers);

                this.client.Socket.BeginSend(data.Item1, 0, data.Item2, SocketFlags.None, this.SendCallback, data.Item1);
            }
            catch
            {
                this.client.Buffers.Return(data?.Item1);
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            byte[] buffer = (byte[])result.AsyncState;
            
            try
            {
                this.client.Socket.EndSend(result);
                this.client.Buffers.Return(buffer);                
            }
            catch
            {
                this.client.Buffers.Return(buffer);
            }
        }
    }
}