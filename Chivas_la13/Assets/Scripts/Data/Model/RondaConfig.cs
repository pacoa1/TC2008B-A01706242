using UnityEngine;

/// <summary>
/// This class keeps the data of one round of the tournament, like the rival, how often it scores and the messages for the end of the round.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
[CreateAssetMenu(fileName = "Ronda", menuName = "Penales/Ronda")]
public class RondaConfig : ScriptableObject
{
    public string nombreRonda;
    public string nombreRival;
    public float probabilidadRival;
    public string mensajeVictoria;
    public string mensajeDerrota;
    public string mensajeReintento;
}