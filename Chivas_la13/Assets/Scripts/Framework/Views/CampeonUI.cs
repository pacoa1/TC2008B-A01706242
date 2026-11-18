using UnityEngine;
using TMPro;

/// <summary>
/// This class shows the results of the tournament on the champion screen.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class CampeonUI : MonoBehaviour
{
    public TextMeshProUGUI textoEstadisticas;

    /// <summary>
    /// This method is called before the first frame update and writes the results of the tournament
    /// </summary>
    void Start()
    {
        textoEstadisticas.text = $"EL CAMINO AL TÍTULO:   3 DE 3 RONDAS   |   {Torneo.tandasJugadas} TANDAS JUGADAS   |   {Torneo.golesTotales} GOLES";
    }
}