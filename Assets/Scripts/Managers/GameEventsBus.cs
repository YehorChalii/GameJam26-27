using System;
using UnityEngine;

public static class GameEventsBus
{
    public static Action<SequenceTrigger> OnSequenceStarted;
    public static Action OnPlayerSpotted;
    public static Action OnGameFinish;

    public static void RaiseSequenceStarted(SequenceTrigger activeSequenceTrigger) => OnSequenceStarted?.Invoke(activeSequenceTrigger);
    public static void RaisePlayerSpotted() => OnPlayerSpotted?.Invoke();
    public static void RaiseGameFinish() => OnGameFinish?.Invoke();
}
