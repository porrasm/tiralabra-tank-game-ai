using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEvents : MonoBehaviour {

    #region fields
    private static TankEvents instance;

    public enum EventType {
        BulletEvent
    }

    public delegate void VoidEvent();
    private VoidEvent bulletEvents;
    #endregion

    public void UnsubscribeFromEvent(VoidEvent e, EventType type) {
        AddOrRemoveEvent(e, type, false);
    }
    public void SubscribeToEvent(VoidEvent e, EventType type) {
        AddOrRemoveEvent(e, type, false);
        AddOrRemoveEvent(e, type, true);
    }
    private void AddOrRemoveEvent(VoidEvent e, EventType type, bool add) {
        switch (type) {
            case EventType.BulletEvent:
                if (add) {
                    bulletEvents += e;
                } else {
                    bulletEvents -= e;
                }
                break;
        }
    }

    public void CallEvent(EventType type) {

        switch (type) {
            case EventType.BulletEvent:
                bulletEvents?.Invoke();
                break;
        }
    }

    public static TankEvents Instance {
        get {
            if (instance == null) {
                instance = Scripts.GetScriptComponent<TankEvents>();
            }

            return instance;
        }
    }
}
