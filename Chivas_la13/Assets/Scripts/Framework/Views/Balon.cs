using UnityEngine;
using System.Collections;

/// <summary>
/// This class moves the ball from the penalty spot to the goal using the chosen direction and power.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class Balon : MonoBehaviour
{
    public float alturaMaxima = 2.3f;
    public float distanciaPorteria = 8.5f;

    /// <summary>
    /// This method starts the shot towards the given direction with the given power
    /// </summary>
    public void Patear(float direccion, float potencia)
    {
        StartCoroutine(Viajar(direccion, potencia));
    }

    /// <summary>
    /// This coroutine moves the ball a little each frame until it reaches the goal; more power means a higher and faster shot
    /// </summary>
    private IEnumerator Viajar(float direccion, float potencia)
    {
        Vector3 inicio = transform.position;
        Vector3 destino = new Vector3(direccion, 0.25f + potencia * alturaMaxima, distanciaPorteria);
        float duracion = 1.2f - potencia * 0.7f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            transform.position = Vector3.Lerp(inicio, destino, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.position = destino;
    }
}