using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float playerSpeed = 2f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 3f;
    public float gravityForce = 9.81f;
    public Transform planet;
    private float yRot;
    private float xRot;

    private Rigidbody rigidBody;
    private Transform camera;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        Cursor.visible = false;
        camera = transform.GetChild(0).GetComponent<Transform>();
    }

    void FixedUpdate() {

        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);
        xRot += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        camera.localEulerAngles = new Vector3(xRot, camera.localEulerAngles.y, camera.localEulerAngles.z);

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f) {
            rigidBody.AddForce(transform.right * Input.GetAxisRaw("Horizontal") * playerSpeed, ForceMode.Acceleration);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f) {
            rigidBody.AddForce(transform.forward * Input.GetAxisRaw("Vertical") * playerSpeed, ForceMode.Acceleration);
        }

        // gravity
        Vector3 gravityDirection = planet.position - transform.position;
        gravityDirection = gravityDirection.normalized;
        rigidBody.AddForce(gravityDirection * gravityForce, ForceMode.Acceleration);
        Quaternion currentRotation = transform.rotation;
        transform.up = -gravityDirection;
        transform.rotation = Quaternion.AngleAxis(currentRotation.eulerAngles.y, transform.up) * Quaternion.FromToRotation(Vector3.up, -gravityDirection);
    }
}
