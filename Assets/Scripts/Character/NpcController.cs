using System;
using ToonPeople;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    public Transform targetNpcPosition;
    public Transform targetToPlayer;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isProcessOrder;
    private bool isMoveToPlayer;
    private bool isMoveToNpcPosition;
    private bool isMovingInProgress;

    private Action currentCallbackAction1;
    private Action currentCallbackAction2;

    private float stopDelayStart;

    public float stopDistance = 0.1f; // Допуск для остановки
    
    private PlayerAnimationController _playerAnimationController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        agent.updateRotation = false;
        agent.autoBraking = false;

        _playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    internal void moveToPlayer(Action callbackAction1 = null, Action callbackAction2 = null)
    {
        Debug.Log("moveToPlayer");
        
        // Wait when waitress return to position
        if (isMoveToNpcPosition)
        {
            return;
        }
        
        isProcessOrder = true;
        isMoveToPlayer = true;
        isMoveToNpcPosition = false;

        if (callbackAction1 != null)
        {
            currentCallbackAction1 = callbackAction1;
        }
        
        if (callbackAction2 != null)
        {
            currentCallbackAction2 = callbackAction2;
        }
        
        agent.updateRotation = true;
    }

    void Update()
    {
        if (moveToTarget())
        {
            return;
        }
        
        moveToBasePosition();
    }

    private bool moveToTarget()
    {
        if (isProcessOrder && isMoveToPlayer && targetToPlayer != null && agent.isOnNavMesh)
        {
            if (!isMovingInProgress)
            {
                agent.updateRotation = true;
                isMovingInProgress = true;
                agent.updateRotation = true;
                
                agent.SetDestination(targetToPlayer.position);
                
                // Debug.Log($"isMoveToPlayer = {isMoveToPlayer}");
            }

            bool isMoving = agent.remainingDistance > stopDistance && agent.velocity.sqrMagnitude > 0.01f;

            if (isMoving)
            {
                _playerAnimationController.playAnimation("TPM_walk1");
            }
            else
            {
                _playerAnimationController.playAnimation("TPM_idle1");
            }
            

            if (!isMoving && math.distancesq(targetToPlayer.position, transform.position) <= stopDistance)
            {
                isMovingInProgress = false;
                agent.updateRotation = false;
                
                if (!lookToDirect(targetToPlayer, 200))
                {
                    return true;
                }
                
                _playerAnimationController.playAnimation("TPM_lookback");
                
                isMoveToPlayer = false;
                isMoveToNpcPosition = true;
                
                currentCallbackAction1?.Invoke();

                stopDelayStart = Time.time;
            }
        }

        return false;
    }
    
    private void moveToBasePosition()
    {
        if (Time.time - stopDelayStart < 2f)
        {
            return;
        }
        
        if (isProcessOrder && isMoveToNpcPosition && targetNpcPosition != null && agent.isOnNavMesh)
        {
            if (!isMovingInProgress)
            {
                agent.updateRotation = true;
                isMovingInProgress = true;
                agent.updateRotation = true;
                
                agent.SetDestination(targetNpcPosition.position);
            }

            bool isMoving = agent.remainingDistance > stopDistance && agent.velocity.sqrMagnitude > 0.01f;
            if (isMoving)
            {
                _playerAnimationController.playAnimation("TPM_walk1");
            }
            else
            {
                _playerAnimationController.playAnimation("TPM_idle1");
            }

            if (!isMoving && math.distancesq(targetNpcPosition.position, transform.position) < stopDistance)
            {
                isMovingInProgress = false;
                agent.updateRotation = false;
                
                if (!lookToDirect(targetNpcPosition, 360))
                {
                    return;
                }
                
                isMoveToPlayer = true;
                isMoveToNpcPosition = false;
                
                isProcessOrder = false;
                
                currentCallbackAction2?.Invoke();
            }
        }
    }

    private bool lookToDirect(Transform target, float rotateSpeed)
    {
        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude < 0.0001f) return true;

        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        Quaternion currentRot = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        Quaternion newRot = Quaternion.RotateTowards(currentRot, targetRot, rotateSpeed * Time.deltaTime);
        transform.rotation = newRot;

        float angle = Quaternion.Angle(currentRot, targetRot);
        
        return angle < 1f;
    }
}
