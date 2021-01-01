using System;

namespace Mission_Impizzable2
{
    internal class KassaSysteem
    {
        private IngredientManager Ingredient_Manager { get; set; }
        private PizzaManager Pizza_Manager { get; set; }
        private OrderManager Order_Manager { get; set; }

        public KassaSysteem()
        {
            IngredientManager ingredientManager = new IngredientManager();
            Ingredient_Manager = ingredientManager;

            PizzaManager pizzaManager = new PizzaManager();
            Pizza_Manager = pizzaManager;

            OrderManager orderManager = new OrderManager();
            Order_Manager = orderManager;

            Ingredient_Manager.SetGlobalIngredients();
        }

        public bool KassaMenu()
        {
            string[] options = new string[] { "Add a new ingredient", "Order an ingredient","View a list of all ingredient", "Create a new pizza",
                    "View a list of all pizzas", "Make a new order", "Show orders", "Quit"};
            int input = UserIO.Menu(options, AppConstands.Title);

            switch (input)
            {
                case 0:
                    Console.Clear();
                    Ingredient_Manager.CreateNewIngredient();
                    Console.ReadLine();
                    return true;

                case 1:
                    Console.Clear();
                    Ingredient_Manager.OrderIngredient();
                    Console.ReadLine();
                    return true;

                case 2:
                    Console.Clear();

                    Ingredient_Manager.ShowAllIngredients();
                    Console.ReadLine();
                    return true;

                case 3:
                    Console.Clear();
                    Pizza_Manager.CreateNewPizza();
                    Console.ReadLine();
                    return true;

                case 4:                    
                    Pizza_Manager.ShowAllPizzas();
                    return true;

                case 5:
                    Console.Clear();
                    Order_Manager.MakeOrder();
                    Console.ReadLine();
                    return true;

                case 6:
                    Order_Manager.ShowOrders();
                    //Order_Manager.DisplayOrderList();                    
                    return true;

                case 7:
                    //quit
                    return false;

                default:
                    Console.WriteLine("Give a vallid input plz!");
                    Console.ReadLine();
                    return true;
            }
        }
    }
}