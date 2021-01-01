using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mission_Impizzable2
{
    internal class IngredientManager
    {
        private Filemanager File_Manager { get; set; }
        private AppConstands App_Constands { get; set; }

        public List<Ingredient> GlobalIngredients = new List<Ingredient>();
        private string[] AllIngredientsJson { get; set; }

        private int Id { get; set; }
        private string Name { get; set; }
        private int Quantity { get; set; }

        public IngredientManager()
        {
            Filemanager filemanager = new Filemanager();
            File_Manager = filemanager;

            AppConstands appConstands = new AppConstands();
            App_Constands = appConstands;

            SetGlobalIngredients();
        }

        public void CreateNewIngredient()
        {
            if (GetValidUserInput())
            {
                Ingredient ingredient = new Ingredient(Id, Name, Quantity);
                Id++;
                StoreIngredient(ingredient);
                SetGlobalIngredients();
                UserIO.PrintGreen("Ingredient succesfully added, press enter to continue...");
            }
            else
            {
                Console.WriteLine();
                UserIO.PrintRed("adding item failed, press enter to continue...");
            }
        }

        private bool GetValidUserInput()
        {
            UserIO.PrintRed("Create new ingredient");
            Console.WriteLine();

            Console.WriteLine("Enter name");
            string name = UserIO.GetUserString();
            if (CheckIfInList(name))
            {
                Console.WriteLine();
                Console.WriteLine("Ingredient already exists!");
                return false;
            }

            Console.WriteLine($"Enter quantity, Max stock is set to 500");
            int quantity = UserIO.GetUserInt(0, 500);

            Name = name;
            Quantity = quantity;
            return true;
        }

        private bool CheckIfInList(string name)
        {
            for (int i = 0; i < GlobalIngredients.Count; i++)
            {
                if (GlobalIngredients[i].Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        private void LoadAllIngredients()
        {
            File_Manager.CreateFolder(App_Constands.FolderPath);
            File_Manager.CreateFile(App_Constands.FilePathIngredient);
            AllIngredientsJson = File_Manager.LoadAllFiles(App_Constands.FilePathIngredient);
        }

        private void StoreIngredient(object ingredient)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(ingredient);

            File_Manager.CreateFolder(App_Constands.FolderPath);
            File_Manager.CreateFile(App_Constands.FilePathIngredient);
            File_Manager.WriteDataToFile(json, App_Constands.FilePathIngredient);
        }

        public void UpdateIngredients()
        {
            // A computer has no feelings, let it work :-)

            File_Manager.DeleteFile(App_Constands.FilePathIngredient);

            for (int i = 0; i < GlobalIngredients.Count; i++)
            {
                StoreIngredient(GlobalIngredients[i]);
            }
        }

        public void SetGlobalIngredients()
        {
            LoadAllIngredients();
            Ingredient[] ingredient = new Ingredient[AllIngredientsJson.Length];
            Id = 0;
            for (int i = 0; i < AllIngredientsJson.Length; i++)
            {
                string json = AllIngredientsJson[i];
                Id++;
                Ingredient result = JsonConvert.DeserializeObject<Ingredient>(json);
                ingredient[i] = result;
            }

            GlobalIngredients = ingredient.ToList();
        }

        public void ShowAllIngredients()
        {
            SetGlobalIngredients();

            for (int i = 0; i < GlobalIngredients.Count; i++)
            {
                UserIO.PrintPretty($"{GlobalIngredients[i].Id}", 10, $"{ GlobalIngredients[i].Name}", 30, $"{ GlobalIngredients[i].Quantity}");
            }
        }

        public string GetOneIngredient(int id)
        {
            return GlobalIngredients[id].Name;
        }

        public void OrderIngredient()
        {
            Console.Clear();
            UserIO.PrintRed("Order Ingredient");
            Console.WriteLine();

            ShowAllIngredients();

            Console.WriteLine();
            Console.WriteLine("Select id of ingredient you want to order");
            int id = UserIO.GetUserInt(0, GlobalIngredients.Count - 1);

            Console.WriteLine();
            Console.WriteLine("Enter the amount (max 500)");
            int amount = UserIO.GetUserInt(0, 500);

            if (GlobalIngredients[id].Order(amount))
            {
                UserIO.PrintGreen($"You succesfully orderd {amount} pieces of {GlobalIngredients[id].Name} ");
                UpdateIngredients();
            }
            else
            {
                UserIO.PrintRed("Order failed");
            }
        }
    }
}