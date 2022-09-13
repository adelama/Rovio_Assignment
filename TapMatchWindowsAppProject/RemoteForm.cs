using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rovio.TapMatch.Logic;

namespace Rovio.TapMatch.WindowsApp
{
    internal partial class RemoteForm : Form
    {
        private TileDummyWindows[] formLevelTiles;
        private LogicController logicController;
        private bool isUpdatingLevel;

        public RemoteForm()
        {
            InitializeComponent();
        }

        public void InitializeLevel(LogicController logicController)
        {
            this.logicController = logicController;

            int tileSize = 80;
            int spacing = 10;

            levelLayoutPanel.MaximumSize = new Size((tileSize + spacing) * logicController.Level.Width, (tileSize + spacing) * logicController.Level.Height);

            formLevelTiles = new TileDummyWindows[logicController.Level.Tiles.Length];

            for (int i = 0; i < formLevelTiles.Length; i++)
            {
                Tile tile = logicController.Level.Tiles[i];
                Button button = new Button();
                //button.Location = new Point(0, i/logicController.Level.Width*tileSize);
                button.Size = new Size(tileSize, tileSize);
                levelLayoutPanel.Controls.Add(button);
                formLevelTiles[i] = new TileDummyWindows(tile.Index,
                    TileDummyWindows.ConvertLogicColorToFormColor(tile.Color),
                    button,
                    OnTileClick);
            }
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
                //TODO: showing some feedbacks that this action couldn't be executed
            }
        }

        private void UpdateUnityLevel(ColorMatchTiles matchTiles)
        {
            isUpdatingLevel = true;
            Tile[] poppedTiles = matchTiles.TilesArray;
            for (int i = 0; i < poppedTiles.Length; i++)
            {
                formLevelTiles[poppedTiles[i].Index].SetColor(Color.Black);
            }
            this.Refresh();
            System.Threading.Thread.Sleep(500);
            for (int i = formLevelTiles.Length - 1; i >= 0; i--)
            {
                formLevelTiles[i].SetColor(
                    TileDummyWindows.ConvertLogicColorToFormColor(logicController.Level.Tiles[i].Color));
            }
            this.Refresh();
            System.Threading.Thread.Sleep(200);
            isUpdatingLevel = false;
        }

    }
}
