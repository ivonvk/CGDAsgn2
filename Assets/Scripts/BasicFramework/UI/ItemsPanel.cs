using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemsPanel : BasePanel
{
    Transform ImageGrid;
    Text txtName,txtType;

    public Sprite TradeItems;
    List<ItemsPicked> playerItems;

    GameObject MainPanel;
    Button btnOpen, btnEquip;

    ItemsPicked itemsPicked;

    public override void Start()
    {
        base.Start();
        MainPanel = GetCtr<Image>("MainPanel").gameObject;
        ImageGrid = GetCtr<Image>("gridItems").transform;
        foreach (Transform t in ImageGrid)
        {
            if (t != ImageGrid.transform)
                t.gameObject.AddComponent<ItemsPicked>();
        }


        GetCtr<Button>("btnEquip").onClick.AddListener(EquipWeapon);
        GetCtr<Button>("btnOpen").onClick.AddListener(SwitchItemsMainPanel);
        GetCtr<Button>("btnClose").onClick.AddListener(SwitchItemsMainPanel);
        txtName = GetCtr<Text>("txtName");
        txtType = GetCtr<Text>("txtType");
        ResetPanel();
    }

    public override void ResetPanel()
    {
        MainPanel.SetActive(true);
        //ItemsPicked[] Data = new ItemsPicked[ImageGrid.childCount];

        ItemsPicked[] list = GetComponentsInChildren<ItemsPicked>();
        playerItems = list.OfType<ItemsPicked>().ToList();

        for(int i = 0;i < list.Length-1; i++)
        {
            list[i].ResetStock();
            list[i].AddMainPanel(this);
        }
        //RefreshStorageUI();
        MainPanel.SetActive(false);
    }
    private void SwitchItemsMainPanel()
    {
        MainPanel.SetActive(!MainPanel.activeInHierarchy);
    }
    public void AddToPlayerItem(ItemInfo picked)
    {
            foreach (ItemsPicked gi in playerItems)
            {
                if (playerItems.Contains(gi))
                {
                   // found = true;
                    gi.itemInfo.Qty += 1;
                    break;
                }
            }

            CheckItemExist(picked);
    }

    public void CheckItemExist(ItemInfo picked)
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].itemInfo.ID == -1)
            {
                playerItems[i].itemInfo = picked;
                ResMgr.GetInstance().ResourceLoadAsync<Sprite>(("ArtRes/Guns/" + playerItems[i].itemInfo.Name), (sprite) => { playerItems[i].ItemIcon.sprite = sprite; });
                playerItems[i].ItemIcon.color = new Color(1f, 1f, 1f, 1f);
                playerItems[i].gameObject.transform.SetSiblingIndex(0);
                break;
            }
        }
    }
    public void RemovePlayerItem(ItemsPicked item)
    {
        if (item.itemInfo.Qty -1>0)
        {
            bool found = false;
            foreach (ItemsPicked gi in playerItems)
            {
                if (playerItems.Contains(gi))
                {
                    found = true;
                    gi.itemInfo.Qty -= 1;

                    break;
                }
            }
            if (!found)
                item.ResetStock();
        }
        else
            item.ResetStock();
       // RefreshStorageUI();
    }

    public void ShowSelectedItemAttributes(ItemsPicked picked)
    {
        if (itemsPicked != null)
            itemsPicked.SwitchSelectedFrameActive(false);


        txtName.text = picked.itemInfo.Name;
        txtType.text = picked.itemInfo.weapon ? txtType.text = "Weapon" : txtType.text = "Item";
        itemsPicked = picked;
        itemsPicked.SwitchSelectedFrameActive(true);
    }

    public void EquipWeapon()
    {
      //  GameMgr.GetInstance().ProvidePlayerToScript().SetWeaponStats(itemsPicked.itemInfo);
    }


}
