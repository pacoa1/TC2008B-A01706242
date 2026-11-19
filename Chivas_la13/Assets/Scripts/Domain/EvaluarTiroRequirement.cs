/// <summary>
/// This class applies the rules of a penalty kick to decide if the shot is a goal, a save, a post, the crossbar or out, using the point of the goal where the ball arrives.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class EvaluarTiroRequirement
{
    private const float LIMITE_POSTE = 3.85f;
    private const float LIMITE_FUERA = 4.35f;
    private const float LIMITE_TRAVESANO = 2.45f;
    private const float LIMITE_ARRIBA = 2.95f;
    private const float ALCANCE_PORTERO = 0.9f;
    private const float ALCANCE_TIRO_DEBIL = 1.45f;
    private const float ALTURA_DE_PIE = 2.4f;
    private const float ALTURA_CLAVADO = 1.8f;
    private const float ALTURA_EXTRA_TIRO_DEBIL = 0.4f;
    private const float POTENCIA_DEBIL = 0.35f;
    private const float POTENCIA_FUERTE = 0.75f;
    private const float ELEVACION_POR_FUERZA = 5f;

    /// <summary>
    /// This method returns the real height of the ball at the goal: a very strong shot goes higher than where the player aimed
    /// </summary>
    public float AlturaFinal(float altura, float potencia)
    {
        if (potencia <= POTENCIA_FUERTE) return altura;
        return altura + (potencia - POTENCIA_FUERTE) * ELEVACION_POR_FUERZA;
    }

    /// <summary>
    /// This method returns the result of the shot using the point where the ball arrives, its power, where the goalkeeper ended his dive and the side he chose
    /// </summary>
    public string Evaluar(float direccion, float altura, float potencia, float posicionPortero, int ladoPortero)
    {
        float distancia = direccion < 0 ? -direccion : direccion;

        if (distancia > LIMITE_FUERA || altura > LIMITE_ARRIBA) return Constants.FUERA;
        if (distancia >= LIMITE_POSTE) return Constants.POSTE;
        if (altura >= LIMITE_TRAVESANO) return Constants.TRAVESANO;

        bool debil = potencia < POTENCIA_DEBIL;
        float diferencia = direccion - posicionPortero;
        float distanciaAlPortero = diferencia < 0 ? -diferencia : diferencia;
        float alcance = debil ? ALCANCE_TIRO_DEBIL : ALCANCE_PORTERO;
        float alturaAlcance = ladoPortero == 0 ? ALTURA_DE_PIE : ALTURA_CLAVADO;
        if (debil) alturaAlcance += ALTURA_EXTRA_TIRO_DEBIL;

        if (distanciaAlPortero < alcance && altura < alturaAlcance) return Constants.ATAJADA;
        return Constants.GOL;
    }
}