using UnityEngine;

/// <summary>
/// This class moves the aim marker from side to side across the goal so the player can choose the shot direction.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class Mira : MonoBehaviour
{
    public float velocidad = 6f;
    public float alcance = 4f;
    public bool activa = true;

    private Vector3 origen;

    /// <summary>
    /// This method is called before the first frame update and saves the starting position of the marker
    /// </summary>
    void Start()
    {
        origen = transform.position;
    }

    /// <summary>
    /// This method is called once per frame and moves the marker between -alcance and alcance using PingPong
    /// </summary>
    void Update()
    {
        if (!activa) return;

        float desfase = Mathf.PingPong(Time.time * velocidad, alcance * 2f) - alcance;
        transform.position = origen + new Vector3(desfase, 0f, 0f);
    }
}