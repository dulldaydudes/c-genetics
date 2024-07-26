namespace Genetics;

using System.IO;
using System.Text.Json;

public class TraitAnalyzer
{
    public static (List<Trait> Traits, int MaxGenomeLength) GetMaxGenomeLength(string jsonFilePath)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        string json = File.ReadAllText(jsonFilePath);
        Console.WriteLine($"JSON Inhalt: {json}");
        TraitsData? traitsData = null;

        try
        {
            traitsData = JsonSerializer.Deserialize<TraitsData>(json, options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (traitsData == null)
        {
            Console.WriteLine("Deserialisierung fehlgeschlagen: traitsData ist null.");
            throw new InvalidOperationException("Die deserialisierten Trait-Daten sind null.");
        }

        if (traitsData.Traits == null)
        {
            Console.WriteLine("Deserialisierung fehlgeschlagen: traitsData.Traits ist null.");
            throw new InvalidOperationException("Die Liste der Traits ist null.");
        }

        int maxLength = 0;
        foreach (var trait in traitsData.Traits)
        {
            Console.WriteLine($"Trait: {trait.Name}");
            foreach (var condition in trait.Present)
            {
                Console.WriteLine(
                    $"  Condition - Position: {condition.Position}, Length: {condition.Length}, Contains: {condition.Contains}, Equals: {condition.EqualValue}, Operator: {condition.Operator}");

                int endPosition = condition.Position + condition.Length;
                if (endPosition > maxLength)
                {
                    maxLength = endPosition;
                }
            }
        }

        return (traitsData.Traits, maxLength);
    }
}
