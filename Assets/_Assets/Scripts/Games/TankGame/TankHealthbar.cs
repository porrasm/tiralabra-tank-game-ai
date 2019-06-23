﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHealthbar : MonoBehaviour {

    #region fields
    private Vector3 pos;

    private Material background, foreground;
    private Transform[] bars;

    private Transform tank;

    private TankNetworking net;

    private Vector3 defaultScale;

    private GameObject collider;

    private ColliderCallback colliderCallback;

    private int barColliderCount;

    private float targetTransparency = 0.3f;
    private float transparency = 0.3f;

    [SerializeField]
    private Color backgroundColor, foregroundColor;

    [SerializeField]
    private Color[] barColors;
    #endregion


    void Start() {

        if (barColors.Length != 3) {
            barColors = new Color[3];
        }

        tank = transform.parent;
        net = tank.GetComponent<TankNetworking>();

        pos = transform.localPosition;

        bars = new Transform[3];

        bars[0] = transform.GetChild(0).GetChild(0);
        bars[1] = transform.GetChild(0).GetChild(1);
        bars[2] = transform.GetChild(0).GetChild(2);

        collider = transform.GetChild(1).gameObject;
        colliderCallback = collider.AddComponent<ColliderCallback>();
        colliderCallback.AddTriggerEnterCallback(BarEnter);
        colliderCallback.AddTriggerExitCallback(BarExit);

        background = transform.GetChild(2).GetComponent<Renderer>().material;
        foreground = transform.GetChild(3).GetComponent<Renderer>().material;

        defaultScale = bars[0].localScale;
    }

    void Update() {
        UpdatePosition();
        UpdateBars();
        UpdateBarTransparency();
        UpdateColors();
    }

    private void UpdatePosition() {
        transform.eulerAngles = Vector3.zero;
        transform.position = tank.position + pos;
    }

    private void UpdateBars() {

        int barIndex = net.Health / 100;

        for (int i = 0; i < barIndex; i++) {
            bars[i].localScale = defaultScale;
            Vector3 pos = bars[i].localPosition;
            pos.x = 0;
            bars[i].localPosition = pos;
        }
        for (int i = 2; i > barIndex; i--) {
            bars[i].gameObject.SetActive(false);
        }

        if (barIndex > 2 || barIndex < 0) {
            return;
        }

        UpdateCurrentBar(barIndex);
    }
    private void UpdateCurrentBar(int i) {

        bars[i].gameObject.SetActive(true);

        int offsetHealth = net.Health % 100;

        float scaleFactor = 0.01f * offsetHealth;

        Vector3 scaleVector = bars[i].localScale;
        scaleVector.x = scaleFactor;

        bars[i].localScale = scaleVector;

        Vector3 posVector = bars[i].localPosition;
        posVector.x = -(1 -scaleFactor) * 0.5f;

        bars[i].localPosition = posVector;
    }

    private void SetTransparency() {

    }

    private void UpdateBarTransparency() {

        print("bar count: " + barColliderCount);

        if (barColliderCount > 0) {

            backgroundColor.a = transparency;
            foregroundColor.a = transparency;

            for (int i = 0; i < 3; i++) {
                barColors[i].a = transparency;
            }
        } else {

            backgroundColor.a = 1;
            foregroundColor.a = 1;

            for (int i = 0; i < 3; i++) {
                barColors[i].a = 1;
            }
        }
    }
    private void UpdateColors() {
        background.color = backgroundColor;
        foreground.color = foregroundColor;

        for (int i = 0; i < 3; i++) {
            bars[i].GetComponent<Renderer>().material.color = barColors[i];
        }
    }

    private void BarEnter(Collider collider) {

        if (collider.transform.parent != null && collider.transform.parent.GetComponent<TankPlayer>() != null) {
            print("Bar enter");
            barColliderCount++;
        }
    }
    private void BarExit(Collider collider) {

        if (collider.transform.parent != null && collider.transform.parent.GetComponent<TankPlayer>() != null) {
            print("Bar exit");
            barColliderCount--;
        }
    }
}