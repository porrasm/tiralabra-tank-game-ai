using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Find out if iterating through class fields is possible like in javascript
public class TankSettings : MonoBehaviour {

    #region Level
    public static int LevelWidth = 10;
    public static int LevelHeight = 10;

    public static float AreaWidth = 1000;

    public static float GenerateTime = 3;

    public static float Scale = 1;

    public static float CameraSizeFactorX = 0.55f;
    public static float CameraSizeFactorY = 1.1f;

    public static float SpawnClearDistance = 1.5f;

    public static float CleanProbability = 0.8f;
    #endregion

    public static bool StartActive = true;

    public static float TankSpeed = 1;
    public static float TankRotateSpeed = 100;

    public static float BulletSpeed = 2f;

    #region Game
    public static int RoundTime = 120;

    public static float ExtraWaitTime = 1;
    public static float RoundEndWaitTime = 2;

    public static int Health = 100;

    public static int BulletDamage = 60;
    public static int BulletBounces = 5;
    public static int BulletDamageBounceReduction = 10;
    public static float BulletAliveTime = 10f;

    #endregion

    #region Score
    public static int WinScore = 250;
    public static int KillScore = 50;
    public static int AssistScore = 25;
    public static int SuicideScore = -50;
    public static float MultiKillFactor = 1.25f;

    #endregion

    #region Tank
    public static int ClipAmount = 5;
    public static float ReloadTime = 2;
    public static float FireRate = 57;

    #endregion

    #region Powerup
    public static float P_ChargeSpeedFactor = 5;
    public static float P_ChargeTime = 0.4f;
    public static int P_ChargeDamage = 200;
    #endregion
}

/*
 * Settings superclass with Dictionary<string, Setting>, where Setting has type, name, category editable (max, min, default)
 * 
 * Auto generate UI to edit settings
 */
