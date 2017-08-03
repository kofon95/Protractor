using UnityEngine;

public class NextLevelInputValidatorListener : MonoBehaviour
{
    void Start()
    {
        InputValidator.AllTextAreValid += fieldCount =>
        {
            LevelJumper.NextLevel();
        };
    }
}