using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Slider : MonoBehaviour {

    #region fields
    [SerializeField]
    private TankControls.Control control;

    private TankControls controls;
    private RectTransform stick;
    
    public float Value { get; private set; }

    private float posLimit;
    private float limitHigh = 0.9f;
    private float limitLow = 0.1f;
    private float endLimit = 0.1f;
    #endregion


    void Start() {
        stick = transform.GetChild(0).GetComponent<RectTransform>();

        float x = GetComponent<RectTransform>().sizeDelta.x;

        posLimit = GetComponent<RectTransform>().sizeDelta.y / 2 - x;
        stick.sizeDelta = new Vector2(x, x);
        print("offset: " + posLimit);

        InitializeEvents();
        GetControls();
    }

    private void GetControls() {
        TankNetworking.MyTank(delegate (TankControls controls) { this.controls = controls; });
    }

    private void InitializeEvents() {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry downEntry = new EventTrigger.Entry();
        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        EventTrigger.Entry upEntry = new EventTrigger.Entry();

        downEntry.eventID = EventTriggerType.PointerDown;
        dragEntry.eventID = EventTriggerType.Drag;
        upEntry.eventID = EventTriggerType.PointerUp;

        downEntry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        dragEntry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        upEntry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });

        trigger.triggers.Add(downEntry);
        trigger.triggers.Add(dragEntry);
        trigger.triggers.Add(upEntry);
    }

    void Update() {
        UpdateStickPosition();
    }

    private void UpdateStickPosition() {


    }

    private void OnDragDelegate(PointerEventData data) {

        Vector3 pos = transform.position;
        Vector3 newPos = pos;
        newPos.y = data.position.y;

        if (newPos.y > pos.y + posLimit) {
            newPos.y = pos.y + posLimit;
        } else if (newPos.y < pos.y - posLimit) {
            newPos.y = pos.y - posLimit;
        }


        SetPosition(newPos);
    }

    public void OnPointerUp(PointerEventData eventData) {
        SetPosition(transform.position);
    }

    private void SetPosition(Vector3 pos) {
        stick.position = pos;
        Value = GetValue(transform.position.y, pos.y);
    }

    private float GetValue(float def, float y) {

        float distance = y - def;
        float value = distance / posLimit;

        int mult = GetMultiplier(distance);

        value *= 1 + endLimit;
        value -= endLimit * mult;


        if (mult > 0) {
            if (value < limitLow) {
                value = 0;
            } else if (value > limitHigh) {
                value = 1;
            }
        } else if (mult < 0) {
            if (value > -limitLow) {
                value = 0;
            } else if (value < -limitHigh) {
                value = -1;
            }
        }

        if (controls != null) {
            controls.ProcessControl(control, value);
        }

        return value;
    }
    private int GetMultiplier(float distance) {
        if (distance > 0) {
            return 1;
        } else if (distance == 0) {
            return 0;
        } else {
            return -1;
        }
    }
}