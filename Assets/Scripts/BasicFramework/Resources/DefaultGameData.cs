using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;



[System.Serializable]
public class WorldCoordinateStats
{
    public int ID = 0;


    public string SNation = "Enemy";
    public string Name = "WCoor";
    public string ChineseName = "WCoor";
    public string Culture = "村民";
    public string Resources = "木棍";

    public string[] Platform = new string[5];

    

    public void SetPlatform(int index,string building_name)
    {
        Platform[index] = building_name;
    }
    public string GetPlatform(int index, int ibuilding)
    {
        return Platform[index];
    }
}




[System.Serializable]
public class BuildingStats
{
    public int ID = -1;
    public string Name = "Building";
    public float MaxHP = 25;
    public float HPRestore = 0.5f;

    public float Damage = 0f;
    public float Def = 1f;

    public float FunctionSpeed = 1f;
    public string SpawnCharacter = "";
    public float TargetDistance = 5f;
    public bool BuffProvide = false;
    public int FunctionMaxCount = 2;
}
[System.Serializable]
public class CharacterStats
{
    public int ID = -1;
    public string Name = "Character";

    public float MaxHP = 10;
    public float HPRestore = 0.75f;

    public float Damage = 1f;
    public float Def = 0.5f;

    public float AttackRange = 2f;
    public float AttackSpeed = 0.5f;

    public float MoveSpeed = 2f;
    public float JumpForce = 5f;
}

[System.Serializable]
public class ItemInfo
{
    public int ID = -1;
    public string Name = "Item";

    public float Damage = 1f;
    public float Def = 0.5f;

    public float AttackRange = 2f;
    public float AttackSpeed = 0.5f;

    public int Qty = 0;
    public bool weapon = false;

    public int Cost = 0;
}

public class DefaultGameData : MonoBehaviour
{

}

