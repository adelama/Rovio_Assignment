﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
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
            this.remoteForm.onTileClicked += OnTileClicked;
            logicController = new LogicController(OnGameStart, OnLevelUpdate);
            StartServer();
        }

        private void StartServer()
        {
            remoteProtocol = new RemoteProtocol(RemoteProtocol.ConnectionType.Socket,logicController, OnRemoteReceiveCommand);
            remoteProtocol.ConnectAsServer();
        }

        private void OnRemoteReceiveCommand(Command cmd)
        {
            logicController.ExecuteCommand(cmd);
        }

        private void OnTileClicked(int tileIndex)
        {
            var popCmd = new PopTileCommand(tileIndex, logicController);
            bool isExecuted = logicController.ExecuteCommand(popCmd);
            if (isExecuted)
            {
                remoteProtocol.SendCommand(popCmd);
            }
        }


        private void OnGameStart()
        {
            remoteForm.Invoke((MethodInvoker)delegate
            {
                remoteForm.InitializeLevel(logicController.Level);
            });
        }

        private void OnLevelUpdate(ColorMatchTiles colorMatch)
        {
            if (colorMatch != null)
            {
                remoteForm.Invoke((MethodInvoker)delegate
                {
                    remoteForm.UpdateWindowsLevel(colorMatch);
                });
            }
        }
    }
}
