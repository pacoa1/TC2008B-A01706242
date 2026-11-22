using UnityEngine;

/// <summary>
/// This class fills the stands with fans, one row on each step, using red shirts on the red steps and white shirts on the white steps, so each fan stands out against the step behind.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class GestorAficion : MonoBehaviour
{
    public GameObject aficionadoRojo;
    public GameObject aficionadoBlanco;
    public int filas = 4;
    public int aficionadosPorFila = 18;
    public float inicioX = -17f;
    public float separacionX = 2f;
    public float inicioZ = 16.75f;
    public float alturaEscalon = 1.5f;
    public float fondoEscalon = 1.5f;
    public float variacion = 0.3f;

    /// <summary>
    /// This method is called before the first frame update and places every row of fans on top of its step
    /// </summary>
    void Start()
    {
        bool escalonRojo = true;

        for (int fila = 0; fila < filas; fila++)
        {
            GameObject aficionado = escalonRojo ? aficionadoRojo : aficionadoBlanco;
            float altura = (fila + 1) * alturaEscalon;
            float z = inicioZ + fila * fondoEscalon;

            for (int i = 0; i < aficionadosPorFila; i++)
            {
                float x = inicioX + i * separacionX + Random.Range(-variacion, variacion);
                Instantiate(aficionado, new Vector3(x, altura, z), Quaternion.identity);
            }

            escalonRojo = !escalonRojo;
        }
    }
}
