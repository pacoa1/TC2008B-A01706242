/// <summary>
/// This class applies the official rules of a penalty shootout to know if one team has already won.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class DecidirTandaRequirement
{
    /// <summary>
    /// This method returns 1 if the player already won, -1 if the rival already won and 0 if the shootout continues
    /// </summary>
    public int Evaluar(int golesJugador, int golesRival, int tirosJugador, int tirosRival, int tirosRegulares)
    {
        if (tirosJugador <= tirosRegulares && tirosRival <= tirosRegulares)
        {
            int restantesJugador = tirosRegulares - tirosJugador;
            int restantesRival = tirosRegulares - tirosRival;

            if (golesJugador > golesRival + restantesRival) return 1;
            if (golesRival > golesJugador + restantesJugador) return -1;
            return 0;
        }

        if (tirosJugador != tirosRival) return 0;
        if (golesJugador > golesRival) return 1;
        if (golesRival > golesJugador) return -1;
        return 0;
    }
}