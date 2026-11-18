/// <summary>
/// This class remembers the progress of the tournament even when the scene is loaded again.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public static class Torneo
{
    /// <summary>
    /// Position of the current round in the list of rounds: 0 = quarterfinal, 1 = semifinal, 2 = final
    /// </summary>
    public static int rondaActual = 0;
}