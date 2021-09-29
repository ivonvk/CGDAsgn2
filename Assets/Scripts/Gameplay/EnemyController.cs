using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;



[System.Serializable]
public class EnemyStatus
{
    public string Name = "";

    public int Health = 20;

    public int Damage = 1;
    public float AttackSpeed = 3f;

    public float MoveSpeed = 1.5f;

    public bool isRangedAtk = false;
    public float AttackRange = 0.75f;
}
public class EnemyController : Photon.MonoBehaviour
{
    public PlayerController TargetPlayer;
    public PlayerController FarTarget;

    private Collider col;
    private NavMeshAgent nav;
    public EnemyStatus enemyStatus;
    public PhotonView PhotonView;

    private Quaternion ModelTargetRot;

    private Vector3 TargetPosition;
    private Quaternion TargetRotation;

    private GameObject WeaponObject;

    private float CurrentAttackSpeed;

    public Animator anim;

    float GetHitDelay = 0f;

    public List<Material> Mats = new List<Material>();
    public List<Color> MatsDefaultColor = new List<Color>();

    public SkinnedMeshRenderer[] render_default;
    bool effectSpawn = false;
    bool death = false;
    void Awake()
    {
        if (enemyStatus == null)
            enemyStatus = new EnemyStatus();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = enemyStatus.MoveSpeed;
        col = GetComponent<Collider>();
        PhotonView = GetComponent<PhotonView>();

        anim = transform.GetChild(0).GetComponent<Animator>();

        render_default = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < render_default.Length - 1; i++)
        {
            for (int x = 0; x < render_default[i].materials.Length; x++)
            {

                render_default[i].materials[x].EnableKeyword("_EMISSION");
                Mats.Add(render_default[i].materials[x]);

                Material m = render_default[i].materials[x];
                MatsDefaultColor.Add(m.color);
            }
        }
        CurrentAttackSpeed = enemyStatus.AttackSpeed;

    }
    void AttackTarget()
    {

        if (!PhotonNetwork.isMasterClient || (PhotonNetwork.isMasterClient && GameMaster.Instance.Generating)|| !PhotonNetwork.inRoom)
        {
            return;
        }
        else
        {
            if (CurrentAttackSpeed > 0)
            {
                CurrentAttackSpeed -= 1f * Time.deltaTime;

            }
            else if (TargetPlayer != null)
            {
                if (!enemyStatus.isRangedAtk)
                {
                    PlayerStatus CheckingStatus = PlayerManager.Instance.GetPlayerStatus(TargetPlayer.photonView.owner);
                    if (CheckingStatus.Health > 0)
                    {
                        CurrentAttackSpeed = enemyStatus.AttackSpeed;
                        PhotonView photonView = TargetPlayer.GetComponent<PhotonView>();

                        var dist = Vector3.Distance(TargetPlayer.transform.position, transform.position);
                        if (dist < enemyStatus.AttackRange)
                        {
                            anim.SetTrigger("attack");
                            PlayerNetwork.Instance.PlayerPlaySound("blade_swing");
                            PlayerManager.Instance.ModifyHealth(photonView.owner, -enemyStatus.Damage);

                        }
                    }
                }
                else
                {
                    var dist = Vector3.Distance(TargetPlayer.transform.position, transform.position);
                    if (dist < enemyStatus.AttackRange)
                    {
                        if (enemyStatus.Name == "Cookies_1")
                        {
                            CurrentAttackSpeed = enemyStatus.AttackSpeed;
                            GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "EnemyMeleeAtk"), (transform.position + (transform.forward * 0.75f) + new Vector3(0, 0.5f, 0)), transform.rotation, 0, null);

                            obj.GetComponent<WeaponAttack>().SetWeapon(photonView.owner, true);

                        }
                        else if (enemyStatus.Name == "Ghost_2")
                        {
                            CurrentAttackSpeed = enemyStatus.AttackSpeed;
                            GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "EnemyAtkBall"), (transform.position + (transform.forward * 1.5f) + new Vector3(0, 0.5f, 0)), transform.rotation, 0, null);

                            obj.GetComponent<WeaponAttack>().SetWeapon(photonView.owner, true);

                            GameObject obj2 = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "EnemyAtkBall"), (transform.position + (transform.forward * 1f) + new Vector3(0, 0.5f, 0)), transform.rotation, 0, null);

                            obj2.GetComponent<WeaponAttack>().SetWeapon(photonView.owner, true);

                            GameObject obj3 = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "EnemyAtkBall"), (transform.position + (transform.forward * 0.5f) + new Vector3(0, 0.5f, 0)), transform.rotation, 0, null);

                            obj3.GetComponent<WeaponAttack>().SetWeapon(photonView.owner, true);
                        }
                        else if (enemyStatus.Name == "Ghoul_3")
                        {
                            CurrentAttackSpeed = enemyStatus.AttackSpeed;
                            GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "EnemyMeleeAtk2"), (transform.position + (transform.forward * 0.75f) + new Vector3(0, 0.5f, 0)), transform.rotation, 0, null);

                            obj.GetComponent<WeaponAttack>().SetWeapon(photonView.owner, true);
                        }
                        else if (enemyStatus.Name == "Alien_4")
                        {
                            CurrentAttackSpeed = enemyStatus.AttackSpeed;
                            GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "EnemyAtkBall2"), (transform.position + (transform.forward * 0.5f) + new Vector3(0, 0.5f, 0)), transform.rotation, 0, null);

                            obj.GetComponent<WeaponAttack>().SetWeapon(photonView.owner, true);

                            GameObject obj2 = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "EnemyAtkBall2"), (transform.position + (transform.forward * 0.5f) + new Vector3(0, 0.5f, 0)), Quaternion.Euler(transform.eulerAngles + new Vector3(0, 20, 0)), 0, null);

                            obj2.GetComponent<WeaponAttack>().SetWeapon(photonView.owner, true);

                            GameObject obj3 = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "EnemyAtkBall2"), (transform.position + (transform.forward * 0.5f) + new Vector3(0, 0.5f, 0)), Quaternion.Euler(transform.eulerAngles + new Vector3(0, -20, 0)), 0, null);

                            obj3.GetComponent<WeaponAttack>().SetWeapon(photonView.owner, true);
                        }


                    }
                }
            }
        }
    }

    public void GetDamage(int dmg, int id)
    {
        // if (!PhotonNetwork.isMasterClient)
        //     return;

        if (enemyStatus.Health <= 0)
        {
            photonView.RPC("RPC_Death", PhotonTargets.All);
            return;
        }
            enemyStatus.Health -= dmg;
        GetHitDelay = 0.9f;

        anim.SetTrigger("gethit");
        if (!effectSpawn)
        {
            photonView.RPC("RPC_SpawnEffect", PhotonTargets.All, "Slash_Effect");
            photonView.RPC("RPC_HitEffect", PhotonTargets.All);
        }

        if (enemyStatus.Health <= 0)
        {

            UpgradeStatus u = new UpgradeStatus();
            u.AttackSpeed = 0;
            u.Cost = 2;
            u.Damage = 0;
            u.GivePoint = false;
            u.MaxHealth = 0;
            u.MaxMagic = 0;
            u.WalkSpeed = 0;
            u.GetPoint = true;
            PlayerManager.Instance.UpdateStatusFunc(u,id);

            photonView.RPC("RPC_Death", PhotonTargets.All);
            col.enabled = false;
            anim.SetBool("death", true);
            
            PhotonView.RPC("RPC_OtherRemoveUpdateListener", PhotonTargets.All);

                Invoke("DelayDestroy", 1f);
            
        }

    }
    void DelayDestroy()
    {


        PhotonView.RPC("RPC_DelayDestroy", PhotonTargets.MasterClient);

    }
    [PunRPC]
    void RPC_DelayDestroy()
    {
        
        PhotonNetwork.Destroy(gameObject);
        GameMaster.Instance.CurrentEnemiesLeft -= 1;
    }
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {

        if (PhotonNetwork.isMasterClient)
        {
            PhotonView.TransferOwnership(PhotonNetwork.masterClient);
            MonoMgr.GetInstance().AddUpdateListener(Movement);
            MonoMgr.GetInstance().AddUpdateListener(TargetingObject);
            MonoMgr.GetInstance().AddUpdateListener(AttackTarget);


            MonoMgr.GetInstance().RemoveUpdateListener(SmoothMovement);
        }

    }
    void OnDestroy()
    {

        //MonoMgr.GetInstance().RemoveUpdateListener(Movement);
        //MonoMgr.GetInstance().RemoveUpdateListener(TargetingObject);
        //MonoMgr.GetInstance().RemoveUpdateListener(SmoothMovement);


        MonoMgr.GetInstance().RemoveUpdateListener(Movement);
        MonoMgr.GetInstance().RemoveUpdateListener(TargetingObject);
        MonoMgr.GetInstance().RemoveUpdateListener(SmoothMovement);
        MonoMgr.GetInstance().RemoveUpdateListener(AttackTarget);

    }


    [PunRPC]
    void RPC_OtherRemoveUpdateListener()
    {
        
           
        
        MonoMgr.GetInstance().RemoveUpdateListener(Movement);
        MonoMgr.GetInstance().RemoveUpdateListener(TargetingObject);
        MonoMgr.GetInstance().RemoveUpdateListener(SmoothMovement);
        MonoMgr.GetInstance().RemoveUpdateListener(AttackTarget);

    }
    void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {


            MonoMgr.GetInstance().AddUpdateListener(Movement);
            MonoMgr.GetInstance().AddUpdateListener(TargetingObject);
            MonoMgr.GetInstance().AddUpdateListener(AttackTarget);
        }
        else
        {
            MonoMgr.GetInstance().AddUpdateListener(SmoothMovement);

        }
    }
    void SmoothMovement()
    {

        if (!effectSpawn)
        {
            for (int i = 0; i < Mats.Count; i++)
            {
                Mats[i].SetVector("_EmissionColor", Color.black);
            }
        }
        else if (effectSpawn)
        {
            for (int i = 0; i < Mats.Count; i++)
            {
                Mats[i].SetVector("_EmissionColor", new Color(3f, 3, 3, 1f));
            }
        }
        transform.position = Vector3.Slerp(transform.position, TargetPosition, 10f);
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, 10f);
    }
    void Movement()
    {
        if (!PhotonNetwork.isMasterClient || !nav || (PhotonNetwork.isMasterClient && GameMaster.Instance.Generating)||!PhotonNetwork.inRoom)
            return;
        if (!nav.isOnNavMesh)
        {
            nav.Warp(transform.position);
        }
        if (TargetPlayer != null)
        {
            PlayerStatus CheckingStatus = PlayerManager.Instance.GetPlayerStatus(TargetPlayer.photonView.owner);
            if (CheckingStatus != null && CheckingStatus.Health > 0&& !CheckingStatus.isDown)
            {
                nav.isStopped = false;
                nav.SetDestination(TargetPlayer.transform.position);
            }
            else
            {
                TargetPlayer = null;
                nav.isStopped = true;
            }
        }
        else if (FarTarget != null)
        {
            PlayerStatus CheckingStatus = PlayerManager.Instance.GetPlayerStatus(FarTarget.photonView.owner);
            if (CheckingStatus != null && CheckingStatus.Health > 0)
            {
                nav.isStopped = false;
                nav.SetDestination(FarTarget.transform.position);
            }
            else
            {
                nav.isStopped = true;
            }
        }
        if (nav.velocity != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(nav.velocity);
            ModelTargetRot = desiredRotation;
        }
        if (anim)
        {
            anim.SetFloat("walk", nav.velocity.magnitude);
            if (enemyStatus.Health > 0)
            {
                anim.SetBool("death", false);
            }
        }
        if (death)
        {
            for (int i = 0; i < Mats.Count; i++)
            {
                Mats[i].SetVector("_EmissionColor", new Color(3f, 0, 0, 1f));
            }
        }else 
        if (!effectSpawn)
        {
            for (int i = 0; i < Mats.Count; i++)
            {
                Mats[i].SetVector("_EmissionColor", Color.black);
            }
        }
        else if (enemyStatus.Health > 0)
        {
            for (int i = 0; i < Mats.Count; i++)
            {
                Mats[i].SetVector("_EmissionColor", new Color(3f, 3, 3, 1f));
            }
        }
        if (GetHitDelay > 0f)
        {
            if (nav)
            {
                nav.velocity = Vector3.zero;
                nav.isStopped = true;
                nav.avoidancePriority = 0;
            }
            GetHitDelay -= 3.75f * Time.deltaTime;
            anim.speed = 0;
        }
        else
        {
            if (effectSpawn)
            {
                photonView.RPC("RPC_NormalEffect", PhotonTargets.All);
            }
            if (nav)
            {
                nav.avoidancePriority = 50;
                anim.speed = 1;
                nav.isStopped = false;
            }
        }
    }
    
    [PunRPC]
    void RPC_SpawnEffect(string name)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.InstantiateSceneObject("Prefabs/" + name, transform.position * Random.Range(0.98f, 1.02f), Quaternion.identity, 0, null);
        }
    }
    [PunRPC]
    void RPC_Death()
    {
        death = true;
        for (int i = 0; i < Mats.Count; i++)
        {
            Mats[i].SetVector("_EmissionColor", new Color(3f, 0, 0, 1f));
        }
    }
    [PunRPC]
    void RPC_HitEffect()
    {
        effectSpawn = true;
    }
    [PunRPC]
    void RPC_NormalEffect()
    {
        effectSpawn = false;
    }
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (photonView == null)
            return;
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(effectSpawn);
            stream.SendNext(death);
        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
            effectSpawn = (bool)stream.ReceiveNext();
            death = (bool)stream.ReceiveNext();
        }
    }
    void TargetingObject()
    {
        if (!PhotonNetwork.isMasterClient || !PhotonNetwork.inRoom){
            MonoMgr.GetInstance().RemoveUpdateListener(Movement);
            MonoMgr.GetInstance().RemoveUpdateListener(TargetingObject);
            MonoMgr.GetInstance().RemoveUpdateListener(SmoothMovement);
            MonoMgr.GetInstance().RemoveUpdateListener(AttackTarget);
            return;
        }
        float minimumDistance = Mathf.Infinity;
        if (transform != null)
        {
            Vector3 center = transform.position;

            float radius = 200f;

            Collider[] hitColliders = Physics.OverlapSphere(center, radius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Player"))
                {
                    var dist = Vector3.Distance(hitCollider.transform.position, transform.position);
                    if (dist < minimumDistance)
                    {
                        minimumDistance = dist;
                        PlayerController PreTarget = hitCollider.GetComponent<PlayerController>();
                        PlayerStatus CheckingStatus = PlayerManager.Instance.GetPlayerStatus(PreTarget.photonView.owner);
                        if (CheckingStatus != null && CheckingStatus.Health > 0 && !CheckingStatus.isDown)
                        {
                            TargetPlayer = PreTarget;
                            nav.isStopped = false;
                        }
                    }
                }
            }
        }
        if (TargetPlayer != null)
        {
            var playerdist = Vector3.Distance(transform.position, TargetPlayer.transform.position);
            if (playerdist > 5f)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                if (FarTarget == null)
                {
                    PlayerController p = players[Random.Range(0, players.Length - 1)].GetComponent<PlayerController>();
                    PlayerStatus CheckingStatus = PlayerManager.Instance.GetPlayerStatus(p.photonView.owner);
                    if (!CheckingStatus.isDown)
                    {
                        FarTarget = p;
                    }
                }
            }
        }
    }
}
