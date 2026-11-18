using UnityEngine;

/// <summary>
/// This class decides if the rival scores its penalty kick using a probability.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class TiroRivalRequirement
{
    /// <summary>
    /// This method returns true if the rival scores, using the given probability from 0 to 1
    /// </summary>
    public bool Anota(float probabilidad)
    {
        return Random.Range(0f, 1f) < probabilidad;
    }
}