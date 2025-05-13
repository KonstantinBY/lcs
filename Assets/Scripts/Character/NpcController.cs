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

    private Action currentCallbackAction1;
    private Action currentCallbackAction2;

    public float stopDistance = 0.1f; // Допуск для остановки

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    internal void moveToPlayer(Action callbackAction1 = null, Action callbackAction2 = null)
    {
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
    }

    void Update()
    {
        if (isProcessOrder && isMoveToPlayer && targetToPlayer != null && agent.isOnNavMesh)
        {
            agent.SetDestination(targetToPlayer.position);

            bool isMoving = agent.remainingDistance > stopDistance && agent.velocity.sqrMagnitude > 0.01f;
            animator.SetBool("Moving", isMoving);

            if (math.distancesq(targetToPlayer.position, transform.position) <= stopDistance)
            {
                isMoveToPlayer = false;
                isMoveToNpcPosition = true;
                
                currentCallbackAction1?.Invoke();
            }
        }
        
        if (isProcessOrder && isMoveToNpcPosition && targetNpcPosition != null && agent.isOnNavMesh)
        {
            agent.SetDestination(targetNpcPosition.position);

            bool isMoving = agent.remainingDistance > stopDistance && agent.velocity.sqrMagnitude > 0.01f;
            animator.SetBool("Moving", isMoving);

            if (math.distancesq(targetNpcPosition.position, transform.position) <= stopDistance)
            {
                isMoveToPlayer = true;
                isMoveToNpcPosition = false;
                
                isProcessOrder = false;
                
                currentCallbackAction2?.Invoke();
            }
        }
    }
}
