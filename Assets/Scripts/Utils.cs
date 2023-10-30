using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static string UI_INPUTMAP { get; } = "UI"; //Action map for UI
    public static string FREEMOVE_INPUTMAP { get; } = "FreeMove"; //ActionMap for FreeMove
    public static string MOVE_INPUT { get; } = FREEMOVE_INPUTMAP + "_Move"; //Move binding from the FreeMove Actionmap
    public static string MOVING_OBJECTS_INPUTMAP { get; } = "MovingObjects"; //ActionMap for MovingObjects
    public static string FISHING_INPUTMAP { get; } = "FishingUI"; // ActionMap for when a player is fishing
}
