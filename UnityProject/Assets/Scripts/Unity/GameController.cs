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
        [Range(LogicConstants.MinLevelWidth, LogicConstants.MaxLevelWidth)]
        [SerializeField] int levelWidth = 5;
        [Range(LogicConstants.MinLevelHeight, LogicConstants.MaxLevelHeight)]
        [SerializeField] int levelHeight = 5;
        [Range(LogicConstants.MinColorsType, LogicConstants.MaxColorsType)]
        [SerializeField] int levelNumberOfColors = 3;

        [SerializeField] Transform levelPanelTransform;
        private GridLayoutGroup levelGridLayout;
        private TileDummy[] unityLevelTiles;
        private bool isUpdatingLevel;

        private LogicController logicController;
        private int randomSeed;
        private RemoteProtocol remoteProtocol;
        Queue<Command> receivedCommands = new Queue<Command>();
        private bool hasRemoteCommands => receivedCommands.Count > 0;

        void Start()
        {
            logicController = new LogicController(OnGameStart, OnLevelUpdate);
            randomSeed = DateTime.Now.Millisecond;
            var startCmd = new StartGameCommand(levelWidth, levelHeight, levelNumberOfColors, randomSeed, logicController);
            logicController.ExecuteCommand(startCmd);
            ConnectToRemote();
        }

        private void ConnectToRemote()
        {
            remoteProtocol = new RemoteProtocol(logicController, OnRemoteReceiveCommand);
            remoteProtocol.ConnectAsClient(OnRemoteConnected);
        }

        private void OnRemoteConnected()
        {
            for (int i = 0; i < logicController.ExecutedCommands.Count; i++)
            {
                remoteProtocol.SendCommand(logicController.ExecutedCommands[i]);
            }
        }

        private void OnRemoteReceiveCommand(Command cmd)
        {
            receivedCommands.Enqueue(cmd);
        }

        private void Update()
        {
            while (hasRemoteCommands)
            {
                logicController.ExecuteCommand(receivedCommands.Dequeue());
            }
        }

        private void InitUnityLevel()
        {
            levelGridLayout = levelPanelTransform.GetComponent<GridLayoutGroup>();
            levelGridLayout.constraintCount = logicController.Level.Width;
            float scaleFactor = levelGridLayout.constraintCount / 5.5f;
            if (scaleFactor < 1)
            {
                scaleFactor = 1;
            }
            levelGridLayout.cellSize /= scaleFactor;

            GameObject tileTemplate = levelPanelTransform.GetChild(0).gameObject;
            unityLevelTiles = new TileDummy[logicController.Level.Tiles.Length];
            for (int i = 0; i < unityLevelTiles.Length; i++)
            {
                if (i == 0)
                {
                    unityLevelTiles[0] = tileTemplate.GetComponent<TileDummy>();
                }
                else
                {
                    unityLevelTiles[i] = Instantiate(tileTemplate, levelPanelTransform)
                        .GetComponent<TileDummy>();
                }
                unityLevelTiles[i].Initialize(
                    i,
                    TileDummy.ConvertLogicColorToUnityColor(logicController.Level.Tiles[i].Color),
                    OnTileClick);
            }
        }

        private void UpdateUnityLevel(ColorMatchTiles matchTiles)
        {
            StartCoroutine(UpdateUnityLevelIE(matchTiles));
        }

        private IEnumerator UpdateUnityLevelIE(ColorMatchTiles matchTiles)
        {
            isUpdatingLevel = true;
            Tile[] poppedTiles = matchTiles.TilesArray;
            for (int i = 0; i < poppedTiles.Length; i++)
            {
                unityLevelTiles[poppedTiles[i].Index].SetColor(Color.black);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = unityLevelTiles.Length - 1; i >= 0; i--)
            {
                unityLevelTiles[i].SetColor(
                    TileDummy.ConvertLogicColorToUnityColor(logicController.Level.Tiles[i].Color));
            }
            yield return new WaitForSeconds(0.2f);
            isUpdatingLevel = false;
        }

        private void OnTileClick(int tileIndex)
        {
            if (isUpdatingLevel || hasRemoteCommands)
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
            InitUnityLevel();
        }

        private void OnLevelUpdate(ColorMatchTiles colorMatch)
        {
            if (colorMatch != null)
            {
                UpdateUnityLevel(colorMatch);
            }
        }


        private void OnDestroy()
        {
            remoteProtocol.Close();
        }
    }
}