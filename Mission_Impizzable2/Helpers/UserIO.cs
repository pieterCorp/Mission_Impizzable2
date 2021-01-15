using System;
using static System.Console;

namespace Mission_Impizzable2
{
    internal class UserIO
    {
        static public bool AskYesNoQ()
        {
            bool validInput = false;
            bool answer = false;
            char input;
            while (!validInput)
            {
                try
                {
                    input = Convert.ToChar(Console.ReadLine());
                    if (input == 'y')
                    {
                        answer = true;
                        validInput = true;
                    }
                    else if (input == 'n')
                    {
                        answer = false;
                        validInput = true;
                    }
                    else
                    {
                        UserIO.PrintRed("Input not valid, plz try again");
                    }
                }
                catch (Exception)
                {
                    UserIO.PrintRed("Input not valid, plz try again");
                }
            }
            return answer;
        }

        static public void PrintPretty(string string1, int indent1, string string2)
        {
            Console.Write(string1);
            int spaces = indent1 - string1.Length;
            for (int i = 0; i < spaces; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine(string2);
        }

        static public void PrintPretty(string string1, int indent1, string string2, int indent2, string string3)
        {
            Console.Write(string1);
            int spaces = indent1 - string1.Length;
            for (int i = 0; i < spaces; i++)
            {
                Console.Write(" ");
            }
            Console.Write(string2);
            int spaces2 = indent2 - (indent1 + string2.Length);
            for (int i = 0; i < spaces2; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine(string3);
        }

        static public void PrintPretty(string string1, int indent1, string string2, int indent2, string string3, int indent3, string string4)
        {
            Console.Write(string1);
            int spaces = indent1 - string1.Length;
            for (int i = 0; i < spaces; i++)
            {
                Console.Write(" ");
            }
            Console.Write(string2);
            int spaces2 = indent2 - (indent1 + string2.Length);
            for (int i = 0; i < spaces2; i++)
            {
                Console.Write(" ");
            }
            Console.Write(string3);
            int spaces3 = indent3 - (indent2 + string3.Length);
            for (int i = 0; i < spaces3; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine(string4);
        }

        static public void PrintPretty(string string1, int indent1, string string2, int indent2, string string3, int indent3, string string4, int indent4, string string5, bool newLine)
        {
            Console.Write(string1);
            int spaces = indent1 - string1.Length;
            for (int i = 0; i < spaces; i++)
            {
                Console.Write(" ");
            }
            Console.Write(string2);
            int spaces2 = indent2 - (indent1 + string2.Length);
            for (int i = 0; i < spaces2; i++)
            {
                Console.Write(" ");
            }
            Console.Write(string3);
            int spaces3 = indent3 - (indent2 + string3.Length);
            for (int i = 0; i < spaces3; i++)
            {
                Console.Write(" ");
            }
            Console.Write(string4);
            int spaces4 = indent4 - (indent3 + string4.Length);
            for (int i = 0; i < spaces4; i++)
            {
                Console.Write(" ");
            }
            if (newLine)
            {
                Console.WriteLine(string5);
            }
            else
            {
                Console.Write(string5);
            }
        }

        static public string GetUserString()
        {
            bool validInput = false;
            string input = "";

            while (!validInput)
            {
                try
                {
                    input = Console.ReadLine();
                    validInput = true;
                }
                catch (Exception)
                {
                    UserIO.PrintRed("Input not valid, plz try again");
                }
            }

            return input;
        }

        static public int GetUserInt(int minValue, int maxValue)
        {
            bool validInput = false;
            int validatedInput = 0;
            while (!validInput)
            {
                try
                {
                    validatedInput = Convert.ToInt32(Console.ReadLine());
                    if (validatedInput >= minValue && validatedInput <= maxValue)
                    {
                        validInput = true;
                    }
                    else
                    {
                        UserIO.PrintRed("Input not valid, plz try again");
                    }
                }
                catch (Exception)
                {
                    UserIO.PrintRed("Input not valid, plz try again");
                }
            }
            return validatedInput;
        }

        static public double GetUserDouble(int minValue, int maxValue)
        {
            bool validInput = false;
            double validatedInput = 0;
            while (!validInput)
            {
                try
                {
                    validatedInput = Convert.ToDouble(Console.ReadLine());
                    if (validatedInput >= minValue && validatedInput <= maxValue)
                    {
                        validInput = true;
                    }
                    else
                    {
                        UserIO.PrintRed("Input not valid, plz try again");
                    }
                }
                catch (Exception)
                {
                    UserIO.PrintRed("Input not valid, plz try again");
                }
            }
            return validatedInput;
        }

        static public int Menu(string[] options, string title)
        {
            ConsoleKey keyPressed;
            int selectionIndex = 0;

            do
            {
                Clear();
                Console.CursorVisible = false;
                UserIO.PrintRed(title);
                Console.WriteLine();

                for (int i = 0; i < options.Length; i++)
                {
                    if (selectionIndex == i)
                    {
                        ForegroundColor = ConsoleColor.White;
                        Write("-> ");
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Blue;
                        if (i == options.Length - 1) ForegroundColor = ConsoleColor.DarkRed;
                        Write("   ");
                    }

                    Console.WriteLine(options[i]);
                }
                ResetColor();

                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectionIndex--;
                    if (selectionIndex < 0)
                    {
                        selectionIndex = options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectionIndex++;
                    if (selectionIndex > options.Length - 1)
                    {
                        selectionIndex = 0;
                    }
                }
            } while (keyPressed != ConsoleKey.Enter);

            Console.CursorVisible = true;
            return selectionIndex;
        }

        static public void PrintRed(string stringToPrint)
        {
            PrintColor(stringToPrint, ConsoleColor.Red);
        }

        static public void PrintBlue(string stringToPrint)
        {
            PrintColor(stringToPrint, ConsoleColor.Blue);
        }

        static public void PrintGreen(string stringToPrint)
        {
            PrintGreen(stringToPrint, true);
        }

        static public void PrintGreen(string stringToPrint, bool newLine)
        {
            PrintColor(stringToPrint, ConsoleColor.Green, newLine);
        }

        static public void PrintYellow(string stringToPrint)
        {
            PrintColor(stringToPrint, ConsoleColor.Yellow);
        }
        static public void PrintDarkRed(string stringToPrint)
        {
            PrintColor(stringToPrint, ConsoleColor.Red);
        }

       static private void PrintColor(string stringToPrint, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;
            Console.Write(stringToPrint);
            Console.ResetColor();
            if (newLine)
            {
                Console.WriteLine();
            }
        }
    }
}