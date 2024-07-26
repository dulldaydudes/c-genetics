namespace Genetics;

public class Individual
{
    public string Name { get; set; }
    public Genome Genome { get; set; }
    public string Gender { get; set; }

    public Individual(string name, int genomeLength, string gender)
    {
        Name = name;
        Genome = new Genome(genomeLength);
        Gender = gender;
    }

    public Individual(string name, Genome genome, string gender)
    {
        Name = name;
        Genome = genome;
        Gender = gender;
    }
}
