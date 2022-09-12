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

        private LogicController logicController;
        private int randomSeed;
        // Start is called before the first frame update
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

        private void UpdateUnityLevel()
        {
            for (int i = 0; i < unityLevelTiles.Length; i++)
            {
                unityLevelTiles[i].SetColor(
                    TileDummy.ConvertLogicColorToUnityColor(logicController.Level.Tiles[i].Color));
            }
        }

        private void OnTileClick(int tileIndex)
        {
            logicController.ExecuteCommand(new PopTileCommand(tileIndex, logicController));
            UpdateUnityLevel();
        }
    }
}