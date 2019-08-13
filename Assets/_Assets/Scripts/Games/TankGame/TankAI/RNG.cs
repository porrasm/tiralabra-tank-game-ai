using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RNG {

    #region fields
    private static System.Random rnd;
    #endregion

    static RNG() {
        rnd = new System.Random();
    }

    /// <summary>
    /// Returns a random float value between 0 [inclusive] and 1 [exclusive]
    /// </summary>
    public static float Float {
        get { return (float)rnd.NextDouble(); }
    }
}
