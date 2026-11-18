/// <summary>
/// This class applies the rules of a penalty kick to decide if the shot is a goal, a save, a post, the crossbar or out.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class EvaluarTiroRequirement
{
    private const float LIMITE_POSTE = 3f;
    private const float LIMITE_FUERA = 3.2f;
    private const float ALCANCE_PORTERO = 0.9f;
    private const float ALCANCE_TIRO_DEBIL = 1.45f;
    private const float POTENCIA_DEBIL = 0.35f;
    private const float POTENCIA_TRAVESANO = 0.75f;
    private const float POTENCIA_VOLADA = 0.85f;

    /// <summary>
    /// This method returns the result of the shot using its direction, its power and where the goalkeeper ended his dive
    /// </summary>
    public string Evaluar(float direccion, float potencia, float posicionPortero)
    {
        if (potencia > POTENCIA_VOLADA) return Constants.FUERA;

        float distancia = direccion < 0 ? -direccion : direccion;

        if (distancia > LIMITE_FUERA) return Constants.FUERA;
        if (distancia >= LIMITE_POSTE) return Constants.POSTE;
        if (potencia > POTENCIA_TRAVESANO) return Constants.TRAVESANO;

        float diferencia = direccion - posicionPortero;
        float distanciaAlPortero = diferencia < 0 ? -diferencia : diferencia;
        float alcance = potencia < POTENCIA_DEBIL ? ALCANCE_TIRO_DEBIL : ALCANCE_PORTERO;

        return distanciaAlPortero < alcance ? Constants.ATAJADA : Constants.GOL;
    }
}