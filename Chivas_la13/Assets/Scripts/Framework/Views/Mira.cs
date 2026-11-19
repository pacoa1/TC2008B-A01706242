using UnityEngine;

/// <summary>
/// This class moves the target on the goal: first from side to side to choose the direction, then up and down to choose the height.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class Mira : MonoBehaviour
{
    public float velocidad = 6f;
    public float alcance = 4.6f;
    public float velocidadVertical = 2f;
    public float alturaMinima = 0.25f;
    public float alturaMaxima = 3.2f;

    private Vector3 origen;
    private bool moviendoLado = true;
    private bool moviendoAltura = false;
    private float tiempoAltura = 0f;

    /// <summary>
    /// This method is called before the first frame update and saves the starting position of the target
    /// </summary>
    void Start()
    {
        origen = transform.position;
    }

    /// <summary>
    /// This method is called once per frame and moves the target from side to side or up and down using PingPong
    /// </summary>
    void Update()
    {
        if (moviendoLado)
        {
            float x = origen.x + Mathf.PingPong(Time.time * velocidad, alcance * 2f) - alcance;
            transform.position = new Vector3(x, origen.y, origen.z);
        }
        else if (moviendoAltura)
        {
            tiempoAltura += Time.deltaTime;
            float y = alturaMinima + Mathf.PingPong(tiempoAltura * velocidadVertical + origen.y - alturaMinima, alturaMaxima - alturaMinima);
            transform.position = new Vector3(transform.position.x, y, origen.z);
        }
    }

    /// <summary>
    /// This method stops the side movement and starts moving the target up and down from its current height
    /// </summary>
    public void FijarDireccion()
    {
        moviendoLado = false;
        moviendoAltura = true;
        tiempoAltura = 0f;
    }

    /// <summary>
    /// This method stops the up and down movement, so the target keeps the chosen height
    /// </summary>
    public void FijarAltura()
    {
        moviendoAltura = false;
    }

    /// <summary>
    /// This method stops every movement of the target, for example when the time to shoot runs out
    /// </summary>
    public void Detener()
    {
        moviendoLado = false;
        moviendoAltura = false;
    }

    /// <summary>
    /// This method makes the target move from side to side again for the next shot
    /// </summary>
    public void Reiniciar()
    {
        moviendoLado = true;
        moviendoAltura = false;
    }
}