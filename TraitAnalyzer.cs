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
        TraitsData? traitsData;

        try
        {
            traitsData = JsonSerializer.Deserialize<TraitsData>(json, options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (traitsData?.Traits == null)
        {
            throw new InvalidOperationException("Die deserialisierten Trait-Daten sind null.");
        }

        int maxLength = 0;
        foreach (var trait in traitsData.Traits)
        {
            foreach (var condition in trait.Present)
            {
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
