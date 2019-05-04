using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFlake : MonoBehaviour {

	private float boundX = 960 + 100;
	private float boundY = 540 + 100;

	private Vector3 direction;
	private float velocity;
	private float rotation;

	private RectTransform rect;

	// Use this for initialization
	void Start () {

		rect = GetComponent<RectTransform>();	

		Randomize();
	}
	
	private void Randomize() {

		velocity = Random.value * 50 + 25;
		rotation = Random.value * 100 - 50;
		if (rotation >= 0) {
			rotation += 25;
		} else {
			rotation -= 25;
		}
		direction = new Vector3(Random.value * 2 - 1, -Random.value).normalized;
	}

	// Update is called once per frame
	void Update () {
		CheckPosition();
		UpdatePosition();
	}

	private void UpdatePosition() {
		rect.position += direction * velocity * Time.deltaTime;
		rect.Rotate(transform.forward * rotation * Time.deltaTime);
	}
	private void CheckPosition() {
		if (Mathf.Abs(rect.localPosition.x) >= boundX) {
			Destroy(gameObject);
		} else if (Mathf.Abs(rect.localPosition.y) >= boundY) {
			Destroy(gameObject);
		}
	}
}
