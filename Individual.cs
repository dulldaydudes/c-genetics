namespace Genetics;

public class Individual
{
    public string Name { get; set; }
    public string Genome { get; set; }
    public string Gender { get; set; }

    public Individual(string name, string genome, string gender)
    {
        Name = name;
        Genome = genome;
        Gender = gender;
    }

    public static Individual Cross(Individual parent1, Individual parent2)
    {
        if (parent1.Gender == parent2.Gender)
        {
            throw new ArgumentException("Beide Elternteile müssen unterschiedliches Geschlecht haben.");
        }

        string childGenome = CrossGenomes(parent1.Genome, parent2.Genome);
        string childGender = (new Random().Next(2) == 0) ? "X" : "Y";
        return new Individual("Child of " + parent1.Name + " and " + parent2.Name, childGenome, childGender);
    }

    private static string CrossGenomes(string genome1, string genome2)
    {
        Random random = new Random();
        char[] childGenome = new char[genome1.Length];

        for (int i = 0; i < genome1.Length; i++)
        {
            // Randomly select a bit from either parent
            childGenome[i] = (random.Next(2) == 0) ? genome1[i] : genome2[i];
        }

        return new string(childGenome);
    }
}
