using Godot;
using System;

public partial class EventManager : Node
{
    // сигнал о смене реальности_ST
    public static Action<bool> ChangedRealityEvent;
    public static void BroadcastChangedRealityEvent(bool reality1){// reality1 - игрок находится на 1 реальности?
        ChangedRealityEvent?.Invoke(reality1);
    }
    // сигнал о смене реальности_END
    // сигнал о выводе буквы в лэйбле_ST
    public static Action TextTimeoutEvent;
    public static void BroadcastTextTimeoutEvent(){
        TextTimeoutEvent?.Invoke();
    }
    // сигнал о выводе буквы в лэйбле_END
    // сигнал о входе игрока в зону получения урона_ST
    public static Action<int, bool> DamagePlayerEvent;
    public static void BroadcastDamagePlayerEvent(int damage, bool directionL){ // damage - урон по игроку, directionL - направление отдачи  
        DamagePlayerEvent?.Invoke(damage, directionL);
    }
    // сигнал о входе игрока в зону получения урона_END
    // сигнал о возможности рестарта уровня_ST
    public static Action RestartAvailableLevelEvent;
    public static void BroadcastRestartLevelEvent(){
        RestartAvailableLevelEvent?.Invoke();
    }
    // сигнал о возможности рестарта уровня_END
    // сигнал о нанесении урона игроком_ST
    public static Action<CharacterBody2D> PlayerAttackEvent;
    public static void BroadcastPlayerAttackEvent(CharacterBody2D enemy){
        PlayerAttackEvent?.Invoke(enemy);
    }
    // сигнал о нанесении урона игроком_END
    // сигнал о нажатии кнопки Resume в меню паузы_ST
    public static Action ResumeButtonPressedEvent;
    public static void BroadcastResumeButtonPressedEvent(){
        ResumeButtonPressedEvent?.Invoke();
    }
    // сигнал о нажатии кнопки Resume в меню паузы_END
    // сигнал о возвращении мыши_ST
    public static Action ReturnMouse;
    public static void BroadcastReturnMouse(){
        ReturnMouse?.Invoke();
    }
    // сигнал о возвращении мыши_END
}
