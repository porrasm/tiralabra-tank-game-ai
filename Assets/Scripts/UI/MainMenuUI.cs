using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {

    private GameObject scripts;
    private Transform currentView;

    private void Start() {
        ChangeView("Main", true);
    }

    public void CreateGame() {
        ChangeView("Lobby");
        Scripts.GetGameObject().GetComponent<UDP_Server>().StartServer();
    }
    public void CancelGame() {
        ChangeView("Main");
        Scripts.GetGameObject().GetComponent<UDP_Server>().StopServer();
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
            currentView.gameObject.SetActive(false);
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
}