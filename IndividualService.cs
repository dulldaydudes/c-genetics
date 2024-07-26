namespace Genetics;

using System;
using System.Collections.Generic;
using System.Linq;

public class IndividualService
{
    private static readonly ConsoleColor[] HighlightColors =
    {
        ConsoleColor.DarkBlue, // 1. Contains
        ConsoleColor.DarkRed, // 2. Contains (Not)
        ConsoleColor.DarkGreen, // 3. Equals
        ConsoleColor.Magenta, // 4. Equals (Not)
        ConsoleColor.Cyan, // 5. Selected, Contains
        ConsoleColor.Yellow, // 6. Selected, Contains (Not)
        ConsoleColor.Gray, // 7. Selected, Equals
        ConsoleColor.White // 8. Selected, Equals (Not)
    };

    public static void CreatePerson(List<Individual> people, int genomeLength)
    {
        Console.WriteLine("Geben Sie den Namen der neuen Person ein:");
        string name = Console.ReadLine();
        var individual = new Individual(name, genomeLength);
        people.Add(individual);
    }

    public static void EditPersons(List<Trait> traits, List<Individual> individuals)
    {
        // Den Namen aller Personen abrufen und das Menü anzeigen
        string[] names = individuals.Select(ind => ind.Name).ToArray();

        // Menü anzeigen und die ausgewählte Person abrufen
        int selectedIndex = DisplayMenu(names, "Zurück");

        if (selectedIndex >= 0 && selectedIndex < individuals.Count)
        {
            // Bearbeiten der ausgewählten Person
            EditPerson(individuals[selectedIndex], traits);
        }
        else if (selectedIndex == names.Length) // Wenn die Zurück-Option ausgewählt wurde
        {
            // Zurück zum Hauptmenü
            return;
        }
        else
        {
            // Ungültige Auswahl
            Console.WriteLine("Ungültige Auswahl. Drücken Sie eine Taste, um fortzufahren.");
            Console.ReadKey();
        }
    }

    public static void EditPerson(Individual individual, List<Trait> traits)
    {
        int selectedIndex = 0;
        int genomeIndex = 0;
        // Formatierung der Traits für die Anzeige im Menü
        string[] traitsArray = traits.Select(trait => FormatTraitName(trait, individual)).ToArray();
        string backElement = "Zurück";
        string[] menuItems = new string[traitsArray.Length + 4]; // +4 für Name, Genom, Sex und Zurück

        // Menüpunkte hinzufügen
        menuItems[0] = $"Name".PadRight(Program.PadRightDistance) + ": " + individual.Name;
        menuItems[1] = $"Genom".PadRight(Program.PadRightDistance) + ": " + individual.Genome.Sequence;
        menuItems[2] = $"Sex".PadRight(Program.PadRightDistance) + ": " + (individual.Genome.Sequence.Last() == '1' ? "Y" : "X");

        int traitKey = 3;
        foreach (var trait in traitsArray)
        {
            menuItems[traitKey] = trait;
            traitKey++;
        }
        menuItems[^1] = backElement;

        bool exit = false;
        int menuStep = 1;

        while (!exit)
        {
            Console.Clear();
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(menuItems[i]);
                    Console.ResetColor();
                }
                else if (i == 1)
                {
                    HighlightGenomeSegments(individual, traits, genomeIndex, menuItems[selectedIndex]);
                }
                else
                {
                    Console.WriteLine(menuItems[i]);
                }
            }

            Console.ResetColor();
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    menuStep = selectedIndex == 2 ? 2 : 1;
                    selectedIndex = (selectedIndex == 0) ? menuItems.Length - 1 : selectedIndex - menuStep;
                    break;
                case ConsoleKey.DownArrow:
                    menuStep = selectedIndex == 0 ? 2 : 1;
                    selectedIndex = (selectedIndex == menuItems.Length - menuStep) ? 0 : selectedIndex + menuStep;
                    break;
                case ConsoleKey.LeftArrow:
                    genomeIndex = (genomeIndex == 0) ? individual.Genome.Length - 1 : genomeIndex - 1;
                    break;
                case ConsoleKey.RightArrow:
                    genomeIndex = (genomeIndex == individual.Genome.Length - 1) ? 0 : genomeIndex + 1;
                    break;
                case ConsoleKey.Enter:
                    if (selectedIndex == menuItems.Length - 1) // Zurück
                    {
                        exit = true;
                    }
                    else if (selectedIndex == 0) // Name ändern
                    {
                        ChangePersonName(individual);
                    }
                    else if (selectedIndex == 1) // Genom überspringen
                    {
                        // Das Genom wird hier einfach übersprungen, keine Aktion
                    }
                    else if (selectedIndex == 2) // Geschlecht umschalten
                    {
                        ToggleGender(individual);
                    }
                    else if (selectedIndex >= 3 && selectedIndex < menuItems.Length - 1) // Trait auswählen
                    {
                        var traitIndex = selectedIndex - 3;
                        var trait = traits[traitIndex];
                        ToggleTraitBit(individual, trait);
                    }
                    break;
            }
            menuStep = 1;
        }
    }

    private static string FormatTraitName(Trait trait, Individual individual)
    {
        bool isPresent = trait.IsPresent(individual);
        return $"{trait.Name.PadRight(Program.PadRightDistance)}: {(isPresent ? "X" : "O")}";
    }

    private static void ChangePersonName(Individual individual)
    {
        Console.WriteLine("Welcher neue Name soll verwendet werden?");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
        {
            individual.Name = newName;
        }
    }

    private static void ToggleGender(Individual individual)
    {
        char[] genomeChars = individual.Genome.Sequence.ToCharArray();
        genomeChars[^1] = genomeChars[^1] == '0' ? '1' : '0';
        individual.Genome.UpdateSegment(genomeChars.Length - 1, genomeChars[^1].ToString());
    }

    private static void HighlightGenomeSegments(Individual currentPerson, List<Trait> traits, int genomeIndex, string selectedTraitName)
    {
        // Konvertiere den Genom-String in ein Zeichen-Array
        char[] genomeChars = currentPerson.Genome.Sequence.ToCharArray();
        Console.Write("Genome".PadRight(Program.PadRightDistance) + ": ");

        // Finde das aktuelle Trait basierend auf dem Namen
        Trait? currentTrait = traits.FirstOrDefault(trait => selectedTraitName.StartsWith(trait.Name));

        for (int i = 0; i < genomeChars.Length; i++)
        {
            // Standardfarben zurücksetzen
            Console.ResetColor();

            bool isHighlighted = false;
            int colorIndex = -1; // Index der Farbe basierend auf den Bedingungen

            if (currentTrait != null)
            {
                foreach (var condition in currentTrait.Present)
                {
                    // Sicherstellen, dass die Condition-Variablen nicht null sind
                    if (condition.Contains == null)
                    {
                        continue; // Überspringe die Condition, wenn Contains null ist
                    }

                    // Bereich hervorheben, falls im Segment
                    if (i >= condition.Position && i < condition.Position + condition.Length)
                    {
                        // Bestimme die Endposition des aktuellen Segments
                        int segmentLength = Math.Min(condition.Length, genomeChars.Length - i);

                        // Extrahiere das Segment aus dem Genom
                        string genomeSegment = new string(genomeChars, i, segmentLength);

                        bool conditionMatches;

                        // Überprüfe die Bedingung basierend auf dem Operator
                        if (condition.Operator == "not")
                        {
                            conditionMatches = ContainsValueNot(genomeSegment, condition.Contains);
                            colorIndex = 1; // Farbe für Contains (Not)
                        }
                        else if (condition.Operator == "equals")
                        {
                            conditionMatches = EqualsValue(genomeSegment, condition.Contains);
                            colorIndex = 2; // Farbe für Equals
                        }
                        else
                        {
                            conditionMatches = ContainsValue(genomeSegment, condition.Contains);
                            colorIndex = 0; // Farbe für Contains
                        }

                        // Wähle die Farbe basierend auf dem Ergebnis
                        if (conditionMatches)
                        {
                            colorIndex = colorIndex + 4; // Erhöht den Index für den ausgewählten Zustand
                        }
                    }
                }
            }

            if (i == genomeIndex)
            {
                // Setze die Farbe für die aktuell ausgewählte Position
                colorIndex = 4 + (colorIndex == -1 ? 0 : colorIndex); // Füge die Farbe für die Auswahl hinzu
            }

            // Wende die Farbe an, falls ein gültiger Index gefunden wurde
            if (colorIndex >= 0 && colorIndex < HighlightColors.Length)
            {
                Console.BackgroundColor = HighlightColors[colorIndex];
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            // Ausgabe des aktuellen Zeichens im Genom
            Console.Write(genomeChars[i] == '1' ? 'X' : 'O');

            // Setze die Farben zurück, falls sie angepasst wurden
            if (colorIndex >= 0)
            {
                Console.ResetColor();
            }
        }

        // Setze die Farben zurück
        Console.ResetColor();
        Console.WriteLine();
    }

    private static bool ContainsValue(string genomeSegment, string containsValue)
    {
        return genomeSegment.Contains(containsValue);
    }

    private static bool ContainsValueNot(string genomeSegment, string containsValue)
    {
        return !genomeSegment.Contains(containsValue);
    }

    private static bool EqualsValue(string genomeSegment, string equalValue)
    {
        return genomeSegment.Equals(equalValue);
    }

    private static bool EqualsValueNot(string genomeSegment, string equalValue)
    {
        return !genomeSegment.Equals(equalValue);
    }

    private static void ToggleTraitBit(Individual individual, Trait trait)
    {
        Console.Clear();
        Console.WriteLine($"Bearbeiten des Traits: {trait.Name}");

        // Zeigt die aktuelle Konfiguration der Bedingungen an
        foreach (var condition in trait.Present)
        {
            // Falls Condition ein anderer Typ ist, musst du möglicherweise in TraitCondition konvertieren
            // Zum Beispiel: var traitCondition = (TraitCondition)condition;
            Console.WriteLine($"Position: {condition.Position}, Länge: {condition.Length}, Zustand: {condition.Contains}");
        }

        Console.WriteLine("Drücken Sie die Leertaste, um den Zustand der Bedingungen zu ändern.");

        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.Spacebar)
        {
            foreach (var condition in trait.Present)
            {
                // Falls Condition ein anderer Typ ist, musst du möglicherweise in TraitCondition konvertieren
                // Zum Beispiel: var traitCondition = (TraitCondition)condition;
                ToggleConditionBit(individual, condition);
            }
            Console.WriteLine($"Genome nach Änderung: {individual.Genome.Sequence}");
        }
    }

    private static void ToggleConditionBit(Individual individual, Condition condition)
    {
        // Holt das aktuelle Segment aus dem Genom basierend auf der Bedingung
        var segment = individual.Genome.GetSegment(condition.Position, condition.Length);

        // Umschalten der Bits im Segment
        var toggledSegment = string.Join("", segment.Select(c => c == '1' ? '0' : '1'));

        // Segment aktualisieren
        individual.Genome.UpdateSegment(condition.Position, toggledSegment);
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
        people.Add(new Individual("Kind von " + parent1.Name + " und " + parent2.Name, childGenome));

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

    private static void ToggleBits(Individual individual, int position, int length)
    {
        char[] genomeChars = individual.Genome.Sequence.ToCharArray();
        for (int i = position; i < position + length; i++)
        {
            genomeChars[i] = genomeChars[i] == '0' ? '1' : '0';
        }
        individual.Genome.UpdateSegment(position, new string(genomeChars, position, length));
    }
}
