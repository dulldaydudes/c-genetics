namespace Genetics;

public class Trait
{
    public string Name { get; set; }
    public Func<Individual, bool> Expression { get; set; }

    public Trait(string name, Func<Individual, bool> expression)
    {
        Name = name;
        Expression = expression;
    }

    public bool IsPresent(Individual individual)
    {
        return Expression(individual);
    }
}
