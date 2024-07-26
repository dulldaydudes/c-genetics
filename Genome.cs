namespace Genetics;

using System;
using System.Linq;

public class Genome
{
    private string _sequence;
    public string Sequence => _sequence;
    public int Length => _sequence.Length;

    public Genome(int length)
    {
        _sequence = GenerateRandomBinaryString(length);
    }

    public Genome(string sequence)
    {
        if (!IsValidSequence(sequence))
        {
            throw new ArgumentException("Invalid genome sequence.");
        }
        _sequence = sequence;
    }

    private static string GenerateRandomBinaryString(int length)
    {
        Random random = new Random();
        char[] binaryString = new char[length];

        for (int i = 0; i < length; i++)
        {
            binaryString[i] = random.Next(2) == 0 ? '0' : '1';
        }

        return new string(binaryString);
    }

    private static bool IsValidSequence(string sequence)
    {
        return sequence.All(c => c == '0' || c == '1');
    }

    public Genome RecombineWith(Genome other)
    {
        int length = Math.Min(this.Length, other.Length);
        Random random = new Random();
        char[] childGenome = new char[length];

        for (int i = 0; i < length; i++)
        {
            childGenome[i] = random.Next(2) == 0 ? this._sequence[i] : other._sequence[i];
        }

        return new Genome(new string(childGenome));
    }

    public string GetSegment(int position, int length)
    {
        return _sequence.Substring(position, length);
    }

    public void UpdateSegment(int position, string segment)
    {
        if (position < 0 || position + segment.Length > _sequence.Length || !IsValidSequence(segment))
        {
            throw new ArgumentException("Invalid segment or position.");
        }

        _sequence = _sequence.Remove(position, segment.Length).Insert(position, segment);
    }
}
