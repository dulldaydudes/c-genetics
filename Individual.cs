namespace Genetics;

public class Individual
{
    public string Name { get; private set; }
    public string Genome { get; private set; }
    public string Gender { get; private set; }

    public Individual(
        string name,
        string genome,
        string gender
    ) {
        Name = name;
        Genome = genome;
        Gender = gender;
    }

    public string GetGenome()
    {
        return Genome;
    }

    public static Individual Cross(
        Individual parent1,
        Individual parent2
    ) {
        var rand = new Random();
        var newGenome = "";

        // Kreuzung: zufällige Auswahl eines Allels von einem der Eltern
        for (int i = 0; i < parent1.Genome.Length; i++)
        {
            newGenome += rand.Next(0, 2) == 0 ? parent1.Genome[i] : parent2.Genome[i];
        }

        // Zufällig das Geschlecht des Kindes bestimmen
        var newGender = rand.Next(0, 2) == 0 ? "X" : "Y";

        return new Individual("Child of " + parent1.Name + " and " + parent2.Name, newGenome, newGender);
    }
}
