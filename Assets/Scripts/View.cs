using UnityEngine;
using UnityEngine.InputSystem;

public class View : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform m_Target;

    [Header("Zoom")]
    [SerializeField] private float m_ZoomMin = 4f;
    [SerializeField] private float m_ZoomMax = 16f;
    [SerializeField] private float m_ZoomSpeed = 25f;
    [SerializeField] private float m_ZoomSmooth = 8f;

    [Header("Rotation")]
    [SerializeField] private float m_RotationSpeedX = 6f;
    [SerializeField] private float m_RotationSpeedY = 10f;
    [SerializeField] private float m_RotationSmooth = 6f;
    [SerializeField] private float m_MinPitch = 0f;
    [SerializeField] private float m_MaxPitch = 80f;

    [Header("Follow")]
    [SerializeField] private float m_FollowSmooth = 4f;

    [Header("References")]
    [SerializeField] private Transform m_CameraPivot;
    [SerializeField] private Transform m_CameraHandle;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference m_LookAction;
    [SerializeField] private InputActionReference m_ZoomAction;

    private Vector2 m_CameraRotation;
    private float m_Zoom = 5f;
    private Transform m_CameraTransform;
    private Vector2 m_CameraRotationSmoothed;
    private float m_ZoomDelta;

    private void Awake()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = -1;
        Application.targetFrameRate = 35;
#else
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = -1;
#endif
    }

    private void OnEnable()
    {
        m_LookAction?.action.Enable();
        m_ZoomAction?.action.Enable();
        m_ZoomAction.action.performed += OnZoomPerformed;
    }

    private void OnDisable()
    {
        m_LookAction?.action.Disable();
        m_ZoomAction?.action.Disable();
        m_ZoomAction.action.performed -= OnZoomPerformed;
    }

    private void OnZoomPerformed(InputAction.CallbackContext ctx)
    {
        m_ZoomDelta = ctx.ReadValue<float>();
    }


    private void Start()
    {
        if (m_CameraPivot == null)
        {
            Debug.LogError("Camera pivot is missing.");
            return;
        }

        if (m_CameraTransform == null && Camera.main != null)
        {
            m_CameraTransform = Camera.main.transform;
        }

        Vector3 angles = transform.eulerAngles;
        m_CameraRotation.x = angles.x + 30f;
        m_CameraRotation.y = angles.y - 130f;

        m_Zoom = Mathf.Clamp(m_Zoom, m_ZoomMin, m_ZoomMax);
    }

    private void LateUpdate()
    {
        float deltaTime = Time.deltaTime;

        HandleInput(deltaTime);
        FollowTarget(deltaTime);
        ApplyRotation(deltaTime);
        ApplyZoom(deltaTime);

        if (m_CameraTransform != null && m_CameraHandle != null)
        {
            m_CameraTransform.SetPositionAndRotation(m_CameraHandle.position, m_CameraHandle.rotation);
        }
    }

    private void FollowTarget(float deltaTime)
    {
        if (m_Target == null) return;

        transform.position = Vector3.Lerp(
            transform.position,
            m_Target.position,
            1f - Mathf.Exp(-m_FollowSmooth * deltaTime)
        );
    }

    private void ApplyRotation(float deltaTime)
    {
        m_CameraRotationSmoothed = Vector2.Lerp(
            m_CameraRotationSmoothed,
            m_CameraRotation,
            deltaTime * m_RotationSmooth
        );

        transform.rotation = Quaternion.Euler(m_CameraRotationSmoothed.x, m_CameraRotationSmoothed.y, 0f);
    }

    private void ApplyZoom(float deltaTime)
    {
        Vector3 desiredLocalPos = new Vector3(0f, 0f, -m_Zoom);

        m_CameraPivot.localPosition = Vector3.Lerp(
            m_CameraPivot.localPosition,
            desiredLocalPos,
            deltaTime * m_ZoomSmooth
        );
    }

    private void HandleInput(float deltaTime)
    {
        Vector2 look = m_LookAction != null
            ? m_LookAction.action.ReadValue<Vector2>()
            : Vector2.zero;

        float scroll = m_ZoomDelta;
        m_ZoomDelta = 0f;

        HandleInputs(look.x / 20f, look.y / 20f, scroll / 10f);
    }

    private void HandleInputs(float mouseX, float mouseY, float scroll)
    {
        m_CameraRotation.x -= mouseY * m_RotationSpeedX;
        m_CameraRotation.y += mouseX * m_RotationSpeedY;

        m_CameraRotation.x = Mathf.Clamp(m_CameraRotation.x, m_MinPitch, m_MaxPitch);

        if (scroll != 0f)
        {
            m_Zoom -= scroll * m_ZoomSpeed;
            m_Zoom = Mathf.Clamp(m_Zoom, m_ZoomMin, m_ZoomMax);
        }
    }
}
