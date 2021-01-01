using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mission_Impizzable2
{
    internal class PizzaManager
    {
        private Filemanager File_Manager { get; set; }
        private AppConstands App_Constands { get; set; }

        public List<Pizza> GlobalPizzas = new List<Pizza>();
        private string[] AllPizzasJson { get; set; }
        private int Id { get; set; }
        private string Name { get; set; }
        private double PriceSmall { get; set; }
        private double PriceMedium { get; set; }
        private double PriceLarge { get; set; }
        private int[] Ingredients { get; set; }
        private bool IsVeg { get; set; }
        private string Size { get; set; }
        private string Crust { get; set; }

        public PizzaManager()
        {
            Filemanager filemanager = new Filemanager();
            File_Manager = filemanager;

            AppConstands appConstands = new AppConstands();
            App_Constands = appConstands;

            SetGlobalPizzas();
        }

        public void CreateNewPizza()
        {
            Console.Clear();
            if (GetValidUserInput())
            {
                Pizza pizza = new Pizza(Id, Name, PriceSmall, PriceMedium, PriceLarge, Ingredients, IsVeg);
                Id++;
                StorePizza(pizza);
                SetGlobalPizzas();
                UserIO.PrintGreen("succesfully added pizza, press enter to continue");
            }
            else
            {
                Console.WriteLine("adding pizza failed, press enter to continue");
                Console.ReadLine();
            }
        }

        private bool GetValidUserInput()
        {
            UserIO.PrintRed("Create new pizza");
            Console.WriteLine();
            Console.WriteLine("Enter name");
            string name = UserIO.GetUserString();
            if (CheckIfInList(name))
            {
                Console.WriteLine("Pizza already exists");
                return false;
            }

            Console.WriteLine("Enter price for a small pizza");
            double priceSmall = UserIO.GetUserDouble(0, 1000);
            Console.WriteLine("Enter price for a medium pizza");
            double priceMedium = UserIO.GetUserDouble(0, 1000);
            Console.WriteLine("Enter price for a large pizza");
            double priceLarge = UserIO.GetUserDouble(0, 1000);

            Console.WriteLine("choose ingredients");
            int[] ingredients = GetIngredients();

            Console.WriteLine("Is this a vegetarian pizza? y/n");
            bool isVeg = UserIO.AskYesNoQ();

            Name = name;
            PriceSmall = priceSmall;
            PriceMedium = priceMedium;
            PriceLarge = priceLarge;
            Ingredients = ingredients;
            IsVeg = isVeg;
            return true;
        }

        private bool CheckIfInList(string name)
        {
            for (int i = 0; i < GlobalPizzas.Count; i++)
            {
                if (GlobalPizzas[i].Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        private int[] GetIngredients()
        {
            bool thatsAll = false;
            List<int> ingredients = new List<int>();

            while (!thatsAll)
            {
                UserIO.PrintYellow("Add new ingredient? y/n");
                bool answer = UserIO.AskYesNoQ();

                if (answer)
                {
                    Console.Clear();
                    IngredientManager ingredientManager = new IngredientManager();
                    ingredientManager.ShowAllIngredients();
                    Console.WriteLine();
                    Console.WriteLine("Enter id of ingredient you want to add to your pizza");
                    int ingredientId = UserIO.GetUserInt(0, ingredientManager.GlobalIngredients.Count - 1);
                    ingredients.Add(ingredientId);
                    UserIO.PrintGreen($"{ingredientManager.GetOneIngredient(ingredientId)} was added as an ingredient");
                }
                else
                {
                    thatsAll = true;
                }
            }

            return ingredients.ToArray();
        }

        private void StorePizza(object pizza)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(pizza);

            File_Manager.CreateFolder(App_Constands.FolderPath);
            File_Manager.CreateFile(App_Constands.FilePathPizza);
            File_Manager.WriteDataToFile(json, App_Constands.FilePathPizza);
        }

        private void LoadAllPizzas()
        {
            File_Manager.CreateFolder(App_Constands.FolderPath);
            File_Manager.CreateFile(App_Constands.FilePathPizza);
            AllPizzasJson = File_Manager.LoadAllFiles(App_Constands.FilePathPizza);
        }

        public void SetGlobalPizzas()
        {
            LoadAllPizzas();
            Pizza[] pizza = new Pizza[AllPizzasJson.Length];
            Id = 0;
            for (int i = 0; i < AllPizzasJson.Length; i++)
            {
                string json = AllPizzasJson[i];
                Pizza result = JsonConvert.DeserializeObject<Pizza>(json);
                Id++;
                pizza[i] = result;
            }

            GlobalPizzas = pizza.ToList();
        }

        public void ShowPizzaList()
        {
            for (int i = 0; i < GlobalPizzas.Count; i++)
            {
                if (GlobalPizzas[i].Id < 10) { Console.Write("0"); }
                Console.WriteLine($"{GlobalPizzas[i].Id} - {GlobalPizzas[i].Name}");
            }
        }

        public void ShowAllPizzas()
        {


            bool checkingPizzas = true;
            string[] listPizzas = new string[GlobalPizzas.Count + 1];

            while (checkingPizzas)
            {
                for (int i = 0; i < GlobalPizzas.Count; i++)
                {
                    listPizzas[i] = GlobalPizzas[i].Name;
                }
                listPizzas[GlobalPizzas.Count] = "Back to menu";
                int id = UserIO.Menu(listPizzas, "Pizzas");

                if (id == GlobalPizzas.Count)
                {
                    checkingPizzas = false;
                }
                else
                {
                    Console.Clear();
                    ShowOnePizza(id);
                    Console.ReadLine();
                }
            }
        }

        public void ShowOnePizza(int id)
        {
            IngredientManager ingredientManager = new IngredientManager();

            UserIO.PrintRed($"{GlobalPizzas[id].Name}");
            Console.WriteLine();
            Console.WriteLine("Ingredients:");
            Console.WriteLine();
            for (int i = 0; i < GlobalPizzas[id].Ingredients.Length; i++)
            {
                int ingredientId = GlobalPizzas[id].Ingredients[i];
                Console.WriteLine($"  -{ingredientManager.GetOneIngredient(ingredientId)}");
            }

            Console.WriteLine();
            if (Convert.ToBoolean(GlobalPizzas[id].Veggy))
            {
                UserIO.PrintGreen("This is a vegetarian pizza");
            }
            UserIO.PrintYellow($"\nPriceSmall = {GlobalPizzas[id].PriceSmall} - PriceMedium = {GlobalPizzas[id].PriceMedium} - PriceLarge = {GlobalPizzas[id].PriceLarge}");
        }
    }
}