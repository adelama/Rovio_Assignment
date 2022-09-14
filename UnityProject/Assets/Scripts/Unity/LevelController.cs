using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rovio.TapMatch.Logic;
using UnityEngine.UI;
using System;

namespace Rovio.TapMatch.Unity
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] Transform levelPanelTransform;
        private TileDummy[] unityLevelTiles;
        private bool isUpdatingLevel;
        private LevelGrid logicLevel;

        public Action<int> onTileClicked;

        public void InitUnityLevel(LevelGrid logicLevel)
        {
            this.logicLevel = logicLevel;
            var levelGridLayout = levelPanelTransform.GetComponent<GridLayoutGroup>();
            levelGridLayout.constraintCount = logicLevel.Width;
            float scaleFactor = levelGridLayout.constraintCount / 5.5f;
            if (scaleFactor < 1)
            {
                scaleFactor = 1;
            }
            levelGridLayout.cellSize /= scaleFactor;

            GameObject tileTemplate = levelPanelTransform.GetChild(0).gameObject;
            unityLevelTiles = new TileDummy[logicLevel.Tiles.Length];
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
                    TileDummy.ConvertLogicColorToUnityColor(logicLevel.Tiles[i].Color),
                    OnTileClick);
            }
        }

        public void UpdateUnityLevel(ColorMatchTiles matchTiles)
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
                    TileDummy.ConvertLogicColorToUnityColor(logicLevel.Tiles[i].Color));
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
            onTileClicked?.Invoke(tileIndex);
        }

    }
}