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
    
    public static float Float {
        get { return (float)rnd.NextDouble(); }
    }
}
