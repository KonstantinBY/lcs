using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;

public class MobileCameraController : MonoBehaviour
{
    [Header("Параметры")]
    public float rotationSpeed = 0.2f;
    public float returnSpeed = 2f;
    public float returnDelay = 1f; // ⏱ Задержка перед возвратом

    private Quaternion initialRotation;
    private float currentYawOffset = 0f;
    private bool isTouching = false;
    private Vector2 lastTouchDelta = Vector2.zero;

    private float timeSinceRelease = 0f;
    private bool waitingToReturn = false;
    
    private float initialYaw;
    private float initialPitch;

    private void Start()
    {
        Vector3 angles = transform.rotation.eulerAngles;
        initialYaw = angles.y;
        initialPitch = angles.x;
    }

    private void Update()
    {
        HandleTouchLogic();
        ApplyRotation();
    }

    private void HandleTouchLogic()
    {
        var touches = Touchscreen.current?.touches;
        if (touches == null || touches.Value.Count == 0) return;

        foreach (var touchControl in touches)
        {
            var touch = touchControl.ReadValue();

            if (touchControl.press.wasPressedThisFrame)
            {
                if (!IsPointerOverUI(touchControl))
                {
                    isTouching = true;
                    waitingToReturn = false;
                    timeSinceRelease = 0f;
                }
            }

            if (touchControl.press.wasReleasedThisFrame)
            {
                isTouching = false;
                timeSinceRelease = 0f;
                waitingToReturn = true;
            }

            if (isTouching && touchControl.delta.ReadValue() != Vector2.zero)
            {
                lastTouchDelta = touchControl.delta.ReadValue();
                currentYawOffset += lastTouchDelta.x * rotationSpeed * Time.deltaTime;
            }
        }

        // ⏱ Задержка перед возвратом
        if (waitingToReturn && !isTouching)
        {
            timeSinceRelease += Time.deltaTime;

            if (timeSinceRelease >= returnDelay)
            {
                waitingToReturn = false; // начать возврат
            }
        }

        // ↩ Плавный возврат
        if (!isTouching && !waitingToReturn && Mathf.Abs(currentYawOffset) > 0.01f)
        {
            float delta = Mathf.DeltaAngle(0f, currentYawOffset);
            float step = returnSpeed * Time.deltaTime;

            if (Mathf.Abs(delta) > step)
                currentYawOffset -= Mathf.Sign(delta) * step;
            else
                currentYawOffset = 0f;
        }
    }

    private void ApplyRotation()
    {
        float yaw = initialYaw - currentYawOffset;
        Quaternion rotation = Quaternion.Euler(initialPitch, yaw, 0f);
        transform.rotation = rotation;
    }

    private bool IsPointerOverUI(TouchControl touchControl)
    {
        if (EventSystem.current == null) return false;
        return EventSystem.current.IsPointerOverGameObject(touchControl.touchId.ReadValue());
    }
}
