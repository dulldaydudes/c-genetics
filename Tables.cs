namespace Genetics;

public class Tables
{
    public static void ShowTraitTable(
        string title,
        List<Trait> traits,
        List<Individual> individuals
    )
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine("Trait\t\t\t" + string.Join("\t", individuals.ConvertAll(p => p.Name)));
        foreach (var trait in traits)
        {
            Console.Write(trait.Name.PadRight(Program.PadRightDistance));
            foreach (var person in individuals)
            {
                Console.Write((trait.IsPresent(person) ? "X" : " ") + "\t\t");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
        Console.ReadKey();
    }
    
    public static void ShowPerson(Individual individual, List<Trait> traits)
    {
        Console.Clear();
        Console.WriteLine($"Name: {individual.Name}");
        Console.WriteLine($"Genome: {individual.Genome.Sequence}");
        Console.WriteLine($"Gender: {individual.Gender}");
        
        foreach (var trait in traits)
        {
            Console.WriteLine($"{trait.Name}: {(trait.IsPresent(individual) ? "X" : "O")}");
        }

        Console.ReadKey();
    }
}