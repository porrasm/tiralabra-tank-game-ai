using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    private Transform currentView;

    [SerializeField]
    private InputField ipField;

    [SerializeField]
    private Text ipText;

    [SerializeField]
    private string startView;

    private void Start() {
        DisableAllViews(transform);
        ChangeView(startView, true);

        Server.SetIP();
        
        if (ipField != null) {
            ipField.text = Server.IP;
        }

        if (ipText != null) {
            ipText.text = Server.IP;
        }
    }
    public void CreateGame() {
        bool success = Server.StartServer();
        if (success) {
            Games.StartGame();
        }
    }
    public void StartGame() {
        Games.SetScene(Games.GameList.TankGame);
    }

    public void CancelGame() {
        ChangeView("Main");

        // Scripts.GetGameObject().GetComponent<UDP_Server>().StopServer();
    }
    
    public void SetIP() {
        string ip = ipField.text;
        Server.IP = ip;
    }
    public void JoinGame() {
        bool success = Server.ConnectToServer();
        if (success) {   
            Games.StartGame();
        }
    }

    public void ChangeView(string view) {
        ChangeView(view, false);
    }
    public void ChangeView(string view, bool force) {

        GameObject newView = GetViewByName(transform, view + " view");

        if (newView == null) {
            print("Unable to find view: " + view);
            return;
        }

        newView.gameObject.SetActive(true);

        if (!force) {
            if (currentView) {
                currentView.gameObject.SetActive(false);
            }
        }

        currentView = newView.transform;
    }
    private GameObject GetViewByName(Transform parent, string viewName) {

        if (parent.name.ToLower().Equals(viewName.ToLower())) {
            return parent.gameObject;
        }

        foreach (Transform child in parent) {
            GameObject view = GetViewByName(child, viewName);
            if (view != null) {
                return view;
            }
        }

        return null;
    }
    public void DisableAllViews(Transform t) {
        foreach (Transform child in t) {
            DisableAllViews(child);
        }

        if (t.name.ToLower().Contains(" view")) {
            t.gameObject.SetActive(false);
        }
    }
}