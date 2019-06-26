using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGameHost : SceneScript {

    [SerializeField]
    private GameObject healthRegenParticlePrefab;

    [SerializeField]
    private GameObject powerupCrosshairObject;

    public GameObject HealthRegenParticlePrefab { get => healthRegenParticlePrefab; }
    public GameObject PowerupCrosshairObject { get => powerupCrosshairObject; set => powerupCrosshairObject = value; }

    private void Start() {
        Initialize();
    }

    protected override void Initialize() {
        base.Initialize();

    }

    public static TankGameHost Game() {
        return GameObject.FindGameObjectWithTag("Game").GetComponent<TankGameHost>();
    }
}
