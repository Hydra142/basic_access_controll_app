using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace SafeMessenge.Helpers;

public class CartesianProduct
{
    public static IEnumerable<IEnumerable<T>> GetProduct<T>(IEnumerable<IEnumerable<T>> sequences, CancellationToken cancellationToken)
    {
        if (!sequences.Any())
        {
            yield return Enumerable.Empty<T>();
            yield break;
        }

        var currentSequence = sequences.First();
        var remainingSequences = sequences.Skip(1).ToArray();

        foreach (var item in currentSequence)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var subSequences = GetProduct(remainingSequences, cancellationToken);
            foreach (var subSequence in subSequences)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return new[] { item }.Concat(subSequence);
            }
        }
    }

    public static IEnumerable<string> GetStringCombinations(string charset, int length, CancellationToken cancellationToken)
    {
        var sequences = new List<IEnumerable<char>>();
        for (int i = 0; i < length; i++)
        {
            sequences.Add(charset);
        }

        var product = GetProduct(sequences, cancellationToken);

        foreach (var combination in product)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return new string(combination.ToArray());
        }
    }

    //public static void Main(string[] args)
    //{
    //    string charset = "abc";
    //    int length = 3;
    //    var cts = new CancellationTokenSource();

    //    // Cancel after 2 seconds
    //    cts.CancelAfter(2000);

    //    try
    //    {
    //        foreach (string combination in GetStringCombinations(charset, length, cts.Token))
    //        {
    //            Console.WriteLine(combination);
    //        }
    //    }
    //    catch (OperationCanceledException)
    //    {
    //        Console.WriteLine("Operation cancelled.");
    //    }
    //}
}

