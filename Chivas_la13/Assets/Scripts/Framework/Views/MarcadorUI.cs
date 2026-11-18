using UnityEngine;
using TMPro;

/// <summary>
/// This class shows the score, the history of shots, the announcements of each moment and the final message on the screen.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class MarcadorUI : MonoBehaviour
{
    public GestorPartida gestor;
    public TextMeshProUGUI textoRonda;
    public TextMeshProUGUI textoMarcador;
    public TextMeshProUGUI textoHistorialChivas;
    public TextMeshProUGUI textoHistorialRival;
    public TextMeshProUGUI textoAnuncio;
    public TextMeshProUGUI textoAviso;

    private string historialChivas = "";
    private string historialRival = "";

    /// <summary>
    /// This method subscribes to the events of the game manager when the object is enabled
    /// </summary>
    private void OnEnable()
    {
        GestorPartida.OnJugadorAlPunto += MostrarJugadorAlPunto;
        GestorPartida.OnTiroJugador += MostrarTiroJugador;
        GestorPartida.OnRivalAlPunto += MostrarRivalAlPunto;
        GestorPartida.OnTiroRival += MostrarTiroRival;
        GestorPartida.OnTandaTerminada += MostrarAviso;
    }

    /// <summary>
    /// This method unsubscribes from the events of the game manager when the object is disabled
    /// </summary>
    private void OnDisable()
    {
        GestorPartida.OnJugadorAlPunto -= MostrarJugadorAlPunto;
        GestorPartida.OnTiroJugador -= MostrarTiroJugador;
        GestorPartida.OnRivalAlPunto -= MostrarRivalAlPunto;
        GestorPartida.OnTiroRival -= MostrarTiroRival;
        GestorPartida.OnTandaTerminada -= MostrarAviso;
    }

    /// <summary>
    /// This method is called before the first frame update and writes the initial texts
    /// </summary>
    void Start()
    {
        textoRonda.text = gestor.ronda.nombreRonda;
        textoAnuncio.text = "";
        ActualizarTextos();
    }

    /// <summary>
    /// This method warns the player when the next shot can decide the shootout
    /// </summary>
    private void MostrarJugadorAlPunto(bool decisivo)
    {
        textoAnuncio.color = Color.white;
        textoAnuncio.text = decisivo ? "¡ESTE PENAL DECIDE TODO!" : "";
    }

    /// <summary>
    /// This method shows the result of the player's shot, green if it was a goal and red if not
    /// </summary>
    private void MostrarTiroJugador(string resultado)
    {
        historialChivas += resultado == Constants.GOL ? "O " : "X ";
        textoAnuncio.color = resultado == Constants.GOL ? Color.green : Color.red;
        textoAnuncio.text = TextoResultado(resultado);
        ActualizarTextos();
    }

    /// <summary>
    /// This method shows that the rival is going to shoot, and warns if that shot can decide the shootout
    /// </summary>
    private void MostrarRivalAlPunto(bool decisivo)
    {
        textoAnuncio.color = Color.white;
        textoAnuncio.text = (decisivo ? "¡ESTE PENAL DECIDE TODO!\n" : "") + gestor.ronda.nombreRival + " AL PUNTO PENAL...";
    }

    /// <summary>
    /// This method shows the result of the rival's shot, red if it scored and green if it missed
    /// </summary>
    private void MostrarTiroRival(bool anoto)
    {
        historialRival += anoto ? "O " : "X ";
        textoAnuncio.color = anoto ? Color.red : Color.green;
        textoAnuncio.text = gestor.ronda.nombreRival + (anoto ? " ¡ANOTA!" : " ¡FALLA!");
        ActualizarTextos();
    }

    /// <summary>
    /// This method shows the final message when the shootout is over
    /// </summary>
    private void MostrarAviso(bool gano)
    {
        textoAviso.color = gano ? Color.green : Color.red;
        textoAviso.text = gano ? "¡GANASTE!" : "PERDISTE";
        textoAviso.gameObject.SetActive(true);
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
        return "¡AFUERA!";
    }

    /// <summary>
    /// This method writes the score and the history of both teams, filling the pending shots with dashes
    /// </summary>
    private void ActualizarTextos()
    {
        textoMarcador.text = $"CHIVAS {gestor.GolesJugador} - {gestor.GolesRival} {gestor.ronda.nombreRival}";
        textoHistorialChivas.text = "CHIVAS  " + historialChivas + Pendientes(gestor.TirosJugador);
        textoHistorialRival.text = gestor.ronda.nombreRival + "  " + historialRival + Pendientes(gestor.TirosRival);
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