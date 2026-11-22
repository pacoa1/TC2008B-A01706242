using UnityEngine;

/// <summary>
/// This class makes a fan jump for a few seconds every time Chivas scores a goal.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class Aficionado : MonoBehaviour
{
    public float alturaSalto = 0.3f;
    public float velocidad = 1.5f;
    public float duracionFestejo = 3f;

    private float alturaInicial;
    private float desfase;
    private float tiempoFestejo = 0f;

    /// <summary>
    /// This method subscribes the fan to the result of the player's shot
    /// </summary>
    void OnEnable()
    {
        GestorPartida.OnTiroJugador += Festejar;
    }

    /// <summary>
    /// This method unsubscribes the fan from the result of the player's shot
    /// </summary>
    void OnDisable()
    {
        GestorPartida.OnTiroJugador -= Festejar;
    }

    /// <summary>
    /// This method is called before the first frame update, saves the height of the fan and gives it its own rhythm so not all fans jump together
    /// </summary>
    void Start()
    {
        alturaInicial = transform.position.y;
        desfase = Random.Range(0f, 1f);
    }

    /// <summary>
    /// This method is called once per frame and, while the celebration lasts, moves the fan up and down
    /// </summary>
    void Update()
    {
        if (tiempoFestejo <= 0) return;

        tiempoFestejo -= Time.deltaTime;

        float y = alturaInicial;
        if (tiempoFestejo > 0) y += Mathf.PingPong((Time.time + desfase) * velocidad, alturaSalto);

        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    /// <summary>
    /// This method starts the celebration when the player's shot is a goal
    /// </summary>
    private void Festejar(string resultado)
    {
        if (resultado != Constants.GOL) return;

        tiempoFestejo = duracionFestejo;
    }
}
