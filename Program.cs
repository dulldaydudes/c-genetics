namespace Genetics;

using System;
using System.Collections.Generic;

class Program
{
    public static void Main(string[] args)
    {
        // Initialisiere Traits und Personen
        (var traits, var people) = Init();

        string[] menuItems =
        {
            "Person erzeugen",
            "Person editieren",
            "Nachkommen zeugen",
            "Tabelle ausgeben",
            "Programm beenden",
        };
        int selectedIndex = 0;
        bool exit = false;

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
                            CreatePerson(people);
                            Console.WriteLine("Person erstellt. Drücke eine Taste, um fortzufahren.");
                            break;
                        case 1:
                            EditPersons(traits, people);
                            Console.WriteLine("Bearbeitung abgeschlossen. Drücke eine Taste, um fortzufahren.");
                            break;
                        case 2:
                            Procreate(people);
                            Console.WriteLine();
                            break;
                        case 3:
                            ShowTraitTable("Traits including Crossed Individuals", traits, people);
                            Console.WriteLine();
                            break;
                        case 4:
                            exit = true;
                            break;
                    }
                    break;
            }
            Console.ReadKey();
        }
    }

    private static (List<Trait>, List<Individual>) Init()
    {
        // Definiere Traits
        var traits = new List<Trait>
        {
            new Trait("Compressed", ind => ind.GetGenome().Substring(0, 2).Contains('1') && !ind.GetGenome().Substring(2, 2).Contains('1')),
            new Trait("Prolate", ind => ind.GetGenome().Substring(2, 2).Contains('1') && !ind.GetGenome().Substring(0, 2).Contains('1')),
            new Trait("Tail Type 1", ind => ind.GetGenome().Substring(4, 2) == "11" && !ind.GetGenome().Substring(0, 4).Contains('1')),
            new Trait("Tail Type 2", ind => ind.GetGenome().Substring(6, 2) == "11" && !ind.GetGenome().Substring(0, 4).Contains('1')),
            new Trait("Scales", ind => ind.GetGenome().Substring(12, 2) == "10"),
            new Trait("Silicate Inclusions", ind => ind.GetGenome().Substring(14, 2) == "11"),
            new Trait("Bone Plates", ind => ind.GetGenome().Substring(16, 2) == "10"),
            new Trait("Increased Strength", ind => ind.GetGenome().Substring(18, 2) == "10"),
            new Trait("Increased Constitution", ind => ind.GetGenome().Substring(20, 2) == "11")
        };

        // Erstelle Personen
        var people = new List<Individual> { };

        CreatePerson(people);
        CreatePerson(people);

        return (traits, people);
    }

    private static void ShowTraitTable(string title, List<Trait> traits, List<Individual> individuals)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine("Trait\t\t\t" + string.Join("\t", individuals.ConvertAll(p => p.Name)));
        foreach (var trait in traits)
        {
            Console.Write(trait.Name.PadRight(30));
            foreach (var person in individuals)
            {
                Console.Write((trait.IsPresent(person) ? "X" : " ") + "\t\t");
            }

            Console.WriteLine();
        }
    }

    private static string GenerateRandomBinaryString(int length)
    {
        Random random = new Random();
        char[] binaryString = new char[length];

        for (int i = 0; i < length; i++)
        {
            // Generiere zufällige 0 oder 1
            binaryString[i] = random.Next(2) == 0 ? '0' : '1';
        }

        return new string(binaryString);
    }

    private static void CreatePerson(List<Individual> people)
    {
        Random random = new Random();
        string gender = random.Next(2) == 0 ? "X" : "Y";

        people.Add(
            new Individual(
                "Person " + (people.Count + 1),
                GenerateRandomBinaryString(22),
                gender
            ));
    }

    private static void EditPersons(List<Trait> traits, List<Individual> individuals)
    {
        string[] namesArray = individuals.Select(ind => ind.Name).ToArray();
        string backElement = "Zurück";
        string[] menuItems = new string[namesArray.Length + 1];
        Array.Copy(namesArray, menuItems, namesArray.Length);
        menuItems[menuItems.Length - 1] = backElement;

        int selectedIndex = 0;
        bool exit = false;

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
                    if (menuItems.Length - 1 == selectedIndex)
                    {
                        exit = true;
                    }
                    else
                    {
                        EditPerson(individuals[selectedIndex], traits);
                    }

                    break;
            }
        }
    }

    private static void EditPerson(Individual person, List<Trait> traits)
    {
        string[] traitsArray = traits.Select(trait => FormatTraitName(trait, person)).ToArray();
        string backElement = "Zurück";
        string[] menuItems = new string[traitsArray.Length + 1];
        Array.Copy(traitsArray, menuItems, traitsArray.Length);
        menuItems[menuItems.Length - 1] = backElement;

        int selectedIndex = 0;
        bool exit = false;

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
                    if (menuItems.Length - 1 == selectedIndex)
                    {
                        exit = true;
                    }
                    else
                    {
                        ToggleTrait(person, traits[selectedIndex]);
                        menuItems[selectedIndex] = FormatTraitName(traits[selectedIndex], person);
                    }

                    break;
            }
        }

        Console.WriteLine("Bearbeitung abgeschlossen. Drücke eine Taste, um fortzufahren.");
        Console.ReadKey();
    }

    private static string FormatTraitName(Trait trait, Individual person)
    {
        return trait.Name.PadRight(30) + (trait.IsPresent(person) ? "X" : " ");
    }

    private static void ToggleTrait(Individual person, Trait trait)
    {
        // Hier könnten wir die Logik implementieren, um das Genom der Person zu ändern
        // um das Vorhandensein des Merkmals zu ändern.
        // Für jetzt, als Platzhalter, geben wir nur eine Nachricht aus:
        Console.WriteLine($"Toggling trait: {trait.Name} for person: {person.Name}");
    }

    private static void Procreate(List<Individual> people)
    {
        if (people.Count < 2)
        {
            Console.WriteLine("Es müssen mindestens zwei Personen existieren, um Nachkommen zu erzeugen.");
            Console.ReadKey();
            return;
        }

        string[] namesArray = people.Select(ind => ind.Name).ToArray();
        string backElement = "Zurück";
        string[] menuItems = new string[namesArray.Length + 1];
        Array.Copy(namesArray, menuItems, namesArray.Length);
        menuItems[menuItems.Length - 1] = backElement;

        int selectedIndex = 0;
        bool parent1Selected = false;
        Individual parent1 = null;
        Individual parent2 = null;
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Wähle zwei Elternteile für die Kreuzung:");

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
                    if (menuItems.Length - 1 == selectedIndex)
                    {
                        exit = true;
                    }
                    else
                    {
                        if (!parent1Selected)
                        {
                            parent1 = people[selectedIndex];
                            parent1Selected = true;
                            menuItems[selectedIndex] += " (Parent 1)";
                        }
                        else
                        {
                            parent2 = people[selectedIndex];
                            exit = true;
                        }
                    }

                    break;
            }
        }

        if (parent1 != null && parent2 != null)
        {
            var child = Individual.Cross(parent1, parent2);
            people.Add(child);
            Console.WriteLine($"Nachkomme {child.Name} wurde von {parent1.Name} und {parent2.Name} erzeugt.");
        }
        else
        {
            Console.WriteLine("Kreuzung abgebrochen.");
        }
    }
}
