using UnityEngine;

/// <summary>
/// This class makes the power bar grow and shrink so the player can choose the shot power.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class BarraPotencia : MonoBehaviour
{
    public float velocidad = 1.5f;
    public float alturaMaxima = 1.5f;
    public bool activa = false;

    /// <summary>
    /// Current shot power, from 0 (empty bar) to 1 (full bar)
    /// </summary>
    public float Potencia
    {
        get { return transform.localScale.y / alturaMaxima; }
    }

    /// <summary>
    /// This method hides the bar again so it starts empty on the next shot
    /// </summary>
    public void Reiniciar()
    {
        activa = false;
        transform.localScale = new Vector3(1f, 0f, 1f);
    }

    /// <summary>
    /// This method is called once per frame and changes the height of the bar using PingPong
    /// </summary>
    void Update()
    {
        if (!activa) return;

        float altura = Mathf.PingPong(Time.time * velocidad, alturaMaxima);
        transform.localScale = new Vector3(1f, altura, 1f);
    }
}