using System;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
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

    public float stopDistance = 0.1f; // Допуск для остановки

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        agent.updateRotation = false;
        agent.autoBraking = false;
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
        if (isProcessOrder && isMoveToPlayer && targetToPlayer != null && agent.isOnNavMesh)
        {
            if (!isMovingInProgress)
            {
                agent.updateRotation = true;
                agent.SetDestination(targetToPlayer.position);
                isMovingInProgress = true;
                agent.updateRotation = true;
                
                Debug.Log($"isMoveToPlayer = {isMoveToPlayer}");
            }

            bool isMoving = agent.remainingDistance > stopDistance && agent.velocity.sqrMagnitude > 0.01f;
            animator.SetBool("Moving", isMoving);

            if (!isMoving && math.distancesq(targetToPlayer.position, transform.position) <= stopDistance)
            {
                isMovingInProgress = false;
                agent.updateRotation = false;
                
                if (!lookToDirect(targetToPlayer, 200))
                {
                    return;
                }
                
                isMoveToPlayer = false;
                isMoveToNpcPosition = true;
                
                currentCallbackAction1?.Invoke();
            }
        }
        
        if (isProcessOrder && isMoveToNpcPosition && targetNpcPosition != null && agent.isOnNavMesh)
        {
            if (!isMovingInProgress)
            {
                agent.updateRotation = true;
                agent.SetDestination(targetNpcPosition.position);
                isMovingInProgress = true;
                agent.updateRotation = true;
            }

            bool isMoving = agent.remainingDistance > stopDistance && agent.velocity.sqrMagnitude > 0.01f;
            animator.SetBool("Moving", isMoving);

            if (!isMoving && math.distancesq(targetNpcPosition.position, transform.position) < stopDistance)
            {
                isMovingInProgress = false;
                agent.updateRotation = false;
                
                if (!lookToDirect(targetNpcPosition, 200))
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
