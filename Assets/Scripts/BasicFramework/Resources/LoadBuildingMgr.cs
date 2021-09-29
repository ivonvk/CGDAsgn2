using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadBuildingMgr : SingleMgr<LoadBuildingMgr>
{
    public Dictionary<string, BuildingStats> StatsDic;
    private List<BuildingStats> LatestStats = new List<BuildingStats>();

    public void RefreshBuildingStats()
    {
        StatsDic = new Dictionary<string, BuildingStats>();
        TextAsset statsdata = Resources.Load<TextAsset>("Datas/building_stats_data");

        string[] data = statsdata.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (row[1] != "")
            {
                BuildingStats s = new BuildingStats();

                int.TryParse(row[0], out s.ID);
                s.Name = row[1];

                float.TryParse(row[2], out s.MaxHP);
                float.TryParse(row[3], out s.HPRestore);

                float.TryParse(row[4], out s.Damage);
                float.TryParse(row[5], out s.Def);

                float.TryParse(row[6], out s.FunctionSpeed);
                s.SpawnCharacter = row[7];
                float.TryParse(row[8], out s.TargetDistance);
                bool.TryParse(row[9], out s.BuffProvide);
                int.TryParse(row[10], out s.FunctionMaxCount);
                StatsDic.Add(s.Name.ToString(), s);

                /*itemPanel = PanelMsr.GetInstance().GetPanelFunction("ItemsPanel") as ItemsPanel;

                if (s.isGot && itemPanel != null)
                    itemPanel.AddToPlayerItem(s);
                    */

                LatestStats.Add(s);

            }
        }

    }
    public BuildingStats GetBuildingStats(string statsname)
    {
        if (StatsDic.ContainsKey(statsname))
            return StatsDic[statsname];
        else
            return new BuildingStats();
    }
}
