using System;
using UnityEngine;

public static class GameEventsBus
{
    public static Action OnPlayerSpotted;

    public static void RaisePlayerSpotted() => OnPlayerSpotted?.Invoke();
}
