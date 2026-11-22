using UnityEngine;

/// <summary>
/// This class plays the sounds of the shootout: the narration after each shot of the player, the reaction of the fans after each shot of the rival and at the end, and pauses the background chant while any of them is playing.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class SonidosPartida : MonoBehaviour
{
    public GestorPartida gestor;
    public AudioSource fuente;
    public AudioSource porra;
    public AudioClip golNormal;
    public AudioClip[] golesMuerteSubita;
    public AudioClip[] fallas;
    public AudioClip aplausos;
    public AudioClip chiflidos;

    private float duracionUltimoClip = 0f;
    private float tiempoPorraPausada = 0f;

    /// <summary>
    /// Seconds that the last clip lasts, so the game can wait for it to finish
    /// </summary>
    public float DuracionUltimoClip
    {
        get { return duracionUltimoClip; }
    }

    /// <summary>
    /// This method subscribes to the shots of both teams and to the end of the shootout
    /// </summary>
    void OnEnable()
    {
        GestorPartida.OnTiroJugador += SonarTiro;
        GestorPartida.OnTiroRival += SonarTiroRival;
        GestorPartida.OnTandaTerminada += SonarFinal;
    }

    /// <summary>
    /// This method unsubscribes from the shots of both teams and from the end of the shootout
    /// </summary>
    void OnDisable()
    {
        GestorPartida.OnTiroJugador -= SonarTiro;
        GestorPartida.OnTiroRival -= SonarTiroRival;
        GestorPartida.OnTandaTerminada -= SonarFinal;
    }

    /// <summary>
    /// This method is called once per frame and resumes the background chant when the last clip is over
    /// </summary>
    void Update()
    {
        if (tiempoPorraPausada <= 0) return;

        tiempoPorraPausada -= Time.deltaTime;
        if (tiempoPorraPausada <= 0) porra.UnPause();
    }

    /// <summary>
    /// This method chooses the narration that matches the result of the player's shot
    /// </summary>
    void SonarTiro(string resultado)
    {
        if (resultado != Constants.GOL)
        {
            Reproducir(fallas[Random.Range(0, fallas.Length)]);
            return;
        }

        if (gestor.TirosJugador > gestor.tirosRegulares) Reproducir(golesMuerteSubita[Random.Range(0, golesMuerteSubita.Length)]);
        else Reproducir(golNormal);
    }

    /// <summary>
    /// This method makes the fans whistle when the rival scores and applaud when the rival misses
    /// </summary>
    void SonarTiroRival(bool anoto)
    {
        Reproducir(anoto ? chiflidos : aplausos);
    }

    /// <summary>
    /// This method makes the fans applaud if Chivas won or whistle if it lost, only when the shootout ended on the player's shot because the rival's shot already has its own reaction
    /// </summary>
    void SonarFinal(bool gano)
    {
        if (gestor.TirosRival >= gestor.TirosJugador) return;

        Reproducir(gano ? aplausos : chiflidos);
    }

    /// <summary>
    /// This method plays a clip once, saves how long it lasts and pauses the background chant while it plays
    /// </summary>
    void Reproducir(AudioClip clip)
    {
        fuente.PlayOneShot(clip);
        duracionUltimoClip = clip.length;

        if (clip.length > tiempoPorraPausada) tiempoPorraPausada = clip.length;
        porra.Pause();
    }
}
