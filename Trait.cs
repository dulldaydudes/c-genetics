namespace Genetics;

using System.Collections.Generic;

public class Trait
{
    public string Name { get; set; }
    public List<TraitCondition> Present { get; set; }

    public Trait(string name, List<TraitCondition> present)
    {
        Name = name;
        Present = present;
    }

    public bool IsPresent(Individual individual)
    {
        foreach (var condition in Present)
        {
            var genomeSegment = individual.Genome.Substring(condition.Position, condition.Length);

            bool conditionMet = false;

            if (!string.IsNullOrEmpty(condition.Contains))
            {
                conditionMet = genomeSegment.Contains(condition.Contains);
            }
            else if (!string.IsNullOrEmpty(condition.EqualValue))
            {
                conditionMet = genomeSegment == condition.EqualValue;
            }

            if (condition.Operator == "not")
            {
                conditionMet = !conditionMet;
            }

            if (!conditionMet)
            {
                return false;
            }
        }

        return true;
    }
}