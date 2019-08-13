using BeardedManStudios.Forge.Networking.Unity;

public class TankGameClient : SceneScript {

    private void Start() {
        Initialize();
    }

    protected override void Initialize() {
        base.Initialize();

        if (!Server.Networker.IsServer) {
            NetworkManager.Instance.InstantiateTankNetworking();
        }
    }
}
