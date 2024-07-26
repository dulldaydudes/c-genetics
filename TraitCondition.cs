namespace Genetics;

public class TraitCondition
{
    public int Position { get; set; }
    public int Length { get; set; }
    public string? Contains { get; set; }
    public string? EqualValue { get; set; }
    public string? Operator { get; set; }
}