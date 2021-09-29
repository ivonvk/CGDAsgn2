using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class LoadItemsMgr : SingleMgr<LoadItemsMgr>
{
    ItemsPanel itemPanel;
    public Dictionary<string, ItemInfo> ItemDic;

    public LoadItemsMgr()
    {
       // RefreshItem();
    }

    public void RefreshItem()
    {
        ItemDic = new Dictionary<string, ItemInfo>();
        TextAsset itemdata = Resources.Load<TextAsset>("Datas/items_data");

        string[] data = itemdata.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (row[1] != "")
            {
                ItemInfo item = new ItemInfo();

                int.TryParse(row[0], out item.ID);
                item.Name = row[1];
                float.TryParse(row[2], out item.Damage);
                float.TryParse(row[3], out item.Def);
                float.TryParse(row[4], out item.AttackRange);
                float.TryParse(row[5], out item.AttackSpeed);

                int.TryParse(row[6], out item.Qty);
                bool.TryParse(row[7], out item.weapon);
                int.TryParse(row[6], out item.Cost);
                ItemDic.Add(item.Name.ToString(), item);
                



            }
        }
        
    }


    public void SaveItemData()
    {
        foreach (KeyValuePair<string, ItemInfo> key in ItemDic)
        {
            if (ItemDic.ContainsKey(key.Key))
                RemoveItemFromData(ItemDic[key.Key],getPath());
        }
        foreach (KeyValuePair<string, ItemInfo> key in ItemDic)
        {
            if (ItemDic.ContainsKey(key.Key))
                AddItemToData(ItemDic[key.Key], getPath());
        }
    }

    public void GetItem<T>(string itemName, ItemInfo item, UnityAction<T> callBack = null) where T : ItemInfo
    {
        

        if (!ItemDic.ContainsKey(itemName))
        {

            ItemDic.Add(itemName, item);

            if (callBack != null)
                callBack(item as T);
        }
        else
        {
            if (callBack != null)
                callBack(item as T);

        }
        AddItemToData(item, getPath());
    }
    public void DropItem<T>(string itemName, UnityAction<T> callBack = null) where T : ItemInfo
    {
        if (ItemDic.ContainsKey(itemName))
        {
            // RemoveItemFromData(ItemDic[itemName], getPath());
            // callBack(ItemDic[itemName] as T);


            RemoveItemFromData(ItemDic[itemName], getPath());
            ItemDic.Remove(itemName);
        }
    }

    public int GetNextGunID()
    {
        return (ItemDic.Count - 1) + 1;
    }



    public void AddItemToData(ItemInfo _item, string filepath)
    {
        string line;
        List<String> lines = new List<string>();
        try
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(@filepath, false))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (!line.Contains("name")&&line.Contains(","))
                    {
                        string[] split = line.Split(',');
                        if (split[0].Contains(_item.ID.ToString()))
                        {
                            line = _item.ID.ToString() +
                                "," + _item.Name.ToString() +
                                "," + _item.Damage.ToString() +
                                "," + _item.AttackRange.ToString() +
                                "," + _item.AttackSpeed.ToString() +
                                "," + _item.Qty.ToString() +
                                "," + _item.weapon.ToString() +
                                "," + _item.Cost.ToString();
                        }
                    }
                    lines.Add(line);
                }
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, false))
            {
                foreach (string s in lines)
                    file.WriteLine(s);
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.ToString());
        }

    }
    public void RemoveItemFromData(ItemInfo _item, string filepath)
    {
        string line;
        List<String> lines = new List<string>();
        try
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(@filepath, false))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (!line.Contains("name") && line.Contains(","))
                    {
                        string[] split = line.Split(',');

                        if (split[0].Contains(_item.ID.ToString()))
                        {
                                 line = _item.ID.ToString() +
                                "," + _item.Name.ToString() +
                                "," + _item.Damage.ToString() +
                                "," + _item.AttackRange.ToString() +
                                "," + _item.AttackSpeed.ToString() +
                                "," + _item.Qty.ToString() +
                                "," + _item.weapon.ToString() +
                                "," + _item.Cost.ToString();
                        }
                    }
                    lines.Add(line);
                }
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, false))
            {
                file.Write("");
                foreach (string s in lines)
                {
                    file.WriteLine(s);
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.ToString());
        }
    }
    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resources/" + " /Datas/" + "itemsdata.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"gundatas.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"gundatas.csv";
#else
        return Application.dataPath +"/"+"gundatas.csv";
#endif
    }

}
