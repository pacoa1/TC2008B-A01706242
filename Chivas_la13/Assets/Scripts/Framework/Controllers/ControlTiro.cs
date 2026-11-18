using UnityEngine;

/// <summary>
/// This class controls the order of the shot, the time limit of each shot and the timing of each moment of the round.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class ControlTiro : MonoBehaviour
{
    public Mira mira;
    public BarraPotencia barra;
    public Balon balon;
    public Portero portero;
    public GestorPartida gestor;
    public float tiempoResultado = 1.5f;
    public float tiempoAntesRival = 2f;
    public float tiempoSuspenso = 1.5f;
    public float tiempoDespuesRival = 2f;

    /// <summary>
    /// Current phase of the shot: 0 = choosing direction, 1 = choosing power, 2 = shot in progress
    /// </summary>
    private int fase = 0;

    private float tiempoRestante;
    private string resultadoActual;
    private EvaluarTiroRequirement evaluarTiro = new EvaluarTiroRequirement();

    /// <summary>
    /// Seconds left to take the current shot
    /// </summary>
    public float TiempoRestante
    {
        get { return tiempoRestante; }
    }

    /// <summary>
    /// True when the round has a time limit and the player is still choosing the direction or the power
    /// </summary>
    public bool HayTiempoLimite
    {
        get { return gestor.Ronda.tiempoLimite > 0 && fase < 2; }
    }

    /// <summary>
    /// This method is called before the first frame update and applies the difficulty of the current round
    /// </summary>
    void Start()
    {
        mira.velocidad = gestor.Ronda.velocidadMira;
        portero.seMueve = gestor.Ronda.porteroSeMueve;
        tiempoRestante = gestor.Ronda.tiempoLimite;
    }

    /// <summary>
    /// This method is called once per frame, counts down the time limit and moves to the next phase each time the space key is pressed
    /// </summary>
    void Update()
    {
        if (HayTiempoLimite)
        {
            tiempoRestante -= Time.deltaTime;

            if (tiempoRestante <= 0)
            {
                SeAcaboElTiempo();
                return;
            }
        }

        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (fase == 0)
        {
            mira.activa = false;
            barra.activa = true;
            fase = 1;
        }
        else if (fase == 1)
        {
            barra.activa = false;
            fase = 2;

            float direccion = mira.transform.position.x;
            float potencia = barra.Potencia;
            int ladoPortero = Random.Range(-1, 2);
            float posicionPortero = portero.PosicionFinal(ladoPortero);
            resultadoActual = evaluarTiro.Evaluar(direccion, potencia, posicionPortero);

            balon.Patear(direccion, potencia, resultadoActual);
            portero.Lanzarse(ladoPortero);

            Invoke("RegistrarResultado", tiempoResultado);
        }
    }

    /// <summary>
    /// This method ends the shot as a miss when the player runs out of time
    /// </summary>
    void SeAcaboElTiempo()
    {
        mira.activa = false;
        barra.activa = false;
        fase = 2;
        resultadoActual = Constants.TIEMPO;
        RegistrarResultado();
    }

    /// <summary>
    /// This method sends the result of the player's shot once the ball has reached its destination
    /// </summary>
    void RegistrarResultado()
    {
        gestor.RegistrarTiroJugador(resultadoActual);
        if (gestor.Terminada) return;

        Invoke("AnunciarRival", tiempoAntesRival);
    }

    /// <summary>
    /// This method announces that the rival is going to shoot, to build suspense
    /// </summary>
    void AnunciarRival()
    {
        gestor.AnunciarTiroRival();
        Invoke("TirarRival", tiempoSuspenso);
    }

    /// <summary>
    /// This method plays the rival's shot and, if the shootout continues, prepares the next shot
    /// </summary>
    void TirarRival()
    {
        gestor.TirarRival();
        if (gestor.Terminada) return;

        Invoke("PrepararSiguienteTiro", tiempoDespuesRival);
    }

    /// <summary>
    /// This method puts the ball, the goalkeeper, the bar, the marker and the time limit back so the player can shoot again
    /// </summary>
    void PrepararSiguienteTiro()
    {
        balon.Reiniciar();
        portero.Reiniciar();
        barra.Reiniciar();
        mira.activa = true;
        fase = 0;
        tiempoRestante = gestor.Ronda.tiempoLimite;
        gestor.AnunciarTiroJugador();
    }
}