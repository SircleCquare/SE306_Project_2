using UnityEngine;
using System.Collections;


public class LeechEnemy : MonoBehaviour
{
	public Transform player;
	public float hitRadius = 1.5f;
	public float attachTime = 1f;
	public float forwardDistance = 0.2f;

	private float currentTime;
	private Vector3 randomPosition;
	public LeechState state = LeechState.IDLE;

	public enum LeechState { IDLE, MOVING, ATTACHED };
	public AnimationCurve jumpHeight;
	public float heightMultiple = 3f;
	public AnimationCurve jumpSizePulse;

	private IEnumerator AttachToPlayer() {
		state = LeechState.MOVING;
		Vector3 attachPosition = GetAttachPosition();
		attachPosition.z -= forwardDistance;
		Vector3 origPosition = transform.position;
		Vector3 initialScale = transform.localScale;
		float time = 0f;

		while (time < 1f) {
			Vector3 targetPosition = Vector3.Lerp(origPosition, player.TransformPoint(attachPosition), time);
			float extraHeight = jumpHeight.Evaluate(time) * heightMultiple;
			targetPosition.y += extraHeight;
			transform.position = targetPosition;
			transform.localScale = initialScale * jumpSizePulse.Evaluate (time);

			time += Time.deltaTime / attachTime;
			yield return 0;
		}

		transform.parent = player;
		transform.localPosition = attachPosition;
		state = LeechState.ATTACHED;
		GameController.Singleton.getActivePlayer().addLeech(this);

		float randomTimeScale = Random.Range(0.3f, 1f);
		float randomAngle = Random.Range(0,360);
		transform.eulerAngles = new Vector3(0, 0, randomAngle);

		while ( true ) {
			time += Time.deltaTime * randomTimeScale;
			yield return 0f;
			transform.localScale = initialScale * jumpSizePulse.Evaluate (time) * 0.5f;
		}
	}

	private int tries = 0;

	private Vector3 GetAttachPosition() {
		Vector3 startPoint = player.TransformPoint(Random.insideUnitCircle * 5f);
		Vector3 endPoint = player.transform.position;

		Collider collider = player.GetComponentInChildren<MeshCollider> ();
		RaycastHit hit;
		if (collider.Raycast (new Ray (startPoint, (endPoint - startPoint).normalized), out hit, 6f)) {
			return player.InverseTransformPoint (hit.point);
		} else {
			tries++;
			if (tries > 5) {
				Destroy (gameObject);
				return Vector3.zero;
			} else {
				return GetAttachPosition ();
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		switch (state) {
			case LeechState.ATTACHED:
				return;
			case LeechState.IDLE:
				bool hit = Vector3.Distance (transform.position, player.position) <= hitRadius;
				if (hit)
				{
					StartCoroutine(AttachToPlayer());
				}
				break;

		}
	}

	public void Destroy()
	{
		Destroy(this.gameObject);
	}
}
