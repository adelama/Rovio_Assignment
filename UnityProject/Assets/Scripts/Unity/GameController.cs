using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rovio.TapMatch.Logic;
using System;
using UnityEngine.UI;
using Rovio.TapMatch.Remote;

namespace Rovio.TapMatch.Unity
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] LevelController levelController;

        private LogicController logicController;
        private int randomSeed;
        private RemoteProtocol remoteProtocol;
        Queue<Command> receivedCommands = new Queue<Command>();
        private bool hasRemoteCommands => receivedCommands.Count > 0;

        void Start()
        {
            if (levelController == null)
            {
                levelController = GetComponent<LevelController>();
            }

            levelController.onTileClicked += OnTileClicked;

            logicController = new LogicController(OnGameStart, OnLevelUpdate);

            ConnectToRemote();
        }

        public void BtnStartClicked()
        {
            StartGame(
                levelController.LevelSettingsPanel.LevelWidth,
                levelController.LevelSettingsPanel.LevelHeight,
                levelController.LevelSettingsPanel.NumberOfColors
                );
        }

        private void StartGame(int levelWidth, int levelHeight, int levelNumberOfColors)
        {
            if (logicController.IsGameStarted)
            {
                return;
            }
            randomSeed = DateTime.Now.Millisecond;
            var startCmd = new StartGameCommand(levelWidth, levelHeight, levelNumberOfColors, randomSeed, logicController);
            logicController.ExecuteCommand(startCmd);
        }

        private void ConnectToRemote()
        {
            remoteProtocol = new RemoteProtocol(RemoteProtocol.ConnectionType.Socket,logicController, OnRemoteReceiveCommand);
            remoteProtocol.ConnectAsClient(OnRemoteConnected);
        }

        private void OnRemoteConnected()
        {
            for (int i = 0; i < logicController.ExecutedCommands.Count; i++)
            {
                remoteProtocol.SendCommand(logicController.ExecutedCommands[i]);
            }
            StartCoroutine(ExecuteRemoteCommandsIE());
        }

        private void OnRemoteReceiveCommand(Command cmd)
        {
            lock (receivedCommands)
            {
                receivedCommands.Enqueue(cmd);
            }
        }

        private IEnumerator ExecuteRemoteCommandsIE()
        {
            while (remoteProtocol.IsConnected)
            {
                while (hasRemoteCommands)
                {
                    Command cmd;
                    lock (receivedCommands)
                    {
                        cmd = receivedCommands.Dequeue();
                    }
                    logicController.ExecuteCommand(cmd);
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnTileClicked(int tileIndex)
        {
            if (hasRemoteCommands)
            {
                return;
            }
            var popCmd = new PopTileCommand(tileIndex, logicController);
            bool isExecuted = logicController.ExecuteCommand(popCmd);
            if (isExecuted)
            {
                remoteProtocol.SendCommand(popCmd);
            }
            else
            {
                //TODO: showing some feedbacks that this action couldn't be executed
            }
        }

        private void OnGameStart()
        {
            levelController.InitUnityLevel(logicController.Level);
        }

        private void OnLevelUpdate(ColorMatchTiles colorMatch)
        {
            if (colorMatch != null)
            {
                levelController.UpdateUnityLevel(colorMatch);
            }
        }


        private void OnDestroy()
        {
            remoteProtocol.Disconnect(true);
        }
    }
}