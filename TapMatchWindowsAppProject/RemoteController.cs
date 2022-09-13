using System;
using System.Collections.Generic;
using System.Text;
using Rovio.TapMatch.Logic;
using Rovio.TapMatch.Remote;

namespace Rovio.TapMatch.WindowsApp
{
    internal class RemoteController
    {
        private RemoteForm remoteForm;
        private LogicController logicController;
        private RemoteProtocol protocol;

        public RemoteController(RemoteForm remoteForm)
        {
            this.remoteForm = remoteForm;
            StartServer();

            StartGame();
        }

        private void StartServer()
        {
            protocol = new RemoteProtocol((s) => Console.WriteLine(s));
            _ = protocol.StartServer(() => protocol.SendData("Salam"));
        }

        public void StartGame()
        {
            logicController = new LogicController();
            int randomSeed = DateTime.Now.Millisecond;
            var startCmd = new StartGameCommand(5, 5, 3, randomSeed, logicController);
            logicController.ExecuteCommand(startCmd);
            remoteForm.InitializeLevel(logicController);
        }
    }
}
