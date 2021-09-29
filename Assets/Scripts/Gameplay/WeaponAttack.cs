using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : Photon.MonoBehaviour
{

    PlayerController player;
    public bool isPlayer = false;
    public bool isRanged = false;
    private Vector3 TargetPosition;
    bool isHit = false;
    Rigidbody rb;
    float ResetHitCounter = 0f;
    public int Damage = 5;
    public bool isEnemy = false;
    public float DestroyCounter = 5f;
    public bool PlayerHand = false;
    public bool isMagic = false;
    public float EnemyWeaponSpeed = 4f;
    public bool SceneTrap = false;
    public bool SceneOnce = false;
    public bool IsHeal = false;
    public bool actived = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }
    void Start()
    {
        DestroyCounter = 1.75f;

        if (PhotonNetwork.isMasterClient)
        {


            MonoMgr.GetInstance().AddUpdateListener(UpdateFunc);
        }

    }
    public void SetPC(PlayerController p)
    {
        player = p;
    }
    public void SetPlayer(PhotonView pView, bool b)
    {
        photonView.TransferOwnership(pView.owner);
        isPlayer = b;

    }
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {

        if (PhotonNetwork.isMasterClient)
        {
            MonoMgr.GetInstance().RemoveUpdateListener(UpdateFunc);
        }

    }
    void OnDestroy()
    {

        MonoMgr.GetInstance().RemoveUpdateListener(UpdateFunc);
    }
    void UpdateFunc()
    {
        if (!PlayerHand && !SceneTrap)
        {
            if (DestroyCounter > 0)
            {
                DestroyCounter -= 1f * Time.deltaTime;
            }
            else
            {
                if (PhotonNetwork.isMasterClient)
                {
                    photonView.TransferOwnership(PhotonNetwork.masterClient);
                    MonoMgr.GetInstance().RemoveUpdateListener(UpdateFunc);
                    PhotonNetwork.Destroy(gameObject);


                }
            }
        }else if (PlayerHand&&player!=null)
        {
            gameObject.SetActive(player.WeaponShow);
        }

        if (!isRanged && isHit&&PhotonNetwork.isMasterClient)
        {
            ResetHitCounter += 1f * Time.deltaTime;
            if (ResetHitCounter > 0.75f)
            {
               
            }
            
        }

    }

    public void SetWeapon(PhotonPlayer player, bool ranged)
    {
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("RPC_SetWeapon", PhotonTargets.All, player, ranged);
        }
    }
    [PunRPC]
    void RPC_SetWeapon(PhotonPlayer player, bool ranged)
    {
        if (player != null)
        {
            photonView.TransferOwnership(player);
        }
        isRanged = ranged;
        if (isRanged)
        {

            if (!isEnemy)
            {
                rb.velocity = transform.forward * 12f;
            }
            else
            {
                rb.velocity = transform.forward * EnemyWeaponSpeed;
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (SceneTrap && other.gameObject.tag == "Enemy" && !IsHeal)
        {



            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            if (enemy != null&& enemy.enemyStatus.Health>0)
            {
                enemy.GetDamage(Damage, photonView.owner.ID - 1);
            }
            PlayerNetwork.Instance.PlayerPlaySound("blade_hit");
            PlayerNetwork.Instance.PlayerPlaySound("blade_metal");

            if (PhotonNetwork.isMasterClient)
            {
                GameMaster.Instance.SpawnPlayerFuncObj(photonView.owner, transform.position, "Block_Effect");

            }
        }
        else if (SceneTrap && other.gameObject.tag == "Weapon")
        {
            PlayerNetwork.Instance.PlayerPlaySound("blade_hit");
            PlayerNetwork.Instance.PlayerPlaySound("blade_metal");
            if (PhotonNetwork.isMasterClient)
            {
                GameMaster.Instance.SpawnPlayerFuncObj(photonView.owner, transform.position, "Block_Effect");

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }
            if (other.tag == "Enemy"&& PlayerHand)
        {
     
                PlayerManager.Instance.ModifyHealth(photonView.owner, 2);

            
        }
        if (other.tag == "Enemy"&& !isEnemy)
        {
     

            

            //PhotonView pView = other.GetComponent<PhotonView>();
            EnemyController enemy = other.GetComponent<EnemyController>();
            
                PlayerNetwork.Instance.PlayerPlaySound("blade_hit");
                PlayerNetwork.Instance.PlayerPlaySound("blade_metal");
                //GameMaster.Instance.GetDestroyObjectRequest(photonView.viewID, photonView);

                if (player != null && enemy.enemyStatus.Health > 0)
                {
                    enemy.GetDamage(Damage + PlayerManager.Instance.PlayerStatus[photonView.owner.ID-1].Damage, photonView.owner.ID - 1);
                }
                else if (enemy != null && enemy.enemyStatus.Health > 0)
                {
                    enemy.GetDamage(Damage, photonView.owner.ID - 1);
                }

                if (!isMagic)
                {
                    isHit = true;
                }




            

        }
        else if (other.tag == "Weapon" && isEnemy)
        {
            PlayerNetwork.Instance.PlayerPlaySound("blade_hit");
            PlayerNetwork.Instance.PlayerPlaySound("blade_metal");
            if (PhotonNetwork.isMasterClient)
            {
                GameMaster.Instance.SpawnPlayerFuncObj(photonView.owner, transform.position, "Block_Effect");
            
                //photonView.TransferOwnership(PhotonNetwork.masterClient);
                //PhotonNetwork.Destroy(gameObject);
            }


          
        }
        else if (other.tag == "Player" &&/* !isHit && !isEnemy && other.tag != "Weapon"&& */isMagic)
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonView pView = other.GetComponent<PhotonView>();

                PlayerManager.Instance.ModifyHealth(pView.owner, 5);




                Debug.Log("Heal");
            }
        }
        else if (other.tag == "Player" && !isHit && isEnemy)
        {
            if (PhotonNetwork.isMasterClient )
            {
                PhotonView pView = other.GetComponent<PhotonView>();

                PlayerStatus CheckingStatus = PlayerManager.Instance.GetPlayerStatus(pView.owner);



                GameMaster.Instance.SpawnPlayerFuncObj(photonView.owner, transform.position, "Block_Effect");


                if (CheckingStatus.Health > 0)
                {



                    PlayerNetwork.Instance.PlayerPlaySound("blade_swing");
                    PlayerManager.Instance.ModifyHealth(pView.owner, -Damage);

                    isHit = true;
                    photonView.TransferOwnership(PhotonNetwork.masterClient);

                    MonoMgr.GetInstance().RemoveUpdateListener(UpdateFunc);
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

    }


    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {

            //  stream.SendNext(transform);
            stream.SendNext(isRanged);
            stream.SendNext(isHit);
            stream.SendNext(isPlayer);
            stream.SendNext(gameObject.activeInHierarchy);
        }
        else
        {

            // TargetPosition = (Vector3)stream.ReceiveNext();
            isRanged = (bool)stream.ReceiveNext();
            isHit = (bool)stream.ReceiveNext();
            isPlayer = (bool)stream.ReceiveNext();
            actived = (bool)stream.ReceiveNext();
        }
    }
}
