using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;


public class SaveLoadMgr : SingleMgr<SaveLoadMgr>
{
    public string SaveFileName = "";

    public void SetSaveFileName(string s)
    {
        SaveFileName = s;

        
    }
    public string GetSaveFileName()
    {

        return SaveFileName;

    }
    void Nothing()
    {

    }
    public WorldCoordinateStats[,] WorldDataSave()
    {
        WorldCoordinateStats[,] save = new WorldCoordinateStats[1000, 1000];


        return save;
    }

    public void SaveWorld()
    {

        WorldCoordinateStats[,] save = new WorldCoordinateStats[0,0];

        BinaryFormatter bf = new BinaryFormatter();


        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.persistentDataPath);

        


        
        
        

        string world_save_name = "world_save_" + dir.GetFiles().Length.ToString()+".save";


        FileStream file = File.Create(Application.persistentDataPath + "/" + world_save_name);
        bf.Serialize(file, save);
        file.Close();

        Debug.Log(world_save_name + " Saved"+" Path: "+ Application.persistentDataPath + "/" + world_save_name);

    }
    public WorldCoordinateStats[,] LoadWorld()
    {
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.persistentDataPath);


        string world_save_name = "";
        if (SaveFileName != "")
            world_save_name = SaveFileName;
        else
            return null;
        Debug.Log(SaveFileName);
        //string world_save_name = SaveFileName;
        if (File.Exists(Application.persistentDataPath + "/" + world_save_name ))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + world_save_name , FileMode.Open);
            WorldCoordinateStats[,] save = (WorldCoordinateStats[,])bf.Deserialize(file);
            file.Close();

            Debug.Log(world_save_name + " Loaded"+" Path: "+ Application.persistentDataPath + "/" + world_save_name);
            return save;
        }
        else
        {
            Debug.Log("No game saved!");
        }
        return null;
    }
}
