using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rovio.TapMatch.Logic;
using System;
using UnityEngine.UI;

namespace Rovio.TapMatch.Unity
{
    public class GameController : MonoBehaviour
    {
        private const int LevelWidth = 4;
        private const int LevelHeight = 5;
        private const int LevelNumberOfColors = 4;

        [SerializeField] Transform levelPanelTransform;
        private GridLayoutGroup levelGridLayout;
        private TileDummy[] unityLevelTiles;
        private bool isUpdatingLevel;

        private LogicController logicController;
        private int randomSeed;

        void Start()
        {
            randomSeed = DateTime.Now.Millisecond;
            logicController = new LogicController(LevelWidth, LevelHeight, LevelNumberOfColors, randomSeed);
            InitUnityLevel();
        }

        private void InitUnityLevel()
        {
            levelGridLayout = levelPanelTransform.GetComponent<GridLayoutGroup>();
            levelGridLayout.constraintCount = LevelWidth;
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
            for (int i = unityLevelTiles.Length-1; i >=0 ; i--)
            {
                unityLevelTiles[i].SetColor(
                    TileDummy.ConvertLogicColorToUnityColor(logicController.Level.Tiles[i].Color));
            }
            yield return new WaitForSeconds(0.2f);
            isUpdatingLevel = false;
        }

        private void OnTileClick(int tileIndex)
        {
            if (isUpdatingLevel)
            {
                return;
            }
            var popCmd = new PopTileCommand(tileIndex, logicController);
            bool isExecuted = logicController.ExecuteCommand(popCmd);
            if (isExecuted)
            {
                UpdateUnityLevel(popCmd.ColorMatchTiles);
            }
            else
            {
                
            }
        }
    }
}