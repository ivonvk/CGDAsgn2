using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EffectController : Photon.MonoBehaviour
{
    float DestroyCounter = 0.15f;
    public float TargetDestroyCounter = 0.15f;
    Light light;
    float TargetDCounter = 0,TargetLightR = 0;
    void Awake()
    {
        light = GetComponent<Light>();
    }
    // Start is called before the first frame update
    void Start()
    {
        light.range = 1f;
        DestroyCounter = TargetDestroyCounter;
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    // Update is called once per frame
    void Update()
    {
        DestroyCounter -= 1f * Time.deltaTime;
        light.range -=7f * Time.deltaTime;
        if (DestroyCounter <= 0 && PhotonNetwork.isMasterClient)
        {
            photonView.TransferOwnership(PhotonNetwork.masterClient);
            PhotonNetwork.Destroy(gameObject);
        }

    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            stream.SendNext(DestroyCounter);
            stream.SendNext(light.range);
        }
        else
        {

            TargetDCounter = (float)stream.ReceiveNext();

            TargetLightR = (float)stream.ReceiveNext();
        }

      
    }
}
