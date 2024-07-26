namespace Genetics;

using System;
using System.Collections.Generic;
using System.Linq;

public class IndividualService
{
    public static void CreatePerson(
        List<Individual> people,
        int genomeLength
    )
    {
        Console.Clear();
        Random random = new Random();
        string gender = random.Next(2) == 0 ? "X" : "Y";

        people.Add(
            new Individual(
                "Person " + (people.Count + 1),
                GenerateRandomBinaryString(genomeLength),
                gender
            ));
    }
    
    private static string GenerateRandomBinaryString(int length)
    {
        Random random = new Random();
        char[] binaryString = new char[length];

        for (int i = 0; i < length; i++)
        {
            binaryString[i] = random.Next(2) == 0 ? '0' : '1';
        }

        return new string(binaryString);
    }
    
    public static void ShowPersons(
        List<Trait> traits,
        List<Individual> individuals
    )
    {
        string[] namesArray = individuals.Select(ind => ind.Name).ToArray();
        // string editElement = "Bearbeiten";
        string backElement = "Zurück";
        string[] menuItems = new string[namesArray.Length + 1];
        Array.Copy(namesArray, menuItems, namesArray.Length);
        // menuItems[^2] = editElement;
        menuItems[^1] = backElement;
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
                        Tables.ShowPerson(individuals[selectedIndex], traits);
                    }

                    break;
            }
        }
    }

    public static void EditPersons(
        List<Trait> traits,
        List<Individual> individuals
    )
    {
        string[] namesArray = individuals.Select(ind => ind.Name).ToArray();
        string backElement = "Zurück";
        string[] menuItems = new string[namesArray.Length + 1];
        Array.Copy(namesArray, menuItems, namesArray.Length);
        menuItems[^1] = backElement;

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
        menuItems[^1] = backElement;

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
    }

    private static void ToggleTrait(Individual person, Trait trait)
    {
        Console.WriteLine("I'm in");
        foreach (var condition in trait.Present)
        {
            string segment = person.Genome.Substring(condition.Position, condition.Length);
            if (condition.Contains != null)
            {
                if (segment.Contains(condition.Contains))
                {
                    segment = segment.Replace(condition.Contains, "");
                    person.Genome = person.Genome.Remove(condition.Position, condition.Length).Insert(condition.Position, segment);
                }
                else
                {
                    segment += condition.Contains;
                    person.Genome = person.Genome.Remove(condition.Position, condition.Length).Insert(condition.Position, segment);
                }
            }
        }
    }

    private static string FormatTraitName(Trait trait, Individual person)
    {
        return $"{trait.Name}: {(trait.IsPresent(person) ? "X" : "O")}";
    }

    public static void Procreate(List<Individual> people)
    {
        if (people.Count < 2)
        {
            Console.WriteLine("Es müssen mindestens zwei Personen vorhanden sein, um Nachkommen zu erzeugen.");
            Console.ReadKey();
            return;
        }

        string[] namesArray = people.Select(ind => ind.Name).ToArray();
        string backElement = "Zurück";
        string[] menuItems = new string[namesArray.Length + 1];
        Array.Copy(namesArray, menuItems, namesArray.Length);
        menuItems[^1] = backElement;

        int selectedIndex = 0;
        bool parent1Selected = false;
        Individual? parent1 = null;
        Individual? parent2 = null;
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

        Console.ReadKey();
    }
}