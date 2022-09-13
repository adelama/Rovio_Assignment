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
        private bool isConnected;

        public RemoteProtocol(Action<Command> onReceiveData)
        {
            this.onReceiveData = onReceiveData;
        }

        public void ConnectAsServer(Action onConnect = null)
        {
            if (isConnected)
            {
                Close();
            }
            bool isSendPipeConnected = false;
            bool isReceivePipeConnected = false;
            StartSendPipe(PipeServerName,
                () =>
                {
                    isSendPipeConnected = true;
                    if (isReceivePipeConnected)
                    {
                        isConnected = true;
                        Recieving();
                        onConnect?.Invoke();
                    }
                });
            StartRecievePipe(PipeClientName,
                () =>
                {
                    isReceivePipeConnected = true;
                    if (isSendPipeConnected)
                    {
                        isConnected = true;
                        Recieving();
                        onConnect?.Invoke();
                    }
                });
        }

        public void ConnectAsClient(Action onConnect = null)
        {
            if (isConnected)
            {
                Close();
            }
            bool isSendPipeConnected = false;
            bool isReceivePipeConnected = false;
            StartSendPipe(PipeClientName,
                () =>
                {
                    isSendPipeConnected = true;
                    if (isReceivePipeConnected)
                    {
                        isConnected = true;
                        Recieving();
                        onConnect?.Invoke();
                    }
                });
            StartRecievePipe(PipeServerName,
                () =>
                {
                    isReceivePipeConnected = true;
                    if (isSendPipeConnected)
                    {
                        isConnected = true;
                        Recieving();
                        onConnect?.Invoke();
                    }
                });
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
                    while (isConnected)
                    {
                        string data = await stringStreamerReceive.ReadString();
                        if (string.IsNullOrEmpty(data))
                        {
                            continue;
                        }
                        Command cmd=null;
                        onReceiveData?.Invoke(cmd);
                    }
                }
            });
        }

        public void SendCommand(Command cmd)
        {
            using (stringStreamerSend)
            {
                stringStreamerSend.WriteString(cmd.Serialize());
            }
        }

        public void Close()
        {
            isConnected = false;
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
