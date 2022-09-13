using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Rovio.TapMatch.WindowsApp
{
    internal class TileDummyWindows
    {
        private Button button;

        public int Index { get; private set; }

        System.Action<int> onClick;

        // Start is called before the first frame update
        public TileDummyWindows(int tileIndex, System.Drawing.Color color,Button button, System.Action<int> onClick)
        {
            Index = tileIndex;
            this.onClick = onClick;
            this.button = button;
            SetColor(color);
            this.button.Text = "";
            this.button.Click += OnPointerClick;
        }

        public void SetColor(System.Drawing.Color color)
        {
            button.BackColor = color;
        }

        public void OnPointerClick(object sender, EventArgs e)
        {
                onClick?.Invoke(Index);
        }

        public static System.Drawing.Color ConvertLogicColorToFormColor(Rovio.TapMatch.Logic.LogicConstants.TileColor logicColor)
        {
            switch (logicColor)
            {
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Red:
                    return System.Drawing.Color.Red;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Green:
                    return System.Drawing.Color.Green;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Blue:
                    return System.Drawing.Color.Blue;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Yellow:
                    return System.Drawing.Color.Yellow;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Purple:
                    return System.Drawing.Color.Magenta;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.Brown:
                    return System.Drawing.Color.Brown;
                case Rovio.TapMatch.Logic.LogicConstants.TileColor.None:
                    return System.Drawing.Color.Black;
                default:
                    return System.Drawing.Color.White;
            }
        }
    }
}
