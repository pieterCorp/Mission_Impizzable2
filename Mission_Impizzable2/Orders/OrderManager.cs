using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mission_Impizzable2
{
    internal class OrderManager
    {
        private Filemanager File_Manager { get; set; }
        private AppConstands App_Constands { get; set; }
        private PizzaManager Pizza_Manager { get; set; }
        private IngredientManager Ingredient_Manager { get; set; }
        public List<Order> GlobalOrders = new List<Order>();
        private List<DateTime> DaysOrdered = new List<DateTime>();
        private List<Order>[] SortedByDay { get; set; }

        public int ItemIndex { get; set; }

        private double[] ItemPrice { get; set; }
        private string[] AllOrdersJson { get; set; }

        public OrderManager()
        {
            Filemanager filemanager = new Filemanager();
            File_Manager = filemanager;

            AppConstands appConstands = new AppConstands();
            App_Constands = appConstands;

            PizzaManager pizzamanager = new PizzaManager();
            Pizza_Manager = pizzamanager;
        }

        public void MakeOrder()
        {
            PizzaManager pizzamanager = new PizzaManager();
            Pizza_Manager = pizzamanager;

            IngredientManager ingredientManager = new IngredientManager();
            Ingredient_Manager = ingredientManager;

            Order newOrder = new Order();
            bool ordering = true;
            ItemIndex = 0;

            while (ordering)
            {
                Console.Clear();
                ShowCart(newOrder);
                GetUserInput(newOrder);
                SetOrder(newOrder);
                Console.Clear();
                ShowCart(newOrder);
                ItemIndex++;
                Console.WriteLine("Want some more? y/n");
                ordering = UserIO.AskYesNoQ();
            }
            //check if order can be made
            bool succes = CheckItemsAvailable(newOrder);
            if (!succes)
            {
                UserIO.PrintRed("---The order could not be made!---");
                Ingredient_Manager.SetGlobalIngredients();
                return;
            }
            Ingredient_Manager.UpdateIngredients();
            UserIO.PrintGreen("Order was made");
            //print factuur
            Console.ReadLine();
            ShowInvoice(newOrder);
            //store order
            StoreOrder(newOrder);
        }

        private void ShowCart(Order newOrder)
        {
            UserIO.PrintBlue("----------------------------------------------------------------------");
            UserIO.PrintBlue("Currently in cart:");
            Console.WriteLine();

            if (newOrder.ItemCount == 0)
            {
                Console.WriteLine("Nothing in cart");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                UserIO.PrintPretty("Quantity", 15, "Name", 30, "Size", 45, "Crust", 60, "Price", true);
                Console.ResetColor();
                Console.WriteLine();
            }
            for (int i = 0; i < newOrder.Items.Count; i++)
            {
                int itemId = newOrder.Items[i][0];
                string itemSize = ((PizzaSize)newOrder.Items[i][1]).ToString();
                string itemCrust = ((PizzaCrust)newOrder.Items[i][2]).ToString();
                int itemQuantity = newOrder.Items[i][3];

                UserIO.PrintPretty($"{itemQuantity} stuks", 15, $"{Pizza_Manager.GlobalPizzas[itemId].Name}", 30, $"{itemSize}", 45, $"{itemCrust}", 60, $"{newOrder.ItemPrice[i]}", true);
            }
            Console.WriteLine();
            UserIO.PrintYellow($"Total price: {newOrder.TotalPrice} - Items in cart: {newOrder.ItemCount}");
            UserIO.PrintBlue("----------------------------------------------------------------------");
            Console.WriteLine();
        }

        private void GetUserInput(Order newOrder)
        {
            int[] item = new int[4];

            Pizza_Manager.ShowPizzaList();

            Console.WriteLine();
            Console.WriteLine("Select id to add to order");
            int id = UserIO.GetUserInt(0, Pizza_Manager.GlobalPizzas.Count - 1);
            item[0] = id;

            Console.Clear();
            ShowCart(newOrder);
            Pizza_Manager.ShowOnePizza(id);
            Console.WriteLine();
            UserIO.PrintBlue("---------------------------------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("Select the size: Small = 1 | Medium = 2 | Large = 3");
            int size = UserIO.GetUserInt(1, 3);
            item[1] = size;

            Console.WriteLine("Select Crust: PanCrust = 1 | DeepCrust = 2 | CheeseCrust = 3");
            int crust = UserIO.GetUserInt(1, 3);
            item[2] = crust;

            Console.WriteLine("How many of these?");
            int quantity = UserIO.GetUserInt(1, 50);
            item[3] = quantity;

            newOrder.Items.Add(item);
            newOrder.ItemsArray = newOrder.Items.ToArray();
        }

        private void SetOrder(Order newOrder)
        {
            double itemPrice = 0;

            int pizzaId = newOrder.Items[ItemIndex][0];
            int size = newOrder.Items[ItemIndex][1];
            int crust = newOrder.Items[ItemIndex][2];
            int quantity = newOrder.Items[ItemIndex][3];

            if (size == Convert.ToInt32(PizzaSize.small))
            {
                itemPrice = Pizza_Manager.GlobalPizzas[pizzaId].PriceSmall;
            }
            else if (size == Convert.ToInt32(PizzaSize.medium))
            {
                itemPrice = Pizza_Manager.GlobalPizzas[pizzaId].PriceMedium;
            }
            else if (size == Convert.ToInt32(PizzaSize.large))
            {
                itemPrice = Pizza_Manager.GlobalPizzas[pizzaId].PriceLarge;
            }
            else
            {
                UserIO.PrintRed("Error");
            }

            if (crust == Convert.ToInt32(PizzaCrust.CheeseCrust))
            {
                itemPrice += 2;
            }

            itemPrice *= quantity;
            newOrder.ItemPrice.Add(itemPrice);
            newOrder.ItemPriceArray = newOrder.ItemPrice.ToArray();

            newOrder.TotalPrice += itemPrice;
            newOrder.ItemCount += quantity;
            //newOrder.Date = Convert.ToString(DateTime.Now);
            newOrder.Date = DateTime.Now;

            int id = File_Manager.CountLinesFile(App_Constands.FilePathOrder);
            newOrder.Id = id;
        }

        private bool CheckItemsAvailable(Order newOrder)
        {
            bool succes;

            for (int i = 0; i < newOrder.Items.Count; i++)
            {
                int itemId = newOrder.Items[i][0];
                int itemSize = newOrder.Items[i][1];
                int itemQuantity = newOrder.Items[i][3];

                //use 1 ingredient for small, 2 for medium, 3 for large
                int ingredientsRequired = itemSize * itemQuantity;

                int[] ingredients = Pizza_Manager.GlobalPizzas[itemId].Ingredients;

                for (int j = 0; j < ingredients.Length; j++)
                {
                    int ingredientId = Pizza_Manager.GlobalPizzas[itemId].Ingredients[j];

                    // we use, but we dont save here. so its a virtual use. like a check
                    succes = Ingredient_Manager.GlobalIngredients[ingredientId].Use(ingredientsRequired);
                    if (!succes)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ShowInvoice(Order order)
        {
            Console.Clear();
            UserIO.PrintRed($"Order nr:{order.Id}  -  placed on: {order.Date}");
            Console.WriteLine();

            string name = "";
            string size = "";
            string crust = "";
            string quantity = "";
            string itemPrice = "";

            Console.ForegroundColor = ConsoleColor.Yellow;
            UserIO.PrintPretty("Name", 15, "Size", 30, "Crust", 45, "Quantity", 60, "ItemPrice", true);
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < order.ItemsArray.Length; i++)
            {
                int pizzaId = order.ItemsArray[i][0];
                name = Convert.ToString(Pizza_Manager.GlobalPizzas[pizzaId].Name);
                size = Convert.ToString((PizzaSize)order.ItemsArray[i][1]);
                crust = Convert.ToString((PizzaCrust)order.ItemsArray[i][2]);
                quantity = Convert.ToString(order.ItemsArray[i][3]);
                itemPrice = Convert.ToString(order.ItemPriceArray[i]);

                UserIO.PrintPretty(name, 15, size, 30, crust, 45, quantity, 60, itemPrice, true);
            }

            Console.WriteLine();
            UserIO.PrintYellow($"Total items: {order.ItemCount}");
            UserIO.PrintGreen($"Total price: {order.TotalPrice}");
            Console.WriteLine();
            UserIO.PrintDarkRed("Press enter to go back");
        }

        private void StoreOrder(Order Order)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(Order);

            File_Manager.CreateFolder(App_Constands.FolderPath);
            File_Manager.CreateFile(App_Constands.FilePathOrder);
            File_Manager.WriteDataToFile(json, App_Constands.FilePathOrder);
        }

        private void LoadAllOrders()
        {
            File_Manager.CreateFolder(App_Constands.FolderPath);
            File_Manager.CreateFile(App_Constands.FilePathOrder);
            AllOrdersJson = File_Manager.LoadAllFiles(App_Constands.FilePathOrder);
        }

        private void SetGlobalOrders()
        {
            LoadAllOrders();
            Order[] order = new Order[AllOrdersJson.Length];

            for (int i = 0; i < AllOrdersJson.Length; i++)
            {
                string json = AllOrdersJson[i];
                Order result = JsonConvert.DeserializeObject<Order>(json);
                order[i] = result;
            }

            GlobalOrders = order.ToList();
        }

        public void DisplayOrderList()
        {
            SetGlobalOrders();
            Console.Clear();
            UserIO.PrintRed("Order List");
            Console.WriteLine();
            string[] listOrders = new string[GlobalOrders.Count + 1];

            bool checkingOrders = true;

            while (checkingOrders)
            {
                for (int i = 0; i < GlobalOrders.Count; i++)
                {
                    if (GlobalOrders[i].Id < 10)
                    {
                        listOrders[i] = $"Order: 0{GlobalOrders[i].Id}   Placed on: {GlobalOrders[i].Date}";
                    }
                    else
                    {
                        listOrders[i] = $"Order: {GlobalOrders[i].Id}   Placed on: {GlobalOrders[i].Date}";
                    }
                }

                //add last option to menu
                listOrders[GlobalOrders.Count] = "Back to menu";
                int id = UserIO.Menu(listOrders, "Orders");

                //check if selected option is "back to menu" if not, show the selected order
                if (id == GlobalOrders.Count)
                {
                    checkingOrders = false;
                }
                else
                {
                    ShowInvoice(GlobalOrders[id]);
                    Console.ReadLine();
                }
            }
        }

        public void ShowOrders()
        {
            SetGlobalOrders();
            MakeListDaysOrdered();
            SortOrdersByDay();

            bool goingTrueDays = true;

            while (goingTrueDays)
            {
                int selection = ShowMenuDaysOrdered();
                if (selection == DaysOrdered.Count)
                {
                    goingTrueDays = false;
                }
                else
                {
                    DisplayOrderListOneDay(selection);
                }
            }
        }

        private void MakeListDaysOrdered()
        {
            //make list of all days an order was made

            for (int i = 0; i < GlobalOrders.Count; i++)
            {
                if (!DaysOrdered.Contains(GlobalOrders[i].Date.Date))
                {
                    DaysOrdered.Add(GlobalOrders[i].Date.Date);
                }
            }
        }

        private void SortOrdersByDay()
        {
            //make array with a list of orders for each day

            SortedByDay = new List<Order>[DaysOrdered.Count];

            int counter = 0;
            foreach (var day in DaysOrdered)
            {
                List<Order> filteredByDay = new List<Order>();

                var filteredDate = GlobalOrders.Where(i => i.Date.Date == day);
                filteredByDay = filteredDate.ToList();

                SortedByDay[counter] = filteredByDay;
                counter++;
            }
        }

        private int ShowMenuDaysOrdered()
        {
            string[] menuDays = new string[DaysOrdered.Count + 1];
            for (int i = 0; i < DaysOrdered.Count; i++)
            {
                menuDays[i] = DaysOrdered[i].ToString("d");
            }
            menuDays[DaysOrdered.Count] = "Back to menu";
            return UserIO.Menu(menuDays, "Day's with orders");
        }

        private void ShowOrdersOneDay(int selected)
        {
            for (int i = 0; i < SortedByDay[selected].Count; i++)
            {
                Console.WriteLine(SortedByDay[selected][i].Id);
            }
        }

        private void DisplayOrderListOneDay(int selectedDay)
        {
            //get all orders for the selected day

            string[] listOrders = new string[SortedByDay[selectedDay].Count + 2];

            bool checkingOrders = true;

            while (checkingOrders)
            {
                for (int i = 0; i < SortedByDay[selectedDay].Count; i++)
                {
                    if (SortedByDay[selectedDay][i].Id < 10)
                    {
                        listOrders[i] = $"Order: 0{SortedByDay[selectedDay][i].Id}   Placed on: {SortedByDay[selectedDay][i].Date}";
                    }
                    else
                    {
                        listOrders[i] = $"Order: {SortedByDay[selectedDay][i].Id}   Placed on: {SortedByDay[selectedDay][i].Date}";
                    }
                }

                //add "Show Day Balance" to menu options
                listOrders[SortedByDay[selectedDay].Count] = "--------Show balance for this day--------";

                //add "Back to previous menu" to menu options
                listOrders[SortedByDay[selectedDay].Count + 1] = "Back";
                int id = UserIO.Menu(listOrders, "Orders");
                if (id == SortedByDay[selectedDay].Count)
                {
                    ShowBalanceForDay(selectedDay);
                }
                else if (id == SortedByDay[selectedDay].Count + 1)
                {
                    checkingOrders = false;
                }
                else
                {
                    ShowInvoice(SortedByDay[selectedDay][id]);
                    Console.ReadLine();
                }
            }
        }

        private void ShowBalanceForDay(int day)
        {
            //Loop true all orders and add the prices. fill 2 lists: one with all used ingredients (id),
            //the other with times they where used (quantity * size) 1=small 2=med 3=large
            double earningsDay = 0;
            int itemsDay = 0;
            List<int> ingredientId = new List<int>();
            List<int> ingredientCount = new List<int>();

            foreach (var order in SortedByDay[day])
            {
                earningsDay += order.TotalPrice;
                itemsDay += order.ItemCount;

                for (int i = 0; i < order.ItemsArray.Length; i++)
                {
                    int pizzaId = order.ItemsArray[i][0];
                    int pizzaSize = order.ItemsArray[i][1];
                    int pizzaQuantity = order.ItemsArray[i][3];
                    //Console.WriteLine(pizzaId);

                    int[] pizzaIngr = Pizza_Manager.GlobalPizzas[pizzaId].Ingredients;

                    //loop over pizza ingredients
                    for (int j = 0; j < pizzaIngr.Length; j++)
                    {
                        //if id is not in list, ad to list, else add count to existing
                        if (!ingredientId.Contains(pizzaIngr[j]))
                        {
                            ingredientId.Add(pizzaIngr[j]);
                            ingredientCount.Add(pizzaSize * pizzaQuantity);
                        }
                        else
                        {
                            int index = ingredientId.IndexOf(pizzaIngr[j]);
                            ingredientCount[index] += pizzaSize * pizzaQuantity;
                        }
                    }
                }
            }
            IngredientManager ingredientManager = new IngredientManager();
            Ingredient_Manager = ingredientManager;

            //Display obtained info

            Console.Clear();
            UserIO.PrintRed($"Balance for Day {SortedByDay[day][0].Date.ToString("d")}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            UserIO.PrintPretty("Ingredient used", 20, "Times used");
            Console.WriteLine();
            Console.ResetColor();

            for (int i = 0; i < ingredientId.Count; i++)
            {
                UserIO.PrintPretty($"{ Ingredient_Manager.GetOneIngredient(ingredientId[i]) }", 20, $"{ ingredientCount[i] }");
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Total Items sold: {itemsDay}");
            UserIO.PrintGreen($"Total income: {earningsDay} EUR");
            Console.WriteLine();

            UserIO.PrintDarkRed("Press enter to go back");

            Console.ReadLine();
        }
    }
}