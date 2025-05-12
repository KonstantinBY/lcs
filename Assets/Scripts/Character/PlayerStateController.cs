using RootMotion.FinalIK;
using ToonPeople;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerStateController : MonoBehaviour
{
    [SerializeField] private FullBodyBipedIK ik;
    
    [SerializeField] private GameObject stateIdle;
    [SerializeField] private GameObject stateCallWaitress;
    
    private PlayerStateEnum currentState;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setState(stateIdle);
    }

    private void disableAllStateFalders()
    {
        stateIdle.SetActive(false);
        stateCallWaitress.SetActive(false);
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
            case PlayerStateEnum.callWaitress: setState(stateCallWaitress, weight); break;
            default: return;
        }

        currentState = stateEnum;
    }

    private void setState(GameObject stateFolder, float weight = 0.5f)
    {
        disableAllStateFalders();
        stateFolder.SetActive(true);
        
        PlayerState playerState = readState(stateFolder);
        
        setEffector(playerState.body.transform, FullBodyBipedEffector.Body,weight);
        setEffector(playerState.rightHand.transform, FullBodyBipedEffector.RightHand, weight);
        setEffector(playerState.leftHand.transform, FullBodyBipedEffector.LeftHand, weight);
    }

    private void setEffector(Transform effectorTransform, FullBodyBipedEffector effectorType, float weight = 0.5f)
    {
        IKEffector effector = ik.solver.GetEffector(effectorType);

        // Назначаем цель и веса
        effector.target = effectorTransform;
        effector.positionWeight = weight;
        effector.rotationWeight = weight;
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
    }
}
