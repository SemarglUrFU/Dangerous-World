using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static int lastSceneIndex = -1;

    public void LoadScene(int sceneIndex)
    {

        lastSceneIndex = SceneManager.GetActiveScene().buildIndex;


        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadLastScene()
    {
        if (lastSceneIndex != -1)
        {
            SceneManager.LoadScene(lastSceneIndex);
        }
    }
}
