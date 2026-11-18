/// <summary>
/// This class remembers the progress and the results of the tournament even when the scene is loaded again.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public static class Torneo
{
    /// <summary>
    /// Position of the current round in the list of rounds: 0 = quarterfinal, 1 = semifinal, 2 = final
    /// </summary>
    public static int rondaActual = 0;

    /// <summary>
    /// Shootouts finished in the whole tournament, counting the ones that were lost
    /// </summary>
    public static int tandasJugadas = 0;

    /// <summary>
    /// Goals scored by the player in the whole tournament
    /// </summary>
    public static int golesTotales = 0;

    /// <summary>
    /// This method sets the tournament back to the start, before the quarterfinal
    /// </summary>
    public static void Reiniciar()
    {
        rondaActual = 0;
        tandasJugadas = 0;
        golesTotales = 0;
    }
}