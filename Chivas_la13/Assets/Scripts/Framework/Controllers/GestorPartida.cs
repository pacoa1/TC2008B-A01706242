using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class keeps the score of the penalty shootout, plays the rival's shots and decides who wins.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class GestorPartida : MonoBehaviour
{
    public int rondasTotales = 5;
    public float probabilidadRival = 0.25f;
    public float tiempoParaReiniciar = 3f;

    private int golesJugador = 0;
    private int golesRival = 0;
    private int rondasJugadas = 0;
    private bool terminada = false;
    private TiroRivalRequirement tiroRival = new TiroRivalRequirement();

    /// <summary>
    /// True when the shootout already has a winner and no more shots are accepted
    /// </summary>
    public bool Terminada
    {
        get { return terminada; }
    }

    /// <summary>
    /// This method saves the result of the player's shot, plays the rival's shot and checks if the shootout is over
    /// </summary>
    public void RegistrarTiro(string resultado)
    {
        if (terminada) return;

        if (resultado == Constants.GOL) golesJugador++;

        bool anotoRival = tiroRival.Anota(probabilidadRival);
        if (anotoRival) golesRival++;

        rondasJugadas++;

        Debug.Log("Ronda " + rondasJugadas + " | Chivas: " + resultado + " | Rival: " + (anotoRival ? Constants.GOL : "Falla"));
        Debug.Log("Marcador: Chivas " + golesJugador + " - " + golesRival + " Rival");

        if (rondasJugadas >= rondasTotales && golesJugador != golesRival)
        {
            if (golesJugador > golesRival) Ganar();
            else Perder();
        }
    }

    /// <summary>
    /// This method ends the shootout as a win
    /// </summary>
    void Ganar()
    {
        terminada = true;
        Debug.Log("¡Ganaste la tanda!");
    }

    /// <summary>
    /// This method ends the shootout as a loss and restarts the scene after a few seconds
    /// </summary>
    void Perder()
    {
        terminada = true;
        Debug.Log("Perdiste la tanda");
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