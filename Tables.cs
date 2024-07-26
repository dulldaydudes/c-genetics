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
    
    public static void ShowPerson(
        Individual currentPerson,
        List<Trait> traits
    )
    {
        Console.Clear();
        Console.WriteLine("Name:".PadRight(Program.PadRightDistance) + currentPerson.Name);
        Console.WriteLine("Sex:".PadRight(Program.PadRightDistance) + currentPerson.Sex);
        Console.WriteLine("Genome:".PadRight(Program.PadRightDistance) + currentPerson.Genome);
     
        foreach (var trait in traits)
        {
            Console.Write(trait.Name.PadRight(Program.PadRightDistance));
            Console.Write((trait.IsPresent(currentPerson) ? "X" : " "));
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.ReadKey();
    }
}