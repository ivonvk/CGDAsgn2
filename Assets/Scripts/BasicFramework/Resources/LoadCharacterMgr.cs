using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadCharacterMgr : SingleMgr<LoadCharacterMgr>
{
    public Dictionary<string, CharacterStats> StatsDic;
    private List<CharacterStats> LatestStats = new List<CharacterStats>();

    public void RefreshCharacterStats()
    {
        StatsDic = new Dictionary<string, CharacterStats>();
        TextAsset statsdata = Resources.Load<TextAsset>("Datas/character_stats_data");

        string[] data = statsdata.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (row[1] != "")
            {
                CharacterStats s = new CharacterStats();

                int.TryParse(row[0], out s.ID);
                s.Name = row[1];

                float.TryParse(row[2], out s.MaxHP);
                float.TryParse(row[3], out s.HPRestore);

                float.TryParse(row[4], out s.Damage);
                float.TryParse(row[5], out s.Def);

                float.TryParse(row[6], out s.AttackRange);
                float.TryParse(row[7], out s.AttackSpeed);

                float.TryParse(row[8], out s.MoveSpeed);
                float.TryParse(row[9], out s.JumpForce);
                StatsDic.Add(s.Name.ToString(), s);
                /*itemPanel = PanelMsr.GetInstance().GetPanelFunction("ItemsPanel") as ItemsPanel;

                if (s.isGot && itemPanel != null)
                    itemPanel.AddToPlayerItem(s);
                    */

                LatestStats.Add(s);

            }
        }

    }
    public CharacterStats GetCharacterStats(string statsname)
    {
        if (StatsDic.ContainsKey(statsname))
            return StatsDic[statsname];
        else
            return new CharacterStats();
    }
}
