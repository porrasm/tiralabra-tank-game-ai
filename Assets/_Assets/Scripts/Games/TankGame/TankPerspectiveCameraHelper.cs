using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPerspectiveCameraHelper : MonoBehaviour {

    private Transform prsCamera;
    private Transform prsArea;
    private GameObject level;

    private void Start() {

        print("Perspective start: " + gameObject.name);

        prsCamera = transform.GetChild(0);
        prsArea = new GameObject().transform;
        prsArea.name = "Perspective Level";
        level = GameObject.FindGameObjectWithTag("Level");

        SetPerspectiveGameArea();
    }

    public void SetPerspectiveGameArea() {
        Clean();
        SetArea();
        CopyLevel();
        SetCamera();
    }

    private void Clean() {
        foreach (Transform child in prsArea) {
            Destroy(child.gameObject);
        }
    }

    private void SetArea() {
        prsArea.position = ToPrspPos(Vector3.zero);
    }
    private void CopyLevel() {

        GameObject copyLevel = Instantiate(level);
        copyLevel.transform.parent = prsArea;
        copyLevel.transform.localPosition = Vector3.zero;

        CleanLevel(copyLevel.transform);
    }
    private void CleanLevel(Transform level) {

        Destroy(level.Find("Spawns").gameObject);
        Destroy(level.GetComponent<TankLevelGenerator>());
    }

    private void SetCamera() {
        prsCamera.position = ToPrspPos(transform.position);
    }

    public static Vector3 ToPrspPos(Vector3 pos) {
        pos.x += TankSettings.LevelWidth * 2;
        return pos;
    }
    public static Vector3 ToNormalPos(Vector3 pos) {
        pos.x -= TankSettings.LevelWidth * 2;
        return pos;
    }
}
