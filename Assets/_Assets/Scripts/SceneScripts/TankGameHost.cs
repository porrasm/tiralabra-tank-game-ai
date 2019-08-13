using UnityEngine;

public class TankGameHost : SceneScript {

    [SerializeField]
    private GameObject healthRegenParticlePrefab;

    [SerializeField]
    private GameObject powerupCrosshairObject;

    [SerializeField]
    private GameObject multiballPrefab;

    [SerializeField]
    private GameObject shieldPrefab;

    public GameObject HealthRegenParticlePrefab { get => healthRegenParticlePrefab; }
    public GameObject PowerupCrosshairObject { get => powerupCrosshairObject; set => powerupCrosshairObject = value; }
    public GameObject MultiballPrefab { get => multiballPrefab; set => multiballPrefab = value; }
    public GameObject ShieldPrefab { get => shieldPrefab; set => shieldPrefab = value; }

    private void Start() {
        Initialize();
    }

    protected override void Initialize() {
        base.Initialize();

        Scripts.GetGameObject().AddComponent<TankAIManager>();
        Scripts.GetGameObject().AddComponent<TankEvents>();
    }

    public static TankGameHost Game() {
        return GameObject.FindGameObjectWithTag("Game").GetComponent<TankGameHost>();
    }
}
