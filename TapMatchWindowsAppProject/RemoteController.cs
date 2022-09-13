using System;
using System.Collections.Generic;
using System.Text;
using Rovio.TapMatch.Logic;

namespace Rovio.TapMatch.WindowsApp
{
    internal class RemoteController
    {
        private RemoteForm remoteForm;
        private LogicController logicController;

        public RemoteController(RemoteForm remoteForm)
        {
            this.remoteForm = remoteForm; 
            
            StartGame();
        }

        public void StartGame()
        {
            logicController = new LogicController(5,5,3,DateTime.Now.Millisecond);
            remoteForm.InitializeLevel(logicController);
        }
    }
}
