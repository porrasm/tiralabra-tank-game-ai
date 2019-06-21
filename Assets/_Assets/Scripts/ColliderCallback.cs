using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCallback : MonoBehaviour {
    
    public delegate void CollisionCallback(Collision collision);
    CollisionCallback callBacks;

    public void AddCallback(CollisionCallback callBack) {
        callBacks += callBack;
    }

    public void OnCollisionEnter(Collision collision) {
        callBacks(collision);
    }
}
