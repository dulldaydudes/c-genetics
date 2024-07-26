namespace Genetics;

using System;
using System.Collections.Generic;

public class Trait
{
    public string Name { get; set; }
    public List<Condition> Present { get; set; }

    public bool IsPresent(Individual individual)
    {
        foreach (var condition in Present)
        {
            string segment = individual.Genome.GetSegment(condition.Position, condition.Length);
            if (condition.Contains != null && !segment.Contains(condition.Contains))
            {
                return false;
            }
        }
        return true;
    }
}
