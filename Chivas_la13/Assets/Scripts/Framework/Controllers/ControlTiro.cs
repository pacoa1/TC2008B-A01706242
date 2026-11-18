using UnityEngine;

/// <summary>
/// This class controls the order of the shot: first the direction is chosen and then the power.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class ControlTiro : MonoBehaviour
{
    public Mira mira;
    public BarraPotencia barra;
    public Balon balon;
    public Portero portero;

    /// <summary>
    /// Current phase of the shot: 0 = choosing direction, 1 = choosing power, 2 = shot in progress
    /// </summary>
    private int fase = 0;

    /// <summary>
    /// This method is called once per frame and moves to the next phase each time the space key is pressed
    /// </summary>
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
            balon.Patear(mira.transform.position.x, barra.Potencia);

            int ladoPortero = Random.Range(-1, 2);
            portero.Lanzarse(ladoPortero);
        }
    }
}