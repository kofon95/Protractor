using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelJumper : MonoBehaviour {

	public void LoadPreviousLevel()
    {
        PreviousLevel();
    }

    private static void PreviousLevel()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        if (index > 0)
            SceneManager.LoadScene(index - 1);
        else
            Debug.LogWarning("first level");
    }

    public void LoadNextLevel()
    {
        NextLevel();
    }
    public static void NextLevel()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        if (index < SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(index + 1);
        else
            Debug.LogWarning("last level");
    }
}
