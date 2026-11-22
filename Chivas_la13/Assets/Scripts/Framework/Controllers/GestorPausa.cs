using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class pauses and resumes the match with the escape key and handles the buttons of the pause menu.
/// Standard coding documentation can be found in
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class GestorPausa : MonoBehaviour
{
    public GameObject panelPausa;

    private bool pausado = false;

    /// <summary>
    /// This method is called once per frame and pauses or resumes the match each time the escape key is pressed
    /// </summary>
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (pausado) Continuar();
        else Pausar();
    }

    /// <summary>
    /// This method stops the time of the match and shows the pause menu
    /// </summary>
    public void Pausar()
    {
        pausado = true;
        Time.timeScale = 0f;
        panelPausa.SetActive(true);
    }

    /// <summary>
    /// This method hides the pause menu and lets the time of the match run again
    /// </summary>
    public void Continuar()
    {
        pausado = false;
        Time.timeScale = 1f;
        panelPausa.SetActive(false);
    }

    /// <summary>
    /// This method starts the current round again from the first shot
    /// </summary>
    public void ReiniciarRonda()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.ESCENA_PARTIDO);
    }

    /// <summary>
    /// This method leaves the match and goes back to the main menu
    /// </summary>
    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.ESCENA_MENU);
    }
}
