using Rovio.TapMatch.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Rovio.TapMatch.Remote
{
    public class RemoteProtocol
    {
        private const string PipeServerName = "TapMatchServerPipe";
        private const string PipeClientName = "TapMatchClientPipe";

        public enum ConnectionType { Pipe, Socket }

        private ConnectionType connectionType;
        private StreamString stringStreamerSend;
        private StreamString stringStreamerReceive;
        private NamedPipeServerStream serverPipe;
        private NamedPipeClientStream clientPipe;
        private TcpClient tcpServer;
        private TcpClient tcpClient;

        private Action<Command> onReceiveData;
        public bool IsConnected { get; private set; }

        private LogicController logicController;

        public RemoteProtocol(ConnectionType connectionType, LogicController logicController, Action<Command> onReceiveData)
        {
            this.connectionType = connectionType;
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
            bool isSendConnected = false;
            bool isReceiveConnected = false;
            switch (connectionType)
            {
                case ConnectionType.Pipe:
                    StartSendPipe(PipeServerName,
                        () =>
                        {
                            isSendConnected = true;
                        });
                    StartRecievePipe(PipeClientName,
                        () =>
                        {
                            isReceiveConnected = true;
                        });
                    break;
                case ConnectionType.Socket:
                    isReceiveConnected = true;
                    StartServerSocket(() =>
                        {
                            isSendConnected = true;
                        });
                    break;
            }
            while (!isReceiveConnected || !isSendConnected)
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

            bool isSendConnected = false;
            bool isReceiveConnected = false;
            switch (connectionType)
            {
                case ConnectionType.Pipe:
                    StartSendPipe(PipeClientName,
                        () =>
                        {
                            isSendConnected = true;
                        });
                    StartRecievePipe(PipeServerName,
                        () =>
                        {
                            isReceiveConnected = true;
                        });
                    break;
                case ConnectionType.Socket:
                    isSendConnected = true;
                    StartClientSocket(() =>
                        {
                            isReceiveConnected = true;
                        });
                    break;
            }


            while (!isReceiveConnected || !isSendConnected)
            {
                await Task.Delay(100);
            }
            IsConnected = true;
            Recieving();
            onConnect?.Invoke();
        }

        private void StartServerSocket(Action onConnected)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    var server = new TcpListener(System.Net.IPAddress.Loopback, 2020);
                    server.Start();
                    tcpServer = await server.AcceptTcpClientAsync();
                    stringStreamerReceive = stringStreamerSend = new StreamString(tcpServer.GetStream());

                    onConnected?.Invoke();
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError(e.Message);
#else
                    System.Diagnostics.Debug.WriteLine(e.Message);
#endif                
                    Close();
                }

            });
        }

        private void StartClientSocket(Action onConnected)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    tcpClient = new TcpClient();
                    while (!tcpClient.Connected)
                    {
                        try
                        {
                            await tcpClient.ConnectAsync(System.Net.IPAddress.Loopback, 2020);
                        }
                        catch { }
                    }
                    stringStreamerSend = stringStreamerReceive = new StreamString(tcpClient.GetStream());

                    onConnected?.Invoke();
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError(e.Message);
#else
                    System.Diagnostics.Debug.WriteLine(e.Message);
#endif
                    Close();
                }
            });
        }

        private void StartSendPipe(string pipeName, Action onConnected)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    serverPipe = new NamedPipeServerStream(pipeName, PipeDirection.Out, 1);
                    await serverPipe.WaitForConnectionAsync();
                    stringStreamerSend = new StreamString(serverPipe);

                    onConnected?.Invoke();
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError(e.Message);
#else
                    System.Diagnostics.Debug.WriteLine(e.Message);
#endif
                    Close();
                }
            });
        }

        private void StartRecievePipe(string pipName, Action onConnected)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    clientPipe = new NamedPipeClientStream(".", pipName, PipeDirection.In);
                    await clientPipe.ConnectAsync();
                    stringStreamerReceive = new StreamString(clientPipe);

                    onConnected?.Invoke();
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError(e.Message);
#else
                    System.Diagnostics.Debug.WriteLine(e.Message);
#endif
                    Close();
                }
            });
        }

        private async void Recieving()
        {
            await Task.Run(async () =>
            {
                while (IsConnected)
                {
                    try
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
                    catch (IOException)
                    {
                        Close();
#if UNITY_EDITOR
                        UnityEngine.Debug.LogWarning("Connection Closed by Force.");
#else
                        System.Diagnostics.Debug.WriteLine("Connection Closed by Force.");
#endif
                    }
                    catch (Exception e)
                    {
#if UNITY_EDITOR
                        UnityEngine.Debug.LogError(e.Message);
#else
                        System.Diagnostics.Debug.WriteLine(e.Message);
#endif
                    }
                }
            });
        }

        public void SendCommand(Command cmd)
        {
            if (!IsConnected)
                return;
            try
            {
                stringStreamerSend.WriteString(cmd.Serialize());
            }
            catch { }
        }

        private void Close()
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("Connection Closed.");
#else
            System.Diagnostics.Debug.WriteLine("Connection Closed.");
#endif
            IsConnected = false;

            if (serverPipe != null)
            {
                serverPipe.Close();
                serverPipe = null;
            }
            if (clientPipe != null)
            {
                clientPipe.Close();
                clientPipe = null;
            }

            if (tcpServer != null)
            {
                tcpServer.Close();
                tcpServer = null;
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null;
            }

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
