namespace Genetics;

using System;
using System.Collections.Generic;

class Program
{
    private const string JsonFilePath = "/Users/amlor/CSharpProjects/Genetics/traits.json";
    public const int PadRightDistance = 30;

    public static void Main(string[] args)
    {
        var people = new List<Individual>();
        (List<Trait> traits, int genomeLength) = TraitAnalyzer.GetMaxGenomeLength(JsonFilePath);
        string[] menuItems =
        {
            "Person erzeugen",
            "Person anzeigen",
            "Person editieren",
            "Nachkommen zeugen",
            "Tabelle ausgeben",
            "Programm beenden",
        };
        int selectedIndex = 0;
        bool exit = false;

        IndividualService.CreatePerson(people, genomeLength);
        IndividualService.CreatePerson(people, genomeLength);

        while (!exit)
        {
            Console.Clear();
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }

                Console.WriteLine(menuItems[i]);
            }
            Console.ResetColor();


            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex == 0) ? menuItems.Length - 1 : selectedIndex - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex == menuItems.Length - 1) ? 0 : selectedIndex + 1;
                    break;
                case ConsoleKey.Enter:
                    switch (selectedIndex)
                    {
                        case 0:
                            IndividualService.CreatePerson(people, genomeLength);
                            Console.WriteLine(people[^1] .Name + " erstellt. Drücke eine Taste, um fortzufahren.");
                            Console.ReadKey();
                            break;
                        case 1:
                            IndividualService.ShowPersons(traits, people);
                            break;
                        case 2:
                            IndividualService.EditPersons(traits, people);
                            break;
                        case 3:
                            IndividualService.Procreate(people);
                            break;
                        case 4:
                            Tables.ShowTraitTable("Traits including Crossed Individuals", traits, people);
                            break;
                        case 5:
                            exit = true;
                            break;
                    }
                    break;
            }
        }
    }
}