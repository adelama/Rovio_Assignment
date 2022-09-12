using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rovio.TapMatch.Unity
{
    public class TileDummy : MonoBehaviour, IPointerClickHandler
    {
        private Image image;

        public int Index { get; private set; }

        System.Action<int> onClick;

        // Start is called before the first frame update
        public void Initialize(int tileIndex, Color color, System.Action<int> onClick)
        {
            Index = tileIndex;
            image = GetComponent<Image>();
            image.color = color;
            this.onClick = onClick;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onClick?.Invoke(Index);
            }
        }

        public static Color ConvertLogicColorToUnityColor(Rovio.TapMatch.Logic.LogicConstants.TileColor logicColor)
        {
            switch (logicColor)
            {
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Red:
                    return Color.red;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Green:
                    return Color.green;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Blue:
                    return Color.blue;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Yellow:
                    return Color.yellow;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Purple:
                    return Color.magenta;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Brown:
                    return new Color(1, 0.5f, 0);
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.None:
                    return Color.white;
                default:
                    return Color.clear;
            }
        }
    }
}