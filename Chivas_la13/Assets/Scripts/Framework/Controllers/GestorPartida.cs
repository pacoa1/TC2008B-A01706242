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

    public RondaConfig[] rondas;
    public int tirosRegulares = 5;
    public float tiempoParaReiniciar = 3f;
    public float tiempoParaAvanzar = 3f;

    private int golesJugador = 0;
    private int golesRival = 0;
    private int tirosJugador = 0;
    private int tirosRival = 0;
    private bool terminada = false;
    private TiroRivalRequirement tiroRival = new TiroRivalRequirement();
    private DecidirTandaRequirement decidirTanda = new DecidirTandaRequirement();

    /// <summary>
    /// Data of the round that is being played right now
    /// </summary>
    public RondaConfig Ronda
    {
        get { return rondas[Torneo.rondaActual]; }
    }

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
    /// This method plays the rival's shot using the probability of the current round and checks if the shootout is over
    /// </summary>
    public void TirarRival()
    {
        if (terminada) return;

        tirosRival++;
        bool anoto = tiroRival.Anota(Ronda.probabilidadRival);
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
    /// This method ends the shootout as a win and moves to the next round after a few seconds
    /// </summary>
    void Ganar()
    {
        terminada = true;
        OnTandaTerminada?.Invoke(true);
        Invoke("SiguienteRonda", tiempoParaAvanzar);
    }

    /// <summary>
    /// This method ends the shootout as a loss and restarts the same round after a few seconds
    /// </summary>
    void Perder()
    {
        terminada = true;
        OnTandaTerminada?.Invoke(false);
        Invoke("Reiniciar", tiempoParaReiniciar);
    }

    /// <summary>
    /// This method moves the tournament to the next round, unless the final was already won
    /// </summary>
    void SiguienteRonda()
    {
        if (Torneo.rondaActual >= rondas.Length - 1) return;

        Torneo.rondaActual++;
        Reiniciar();
    }

    /// <summary>
    /// This method loads the current scene again to start a new shootout
    /// </summary>
    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}