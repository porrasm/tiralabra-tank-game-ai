using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFlaker : MonoBehaviour {

	[SerializeField]
	private float time;
	private float timePassed;

	private float width;
	private float height;
	

	private GameObject flakePrefab;

	// Use this for initialization
	void Start () {

		Vector3[] corners = new Vector3[4];
		GetComponent<RectTransform>().GetLocalCorners(corners);

		width = Vector3.Distance(corners[0], corners[3]);
		height = Vector3.Distance(corners[0], corners[1]);

		print("W: " + width);
		print("H: " + height);

		if (time == 0) {
			time = 1;
		}
		flakePrefab = Resources.Load<GameObject>("ResourcePrefabs/BackgroundFlake");
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTimes();
	}

	private void UpdateTimes() {
		timePassed += Time.deltaTime;

		while (timePassed >= time) {
			timePassed -= time;
			SpawnFlake();
		}
	}
	private void SpawnFlake() {
		GameObject newFlake = Instantiate(flakePrefab);
		newFlake.GetComponent<RectTransform>().localPosition = new Vector3(Random.value * width, height + 50);
		print(newFlake.GetComponent<RectTransform>().localPosition);
		newFlake.GetComponent<RectTransform>().SetParent(transform);
	}
}
