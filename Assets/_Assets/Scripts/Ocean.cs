using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour {
    private Renderer water;

    public Renderer Water { get => water; set => water = value; }

    private void Update() {
        water.material.mainTextureOffset = new Vector2(0, Time.time / 100);
        water.material.SetTextureOffset("_DetailAlbedoMap", new Vector2(0, Time.time / 80));
    }
}
