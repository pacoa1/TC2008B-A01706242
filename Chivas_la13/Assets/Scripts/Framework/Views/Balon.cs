using UnityEngine;
using System.Collections;

/// <summary>
/// This class moves the ball from the penalty spot to the point of the goal where the shot goes, following the path that matches the result.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class Balon : MonoBehaviour
{
    public float lineaDeGol = 8f;
    public float distanciaPorteria = 8.5f;
    public float distanciaPortero = 7.45f;
    public float distanciaPoste = 7.65f;
    public float distanciaFuera = 11f;
    public float duracionRebote = 0.6f;
    public float duracionTiroLento = 0.6f;
    public float duracionTiroRapido = 0.35f;

    private Vector3 posicionInicial;

    /// <summary>
    /// This method is called before the first frame update and saves the position of the penalty spot
    /// </summary>
    void Start()
    {
        posicionInicial = transform.position;
    }

    /// <summary>
    /// This method puts the ball back on the penalty spot for the next shot
    /// </summary>
    public void Reiniciar()
    {
        transform.position = posicionInicial;
    }

    /// <summary>
    /// This method builds the path of the ball to the chosen point of the goal for the given result and starts moving it; a stronger shot takes less time to arrive
    /// </summary>
    public void Patear(float direccion, float altura, float potencia, string resultado)
    {
        float duracion = duracionTiroLento - potencia * (duracionTiroLento - duracionTiroRapido);
        int lado = direccion < 0 ? -1 : 1;

        Vector3[] puntos;
        float[] duraciones;

        if (resultado == Constants.ATAJADA)
        {
            puntos = new Vector3[] { new Vector3(direccion, altura, distanciaPortero), new Vector3(direccion * 0.5f, posicionInicial.y, 4f) };
            duraciones = new float[] { duracion, duracionRebote };
        }
        else if (resultado == Constants.POSTE)
        {
            puntos = new Vector3[] { new Vector3(direccion, altura, distanciaPoste), new Vector3(lado * 5f, posicionInicial.y, 5f) };
            duraciones = new float[] { duracion, duracionRebote };
        }
        else if (resultado == Constants.TRAVESANO)
        {
            puntos = new Vector3[] { new Vector3(direccion, altura, distanciaPoste), new Vector3(direccion * 0.8f, posicionInicial.y, 4f) };
            duraciones = new float[] { duracion, duracionRebote };
        }
        else if (resultado == Constants.FUERA)
        {
            float factor = distanciaFuera / lineaDeGol;
            float alturaFuera = posicionInicial.y + (altura - posicionInicial.y) * factor;
            puntos = new Vector3[] { new Vector3(direccion * factor, alturaFuera, distanciaFuera) };
            duraciones = new float[] { duracion * factor };
        }
        else
        {
            puntos = new Vector3[] { new Vector3(direccion, altura, distanciaPorteria) };
            duraciones = new float[] { duracion };
        }

        StartCoroutine(Recorrer(puntos, duraciones));
    }

    /// <summary>
    /// This coroutine moves the ball through each point of the path, one after the other
    /// </summary>
    private IEnumerator Recorrer(Vector3[] puntos, float[] duraciones)
    {
        for (int i = 0; i < puntos.Length; i++)
        {
            Vector3 inicio = transform.position;
            float tiempo = 0f;

            while (tiempo < duraciones[i])
            {
                transform.position = Vector3.Lerp(inicio, puntos[i], tiempo / duraciones[i]);
                tiempo += Time.deltaTime;
                yield return null;
            }

            transform.position = puntos[i];
        }
    }
}