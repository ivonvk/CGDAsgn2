using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LocalPlayerPanel : BasePanel
{
    private Slider TimeSlider;
    bool Ready = false;
    public override void Start()
    {
        base.Start();

        TimeSlider = GetCtr<Slider>("TimeSlider");

    }
    void Update()
    {
      //  if(GameMaster.Instance!=null)
      //  {
  //          TimeSlider.value = GameMaster.Instance.GetGameTimer();
      //  }
    }
}
