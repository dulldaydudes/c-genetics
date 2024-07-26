namespace Genetics;

public class Individual
{
    public string Name { get; set; }
    public Genome Genome { get; set; }
    public string Gender => Genome.Sequence.Last() == '1' ? "Y" : "X";

    public Individual(string name, int genomeLength)
    {
        Name = name;
        Genome = new Genome(genomeLength);
    }

    public Individual(string name, Genome genome)
    {
        Name = name;
        Genome = genome;
    }
}
