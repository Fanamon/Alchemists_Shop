using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LeavingQueueAnimation : MonoBehaviour
{
    private const string LeavingLeft = "LeavingLeft";
    private const string LeavingRight = "LeavingRight";
    private const string Idle = "Idle";

    private System.Random _random = new System.Random();

    private Animator _animator;

    private string[] _leavingAnimations = { LeavingLeft, LeavingRight };

    public bool IsStarted { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        IsStarted = false;
    }

    public void StartAnimation()
    {
        IsStarted = true;
        StartCoroutine(Leave());
    }

    private string GetRandomAnimatorStateName()
    {
        return _leavingAnimations[_random.Next(_leavingAnimations.Length)];
    }

    private IEnumerator Leave()
    {
        string animatorStateName = GetRandomAnimatorStateName();

        _animator.Play(animatorStateName);

        do
        {
            yield return null;
        }
        while (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != Idle);

        IsStarted = false;
    }
}