using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustPower = 1000;
    [SerializeField] float rotateSpeed = 100;

    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(Vector3.forward);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Rotate(Vector3.back);
        }
    }

    void Rotate(Vector3 vector)
    {
        rigidBody.freezeRotation = true;
        var backwardRotate = vector * rotateSpeed * Time.deltaTime;
        transform.Rotate(backwardRotate);
        rigidBody.freezeRotation = false;
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            
            var power = Time.deltaTime * thrustPower * Vector3.up;

            Debug.Log($"thurst {power.x}, {power.y}, {power.z}");
            rigidBody.AddRelativeForce(power);
        }
    }
}
