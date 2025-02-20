using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject levelSelect;

    private void Awake()
    {
        levelSelect.SetActive(false);
    }

    public void PlayButton()
    {
        levelSelect.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SelectLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
