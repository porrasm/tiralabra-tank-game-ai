using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region variables
    private int id;
    private string name;
    private PlayerColor color;

    // Public getters
    public int ID { get { return id; } set { id = value; } }
    public string Name { get { return name; } set { name = value; } }
    public PlayerColor Color { get { return color; } set { color = value; } }
    #endregion


}
