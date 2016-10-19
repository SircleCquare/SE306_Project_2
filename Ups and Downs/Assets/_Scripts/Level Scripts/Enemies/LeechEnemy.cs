using UnityEngine;
using System.Collections;
using System;


public class LeechEnemy : Enemy
{
    private PlayerController player;
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

    private Coroutine animationRoutine;

    protected override void Start()
    {
        var gameController = GameController.Singleton;
        var initPosition = transform.position;
        if (initPosition.z > 0)
        {
            player = gameController.getLightPlayer();
            initPosition.z = gameController.lightSideZ;
        }
        else
        {
            player = gameController.getDarkPlayer();
            initPosition.z = gameController.darkSideZ;
        }
        transform.position = initPosition;

        base.Start();
    }

	private IEnumerator AttachToPlayer() {
		state = LeechState.MOVING;
		Vector3 attachPosition = GetAttachPosition();
		attachPosition.z -= forwardDistance;
        Vector3 origPosition = transform.position;
        Vector3 initialScale = transform.localScale;
		float time = 0f;

		while (time < 1f) {
			Vector3 targetPosition = Vector3.Lerp(origPosition, player.transform.TransformPoint(attachPosition), time);
			float extraHeight = jumpHeight.Evaluate(time) * heightMultiple;
			targetPosition.y += extraHeight;
			transform.position = targetPosition;
			transform.localScale = initialScale * jumpSizePulse.Evaluate (time);

			time += Time.deltaTime / attachTime;
			yield return 0;
		}

		transform.parent = player.transform;
		transform.localPosition = attachPosition;
		state = LeechState.ATTACHED;
		player.addLeech(this);
        GameController.Singleton.enableShakyCam();

		float randomTimeScale = UnityEngine.Random.Range(0.3f, 1f);
        float randomAngle = UnityEngine.Random.Range(0,360);
		transform.eulerAngles = new Vector3(0, 0, randomAngle);

		while ( true ) {
			time += Time.deltaTime * randomTimeScale;
			yield return 0f;
			transform.localScale = initialScale * jumpSizePulse.Evaluate (time) * 0.5f;
		}
	}

	private int tries = 0;

	private Vector3 GetAttachPosition() {
        Vector3 startPoint = player.transform.TransformPoint(UnityEngine.Random.insideUnitCircle * 5f);
        Vector3 newPoint = startPoint;
        newPoint.y = newPoint.y - 0.2f;
        var chest = Array.Find(player.GetComponentsInChildren<Transform>(), transform => transform.name.Equals("chest"));
        Vector3 endPoint = chest.position;

        Collider collider = chest.GetComponent<BoxCollider> ();
        RaycastHit hit;
        if (collider.Raycast(new Ray(startPoint, (endPoint - startPoint).normalized), out hit, 6f)) {
			return player.transform.InverseTransformPoint (hit.point);
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
	protected override void UpdateActive()
	{
		switch (state) {
			case LeechState.ATTACHED:
				return;
			case LeechState.IDLE:
				bool hit = Vector3.Distance (transform.position, player.transform.position) <= hitRadius;
				if (hit)
				{
                    animationRoutine = StartCoroutine(AttachToPlayer());
				}
				break;

		}
	}

    // Reset leech state on player death
    public override void ResetBehaviour()
    {
        if (animationRoutine != null)
        {
            GameController.Singleton.disableShakyCam();
            StopCoroutine(animationRoutine);
            transform.parent = null;
            state = LeechState.IDLE;
            base.ResetBehaviour();
        }
    }

    // Destroy leech object on de-leech collectible pickup
    public void Destroy()
	{
        GameController.Singleton.disableShakyCam();
        Destroy(this.gameObject);
	}
}
