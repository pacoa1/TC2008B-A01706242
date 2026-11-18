/// <summary>
/// This class applies the rules of a penalty kick to decide if the shot is a goal, a save, a post or out.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class EvaluarTiroRequirement
{
    private const float LIMITE_CENTRO = 1f;
    private const float LIMITE_ESQUINA = 2.4f;
    private const float LIMITE_POSTE = 3f;
    private const float LIMITE_FUERA = 3.2f;
    private const float POTENCIA_DEBIL = 0.35f;
    private const float POTENCIA_VOLADA = 0.85f;

    /// <summary>
    /// This method returns the result of the shot using its direction, its power and the side where the goalkeeper dove
    /// </summary>
    public string Evaluar(float direccion, float potencia, int ladoPortero)
    {
        if (potencia > POTENCIA_VOLADA) return Constants.FUERA;

        float distancia = direccion < 0 ? -direccion : direccion;
        int ladoTiro = direccion < 0 ? -1 : 1;

        if (distancia > LIMITE_FUERA) return Constants.FUERA;
        if (distancia >= LIMITE_POSTE) return Constants.POSTE;

        if (distancia < LIMITE_CENTRO)
        {
            return ladoPortero == 0 ? Constants.ATAJADA : Constants.GOL;
        }

        if (ladoPortero != ladoTiro) return Constants.GOL;
        if (potencia < POTENCIA_DEBIL) return Constants.ATAJADA;

        return distancia < LIMITE_ESQUINA ? Constants.ATAJADA : Constants.GOL;
    }
}