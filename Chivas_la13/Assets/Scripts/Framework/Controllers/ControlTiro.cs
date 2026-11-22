using UnityEngine;

/// <summary>
/// This class controls the order of the shot, the time limit of each shot, the animations of the shooter and the timing of each moment of the round.
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
    public Animator tirador;
    public SonidosPartida sonidos;
    public float tiempoContacto = 0.9f;
    public float tiempoResultado = 1.5f;
    public float tiempoAntesRival = 2f;
    public float tiempoSuspenso = 1.5f;
    public float tiempoDespuesRival = 2f;

    /// <summary>
    /// Current phase of the shot: 0 = choosing direction, 1 = choosing height, 2 = choosing power, 3 = shot in progress
    /// </summary>
    private int fase = 0;

    private float tiempoRestante;
    private float direccionActual;
    private float alturaActual;
    private float potenciaActual;
    private string resultadoActual;
    private EvaluarTiroRequirement evaluarTiro = new EvaluarTiroRequirement();

    /// <summary>
    /// Current phase of the shot, so the screen can tell the player what the space key does
    /// </summary>
    public int Fase
    {
        get { return fase; }
    }

    /// <summary>
    /// Seconds left to take the current shot
    /// </summary>
    public float TiempoRestante
    {
        get { return tiempoRestante; }
    }

    /// <summary>
    /// True when the round has a time limit and the player is still choosing the direction, the height or the power
    /// </summary>
    public bool HayTiempoLimite
    {
        get { return gestor.Ronda.tiempoLimite > 0 && fase < 3; }
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
    /// This method is called once per frame, counts down the time limit and moves to the next phase each time the space key is pressed; while the game is paused it ignores the keyboard
    /// </summary>
    void Update()
    {
        if (Time.timeScale == 0) return;

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
            mira.FijarDireccion();
            fase = 1;
        }
        else if (fase == 1)
        {
            mira.FijarAltura();
            barra.activa = true;
            fase = 2;
        }
        else if (fase == 2)
        {
            barra.activa = false;
            fase = 3;

            direccionActual = mira.transform.position.x;
            alturaActual = mira.transform.position.y;
            potenciaActual = barra.Potencia;

            tirador.SetTrigger("Patear");
            Invoke("Disparar", tiempoContacto);
        }
    }

    /// <summary>
    /// This method kicks the ball to the chosen point and makes the goalkeeper dive at the moment the foot of the shooter touches the ball
    /// </summary>
    private void Disparar()
    {
        int ladoPortero = Random.Range(-1, 2);
        float posicionPortero = portero.PosicionFinal(ladoPortero);
        float alturaFinal = evaluarTiro.AlturaFinal(alturaActual, potenciaActual);
        resultadoActual = evaluarTiro.Evaluar(direccionActual, alturaFinal, potenciaActual, posicionPortero, ladoPortero);

        balon.Patear(direccionActual, alturaFinal, potenciaActual, resultadoActual);
        portero.Lanzarse(ladoPortero);

        Invoke("RegistrarResultado", tiempoResultado);
    }

    /// <summary>
    /// This method ends the shot as a miss when the player runs out of time
    /// </summary>
    private void SeAcaboElTiempo()
    {
        mira.Detener();
        barra.activa = false;
        fase = 3;
        resultadoActual = Constants.TIEMPO;
        RegistrarResultado();
    }

    /// <summary>
    /// This method sends the result of the player's shot once the ball has reached its destination, makes the shooter celebrate a goal and waits for the narration to finish before the rival shoots or the final message appears
    /// </summary>
    private void RegistrarResultado()
    {
        gestor.RegistrarTiroJugador(resultadoActual);
        if (resultadoActual == Constants.GOL) tirador.SetTrigger("Festejar");

        if (gestor.Terminada) Invoke("AnunciarFinal", Espera(tiempoAntesRival));
        else Invoke("AnunciarRival", Espera(tiempoAntesRival));
    }

    /// <summary>
    /// This method clears the pitch for the rival: puts the ball and the goalkeeper back in place, hides the target and empties the power bar, then announces that the rival is going to shoot, to build suspense
    /// </summary>
    private void AnunciarRival()
    {
        balon.Reiniciar();
        portero.Reiniciar();
        OcultarIndicadores();

        gestor.AnunciarTiroRival();
        Invoke("TirarRival", tiempoSuspenso);
    }

    /// <summary>
    /// This method plays the rival's shot and, once the reaction of the fans is over, prepares the next shot or shows the final message
    /// </summary>
    private void TirarRival()
    {
        gestor.TirarRival();

        if (gestor.Terminada) Invoke("AnunciarFinal", Espera(tiempoDespuesRival));
        else Invoke("PrepararSiguienteTiro", Espera(tiempoDespuesRival));
    }

    /// <summary>
    /// This method hides the target and the power bar and shows the final message of the shootout once the last shot has been seen
    /// </summary>
    private void AnunciarFinal()
    {
        OcultarIndicadores();
        gestor.AnunciarFinal();
    }

    /// <summary>
    /// This method puts the shooter back, shows the target again and restarts the time limit so the player can shoot again; the ball, the goalkeeper and the bar were already put back before the rival's shot
    /// </summary>
    private void PrepararSiguienteTiro()
    {
        if (resultadoActual == Constants.GOL) tirador.SetTrigger("Reiniciar");

        tirador.transform.localPosition = Vector3.zero;
        tirador.transform.localRotation = Quaternion.identity;

        mira.gameObject.SetActive(true);
        mira.Reiniciar();
        fase = 0;
        tiempoRestante = gestor.Ronda.tiempoLimite;
        gestor.AnunciarTiroJugador();
    }

    /// <summary>
    /// This method hides the target and empties the power bar while the player is not shooting
    /// </summary>
    private void OcultarIndicadores()
    {
        mira.gameObject.SetActive(false);
        barra.Reiniciar();
    }

    /// <summary>
    /// This method returns how long to wait before the next moment: the length of the last clip, or the given time if the clip is shorter
    /// </summary>
    private float Espera(float minimo)
    {
        return sonidos.DuracionUltimoClip > minimo ? sonidos.DuracionUltimoClip : minimo;
    }
}