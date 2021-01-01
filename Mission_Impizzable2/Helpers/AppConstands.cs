namespace Mission_Impizzable2
{
    internal enum PizzaSize
    { small = 1, medium = 2, large = 3 }

    internal enum PizzaCrust
    { PanCrust = 1, DeepCrust = 2, CheeseCrust = 3 }

    internal class AppConstands
    {
        public string FolderPath { get; set; } = "../../../Db/";
        public string FilePathIngredient { get; set; } = "../../../Db/Ingredients.txt";
        public string FilePathPizza { get; set; } = "../../../Db/Pizzas.txt";
        public string FilePathOrder { get; set; } = "../../../Db/Orders.txt";
        public static string Title { get; } = @"   _____  .__              .__                .__               .__                      ___.   .__           ________  
  /     \ |__| ______ _____|__| ____   ____   |__| _____ ______ |__|____________________ \_ |__ |  |   ____   \_____  \ 
 /  \ /  \|  |/  ___//  ___/  |/  _ \ /    \  |  |/     \\____ \|  \___   /\___   /\__  \ | __ \|  | _/ __ \   /  ____/ 
/    Y    \  |\___ \ \___ \|  (  <_> )   |  \ |  |  Y Y  \  |_> >  |/    /  /    /  / __ \| \_\ \  |_\  ___/  /       \ 
\____|__  /__/____  >____  >__|\____/|___|  / |__|__|_|  /   __/|__/_____ \/_____ \(____  /___  /____/\___  > \_______ \
        \/        \/     \/               \/           \/|__|            \/      \/     \/    \/          \/          \/";
    }
}