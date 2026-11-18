using UnityEngine;

public class ControlTiro : MonoBehaviour
{
    public Mira mira;
    public BarraPotencia barra;

    // 0 = eligiendo dirección, 1 = eligiendo potencia, 2 = tiro en curso
    private int fase = 0;

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (fase == 0)
        {
            mira.activa = false;
            barra.activa = true;
            fase = 1;
        }
        else if (fase == 1)
        {
            barra.activa = false;
            fase = 2;
            Debug.Log("Dirección: " + mira.transform.position.x + " | Potencia: " + barra.Potencia);
        }
    }
}