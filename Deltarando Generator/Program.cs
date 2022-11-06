using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deltarando_Generator
{
    class Program
    {
        public static void Main(string[] args)
        {
            World world = new World();
            Logic logic = new Logic();
            logic.SetLogic(world);
            world.AddItems();
        }
    }
}
