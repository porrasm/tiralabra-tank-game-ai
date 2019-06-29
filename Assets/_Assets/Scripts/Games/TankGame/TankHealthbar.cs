using System.Collections;
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

    private ColliderCallback colliderCallback;

    private int barColliderCount;

    private float targetTransparency = 0.3f;

    [SerializeField]
    private Color backgroundColor, foregroundColor;

    [SerializeField]
    private Color[] barColors;

    private float transparency;
    private enum TransparencyState { Transparent, Opaque, ToTransparent, ToOpaque }
    private TransparencyState state;
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

        colliderCallback = transform.GetChild(1).gameObject.AddComponent<ColliderCallback>();
        colliderCallback.AddTriggerEnterCallback(BarEnter);
        colliderCallback.AddTriggerExitCallback(BarExit);

        background = transform.GetChild(2).GetComponent<Renderer>().material;
        foreground = transform.GetChild(3).GetComponent<Renderer>().material;

        defaultScale = bars[0].localScale;

        state = TransparencyState.Opaque;
        transparency = 1;
        SetTransparency();
        UpdateColors();
    }

    void Update() {
        UpdatePosition();
        UpdateBars();
        UpdateColors();
    }

    private void UpdatePosition() {
        transform.eulerAngles = Vector3.zero;
        transform.position = tank.position + pos;
    }

    private void UpdateBars() {

        int barIndex = net.Health / 100;

        if (barIndex < 0) {
            return;
        }

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
        posVector.x = -(1 - scaleFactor) * 0.5f;

        bars[i].localPosition = posVector;
    }

    private void SetToTransparent() {

        if (state == TransparencyState.Transparent || state == TransparencyState.ToTransparent) {
            return;
        }

        StopAllCoroutines();

        state = TransparencyState.ToTransparent;

        IEnumerator TCoroutine() {

            float distance = 1 - targetTransparency;
            float speed = distance / 1;

            while (transparency > targetTransparency) {
                transparency -= speed * Time.deltaTime;
                SetTransparency();
                UpdateColors();

                yield return null;
            }

            transparency = targetTransparency;
            SetTransparency();
            UpdateColors();

            state = TransparencyState.Transparent;
        }

        StartCoroutine(TCoroutine());
    }
    private void SetToOpaque() {

        if (state == TransparencyState.Opaque || state == TransparencyState.ToOpaque) {
            return;
        }

        StopAllCoroutines();

        state = TransparencyState.ToOpaque;

        IEnumerator TCoroutine() {

            float distance = 1 - targetTransparency;
            float speed = distance / 1;

            while (transparency < 1) {
                transparency += speed * Time.deltaTime;
                SetTransparency();
                UpdateColors();

                yield return null;
            }

            transparency = 1;
            SetTransparency();
            UpdateColors();

            state = TransparencyState.Opaque;
        }

        StartCoroutine(TCoroutine());
    }

    private void SetTransparency() {

        backgroundColor.a = transparency;
        foregroundColor.a = transparency;

        for (int i = 0; i < 3; i++) {
            barColors[i].a = transparency;
        }
    }
    private void UpdateColors() {
        background.color = backgroundColor;
        foreground.color = foregroundColor;

        for (int i = 0; i < 3; i++) {
            bars[i].GetComponent<Renderer>().material.color = barColors[i];
        }
    }

    private void BarEnter(GameObject obj, Collider collider) {

        if (collider.transform.parent != null && collider.transform.parent.GetComponent<TankPlayer>() != null) {
            barColliderCount++;
            ChangeTransparency();
        }
    }
    private void BarExit(GameObject obj, Collider collider) {

        if (collider.transform.parent != null && collider.transform.parent.GetComponent<TankPlayer>() != null) {
            barColliderCount--;
            ChangeTransparency();
        }
    }

    private void ChangeTransparency() {

        if (barColliderCount == 0 && (state == TransparencyState.Transparent || state == TransparencyState.ToTransparent)) {
            SetToOpaque();
        }
        if (barColliderCount != 0 && (state == TransparencyState.Opaque || state == TransparencyState.ToOpaque)) {
            SetToTransparent();
        }
    }
}
