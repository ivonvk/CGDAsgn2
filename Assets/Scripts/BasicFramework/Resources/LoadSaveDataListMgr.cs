using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;


public class LoadSaveDataListMgr : SingleMgr<LoadSaveDataListMgr>
{
    public List<string> SavedNames = new List<string>(new string[100]);

    public void LoadAllSavedWorld()
    {
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.persistentDataPath);


        for (int i = 0; i < dir.GetFiles().Length; i++)
        {
            for (int j = 0; j < SavedNames.Count; j++)
            {
                if (SavedNames[j] == null && !SavedNames.Contains(dir.GetFiles()[i].Name))
                {
                    SavedNames[j] = dir.GetFiles()[i].Name;
                }
            }
        


              
            

        }
        




        /*string world_save_name = "world_save_" + (dir.GetFiles().Length - 1).ToString();
        //string world_save_name = SaveFileName;
        if (File.Exists(Application.persistentDataPath + "/" + world_save_name + ".save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + world_save_name + ".save", FileMode.Open);
            WorldCoordinateStats[] save = (WorldCoordinateStats[])bf.Deserialize(file);

            for (int i = 0; i < dir.GetFiles().Length - 1; i++)
            {
                SavedName.Add(Path.GetFileName(dir.GetFiles()[i].Name));
                Debug.Log(Path.GetFileName(dir.GetFiles()[i].Name));
            }


            file.Close();

        }
        else
        {
            Debug.Log("No game saved!");
        }
        */
    }
    public string[] GetSavedNames()
    {
        return SavedNames.ToArray();
         }
}
