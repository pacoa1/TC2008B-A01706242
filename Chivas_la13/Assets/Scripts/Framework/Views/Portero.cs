using UnityEngine;
using System.Collections;

/// <summary>
/// This class moves the goalkeeper from side to side before the shot, if the round asks for it, makes him dive to the left, the center or the right, and plays the matching animations.
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
    private float xAnterior;
    private bool lanzado = false;
    private Animator animator;

    /// <summary>
    /// This method is called before the first frame update, saves the starting position and finds the animator of the character
    /// </summary>
    void Start()
    {
        posicionInicial = transform.position;
        xAnterior = posicionInicial.x;
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// This method is called once per frame, moves the goalkeeper from side to side using PingPong and tells the animator his speed
    /// </summary>
    void Update()
    {
        if (!seMueve || lanzado) return;

        float x = posicionInicial.x + Mathf.PingPong(Time.time * velocidadMovimiento, alcanceMovimiento * 2f) - alcanceMovimiento;
        transform.position = new Vector3(x, posicionInicial.y, posicionInicial.z);

        animator.SetFloat("Velocidad", (x - xAnterior) / Time.deltaTime);
        xAnterior = x;
    }

    /// <summary>
    /// This method puts the goalkeeper back standing in the middle of the goal for the next shot
    /// </summary>
    public void Reiniciar()
    {
        if (lanzado) animator.SetTrigger("Reiniciar");

        lanzado = false;
        transform.position = posicionInicial;
        xAnterior = posicionInicial.x;
    }

    /// <summary>
    /// This method returns the X position where the goalkeeper will end if he dives to the given side
    /// </summary>
    public float PosicionFinal(int lado)
    {
        return transform.position.x + lado * distanciaClavado;
    }

    /// <summary>
    /// This method stops the side movement, plays the dive or the save animation and starts the dive: -1 = left, 0 = center, 1 = right
    /// </summary>
    public void Lanzarse(int lado)
    {
        lanzado = true;

        if (lado < 0) animator.SetTrigger("ClavadoIzquierda");
        else if (lado > 0) animator.SetTrigger("ClavadoDerecha");
        else animator.SetTrigger("AtajadaCentro");

        StartCoroutine(Clavado(lado));
    }

    /// <summary>
    /// This coroutine moves the goalkeeper a little each frame towards the chosen side, while the animation makes the body dive
    /// </summary>
    private IEnumerator Clavado(int lado)
    {
        Vector3 inicio = transform.position;
        Vector3 destino = inicio + new Vector3(lado * distanciaClavado, 0f, 0f);
        float tiempo = 0f;

        while (tiempo < duracionClavado)
        {
            transform.position = Vector3.Lerp(inicio, destino, tiempo / duracionClavado);
            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.position = destino;
    }
}