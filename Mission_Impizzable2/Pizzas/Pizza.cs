using System;
using System.Collections.Generic;
using System.Text;

namespace Mission_Impizzable2
{
    class Pizza
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double PriceSmall { get; set; }
        public double PriceMedium { get; set; }
        public double PriceLarge { get; set; }
        public int[] Ingredients { get; set; }
        public bool Veggy { get; set; }       

        public Pizza(int id, string name, double priceSmall, double priceMedium, double priceLarge, int[] ingredients, bool veggy)
        {
            Id = id;
            Name = name;
            PriceSmall = priceSmall;
            PriceMedium = priceMedium;
            PriceLarge = priceLarge;
            Ingredients = ingredients;
            Veggy = veggy;
        }
    }
}
