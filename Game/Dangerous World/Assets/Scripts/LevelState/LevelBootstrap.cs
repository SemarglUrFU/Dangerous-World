using UnityEngine;
using UnityEngine.Events;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private UnityEvent OnLoad;
    [SerializeField] private UnityEvent OnExit;
    public static LevelBootstrap Instance {get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }
    
}
