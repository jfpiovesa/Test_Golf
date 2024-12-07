using UnityEngine;

public class GolfController : MonoBehaviour
{
    public Rigidbody ballRigidbody; 
    public Transform cameraTransform; 
    public LineRenderer lineRenderer;
    public float maxForce = 20f; 
    public float cameraRotationSpeed = 2f;
    public float pullDistanceLimit = 5f; 

    private Vector3 startDrag; 
    private Vector3 endDrag; 
    private bool isDragging; 
    private float _cinemachineTargetYaw = 0f; 
    private Camera mainCamera; 
    private float lastMouseX; 
    public LayerMask LayerMask;
    void Start()
    {
        mainCamera = Camera.main; 
    }

    void Update()
    {
        HandleInput();
    }
    void LateUpdate()
    {
        if (!isDragging && Input.GetMouseButton(0))
        {
            RotateCameraWhenNotDragging();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsClickOnBall())
            {
                StartDragging();
            }
            else
            {
                StartRotatingCamera();
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            UpdateDragging();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            EndDragging();
        }
    }


    private bool IsClickOnBall()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,100f,LayerMask))
        {
            return hit.collider.gameObject == ballRigidbody.gameObject;
        }
        return false;
    }

    private void StartDragging()
    {
        startDrag = Input.mousePosition;
        isDragging = true;

        lineRenderer.enabled = true;
    }


    private void UpdateDragging()
    {
        Vector3 currentDrag = Input.mousePosition;
        float horizontalDelta = currentDrag.x - startDrag.x;

        _cinemachineTargetYaw += horizontalDelta * cameraRotationSpeed * Time.deltaTime;

        RotateCamera();

        UpdatePullLine();

        startDrag = currentDrag;
    }


    private void EndDragging()
    {
        endDrag = Input.mousePosition;
        isDragging = false;

        ApplyForceToBall();

        lineRenderer.enabled = false;
    }


    private void RotateCamera()
    {
        cameraTransform.rotation = Quaternion.Euler(30, _cinemachineTargetYaw, 0);
    }


    private void UpdatePullLine()
    {
        Vector3 direction = new Vector3(Input.mousePosition.x - startDrag.x, 0, Input.mousePosition.y - startDrag.y);

        float distance = direction.magnitude;
        distance = Mathf.Min(distance, pullDistanceLimit); 

        lineRenderer.SetPosition(0, ballRigidbody.transform.position); 
        lineRenderer.SetPosition(1, ballRigidbody.transform.position + direction.normalized * distance); 

        float colorValue = Mathf.Lerp(0, 1, distance / pullDistanceLimit);
        lineRenderer.startColor = Color.Lerp(Color.green, Color.red, colorValue);
        lineRenderer.endColor = lineRenderer.startColor;
    }

    private void ApplyForceToBall()
    {
        Vector3 dragVector = endDrag - startDrag;
        float distance = dragVector.magnitude;

        distance = Mathf.Min(distance, pullDistanceLimit);

        Vector3 force = (dragVector.normalized) * (distance / pullDistanceLimit) * maxForce;

        ballRigidbody.AddForce(force, ForceMode.Impulse);
    }


    private void StartRotatingCamera()
    {
        lastMouseX = Input.mousePosition.x;
    }


    private void RotateCameraWhenNotDragging()
    {
        float mouseDeltaX = Input.mousePosition.x - lastMouseX;

        _cinemachineTargetYaw += mouseDeltaX * cameraRotationSpeed * Time.deltaTime;

        RotateCamera();

        lastMouseX = Input.mousePosition.x;
    }

   
}
