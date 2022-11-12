using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            world.Randomize();
            var seed = File.CreateText(Directory.GetCurrentDirectory() + "/deltarando.seed");
            for (int i = 0; i < world.locations.Count; i++)
            {
                if (world.locations[i].id != -999)
                {
                    seed.WriteLine(i);
                    seed.WriteLine(world.locations[i].category);
                    seed.WriteLine(world.locations[i].id);
                }
            }
            seed.Close();
        }
    }
}
