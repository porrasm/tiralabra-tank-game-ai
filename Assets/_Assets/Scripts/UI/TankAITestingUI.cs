using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TankAITestingUI : MonoBehaviour {

    #region fields
    [SerializeField]
    private Text countText;

    [SerializeField]
    private Slider countSlider;

    [SerializeField]
    private Toggle onlyAIsToggle;

    [SerializeField]
    private Text infoText;

    private bool onlyAIs;
    #endregion

    private void Start() {

    }

    private void Update() {
        if (countText != null) {
            countText.text = "AI Count " + countSlider.value;
        }
    }

    public void CreateGame() {

        bool success = Server.StartServer();
        if (success) {
            print("Created server, loading scene.");
            SceneManager.LoadScene("TankAITestingLobby");
        } else {
            infoText.text = "Unable to create server. The port 15937 needs to be available.";
        }
    }

    public void StartGame() {
        print("Starting game");

        int aiCount = (int)countSlider.value;

        if (aiCount > 7 && !onlyAIs) {
            aiCount = 7;
        } else if (aiCount > 8) {
            aiCount = 8;
        }

        TankAIManager.AI_COUNT = aiCount;
        TankAIManager.PLAYER_ENABLED = !onlyAIs;

        IEnumerator StartCor() {

            // Instantiate local player
            if (!onlyAIs) {
                Player.CreateNewClient();
                yield return new WaitForSeconds(1);
            }

            TankAIManager.ClaimLocalPlayer();

            for (int i = 0; i < aiCount; i++) {
                Player.CreateNewClient();
                yield return null;
            }

            yield return new WaitForSeconds(1);

            GameManager.StartGame();
        }


        StartCoroutine(StartCor());
    }
}
