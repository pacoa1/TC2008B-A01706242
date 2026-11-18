using UnityEngine;

public class BarraPotencia : MonoBehaviour
{
    public float velocidad = 1.5f;
    public float alturaMaxima = 1.5f;
    public bool activa = false;

    public float Potencia
    {
        get { return transform.localScale.y / alturaMaxima; }
    }

    void Update()
    {
        if (!activa) return;

        float altura = Mathf.PingPong(Time.time * velocidad, alturaMaxima);
        transform.localScale = new Vector3(1f, altura, 1f);
    }
}