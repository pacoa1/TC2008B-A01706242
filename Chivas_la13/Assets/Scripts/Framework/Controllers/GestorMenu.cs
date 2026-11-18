using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the buttons of the menus: start the tournament, go back to the main menu or close the game.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class GestorMenu : MonoBehaviour
{
    /// <summary>
    /// This method starts a new tournament from the quarterfinal
    /// </summary>
    public void Jugar()
    {
        Torneo.Reiniciar();
        SceneManager.LoadScene(Constants.ESCENA_PARTIDO);
    }

    /// <summary>
    /// This method goes back to the main menu
    /// </summary>
    public void VolverAlMenu()
    {
        SceneManager.LoadScene(Constants.ESCENA_MENU);
    }

    /// <summary>
    /// This method closes the game; inside the editor it only writes a message, because Unity does not let the game close the editor
    /// </summary>
    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}