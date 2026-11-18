using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// This class keeps the score of the penalty shootout, plays the rival's shots and decides who wins.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class GestorPartida : MonoBehaviour
{
    public static Action<bool> OnJugadorAlPunto;
    public static Action<string> OnTiroJugador;
    public static Action<bool> OnRivalAlPunto;
    public static Action<bool> OnTiroRival;
    public static Action<bool> OnTandaTerminada;

    public string nombreRonda = "CUARTOS DE FINAL";
    public string nombreRival = "GALLOS";
    public int tirosRegulares = 5;
    public float probabilidadRival = 0.25f;
    public float tiempoParaReiniciar = 3f;

    private int golesJugador = 0;
    private int golesRival = 0;
    private int tirosJugador = 0;
    private int tirosRival = 0;
    private bool terminada = false;
    private TiroRivalRequirement tiroRival = new TiroRivalRequirement();
    private DecidirTandaRequirement decidirTanda = new DecidirTandaRequirement();

    /// <summary>
    /// True when the shootout already has a winner and no more shots are accepted
    /// </summary>
    public bool Terminada
    {
        get { return terminada; }
    }

    /// <summary>
    /// Goals scored by the player in this shootout
    /// </summary>
    public int GolesJugador
    {
        get { return golesJugador; }
    }

    /// <summary>
    /// Goals scored by the rival in this shootout
    /// </summary>
    public int GolesRival
    {
        get { return golesRival; }
    }

    /// <summary>
    /// Shots already taken by the player
    /// </summary>
    public int TirosJugador
    {
        get { return tirosJugador; }
    }

    /// <summary>
    /// Shots already taken by the rival
    /// </summary>
    public int TirosRival
    {
        get { return tirosRival; }
    }

    /// <summary>
    /// This method announces the player's next shot and tells if that shot can decide the shootout
    /// </summary>
    public void AnunciarTiroJugador()
    {
        bool decisivo = decidirTanda.Evaluar(golesJugador + 1, golesRival, tirosJugador + 1, tirosRival, tirosRegulares) != 0
                     || decidirTanda.Evaluar(golesJugador, golesRival, tirosJugador + 1, tirosRival, tirosRegulares) != 0;
        OnJugadorAlPunto?.Invoke(decisivo);
    }

    /// <summary>
    /// This method saves the result of the player's shot and checks if the shootout is over
    /// </summary>
    public void RegistrarTiroJugador(string resultado)
    {
        if (terminada) return;

        tirosJugador++;
        if (resultado == Constants.GOL) golesJugador++;

        OnTiroJugador?.Invoke(resultado);
        RevisarFinal();
    }

    /// <summary>
    /// This method announces the rival's next shot and tells if that shot can decide the shootout
    /// </summary>
    public void AnunciarTiroRival()
    {
        bool decisivo = decidirTanda.Evaluar(golesJugador, golesRival + 1, tirosJugador, tirosRival + 1, tirosRegulares) != 0
                     || decidirTanda.Evaluar(golesJugador, golesRival, tirosJugador, tirosRival + 1, tirosRegulares) != 0;
        OnRivalAlPunto?.Invoke(decisivo);
    }

    /// <summary>
    /// This method plays the rival's shot and checks if the shootout is over
    /// </summary>
    public void TirarRival()
    {
        if (terminada) return;

        tirosRival++;
        bool anoto = tiroRival.Anota(probabilidadRival);
        if (anoto) golesRival++;

        OnTiroRival?.Invoke(anoto);
        RevisarFinal();
    }

    /// <summary>
    /// This method asks the rules if there is already a winner and ends the shootout if so
    /// </summary>
    void RevisarFinal()
    {
        int ganador = decidirTanda.Evaluar(golesJugador, golesRival, tirosJugador, tirosRival, tirosRegulares);

        if (ganador == 1) Ganar();
        else if (ganador == -1) Perder();
    }

    /// <summary>
    /// This method ends the shootout as a win
    /// </summary>
    void Ganar()
    {
        terminada = true;
        OnTandaTerminada?.Invoke(true);
    }

    /// <summary>
    /// This method ends the shootout as a loss and restarts the scene after a few seconds
    /// </summary>
    void Perder()
    {
        terminada = true;
        OnTandaTerminada?.Invoke(false);
        Invoke("Reiniciar", tiempoParaReiniciar);
    }

    /// <summary>
    /// This method loads the current scene again to start a new shootout
    /// </summary>
    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}