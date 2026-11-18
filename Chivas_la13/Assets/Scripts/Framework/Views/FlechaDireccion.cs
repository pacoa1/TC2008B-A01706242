using UnityEngine;

/// <summary>
/// This class points the arrow from the ball to the place where the shot is going, and shows it only while the player is aiming or choosing the power.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class FlechaDireccion : MonoBehaviour
{
    public Mira mira;
    public BarraPotencia barra;
    public GameObject modelo;
    public float distanciaPorteria = 8f;

    /// <summary>
    /// This method is called once per frame, shows or hides the arrow and turns it towards the marker on the goal line
    /// </summary>
    void Update()
    {
        bool visible = mira.activa || barra.activa;
        modelo.SetActive(visible);
        if (!visible) return;

        Vector3 objetivo = new Vector3(mira.transform.position.x, transform.position.y, distanciaPorteria);
        transform.rotation = Quaternion.LookRotation(objetivo - transform.position);
    }
}