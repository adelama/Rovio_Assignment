using Rovio.TapMatch.Logic;
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
    public class RemoteProtocol
    {
        private const string PipeServerName = "TapMatchServerPipe";
        private const string PipeClientName = "TapMatchClientPipe";

        private StreamString stringStreamerSend;
        private StreamString stringStreamerReceive;
        private Action<Command> onReceiveData;
        public bool IsConnected { get; private set; }

        private LogicController logicController;

        public RemoteProtocol(LogicController logicController, Action<Command> onReceiveData)
        {
            this.logicController = logicController;
            this.onReceiveData = onReceiveData;
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                Close();
            }
        }

        public async void ConnectAsServer(Action onConnect = null)
        {
            if (IsConnected)
            {
                Close();
            }
            bool isSendPipeConnected = false;
            bool isReceivePipeConnected = false;
            StartSendPipe(PipeServerName,
                () =>
                {
                    isSendPipeConnected = true;
                });
            StartRecievePipe(PipeClientName,
                () =>
                {
                    isReceivePipeConnected = true;
                });
            while (!isReceivePipeConnected || !isSendPipeConnected)
            {
                await Task.Delay(100);
            }
            IsConnected = true;
            Recieving();
            onConnect?.Invoke();
        }

        public async void ConnectAsClient(Action onConnect = null)
        {
            if (IsConnected)
            {
                Close();
            }
            bool isSendPipeConnected = false;
            bool isReceivePipeConnected = false;
            StartSendPipe(PipeClientName,
                () =>
                {
                    isSendPipeConnected = true;
                });
            StartRecievePipe(PipeServerName,
                () =>
                {
                    isReceivePipeConnected = true;
                });
            while (!isReceivePipeConnected || !isSendPipeConnected)
            {
                await Task.Delay(100);
            }
            IsConnected = true;
            Recieving();
            onConnect?.Invoke();
        }

        private void StartSendPipe(string pipeName, Action onConnected)
        {
            _ = Task.Run(async () =>
            {
                var pipe = new NamedPipeServerStream(pipeName, PipeDirection.Out, 1);
                stringStreamerSend = new StreamString(pipe);
                await pipe.WaitForConnectionAsync();
                onConnected?.Invoke();
            });
        }

        private void StartRecievePipe(string pipName, Action onConnected)
        {
            _ = Task.Run(async () =>
            {
                var pipe = new NamedPipeClientStream(".", pipName, PipeDirection.In);
                stringStreamerReceive = new StreamString(pipe);
                await pipe.ConnectAsync();
                onConnected?.Invoke();
            });
        }

        private async void Recieving()
        {
            await Task.Run(async () =>
            {
                using (stringStreamerReceive)
                {
                    while (IsConnected)
                    {
                        string data = await stringStreamerReceive.ReadString();
                        if (string.IsNullOrEmpty(data))
                        {
                            continue;
                        }
                        Command cmd = Command.Deserialize(data, logicController);
                        if (cmd == null)
                        {
                            continue;
                        }
                        onReceiveData?.Invoke(cmd);
                    }
                }
            });
        }

        public void SendCommand(Command cmd)
        {
            if (!IsConnected)
                return;
            using (stringStreamerSend)
            {
                stringStreamerSend.WriteString(cmd.Serialize());
            }
        }

        private void Close()
        {
            IsConnected = false;
            if (stringStreamerSend != null)
            {
                stringStreamerSend.Close();
                stringStreamerSend = null;
            }
            if (stringStreamerReceive != null)
            {
                stringStreamerReceive.Close();
                stringStreamerReceive = null;
            }
        }
    }
}
