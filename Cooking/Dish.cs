using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Cooking
{
    [Serializable]
    class Dish
    {
        public string Description { get; set; }
        public string Results { get; set; }
        public System.DateTime LastUsed { get; set; }
        public string DishPath { get; set; }
        public string Resipe { get; set; }

        public Dish(Random random, string folder)
        {
            string[] fileNames = Directory.GetFiles(folder, "*.dish");
            OpenFile(fileNames[random.Next(fileNames.Length)]);
        }

        public Dish(string dishPath)
        {
            OpenFile(dishPath);
        }

        public Dish()
        {
            DishPath = "";
        }

        public void OpenFile(string dishPath)
        {
            DishPath = dishPath;
            using (Stream input = File.OpenRead(dishPath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Dish simple = (Dish)bf.Deserialize(input);

                Description = simple.Description;
                Results = simple.Results;
                LastUsed = simple.LastUsed;
                Resipe = simple.Resipe;
            }
        }

        public void Save(string fileName)
        {
            using (Stream output = File.Create(fileName))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(output, this);
            }
        }
    }
}
