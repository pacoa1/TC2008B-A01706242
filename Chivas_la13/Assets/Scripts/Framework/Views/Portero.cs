using UnityEngine;
using System.Collections;

/// <summary>
/// This class makes the goalkeeper dive to the left, stay in the center or dive to the right.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class Portero : MonoBehaviour
{
    public float distanciaClavado = 1.5f;
    public float duracionClavado = 0.4f;

    /// <summary>
    /// This method starts the dive: -1 = left, 0 = center, 1 = right
    /// </summary>
    public void Lanzarse(int lado)
    {
        StartCoroutine(Clavado(lado));
    }

    /// <summary>
    /// This coroutine moves and tilts the goalkeeper a little each frame towards the chosen side
    /// </summary>
    private IEnumerator Clavado(int lado)
    {
        Vector3 inicio = transform.position;
        Vector3 destino = inicio + new Vector3(lado * distanciaClavado, -0.4f * Mathf.Abs(lado), 0f);
        float tiempo = 0f;

        while (tiempo < duracionClavado)
        {
            transform.position = Vector3.Lerp(inicio, destino, tiempo / duracionClavado);
            transform.Rotate(Vector3.forward, -lado * 70f * Time.deltaTime / duracionClavado);
            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.position = destino;
    }
}