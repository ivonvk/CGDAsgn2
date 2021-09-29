using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
public class ExtendMenuItem : MonoBehaviour
{
    static void RefreshWCoorTypeEnum()
    {
        TextAsset statsdata = Resources.Load<TextAsset>("Datas/world_coordinate_stats_data");

        string[] data = statsdata.text.Split(new char[] { '\n' });

        string enumString = "public enum WCoorStats { ";

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (row[1] != "")
            {
                if (i + 1 >= data.Length - 1)
                    enumString += row[1] + "=" + row[0] + ", NumberOfTypes}";
                else
                    enumString += row[1] + "="+ row[0] + ",";




            }
        }
        StreamWriter writer = new StreamWriter("Assets/Resources/Datas/Enums/WCoorEnum.cs");

        writer.WriteLine(enumString);
        writer.Close();
        AssetDatabase.Refresh();
    }
    static void RefreshBuildingEnum()
    {
        TextAsset statsdata = Resources.Load<TextAsset>("Datas/building_stats_data");

        string[] data = statsdata.text.Split(new char[] { '\n' });

        string enumString = "public enum SpawnBuildFunc {";

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (row[1] != "")
            {
                if (i + 1 >= data.Length - 1)
                    enumString += row[1] + "}";
                else
                    enumString += row[1] + ",";




            }
        }
        StreamWriter writer = new StreamWriter("Assets/Resources/Datas/Enums/BuildingFunctionNameEnum.cs");

        writer.WriteLine(enumString);
        writer.Close();
        AssetDatabase.Refresh();
    }
    static void RefreshNationEnum()
    {
        TextAsset statsdata = Resources.Load<TextAsset>("Datas/nation_stats_data");

        string[] data = statsdata.text.Split(new char[] { '\n' });

        string enumString = "public enum Nation {";

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (row[1] != "")
            {
                if (i + 1 >= data.Length - 1)
                    enumString += row[1] + "=" + row[0] + ", NumberOfTypes}";
                else
                    enumString += row[1] + "=" + row[0] + ",";




            }
        }
        StreamWriter writer = new StreamWriter("Assets/Resources/Datas/Enums/NationEnum.cs");

        writer.WriteLine(enumString);
        writer.Close();
        AssetDatabase.Refresh();
    }

    [MenuItem("ExtendMenuItem/SetupSceneObjects %g")]
    static void SetupSceneObjects()
    {
        RefreshBuildingEnum();
        RefreshNationEnum();
        RefreshWCoorTypeEnum();
        GameObject checkInstances = GameObject.Find("GameMaster");
        if (!checkInstances)
        {
            CreateInstances instances = new GameObject().AddComponent<CreateInstances>();
            instances.name = "GameSetupObject";
        }
        string[] path = EditorSceneManager.GetActiveScene().path.Split(char.Parse("/"));
        bool saveOK = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), string.Join("/", path));
        Debug.Log("Refresh All Gameplay Enum");

    }


    [MenuItem("ExtendMenuItem/RefreshGameplayEnum")]
    static void RefreshGameplayEnum()
    {
        RefreshBuildingEnum();
        RefreshNationEnum();
        RefreshWCoorTypeEnum();
        Debug.Log("Refresh All Gameplay Enum");
    }

    
}
