using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelInputValidatorListener : MonoBehaviour
{
    void Start()
    {
        InputValidator.AllTextAreValid += fieldCount =>
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            print(buildIndex);
            SceneManager.LoadScene(buildIndex+1);
        };
    }
}