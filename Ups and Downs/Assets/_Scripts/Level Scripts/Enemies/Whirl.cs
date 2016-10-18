using UnityEngine;
using System.Collections;

public class whirl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public float xSpeed = 1;
    public float ySpeed = 1;
    public float zSpeed = 1;
    public bool manual = false;

    void Update()
    {
        if (!manual)
        {

            transform.RotateAround(transform.position, Vector3.right, ySpeed * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.up, xSpeed * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.forward, zSpeed * Time.deltaTime);

        }
        else
        {

            if (Input.GetAxis("Horizontal") != 0)
            {

                transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Horizontal") * xSpeed * Time.deltaTime);

            }

            if (Input.GetAxis("Vertical") != 0)
            {

                transform.RotateAround(transform.position, Vector3.right, Input.GetAxis("Vertical") * ySpeed * Time.deltaTime);

            }

        }

    }
}
