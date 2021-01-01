using System;
using System.Collections.Generic;
using System.Text;

namespace Mission_Impizzable2
{
    class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }       
        public int Quantity { get; set; }
        private int MinThreshold { get; set; } = 10;

        public Ingredient(int id, string name, int quantity)
        {
            Id = id;
            Name = name;            
            Quantity = quantity;
        }

        public bool Order(int amount)
        {
            if (Quantity + amount <= 500)
            {
                Quantity += amount;
                return true;
            }
            else
            {
                Console.WriteLine("Max stockcapacity is 500, buy less");
                return false;
            }
        }

        public bool Use(int amount)
        {
            if (Quantity - amount >= MinThreshold)
            {
                Quantity -= amount;
                return true;
            }
            if (Quantity - amount >= 0)
            {
                UserIO.PrintRed($"Warning! Less then {MinThreshold} {Name} left!");
                Quantity -= amount;
                return true;
            }
            else
            {
                UserIO.PrintRed($"Not enough {Name}, order some more!");                
                return false;
            }
        }
    }
}
