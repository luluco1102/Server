using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6_ThreadSafeSingleton
{
    internal class GameManager : Singleton<GameManager>
    {
        public GameManager()
        {
            Console.WriteLine("GameManager created");
        }

        public void StartGame()
        {
            Console.WriteLine("Start game.");
        }
    }
}
