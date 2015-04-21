﻿using UnityEngine;
using System.Collections;

public class Ballonist : MonoBehaviour {

	public Transform balloonPrefab;
	public float moveForce = 55f;
	public float maxBalloonVelocity = 50f;
	public float balloonUpForce = 15f;
	//public float maxHorizDistance = 30f;
	//public float maxVerticalDistance = 10f;
	public float detachDistance = 4.5f;
	public float maxDistance = 3.0f;

	public float jointForceConst = 300f;
	public float jointForceLin = 30f;
	public float jointForceQuad = 3f;

	private Transform balloonInst;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (balloonInst == null) {
			balloonInst = Instantiate(balloonPrefab, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity) as Transform;
			//balloonInst.GetComponent<SpringJoint2D>().connectedBody = transform.rigidbody2D;
		}
		GetComponent<PlayerBallControl> ().balloonActive = true;
		float h = Input.GetAxis("Horizontal");

		if (h != 0 /*&& (Mathf.Abs(balloonInst.rigidbody2D.velocity.x) < maxBalloonVelocity || Mathf.Sign(h) != Mathf.Sign(rigidbody2D.velocity.x))*/) {
			//balloonInst.rigidbody2D.AddForce(new Vector2(moveForce * h, 0f));

			if(Mathf.Sign(h) != Mathf.Sign (balloonInst.rigidbody2D.velocity.x))
			{
				balloonInst.rigidbody2D.AddForce(Vector2.right * h * moveForce * 1.5f);
			}
			else if (Mathf.Abs(balloonInst.rigidbody2D.velocity.x) < maxBalloonVelocity)
			{
				balloonInst.rigidbody2D.AddForce(Vector2.right * h * moveForce);
			}
		}

		if ((balloonInst.transform.position - this.transform.position).magnitude > detachDistance) {
			//probably could use snapping noise here
			GetComponent<PowerupManager>().EndPowerup();
		}
		else if ((balloonInst.transform.position - this.transform.position).magnitude > maxDistance) {
			Vector3 toBalloon = (balloonInst.transform.position - this.transform.position).normalized;
			float distOut = (balloonInst.transform.position - this.transform.position).magnitude - maxDistance;
			//this.transform.position = balloonInst.position - toBalloon * maxDistance;
			this.transform.rigidbody2D.AddForce(toBalloon * (jointForceConst + jointForceLin*distOut + jointForceQuad*distOut*distOut));
		}
		rigidbody2D.angularVelocity = 0f;

		if (balloonInst != null) {
			balloonInst.rigidbody2D.AddForce (new Vector2 (0f, balloonUpForce));
		}
		//balloonInst.transform.position = balloonInst.transform.position + new Vector3 (0f, 0.01f, 0f);

		/*
		if (Mathf.Abs(balloonInst.transform.position.x - transform.position.x) > maxHorizDistance) {
			//transform.rigidbody2D.AddForce(new Vector2(0f, moveForce * Mathf.Sign(balloonInst.transform.position.x - transform.position.x)));
			//transform.position = balloonInst.transform.position.x - 
		}
		if (balloonInst.transform.position.y - transform.position.y > maxVerticalDistance) {
			transform.position = new Vector3(transform.position.x, balloonInst.transform.position.y - maxVerticalDistance, 0f);
		}
		*/
	}

	/*
	void OnCollisionEnter2D(Collision2D collision) {
		foreach (ContactPoint2D point in collision.contacts) {
			if (point.normal.y < 0) {
				Debug.Log ("detach balloon");
			}
		}
	}
	*/

	public void ForceQuit() {
		if (balloonInst != null && balloonInst.GetComponent<SelfRemove>() != null) {
			balloonInst.GetComponent<SelfRemove>().enabled = true;
		}
		balloonInst = null;
		GetComponent<PlayerBallControl>().balloonActive = false;
		//rigidbody2D.angularVelocity = 0f;
		//rigidbody2D.velocity = Vector2.zero;
	}
}
