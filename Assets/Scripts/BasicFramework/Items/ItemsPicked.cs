using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemsPicked : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
{


    public ItemInfo itemInfo;
    public Sprite NullItem;
    public Image ItemIcon;
    public Image imgSelected, imgQualityFrame;
    ItemsPanel MainPanel;

    GridLayoutGroup grid;

    void Awake()
    {
        ItemIcon = GetComponent<Image>();
        itemInfo = new ItemInfo();
        grid = transform.parent.GetComponent<GridLayoutGroup>();

        imgSelected = transform.Find("imgSelected").GetComponent<Image>();
        imgQualityFrame = transform.Find("imgQualityFrame").GetComponent<Image>();
        
    }
    void Start()
    {
        SwitchQualityFrameActive(false);
        SwitchSelectedFrameActive(false);
    }
    public void AddMainPanel(ItemsPanel panel)
    {
        MainPanel = panel;
    }


    public void ResetStock()
    {
       /* SwitchQualityFrameActive(false);
        SwitchSelectedFrameActive(false);
        itemInfo.id = -1;
        ItemIcon.color = new Color(1f, 1f, 1f, 0.25f);
        ItemIcon.sprite = NullItem;
        itemInfo.Qty = 0;

        itemInfo = new ItemInfo();*/
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        grid.enabled = false;

        OnPointerClick(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (Transform t in transform.parent.GetComponentInChildren<Transform>())
        {
            if (t == transform)
                continue;
            if (Vector2.Distance(transform.position, t.position) < 20f)
            {
                int i = gameObject.transform.GetSiblingIndex();
                gameObject.transform.SetSiblingIndex(t.GetSiblingIndex());
                t.SetSiblingIndex(i);
                Debug.Log(Vector2.Distance(transform.position, t.position));
                break;
            }
        }
        grid.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       /* if (itemInfo.isGot)
        {
            
            MainPanel.ShowSelectedItemAttributes(this);
        }*/
    }

    public void SwitchQualityFrameActive(bool active)
    {
        imgQualityFrame.gameObject.SetActive(active);
    }
    public void SwitchSelectedFrameActive(bool active)
    {
        imgSelected.gameObject.SetActive(active);
    }
}
