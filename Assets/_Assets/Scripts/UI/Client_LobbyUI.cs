using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Client_LobbyUI : MonoBehaviour {

    [SerializeField]
    private InputField nameField;
    [SerializeField]
    private Button colorButton, readyButton, startButton;
    private Image colorImage;

    private Player player;

    private Player Player {
        get {
            if (player == null) {
                player = Player.MyPlayer();
                return player;
            } else {
                return player;
            }
        }
    }

    private void Start() {
        colorImage = colorButton.GetComponent<Image>();
    }
    private void Update() {
        UpdateUI();
    }

    public void ChangeName(string name) {

        if (!AllowedInput(ref name)) {
            nameField.text = name;
        }
    }
    private bool AllowedInput(ref string input) {
        string clean = Regex.Replace(input, @"[^a-zA-Z0-9\söäåÖÄÅ(:)]", string.Empty);
        if (input.Equals(clean)) {
            return true;
        }
        input = clean;
        return false;
    }

    public void UpdateName(string name) {

        if (name.Length < 2) {
            return;
        }

        print("Updating client name: " + name);

        Player.Ready = false;
        Player.UpdateClient();
        Player.ChangeName(name);
    }

    public void ToggleReady() {
        Player.Ready = !Player.Ready;
        Player.UpdateClient();
    }

    private void UpdateUI() {

        ClientManager manager = Scripts.GetScriptComponent<ClientManager>();

        // Color
        if (colorImage.color != Colors.GetBasicColor(Player.Color)) {
            colorImage.color = Colors.GetBasicColor(Player.Color);
        }

        // Name field
        if (!nameField.isFocused) {
            if (!Player.Name.Equals(nameField.text)) {
                nameField.text = Player.Name;
            }
        }

        // Ready button
        Image bImage = readyButton.GetComponent<Image>();

        if (Player.Ready && bImage.color != Color.green) {
            bImage.color = Color.green;
            readyButton.transform.GetChild(0).GetComponent<Text>().text = "Ready";
        } else if (!Player.Ready && bImage.color != Color.red) {
            bImage.color = Color.red;
            readyButton.transform.GetChild(0).GetComponent<Text>().text = "Not Ready";
        }

        // Start button
        if (Player.Primary) {
            startButton.gameObject.SetActive(true);
            startButton.interactable = ClientManager.AllReady();
        } else {
            startButton.gameObject.SetActive(false);
        }
    }

    public void ToggleColor() {
        Player.Ready = false;
        Player.UpdateClient();
        Player.MyPlayer().ToggleColor();
    }
}
