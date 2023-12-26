using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [Space]
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip _extraJump;
    [SerializeField] private AudioClip _grounded;
    [SerializeField] private AudioClip _dash;
    [SerializeField] private AudioClip _death;
    [SerializeField] private AudioClip[] _step;

    private bool _isGrounded;
    private bool _isDead;
    private readonly System.Random _random = new();

    public void Grounding(bool grounded)
    {
        _isGrounded = grounded;
        if (grounded && !_isDead) { _audioSource.PlayOneShot(_grounded); }
    }
    public void Dash(bool started) { if (started) { _audioSource.PlayOneShot(_dash); } }
    public void Jump(int state)
    {
        if (state == 1) { _audioSource.PlayOneShot(_jump); }
        else if (state == 2) { _audioSource.PlayOneShot(_extraJump); }
    }
    public void Move(float velocity)
    {
        if (_isGrounded && !_isDead && !_audioSource.isPlaying && Mathf.Abs(velocity) > 0.01f)
        {
            _audioSource.PlayOneShot(_step[_random.Next(0, _step.Length - 1)]);
        }
    }
    public void Death() { _isDead = true; _audioSource.PlayOneShot(_death); }
    public void Rewive() { _isDead = false; }

    private void OnValidate()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }
}
