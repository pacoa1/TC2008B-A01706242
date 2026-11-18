using UnityEngine;

public class Mira : MonoBehaviour
{
    public float velocidad = 6f;
    public float alcance = 4f;
    public bool activa = true;

    private Vector3 origen;

    void Start()
    {
        origen = transform.position;
    }

    void Update()
    {
        if (!activa) return;

        // PingPong va de 0 a alcance*2; al restar alcance queda centrado en el origen
        float desfase = Mathf.PingPong(Time.time * velocidad, alcance * 2f) - alcance;
        transform.position = origen + new Vector3(desfase, 0f, 0f);
    }
}