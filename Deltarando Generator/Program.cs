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
            for (int i = 0; i < world.options.Count; i++)
            {
                seed.WriteLine(i);
                seed.WriteLine(world.options[i]);
            }
            for (int i = 0; i < world.trueStarting.Count; i++)
            {
                if (world.trueStarting[i].name != "")
                {
                    seed.WriteLine(-1);
                    seed.WriteLine(world.trueStarting[i].name);
                }
            }
            
            for (int i = 0; i < world.locations.Count; i++)
            {
                if (world.locations[i].name != "")
                {
                    seed.WriteLine(i);
                    seed.WriteLine(world.locations[i].name);
                }
            }
            seed.Close();
        }
    }
}
