using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public GameObject Drone;

    int maxRotation = 35;

    float movementSpeed = 0.1f;
    float minMovementSpeed = 0.1f;
    float maxMovementSpeed = 0.5f;
    float movementSpeedIncrease = 0.05f;

    float rotationSpeed = 50f;

    void Start()
    {
    }

    void Update()
    {
        MoveDrone();
    }

    void MoveDrone()
    {
        Vector3 deltaPosition = Vector3.zero;
        Vector3 deltaRotation = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            deltaPosition += Vector3.forward;
            deltaRotation += Vector3.right;
        }

        if (Input.GetKey(KeyCode.S))
        {
            deltaPosition -= Vector3.forward;
            deltaRotation -= Vector3.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            deltaPosition -= Vector3.right;
            deltaRotation += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            deltaPosition += Vector3.right;
            deltaRotation -= Vector3.forward;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            deltaRotation -= Vector3.up;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            deltaRotation += Vector3.up;
        }

        movementSpeed = Mathf.Lerp(movementSpeed, maxMovementSpeed, movementSpeedIncrease);
        if (deltaPosition == Vector3.zero) movementSpeed = minMovementSpeed;

        Vector3 movementVector = deltaPosition * movementSpeed;
        Debug.Log("movementVector");
        Debug.Log(movementVector);
        transform.position += movementVector;

        transform.rotation = Quaternion.RotateTowards(Drone.transform.rotation, Quaternion.Euler(new Vector3(deltaRotation.x * maxRotation, Drone.transform.rotation.eulerAngles.y + deltaRotation.y * rotationSpeed, deltaRotation.z * maxRotation)), Time.deltaTime * rotationSpeed);
    }
}
