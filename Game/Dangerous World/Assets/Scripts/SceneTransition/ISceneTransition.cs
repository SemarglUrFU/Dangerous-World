using UnityEngine;
using UnityEngine.Events;

public interface ISceneTransition 
{
    public UnityEvent OnCurrentAnimationEnded {get;}
    public GameObject GameObject {get;}
    public void PlayIn();
    public void Hide();
    public void PlayOut();
}

