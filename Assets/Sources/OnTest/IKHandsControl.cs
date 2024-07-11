using System.Collections;
using UnityEngine;

public class IKHandsControl : MonoBehaviour
{
    private const float IKOriginalWeight = 0.0f;
    private const float IKGrabWeight = 1.0f;

    [SerializeField] private Animator _animator;

    [SerializeField] private KeeperPlace _leftHandKeeper;
    [SerializeField] private KeeperPlace _rightHandKeeper;

    [SerializeField] private Transform _leftKeeperHandler;
    [SerializeField] private Transform _rightKeeperHandler;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    private bool _isLeftHandIKActive = false;
    private bool _isRightHandIKActive = false;

    private void OnEnable()
    {
        _leftHandKeeper.Took += OnLeftKeeperTook;
        _leftHandKeeper.GaveAway += OnLeftKeeperGaveAway;

        _rightHandKeeper.Took += OnRightKeeperTook;
        _rightHandKeeper.GaveAway += OnRightKeeperGaveAway;
    }

    private void OnDisable()
    {
        _leftHandKeeper.Took -= OnLeftKeeperTook;
        _leftHandKeeper.GaveAway -= OnLeftKeeperGaveAway;

        _rightHandKeeper.Took -= OnRightKeeperTook;
        _rightHandKeeper.GaveAway -= OnRightKeeperGaveAway;
    }

    /*private void OnAnimatorIK(int layerIndex)
    {
        TrySetIKPositionAndRotation(_isLeftHandIKActive, AvatarIKGoal.LeftHand, _leftKeeperHandler);
        TrySetIKPositionAndRotation(_isRightHandIKActive, AvatarIKGoal.RightHand, _rightKeeperHandler);
    }

    private void TrySetIKPositionAndRotation(bool isHandIKActive, AvatarIKGoal avatarIKGoal, 
        Transform keeperHandler)
    {
        if (isHandIKActive)
        {
            Debug.Log($"Done {avatarIKGoal}");
            _animator.SetIKPositionWeight(avatarIKGoal, IKGrabWeight);
            _animator.SetIKRotationWeight(avatarIKGoal, IKGrabWeight);

            _animator.SetIKPosition(avatarIKGoal, keeperHandler.position);
            _animator.SetIKRotation(avatarIKGoal, keeperHandler.rotation);
        }
        else
        {
            _animator.SetIKPositionWeight(avatarIKGoal, IKOriginalWeight);
            _animator.SetIKRotationWeight(avatarIKGoal, IKOriginalWeight);
        }
    }*/

    private void OnLeftKeeperTook()
    {
        _isLeftHandIKActive = true;

        StartCoroutine(Take(_isLeftHandIKActive, _leftHand, _leftKeeperHandler.position));
    }

    private void OnLeftKeeperGaveAway()
    {
        _isLeftHandIKActive = false;
    }

    private void OnRightKeeperTook()
    {
        _isRightHandIKActive = true;

        StartCoroutine(Take(_isRightHandIKActive, _rightHand, _rightKeeperHandler.position));
    }

    private void OnRightKeeperGaveAway()
    {
        _isRightHandIKActive = false;
    }

    private IEnumerator Take(bool isHandIKActive, Transform hand, Vector3 handlerPosition)
    {
        Debug.Log("Done");

        while(isHandIKActive)
        {
            hand.position = Vector3.MoveTowards(hand.position, handlerPosition, 1);

            yield return null;
        }
    }
}
