using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    #region FreeMove inputMap
    public static string FREEMOVE_INPUTMAP { get; } = "FreeMove"; //ActionMap for player generic action map
    public static string FREEMOVE_MOVE { get; } = FREEMOVE_INPUTMAP + "_Move"; 
    public static string FREEMOVE_SPRINT { get; } = FREEMOVE_INPUTMAP + "_Sprint"; 
    public static string FREEMOVE_INVENTORY { get; } = FREEMOVE_INPUTMAP + "_Inventory";
    public static string FREEMOVE_INTERACT { get; } =  FREEMOVE_INPUTMAP + "_Interact";
    #endregion
    
    #region UI inputMap
    public static string UI_INPUTMAP { get; } = "UI"; //Action map for when player is on an UI
    public static string UI_INPUTMAP_NAVIGATE { get; } = UI_INPUTMAP + "_Navigate"; 
    public static string UI_INPUTMAP_CONFIRM { get; } = UI_INPUTMAP + "_Confirm"; 
    public static string UI_INPUTMAP_CLOSEMENU { get; } = UI_INPUTMAP + "_CloseMenu";
    public static string UI_INPUTMAP_ALTERNATIVECONFIRM { get; } = UI_INPUTMAP + "_AlternativeConfirm";
    #endregion

    #region MovingObject inputMap

    public static string MOVINGOBJECTS_INPUTMAP { get; } = "MovingObjects"; //Action map for when the player is Moving an object 
    public static string MOVINGOBJECTS_DIRECTION { get; } = MOVINGOBJECTS_INPUTMAP + "_ObjectDirection";
    public static string MOVINGOBJECTS_EXIT { get; } = MOVINGOBJECTS_INPUTMAP + "_Exit"; 
    #endregion


    #region Fishing UI inputMap
    public static string FISHING_INPUTMAP { get; } = "FishingUI"; // ActionMap for when a player is fishing
    public static string FISHING_INPUTMAP_ACTION { get; } = FISHING_INPUTMAP + "_Action"; 
    public static string FISHING_INPUTMAP_QUIT { get; } = FISHING_INPUTMAP + "_Quit"; 

    #endregion
}
