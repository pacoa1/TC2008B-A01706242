using UnityEngine;

/// <summary>
/// This class keeps the data of one round of the tournament, like the rival and how often it scores.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
[CreateAssetMenu(fileName = "Ronda", menuName = "Penales/Ronda")]
public class RondaConfig : ScriptableObject
{
    public string nombreRonda;
    public string nombreRival;
    public float probabilidadRival;
}