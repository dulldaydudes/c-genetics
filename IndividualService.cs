namespace Genetics;

using System;
using System.Collections.Generic;
using System.Linq;

public class IndividualService
{
    public static void CreatePerson(List<Individual> people, int genomeLength)
    {
        Console.Clear();
        Random random = new Random();
        string gender = random.Next(2) == 0 ? "X" : "Y";

        people.Add(
            new Individual(
                "Person " + (people.Count + 1),
                genomeLength,
                gender
            ));
    }

    public static void ShowPersons(List<Trait> traits, List<Individual> individuals)
    {
        int selectedIndex = DisplayMenu(individuals.Select(ind => ind.Name).ToArray(), "Zurück");
        if (selectedIndex != -1)
        {
            Tables.ShowPerson(individuals[selectedIndex], traits);
        }
    }

    public static void EditPersons(List<Trait> traits, List<Individual> individuals)
    {
        int selectedIndex = DisplayMenu(individuals.Select(ind => ind.Name).ToArray(), "Zurück");
        if (selectedIndex != -1)
        {
            EditPerson(individuals[selectedIndex], traits);
        }
    }

    private static void EditPerson(Individual person, List<Trait> traits)
    {
        int selectedIndex = DisplayMenu(traits.Select(trait => FormatTraitName(trait, person)).ToArray(), "Zurück");
        if (selectedIndex != -1)
        {
            ToggleTrait(person, traits[selectedIndex]);
        }
    }

    private static void ToggleTrait(Individual person, Trait trait)
    {
        foreach (var condition in trait.Present)
        {
            string segment = person.Genome.GetSegment(condition.Position, condition.Length);
            if (condition.Contains != null)
            {
                if (segment.Contains(condition.Contains))
                {
                    segment = segment.Replace(condition.Contains, "");
                    person.Genome.UpdateSegment(condition.Position, segment);
                }
                else
                {
                    segment += condition.Contains;
                    person.Genome.UpdateSegment(condition.Position, segment);
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

        int selectedIndex1 = DisplayMenu(people.Select(ind => ind.Name).ToArray(), "Zurück");
        if (selectedIndex1 == -1) return;

        int selectedIndex2 = DisplayMenu(people.Select(ind => ind.Name).ToArray(), "Zurück");
        if (selectedIndex2 == -1) return;

        var parent1 = people[selectedIndex1];
        var parent2 = people[selectedIndex2];

        if (parent1 == parent2)
        {
            Console.WriteLine("Eine Person kann nicht mit sich selbst Nachkommen zeugen.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"{parent1.Name} und {parent2.Name} haben ein Kind gezeugt.");

        var childGenome = parent1.Genome.RecombineWith(parent2.Genome);
        var gender = new Random().Next(2) == 0 ? "X" : "Y";
        people.Add(new Individual("Kind von " + parent1.Name + " und " + parent2.Name, childGenome, gender));

        Console.ReadKey();
    }

    private static int DisplayMenu(string[] menuItems, string backOption)
    {
        int selectedIndex = 0;
        string[] extendedMenuItems = menuItems.Append(backOption).ToArray();

        while (true)
        {
            Console.Clear();
            for (int i = 0; i < extendedMenuItems.Length; i++)
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
                Console.WriteLine(extendedMenuItems[i]);
            }
            Console.ResetColor();

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex == 0) ? extendedMenuItems.Length - 1 : selectedIndex - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex == extendedMenuItems.Length - 1) ? 0 : selectedIndex + 1;
                    break;
                case ConsoleKey.Enter:
                    return (selectedIndex == extendedMenuItems.Length - 1) ? -1 : selectedIndex;
            }
        }
    }
}
