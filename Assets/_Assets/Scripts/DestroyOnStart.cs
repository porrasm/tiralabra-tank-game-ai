using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStart : MonoBehaviour {

    [SerializeField]
    private bool disable;

    void Start() {
       if (disable) {
            gameObject.SetActive(false);
        } else {
            Destroy(gameObject);
        }
    }
}
