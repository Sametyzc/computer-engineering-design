using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public GameObject Drone;
    public Transform DroneModel;


    [Header("Rotation")]
    [Header("Physics")]
    public float PitchSpeed;
    public float YawSpeed;
    public float RollSpeed;

    public float PitchLean;
    public float YawLean;
    public float RollLean;

    [Header("Movement")]
    public float XSpeed;
    public float YSpeed;
    public float ZSpeed;

    new Rigidbody rigidbody;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MoveDrone();
    }

    void MoveDrone()
    {
        Vector3 deltaMovement = Vector3.zero;
        Vector3 deltaRotation = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            deltaMovement += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            deltaMovement += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            deltaMovement += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            deltaMovement += Vector3.right;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            deltaRotation += Vector3.down;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            deltaRotation += Vector3.up;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            deltaMovement += Vector3.up;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            deltaMovement += Vector3.down;
        }

        Debug.Log(deltaRotation);

        #region Rotation

        Vector3 newTorque = new Vector3(deltaRotation.x * PitchSpeed, deltaRotation.y * YawSpeed, 0);
        rigidbody.AddRelativeTorque(newTorque);
        rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, Quaternion.Euler(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0)), .1f);

        #endregion

        #region Movement

        Vector3 newForce = new Vector3(deltaMovement.x * XSpeed, deltaMovement.y * YSpeed, deltaMovement.z * ZSpeed);
        rigidbody.AddRelativeForce(newForce);
        rigidbody.position = Vector3.Lerp(rigidbody.position, transform.position, .5f);

        #endregion

        DroneModel.localEulerAngles = Quaternion.Lerp(DroneModel.transform.localRotation, Quaternion.Euler((deltaRotation.x + deltaMovement.z) * PitchLean, deltaRotation.y * YawLean, (deltaRotation.z - deltaMovement.x) * RollLean), Time.deltaTime * 5f).eulerAngles;

    }
}
