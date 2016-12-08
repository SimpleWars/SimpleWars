namespace Server.CommHandlers
{
    using System;
    using System.Net.Sockets;
    using Server.CommHandlers.Interfaces;

    using SimpleWars.Comms;
    using SimpleWars.ModelDTOs;
    using SimpleWars.Serialization;

    public class DefaultReader : Reader
    {
        private readonly AsynchronousSocketClient client;

        public DefaultReader(AsynchronousSocketClient client)
        {
            this.client = client;
        }

        public void ReadSingleMessage()
        {
            this.ReadLengthPrefix(false);
        }

        public void ReadMessagesContinuously()
        {
            this.ReadLengthPrefix(true);
        }

        private void ReadMessage(int messageLength, bool continuous)
        {
            MessageReader packetAssembler = new MessageReader(messageLength, this.client.Buffers);

            try
            {
                this.client.Socket.BeginReceive(
                    packetAssembler.DataBuffer,
                    0,
                    messageLength,
                    SocketFlags.None, this.MessageReceivedCallback,
                    Tuple.Create(packetAssembler, continuous));
            }
            catch
            {
                packetAssembler.Dispose();
            }
        }

        private void MessageReceivedCallback(IAsyncResult result)
        {
            Tuple<MessageReader, bool> state =
                result.AsyncState as Tuple<MessageReader, bool>;

            MessageReader packetAssembler = state.Item1;
            bool listenForNextMessage = state.Item2;

            try
            {
                int bytesReceived = this.client.Socket.EndReceive(result);

                if (bytesReceived > 0)
                {
                    Message message =
                        SerManager.Deserialize<Message>(packetAssembler.DataBuffer);

                    packetAssembler.Dispose();

                    if (listenForNextMessage)
                    {
                        this.ReadLengthPrefix(listenForNextMessage);
                    }
                }
            }
            catch
            {
                packetAssembler.Dispose();
            }
        }

        private void ReadLengthPrefix(bool continuous)
        {
            PrefixReader prefixReader = new PrefixReader(this.client.Buffers);

            try
            {
                this.client.Socket.BeginReceive(
                    prefixReader.Buffer,
                    0,
                    PrefixReader.PrefixBytes,
                    SocketFlags.None, this.LengthPrefixReceivedCallback,
                    Tuple.Create(prefixReader, continuous));
            }
            catch
            {
                prefixReader.Dispose();
                this.ReadMessagesContinuously();
            }
        }

        private void LengthPrefixReceivedCallback(IAsyncResult result)
        {
            Tuple<PrefixReader, bool> state = (Tuple<PrefixReader, bool>)result.AsyncState;

            try
            {
                int bytesRead = this.client.Socket.EndReceive(result);
                state.Item1.PushReceivedData(bytesRead);

                if (state.Item1.BytesToRead == 0)
                {
                    int messageLength = SerManager.GetLengthPrefix(state.Item1.PrefixData) - PrefixReader.PrefixBytes;
                    state.Item1.Dispose();
                    this.ReadMessage(messageLength, state.Item2);
                }
                else
                {
                    this.ContinueReadingLengthPrefix(state);
                }
            }
            catch
            {
                state.Item1.Dispose();
                this.ReadMessagesContinuously();
            }
        }

        private void ContinueReadingLengthPrefix(Tuple<PrefixReader, bool> state)
        {
            try
            {
                this.client.Socket.BeginReceive(
                    state.Item1.Buffer,
                    0,
                    state.Item1.BytesToRead,
                    SocketFlags.None, this.LengthPrefixReceivedCallback,
                    state);
            }
            catch
            {
                state.Item1.Dispose();
                this.ReadMessagesContinuously();
            }
        }
    }
}