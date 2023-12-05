using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private SceneAsset _scene;

    void Start()
    {
        // ? This may come in handy later
        SceneManager.LoadScene(_scene.name);
    }
}
