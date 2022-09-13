using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Rovio.TapMatch.Remote
{
    internal class RemoteProtocol
    {
        private const string PipeName = "TapMatchPipe";

        private NamedPipeServerStream pipeServer;
        private NamedPipeClientStream pipeClient;
        private StreamString stringStreamer;
        private Action<string> onReceiveData;
        private bool isConnected;

        public RemoteProtocol(Action<string> onReceiveData)
        {
            this.onReceiveData = onReceiveData;
        }

        public async Task StartServer(Action onConnect = null)
        {
            pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 1);
            stringStreamer = new StreamString(pipeServer);
            await pipeServer.WaitForConnectionAsync();
            isConnected = true;
            onConnect?.Invoke();
            StartRecieving();
        }

        public async Task StartClient(Action onConnect = null)
        {
            pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut);
            stringStreamer = new StreamString(pipeClient);
            await pipeClient.ConnectAsync();
            isConnected = true;
            onConnect?.Invoke();
            StartRecieving();
        }

        private async void StartRecieving()
        {
            using (stringStreamer)
            {
                while (isConnected)
                {
                    string data = await stringStreamer.ReadString();
                    onReceiveData?.Invoke(data);
                }
            }
        }

        public void SendData(string data)
        {
            using (stringStreamer)
            {
                stringStreamer.WriteString(data);
            }
        }

        public void Close()
        {
            if (pipeServer != null)
            {
                isConnected = false;
                pipeServer.Close();
            }
        }
    }
}
