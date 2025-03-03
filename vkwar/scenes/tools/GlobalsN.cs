using Godot;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;

public partial class GlobalsN : Node 
{
    // singleton_ST
    private static GlobalsN instance;
    private GlobalsN(){}
    public static GlobalsN getInstance(){
        if (instance == null)
            instance = new GlobalsN();
        return instance;
    }
    // singleton_END
    // player stateMachine_ST
    public enum StateMachine{
        MOVE,
        JUMP,
        ATTACK,
        DAMAGED,
        DEATH
    }  
    // player stateMachine_END
    // player_ST
    private static StateMachine state;
    public static StateMachine State{
        get{return state;}
        set{
            state = value;
        }
    }
    public static int MAX_HP = 300;
    public static int MAX_SPEED = 500;
    public static int MAX_FALL = 500;
    public static int speed = 300;
    public static bool allowJump;
    public static int direction;
    public static Vector2 velocity;
    //public static CharacterBody2D player;
    public static RayCast2D mouseRC2D;
    public static bool playerReality1; // игрок находится в реальности cReality1
    public static bool allowChangeState;
    //  player_END
    // enemy stateMachine_ST
    public enum eStateMachine{
        MOVE,
        CHASE,
        ATTACK,
        DAMAGED,
        DEATH
    }
    // enemy stateMachine_END

    // platfrom_ST
    public static Vector2 scale = new Vector2(0.6f, 0.4f); 

    // platfrom_END
    // damageAreas_ST
    public static int damage = 100;
    // damageAreas_END
    // глобальные переменные_ST
    public static float[] decibelArray = [1, 1, 1];
    public static Vector2[] errorPosition = [new Vector2(40, 30), new Vector2(10, -20), new Vector2(-20, 10), new Vector2(-35, -5), new Vector2(0, 0)];
    public static int resolutionIndex = 0;
    public static Vector2I screenResolution = new Vector2I(1280, 720);
    public static String currentScene;
    public static Color modulate1 = new Color(255, 0, 0,255);
    public static Color modulate2 = new Color(0, 0, 255,255);
    // глобальные переменные_END
    // глобальные методы_ST
    public static void setEnemyCollision(CharacterBody2D enemy){ // enemy - сам противник
        Color modulate = enemy.Modulate;
        if (enemy.IsInGroup("cReality1")){
            // enemy.Set("modulate.a", modulate.R + 100f);
            enemy.SetCollisionLayerValue(5, false);
            enemy.SetCollisionMaskValue(3, false);
        }
        else if (enemy.IsInGroup("cReality2")){
            enemy.SetCollisionLayerValue(4, false);
            enemy.SetCollisionMaskValue(2, false);
        }
        enemy.Modulate = modulate;
    }

    public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>{ // метод для вычилсения значения в заданной допустимой области
        if (val.CompareTo(min) < 0) return min;
        else if (val.CompareTo(max) > 0) return max;
        else return val;
    }
    // глобальные методы_END
}
