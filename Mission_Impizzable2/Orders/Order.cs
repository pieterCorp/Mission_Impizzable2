using System;
using System.Collections.Generic;

namespace Mission_Impizzable2
{
    internal class Order
    {
        public List<int[]> Items = new List<int[]>();
        public List<double> ItemPrice = new List<double>();
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double TotalPrice { get; set; }
        public int ItemCount { get; set; }
        public int[][] ItemsArray { get; set; }     
        public double[] ItemPriceArray { get; set; }
    }
}