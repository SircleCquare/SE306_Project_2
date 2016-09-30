using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Side PlayerSide;
    public GameController inputControl;

	public float speed = 10.0F;
    public float jumpSpeed = 20.0F;
    public float gravity = 20.0F;
    public float gravityForce = 3.0f;
    public float airTime = 1f;
	
	public float switchSearchRadius = 5.0f;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private float forceY = 0;
    private float invertGrav;
	
    void Start() {
        invertGrav = gravity + airTime;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
		updateMovement();
		if (inputControl.getSide() == PlayerSide) {
			if (inputControl.isActivate()) {
				activateSwitchs();
			}
		}
    }
	
	
	private void updateMovement() {
		float horizontalMag;
		bool jump;
		if (inputControl.getSide() != PlayerSide) {
			horizontalMag = 0f;
			jump = false;
		} else {
			horizontalMag = inputControl.getHorizontalMagnitude();
			jump = inputControl.isJump();
		}
		
        moveDirection = new Vector3(horizontalMag, 0, 0);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        if (controller.isGrounded) {
            forceY = 0;
            invertGrav = gravity * airTime;
            if (jump)
            {
                forceY = jumpSpeed;
            }
        }

        if (jump && forceY != 0) {
            invertGrav -= Time.deltaTime;
            forceY += invertGrav * Time.deltaTime;
        }

        forceY -= gravity * Time.deltaTime * gravityForce;
        moveDirection.y = forceY;
        controller.Move(moveDirection * Time.deltaTime);		
	}
	
	private void activateSwitchs() {
		Switch closeSwitch = getNearbySwitch();
		if (closeSwitch != null) {
			closeSwitch.toggle();
		}
		
	}
	
	
	private Switch getNearbySwitch() {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, switchSearchRadius);
		for (int i = 0; i < hitColliders.Length; i++) {
			Switch switchObj = hitColliders[i].gameObject.GetComponent<Switch>();
			if (switchObj != null) {
				Debug.Log("Switch found and returning");
				return switchObj;
			}
		}
		return null;
	}
}