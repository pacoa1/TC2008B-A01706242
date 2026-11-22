using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class shows the TV style scoreboard, the history of shots, the time limit, the instructions, the announcements of each moment and the final message on the screen.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class MarcadorUI : MonoBehaviour
{
    public GestorPartida gestor;
    public ControlTiro control;
    public TextMeshProUGUI textoRonda;
    public TextMeshProUGUI textoGolesChivas;
    public TextMeshProUGUI textoGolesRival;
    public TextMeshProUGUI textoNombreRival;
    public Image cajaRival;
    public TextMeshProUGUI textoHistorialChivas;
    public TextMeshProUGUI textoHistorialRival;
    public GameObject fondoTiempo;
    public TextMeshProUGUI textoTiempo;
    public GameObject fondoInstruccion;
    public TextMeshProUGUI textoInstruccion;
    public GameObject fondoAnuncio;
    public TextMeshProUGUI textoAnuncio;
    public Image fondoAviso;
    public TextMeshProUGUI textoAviso;
    public TextMeshProUGUI textoContinuar;
    public Color colorVictoria;
    public Color colorDerrota;

    private string historialChivas = "";
    private string historialRival = "";

    /// <summary>
    /// This method subscribes to the events of the game manager when the object is enabled
    /// </summary>
    void OnEnable()
    {
        GestorPartida.OnJugadorAlPunto += MostrarJugadorAlPunto;
        GestorPartida.OnTiroJugador += MostrarTiroJugador;
        GestorPartida.OnRivalAlPunto += MostrarRivalAlPunto;
        GestorPartida.OnTiroRival += MostrarTiroRival;
        GestorPartida.OnTandaTerminada += MostrarAviso;
        GestorPartida.OnPuedeContinuar += MostrarContinuar;
    }

    /// <summary>
    /// This method unsubscribes from the events of the game manager when the object is disabled
    /// </summary>
    void OnDisable()
    {
        GestorPartida.OnJugadorAlPunto -= MostrarJugadorAlPunto;
        GestorPartida.OnTiroJugador -= MostrarTiroJugador;
        GestorPartida.OnRivalAlPunto -= MostrarRivalAlPunto;
        GestorPartida.OnTiroRival -= MostrarTiroRival;
        GestorPartida.OnTandaTerminada -= MostrarAviso;
        GestorPartida.OnPuedeContinuar -= MostrarContinuar;
    }

    /// <summary>
    /// This method is called before the first frame update, paints the rival box, writes the round and shows the clock only when the round has a time limit
    /// </summary>
    void Start()
    {
        float limite = gestor.Ronda.tiempoLimite;

        textoRonda.text = gestor.Ronda.nombreRonda + (limite > 0 ? $"  ·  {limite:0} SEGUNDOS POR TIRO" : "");
        textoNombreRival.text = gestor.Ronda.abreviaturaRival;

        cajaRival.color = gestor.Ronda.colorRival;
        fondoTiempo.SetActive(limite > 0);
        textoTiempo.text = "";
        textoAnuncio.text = "";
        ActualizarTextos();
    }

    /// <summary>
    /// This method is called once per frame, shows what the space key does now and the seconds left to shoot, in red during the last three seconds
    /// </summary>
    void Update()
    {
        string instruccion = TextoInstruccion(control.Fase);
        textoInstruccion.text = instruccion;
        fondoInstruccion.SetActive(instruccion != "");

        if (!control.HayTiempoLimite) return;

        textoTiempo.color = control.TiempoRestante <= 3 ? colorDerrota : Color.black;
        textoTiempo.text = $"TIEMPO  00:{control.TiempoRestante:00}";
    }

    /// <summary>
    /// This method warns the player when the next shot can decide the shootout
    /// </summary>
    private void MostrarJugadorAlPunto(bool decisivo)
    {
        Anunciar(decisivo ? "¡ESTE PENAL DECIDE TODO!" : "", Color.white, false);
    }

    /// <summary>
    /// This method shows the result of the player's shot, green if it was a goal and red if not
    /// </summary>
    private void MostrarTiroJugador(string resultado)
    {
        historialChivas += resultado == Constants.GOL ? "O " : "X ";
        Anunciar(TextoResultado(resultado), resultado == Constants.GOL ? colorVictoria : colorDerrota, true);
        ActualizarTextos();
    }

    /// <summary>
    /// This method shows that the rival is going to shoot, and warns if that shot can decide the shootout
    /// </summary>
    private void MostrarRivalAlPunto(bool decisivo)
    {
        Anunciar((decisivo ? "¡ESTE PENAL DECIDE TODO!\n" : "") + gestor.Ronda.nombreRival + " AL PUNTO PENAL...", Color.white, true);
    }

    /// <summary>
    /// This method shows the result of the rival's shot, red if it scored and green if it missed
    /// </summary>
    private void MostrarTiroRival(bool anoto)
    {
        historialRival += anoto ? "O " : "X ";
        Anunciar(gestor.Ronda.nombreRival + (anoto ? " ¡ANOTA!" : " ¡FALLA!"), anoto ? colorDerrota : colorVictoria, true);
        ActualizarTextos();
    }

    /// <summary>
    /// This method shows the message of the current round on a green band if Chivas won or a red band if it lost
    /// </summary>
    private void MostrarAviso(bool gano)
    {
        textoAnuncio.text = "";
        textoAviso.text = gano ? gestor.Ronda.mensajeVictoria : gestor.Ronda.mensajeDerrota;
        fondoAviso.color = gano ? colorVictoria : colorDerrota;
        fondoAviso.gameObject.SetActive(true);
        textoAviso.gameObject.SetActive(true);
    }

    /// <summary>
    /// This method writes a message in the middle of the screen and darkens the pitch behind it when the player is only watching
    /// </summary>
    private void Anunciar(string texto, Color color, bool oscurecer)
    {
        textoAnuncio.color = color;
        textoAnuncio.text = texto;
        fondoAnuncio.SetActive(oscurecer);
    }

    /// <summary>
    /// This method tells the player to press space to continue to the next round or to retry the current one
    /// </summary>
    private void MostrarContinuar(bool gano)
    {
        textoContinuar.text = "PRESIONA ESPACIO PARA " + (gano ? "CONTINUAR" : gestor.Ronda.mensajeReintento);
        textoContinuar.gameObject.SetActive(true);
    }

    /// <summary>
    /// This method returns the instruction that matches the current phase of the shot, or nothing while the ball is moving
    /// </summary>
    private string TextoInstruccion(int fase)
    {
        if (fase == 0) return "ESPACIO  ·  ELIGE LA DIRECCIÓN";
        if (fase == 1) return "ESPACIO  ·  ELIGE LA ALTURA";
        if (fase == 2) return "ESPACIO  ·  ELIGE LA FUERZA";
        return "";
    }

    /// <summary>
    /// This method returns the big text that matches the result of the player's shot
    /// </summary>
    private string TextoResultado(string resultado)
    {
        if (resultado == Constants.GOL) return "¡GOOOL!";
        if (resultado == Constants.ATAJADA) return "¡LA ATAJÓ!";
        if (resultado == Constants.POSTE) return "¡AL POSTE!";
        if (resultado == Constants.TRAVESANO) return "¡TRAVESAÑO!";
        if (resultado == Constants.TIEMPO) return "¡SE ACABÓ EL TIEMPO!";
        return "¡AFUERA!";
    }

    /// <summary>
    /// This method writes the goals and the history of both teams, filling the pending shots with dashes
    /// </summary>
    private void ActualizarTextos()
    {
        textoGolesChivas.text = $"{gestor.GolesJugador}";
        textoGolesRival.text = $"{gestor.GolesRival}";
        textoHistorialChivas.text = historialChivas + Pendientes(gestor.TirosJugador);
        textoHistorialRival.text = historialRival + Pendientes(gestor.TirosRival);
    }

    /// <summary>
    /// This method returns a dash for each regular shot that is still pending
    /// </summary>
    private string Pendientes(int tiros)
    {
        string pendientes = "";
        for (int i = tiros; i < gestor.tirosRegulares; i++)
        {
            pendientes += "- ";
        }
        return pendientes;
    }
}
