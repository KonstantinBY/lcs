using RootMotion.FinalIK;
using ToonPeople;
using Unity.Collections;
using UnityEngine;

public class PlayerLimbsController : MonoBehaviour
{
    [SerializeField] private float effectorMoveSpeed = 5f;
    [SerializeField] private float effectorRotationSpeed = 200f;
    
    [SerializeField] private FullBodyBipedIK ik;
    
    [SerializeField] private GameObject stateMain;
    [SerializeField] private GameObject stateIdle;
    [SerializeField] private GameObject stateCallWaitressDrink;
    [SerializeField] private GameObject stateCallWaitressFood;
    [SerializeField] private GameObject stateFly;
    [SerializeField] private GameObject stateBirt;

    private Transform headEffector;
    private Transform bodyEffector;
    [SerializeField, ReadOnly] private Transform rhEffector;
    [SerializeField, ReadOnly] private Transform lhEffector;
    [SerializeField, ReadOnly] private Transform rfEffector;
    [SerializeField, ReadOnly] private Transform lfEffector;
    
    private Transform headTarget;
    private Transform bodyTarget;
    [SerializeField, ReadOnly] private Transform rhTarget;
    [SerializeField, ReadOnly] private Transform lhTarget;    
    [SerializeField, ReadOnly] private Transform rfTarget;
    [SerializeField, ReadOnly] private Transform lfTarget;
    
    private Transform headTargetDefault;
    private Transform bodyTargetDefault;
    [SerializeField, ReadOnly] private Transform rhTargetDefault;
    [SerializeField, ReadOnly] private Transform lhTargetDefault;    
    [SerializeField, ReadOnly] private Transform rfTargetDefault;
    [SerializeField, ReadOnly] private Transform lfTargetDefault;
    
    private PlayerStateEnum currentState;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setMainState(stateMain);
        setDefaultState(stateIdle);
        setState(stateIdle);
    }
    private void setMainState(GameObject stateFolder)
    {
        stateFolder.SetActive(true);
        
        PlayerState playerState = readState(stateFolder);

        headEffector = playerState.head.transform;
        bodyEffector = playerState.body.transform;
        rhEffector = playerState.rightHand.transform;
        lhEffector = playerState.leftHand.transform;        
        rfEffector = playerState.rightFoot.transform;
        lfEffector = playerState.leftFoot.transform;
    }
    
    private void setDefaultState(GameObject stateFolder)
    {
        stateFolder.SetActive(true);
        
        PlayerState playerState = readState(stateFolder);

        headTargetDefault = playerState.head.transform;
        bodyTargetDefault = playerState.body.transform;
        rhTargetDefault = playerState.rightHand.transform;
        lhTargetDefault = playerState.leftHand.transform;
        rfTargetDefault = playerState.rightFoot.transform;
        lfTargetDefault = playerState.leftFoot.transform;
    }
    

    private void Update()
    {
        syncPosition(headEffector, headTarget, headTargetDefault);
        syncPosition(bodyEffector, bodyTarget, bodyTargetDefault);
        syncPosition(rhEffector, rhTarget, rhTargetDefault);
        syncPosition(lhEffector, lhTarget, lhTargetDefault);
        syncPosition(rfEffector, rfTarget, rfTargetDefault);
        syncPosition(lfEffector, lfTarget, lfTargetDefault);
    }

    private void syncPosition(Transform limbTransform, Transform target, Transform targetDefault)
    {
        if (!limbTransform) return;
        if (!target) target = targetDefault;

        // === ДВИЖЕНИЕ ===
        Vector3 toTarget = target.position - limbTransform.position;
        float distance = toTarget.magnitude;

        if (distance > 0.001f)
        {
            Vector3 direction = toTarget.normalized;
            float step = Mathf.Min(effectorMoveSpeed * Time.deltaTime, distance);
            limbTransform.position += direction * step;
        }

        // === ПОВОРОТ — стремиться к повороту target ===
        limbTransform.rotation = Quaternion.RotateTowards(
            limbTransform.rotation,
            target.rotation,
            effectorRotationSpeed * Time.deltaTime
        );
    }


    private void disableAllStateFalders()
    {
        stateIdle.SetActive(false);
        stateCallWaitressDrink.SetActive(false);
        stateCallWaitressFood.SetActive(false);
        stateFly.SetActive(false);
    }

    public void OnSetState(string newState)
    {
        string[] param = newState.Split(",");
        
        if (param.Length > 1)
        {
            float wight;
            float.TryParse(param[1].Trim(), out wight);
            OnSetState(param[0], wight);
        }
        else
        {
            Debug.Log($"params: {newState}");
            OnSetState(newState.Trim(), 0.5f);
        }
    }

    public void OnSetState(string newState, float weight)
    {
        PlayerStateEnum stateParsed;
        PlayerStateEnum.TryParse(newState, out stateParsed);
        setState(stateParsed, weight);
    }

    internal void setState(PlayerStateEnum stateEnum, float weight = 0.5f)
    {
        if (stateEnum == currentState)
        {
            return;
        }
        
        switch (stateEnum)
        {
            case PlayerStateEnum.idle: setState(stateIdle, weight); break;
            case PlayerStateEnum.callWaitressDrink: setState(stateCallWaitressDrink, weight); break;
            case PlayerStateEnum.callWaitressFood: setState(stateCallWaitressFood, weight); break;
            case PlayerStateEnum.fly: setState(stateFly, weight); break;
            case PlayerStateEnum.birt: setState(stateBirt, weight); break;
            default: return;
        }

        currentState = stateEnum;
    }
    
    private void setState(GameObject stateFolder, float weight = 0.5f)
    {
        disableAllStateFalders();
        stateFolder.SetActive(true);
        
        PlayerState playerState = readState(stateFolder);
        
        setEffector(ref bodyTarget, playerState.body.transform, FullBodyBipedEffector.Body,weight);
        setEffector(ref rhTarget, playerState.rightHand.transform, FullBodyBipedEffector.RightHand, weight);
        setEffector(ref lhTarget, playerState.leftHand.transform, FullBodyBipedEffector.LeftHand, weight);
        setEffector(ref rfTarget, playerState.rightFoot.transform, FullBodyBipedEffector.RightFoot, 0.8f);
        setEffector(ref lfTarget, playerState.leftFoot.transform, FullBodyBipedEffector.LeftFoot, 0.8f);
    }

    private void setEffector(ref Transform effectorTransform, Transform target, FullBodyBipedEffector effectorType, float weight = 0.5f)
    {
        IKEffector effector = ik.solver.GetEffector(effectorType);
        effectorTransform = target;

        // Назначаем цель и веса
        effector.positionWeight = weight;
        effector.rotationWeight = weight;
        
        // effector.target.position = effectorTransform.position;
        // StartCoroutine(HelperTools.MoveToTarget(effector.target, effectorTransform.position, 1f));
    }

    

    private PlayerState readState(GameObject state)
    {
        PlayerState playerState = new PlayerState();
        
        foreach (Transform limb in state.transform)
        {
            switch (limb.name)
            {
                case "head" : playerState.head = limb.gameObject; break;
                case "body" : playerState.body = limb.gameObject; break;
                case "lh" : playerState.leftHand = limb.gameObject; break;
                case "rh" : playerState.rightHand = limb.gameObject; break;
                case "lf" : playerState.leftFoot = limb.gameObject; break;
                case "rf" : playerState.rightFoot = limb.gameObject; break;
            }
        }
        
        return playerState;
    }

    private struct PlayerState
    {
        internal GameObject head;
        internal GameObject body;
        internal GameObject leftHand;
        internal GameObject rightHand;
        internal GameObject leftFoot;
        internal GameObject rightFoot;
    }
}
