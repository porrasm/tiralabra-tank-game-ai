using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Client_LobbyUI : MonoBehaviour {

    [SerializeField]
    private InputField nameField;
    [SerializeField]
    private Button colorButton;
    private Image colorImage;

    private Player player;

    private void Start() {
        colorImage = colorButton.GetComponent<Image>();
        player = Player.MyPlayer();
    }
    private void Update() {
        UpdateInfo();
    }

    public void ChangeName(string name) {
        nameField.text = name.Trim();
    }
    public void UpdateName() {

        if (nameField.text.Length < 2) {
            return;
        }

        print("Updating client name: " + nameField.text);

        Player.MyPlayer().Name = nameField.text;
        Player.MyPlayer().UpdateClient();
    }

    
    private void UpdateInfo() {

        if (colorImage.color != Colors.GetBasicColor(player.Color)) {
            colorImage.color = Colors.GetBasicColor(player.Color);
        }
    }

    public void ToggleColor() {
        Player.MyPlayer().ToggleColor();
    }
}
