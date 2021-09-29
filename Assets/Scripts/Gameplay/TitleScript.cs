using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
   public void GameStart()
    {
        ScenesMgr.GetInstance().LoadScene("GameRoomScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
