using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public VirtualJoystick moveJoystick;
    public float moveSpeed = 0.01f;
    public float drag = 0.5f;
    public float terminalRotationSpeed = 25.0f;

    private Rigidbody controller;
    private Transform camTransform;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Rigidbody>();
        controller.maxAngularVelocity = terminalRotationSpeed;
        controller.drag = drag;

        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.zero;
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if (dir.magnitude > 1)
            dir.Normalize();

        if (moveJoystick.InputDirection != Vector3.zero)
        {
            dir = moveJoystick.InputDirection;
            Vector3 rotatedDir = camTransform.TransformDirection(dir);
            rotatedDir = new Vector3(rotatedDir.x, 0, rotatedDir.z);
            rotatedDir = rotatedDir.normalized * dir.magnitude;


            float angle = Mathf.Atan2(dir.z, dir.x);

            // Quaternion rotation = Quaternion.LookRotation(controller.transform.position, new Vector3(0, angle, 0));
            // controller.transform.rotation = new Quaternion(0, angle, 0, 0);

            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.zero);
            rotation *= Quaternion.Euler(0, -90, 0);
            controller.transform.rotation = rotation;
            controller.transform.position += rotatedDir * moveSpeed * Time.deltaTime;
        }
    }
}
