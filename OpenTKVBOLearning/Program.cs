using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKVBOLearning
{
    class Program
    {
        public static GameWindow window;
        static void Main(string[] args)
        {
            window = new GameWindow(800, 600);
            new WindowManager();
            window.Run(1D / 60D);
        }
    }
}
