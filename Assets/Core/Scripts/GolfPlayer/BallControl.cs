using Unity.VisualScripting;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    [SerializeField] private float shotPower;
    [SerializeField] private float stopVelocity = .05f; //The velocity below which the rigidbody will be considered as stopped

    [SerializeField] private LineRenderer lineRenderer;

    public bool canMove = false;
    private bool isIdle;
    private bool isAiming;
    private bool isClickBall;
    bool canShoter = false;
    bool canAiming = false;
    private Rigidbody rb;

    public Transform cameraTransform;
    public LayerMask LayerMask;
    public float cameraRotationSpeed = 2f;
    private float _cinemachineTargetYaw = 0f;
    private float lastMouseX;
    private Vector3 startDrag;
    private Vector3 endDrag;

    private void Awake()
    {
        St_ControlGamplay.SetPlayer(this);
        St_ControlGamplay.SetStartPosition(this.transform.position);
        rb = GetComponent<Rigidbody>();
        canMove = true;
        isAiming = false;
        lineRenderer.enabled = false;
    }
    private void Update()
    {
        if (!canMove) return;

        canShoter = Input.GetMouseButtonUp(0);
        canAiming = Input.GetMouseButtonDown(0);

        if (rb.linearVelocity.magnitude < stopVelocity)
        {
            Stop();
        }
        if (canAiming)
        {
            ClickBall();
            OnAim();

            if (IsClickOnBall())
            {
                StartDragging();
            }
            else
            {
                StartRotatingCamera();
            }

        }
        ProcessAim();

    }
    void LateUpdate()
    {
        if (!canAiming && Input.GetMouseButton(0))
        {
            if (lineRenderer.enabled) return;
            RotateCameraWhenNotDragging();
        }
    }
    private void OnAim()
    {
        if (isIdle)
        {
            isAiming = true;
        }
    }
     void ClickBall()
    {
        if (isClickBall && !isIdle) return;
        isClickBall = IsClickOnBall();
    }
    private void ProcessAim()
    {

        if (!isAiming || !isIdle)
        {
            return;
        }
        if (!isClickBall) return;
            

        Vector3? worldPoint = CastMouseClickRay();

        if (!worldPoint.HasValue)
        {
            return;
        }
        if (IsDistance(worldPoint))
        {
            DrawLine(worldPoint.Value);
        }
        if (canShoter && isIdle)
        {
            Shoot(lineRenderer.GetPosition(1));
        }
    }
    private void RotateCamera()
    {
        cameraTransform.rotation = Quaternion.Euler(30, _cinemachineTargetYaw, 0);
    }
    private void Shoot(Vector3 worldPoint)
    {

        isAiming = false;
        lineRenderer.enabled = false;
        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);
        rb.AddForce(direction * strength * shotPower,ForceMode.Impulse);
        St_ControlGamplay.CleraShorts(1);
        isIdle = false;
        isClickBall = false;
    }

    private void DrawLine(Vector3 worldPoint)
    {
        if (worldPoint.Equals(Vector3.zero)) return;

        Vector3[] positions = {
            transform.position,
           worldPoint};
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private void Stop()
    {

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isIdle = true;
    }

    private Vector3? CastMouseClickRay()
    {

        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, float.PositiveInfinity))
        {


            return hit.point;
        }
        else
        {
            return null;
        }
    }
    private bool IsDistance(Vector3? distance)
    {
        float distanceBall = Vector3.Distance(rb.transform.position, distance.Value);
        if (distanceBall > 2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void StartDragging()
    {
        startDrag = Input.mousePosition;
        canAiming = true;
    }

    private void RotateCameraWhenNotDragging()
    {
        float mouseDeltaX = Input.mousePosition.x - lastMouseX;

        _cinemachineTargetYaw += mouseDeltaX * cameraRotationSpeed * Time.deltaTime;

        RotateCamera();

        lastMouseX = Input.mousePosition.x;
    }
    private void StartRotatingCamera()
    {
        lastMouseX = Input.mousePosition.x;
    }
    private void EndDragging()
    {
        endDrag = Input.mousePosition;
        canAiming = false;

    }
    private bool IsClickOnBall()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask))
        {
            return hit.collider.gameObject == rb.gameObject;
        }
        return false;
    }
}
