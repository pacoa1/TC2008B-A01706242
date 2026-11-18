using UnityEngine;
using System.Collections;

/// <summary>
/// This class makes the goalkeeper move from side to side before the shot, if the round asks for it, and dive to the left, the center or the right.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class Portero : MonoBehaviour
{
    public float distanciaClavado = 1.5f;
    public float duracionClavado = 0.4f;
    public bool seMueve = false;
    public float velocidadMovimiento = 1.2f;
    public float alcanceMovimiento = 1f;

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private bool lanzado = false;

    /// <summary>
    /// This method is called before the first frame update and saves the starting position and rotation of the goalkeeper
    /// </summary>
    void Start()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
    }

    /// <summary>
    /// This method is called once per frame and moves the goalkeeper from side to side using PingPong until he dives
    /// </summary>
    void Update()
    {
        if (!seMueve || lanzado) return;

        float x = posicionInicial.x + Mathf.PingPong(Time.time * velocidadMovimiento, alcanceMovimiento * 2f) - alcanceMovimiento;
        transform.position = new Vector3(x, posicionInicial.y, posicionInicial.z);
    }

    /// <summary>
    /// This method puts the goalkeeper back standing in the middle of the goal for the next shot
    /// </summary>
    public void Reiniciar()
    {
        lanzado = false;
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;
    }

    /// <summary>
    /// This method returns the X position where the goalkeeper will end if he dives to the given side
    /// </summary>
    public float PosicionFinal(int lado)
    {
        return transform.position.x + lado * distanciaClavado;
    }

    /// <summary>
    /// This method stops the side movement and starts the dive: -1 = left, 0 = center, 1 = right
    /// </summary>
    public void Lanzarse(int lado)
    {
        lanzado = true;
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