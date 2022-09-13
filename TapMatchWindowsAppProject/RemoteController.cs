using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Rovio.TapMatch.Logic;
using Rovio.TapMatch.Remote;

namespace Rovio.TapMatch.WindowsApp
{
    internal class RemoteController
    {
        private RemoteForm remoteForm;
        private LogicController logicController;
        private RemoteProtocol remoteProtocol;

        public RemoteController(RemoteForm remoteForm)
        {
            this.remoteForm = remoteForm;
            logicController = new LogicController(OnGameStart, OnLevelUpdate);
            StartServer();
        }

        private void StartServer()
        {
            remoteProtocol = new RemoteProtocol(logicController, OnRemoteReceiveCommand);
            remoteProtocol.ConnectAsServer();
        }

        private void OnRemoteReceiveCommand(Command cmd)
        {
            logicController.ExecuteCommand(cmd);
        }


        private void OnGameStart()
        {
            remoteForm.InitializeLevel(logicController);
        }

        private void OnLevelUpdate(ColorMatchTiles colorMatch)
        {
            if (colorMatch != null)
            {
                remoteForm.UpdateWindowsLevel(colorMatch);
            }
        }
    }
}
