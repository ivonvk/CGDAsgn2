using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class PlayerStatus
{
    public PlayerStatus(PhotonPlayer photonPlayer, int maxhealth, int maxmagic)
    {
        PhotonPlayer = photonPlayer;
        MaxHP = maxhealth;
        MaxMP = maxmagic;
        Name = "";
        Damage = 1;
        AttackSpeed = 1.1f;

        MoveSpeed = 4f;


    }

    public PhotonPlayer PhotonPlayer;
    public string Name = "";

    public float Health = 0f;
    public int MaxHP = 300;
    public int MaxMP = 300;
    public float Magic = 100f;
    public int Damage = 1;
    public float AttackSpeed = 0.5f;

    public float MoveSpeed = 1.5f;

    public bool isHurt = false;
    public float HurtCounter = 0f;

    public int PublicUpgradePoint = 100;
    public int UpgradePoint = 40;

    public int StandupChange = 3;

    public bool isDown = true;

}
public class PlayerController : Photon.MonoBehaviour
{

    public PlayerStatus playerStatus;

    private PhotonView PhotonView;
    private Vector3 TargetPosition;
    private Quaternion TargetRotation;
    private Vector3 TargetVelocity;
    private Quaternion ModelTargetRot;

    private Transform ModelTrans;
    private Transform CameraTrans;
    public GameObject WeaponObject;

    public bool WeaponShow;
    private NavMeshAgent nav;


    public bool RangedAttack;
    public bool MagicAttack;
    public bool Shield;
    private float AttackCooldown;


    private Transform CharacterTrans;

    public Animator anim;
    public List<Animator> AllAnims = new List<Animator>();
    public List<GameObject> AllWeaponObject = new List<GameObject>();

    public List<GameObject> PlayerIcon = new List<GameObject>();

    float FireTimer = 0;

    bool isOpenCharacterPanel = false;

    public void SetPlayerCharacter(PhotonPlayer player, int c, int w, int icon)
    {

        if (PhotonNetwork.player == player)
        {
            
            AllAnims[c].gameObject.SetActive(true);
            anim = AllAnims[c];
            AllWeaponObject[0].gameObject.SetActive(true);
            PlayerIcon[icon].SetActive(true);
            WeaponAttack wAtteck = WeaponObject.GetComponent<WeaponAttack>();
            wAtteck.SetPlayer(PhotonView, true);

            if (c == 2 || c == 6)
            {
                RangedAttack = true;
            }
            else if (c == 3 || c == 7)
            {
                MagicAttack = true;
            }
            else
            {
                if (c == 1 || c == 5)
                {
                    Shield = true;
                }
                WeaponObject = AllWeaponObject[w];
            }
            playerStatus.Health = playerStatus.MaxHP;
        }




        photonView.RPC("RPC_ShowCharacter", PhotonTargets.Others, player, c, w, icon);

    }
    [PunRPC]
    void RPC_ShowCharacter(PhotonPlayer player, int c, int w, int icon)
    {
        if (PhotonNetwork.player != player)
        {
            playerStatus.Health = playerStatus.MaxHP;
            AllAnims[c].gameObject.SetActive(true);
            anim = AllAnims[c];
            PlayerIcon[icon].SetActive(true);
            
            if (c == 2 || c == 6)
            {
                RangedAttack = true;
            }
            else if (c == 3 || c == 7)
            {
                MagicAttack = true;
            }
            else
            {
                if (c == 1 || c == 5)
                {
                    Shield = true;
                }
                WeaponObject = AllWeaponObject[w];
                //AllWeaponObject[w].gameObject.SetActive(true);
            }
        }


    }
    public void UpdateWeapon(int Damage)
    {

    }
   public void SetPlayerStatus(PlayerStatus status)
    {
        playerStatus = status;
    }
    public PlayerStatus GetPlayerStatus()
    {
        return playerStatus;
    }
    void Awake()
    {
        //photonView.ObservedComponents.Add(anim);
        RangedAttack = false;
        MagicAttack = false;
        nav = GetComponent<NavMeshAgent>();

        CharacterTrans = transform.Find("CharacterTrans");
        ModelTrans = CharacterTrans;
        PhotonView = GetComponent<PhotonView>();

        CameraTrans = transform.Find("CameraPos");



        //photonView.ObservedComponents.Add(wAtteck);
        //wAtteck.gameObject.SetActive(false);

    }
    void OnDestroy()
    {
        MonoMgr.GetInstance().RemoveUpdateListener(SmoothMovement);
        MonoMgr.GetInstance().RemoveUpdateListener(Movement);
        MonoMgr.GetInstance().RemoveUpdateListener(Combat);
         MonoMgr.GetInstance().RemoveUpdateListener(UpdateDisplay);

        MonoMgr.GetInstance().RemoveUpdateListener(Attack);
    }


    void Start()
    {

       
        if (PhotonView.isMine)
        {
           

            MonoMgr.GetInstance().AddUpdateListener(Movement);
                MonoMgr.GetInstance().AddUpdateListener(Combat);

                 MonoMgr.GetInstance().AddUpdateListener(UpdateDisplay);
                MonoMgr.GetInstance().AddUpdateListener(Attack);
                GameMaster.Instance.MapCam.transform.position = new Vector3(transform.position.x, GameMaster.Instance.MapCam.transform.position.y, transform.position.z);
                GameMaster.Instance.MapCam.transform.SetParent(CameraTrans);
                Camera.main.transform.position = CameraTrans.position;
                Camera.main.transform.SetParent(CameraTrans);
                Camera.main.fieldOfView = 90f;
                Camera.main.transform.rotation = Quaternion.Euler(50f, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
                // EventsCenter.GetInstance().AddEventListener<KeyCode>("Attack", Attack);
            
        }
        else
        {
            MonoMgr.GetInstance().AddUpdateListener(SmoothMovement);

        }
    }

    // Update is called once per frame
    void SmoothMovement()
    {
        transform.position = Vector3.Slerp(transform.position, TargetPosition, 1f * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation,1f * Time.deltaTime);

    }
    //void Attack(KeyCode key)
    void Attack()
    {
        if (!photonView.isMine)
        {
            MonoMgr.GetInstance().RemoveUpdateListener(SmoothMovement);
            MonoMgr.GetInstance().RemoveUpdateListener(Movement);
            MonoMgr.GetInstance().RemoveUpdateListener(Combat);
            MonoMgr.GetInstance().RemoveUpdateListener(UpdateDisplay);

            MonoMgr.GetInstance().RemoveUpdateListener(Attack);
            return;
        }
        if (PlayerManager.Instance.PlayerStatus.Count <= 0 || PlayerManager.Instance.GetPlayerStatus(photonView.owner) == null)
        {
            AttackCooldown = 0f;
            FireTimer = 0f;
            return;
        }
         if (PlayerManager.Instance.PlayerStatus.Count == PhotonNetwork.playerList.Length - 1 || PlayerManager.Instance.GetPlayerStatus(photonView.owner).isDown)
        {
            AttackCooldown = 0f;
            FireTimer = 0f;
            return;
        }
        

        if (Input.GetKeyDown(KeyCode.U))
        {
            PanelMgr.GetInstance().ShowOrHidePanel("UpgradePanel", true);
        }
        if (Input.GetKeyDown(KeyCode.T) && RangedAttack)
        {
            if (PlayerManager.Instance.GetPlayerStatus(photonView.owner).PublicUpgradePoint - 3 >= 0)
            {

                UpgradeStatus u = new UpgradeStatus();
                u.AttackSpeed = 0;
                u.Cost = 3;
                u.Damage = 0;
                u.MaxHealth = 0;
                u.MaxMagic = 0;
                u.WalkSpeed = 0;
                u.GetPublicPoint = true;
                PlayerManager.Instance.UpdateStatusFunc(u, photonView.owner.ID-1);


                GameMaster.Instance.SpawnPlayerFuncObj(photonView.owner, transform.position, "Trap");
            }

        }
        if (Input.GetKeyDown(KeyCode.T) && Shield)
        {
            Debug.Log("B");
            if (PlayerManager.Instance.GetPlayerStatus(photonView.owner).PublicUpgradePoint - 20 >= 0)
            {

                UpgradeStatus u = new UpgradeStatus();
                u.AttackSpeed = 0;
                u.Cost = 20;
                u.Damage = 0;
                u.MaxHealth = 0;
                u.MaxMagic = 0;
                u.WalkSpeed = 0;
                u.GetPublicPoint = true;
                PlayerManager.Instance.UpdateStatusFunc(u, photonView.owner.ID-1);


                GameMaster.Instance.SpawnPlayerFuncObj(photonView.owner, transform.position, "Barricade");
            }

        }
        /*  if (Input.GetKeyDown(KeyCode.T) && MagicAttack)
          {
              if (PlayerManager.Instance.GetPlayerStatus(photonView.owner).PublicUpgradePoint - 30 >= 0)
              {

                  UpgradeStatus u = new UpgradeStatus();
                  u.AttackSpeed = 0;
                  u.Cost = 30;
                  u.Damage = 0;
                  u.MaxHealth = 0;
                  u.MaxMagic = 0;
                  u.WalkSpeed = 0;
                  u.GetPublicPoint = true;
                  PlayerManager.Instance.UpdateStatusFunc(u, photonView.owner);


                  GameMaster.Instance.SpawnPlayerFuncObj(photonView.owner, transform.position, "Camp");
              }

          }*/
        if (Input.GetMouseButtonDown(0) && ModelTrans != null)
        {




            if (AttackCooldown <= 0f)
            {

                PlayerNetwork.Instance.PlayerPlaySound("blade_swing");
                if (anim.gameObject.activeInHierarchy)
                {
                    anim.SetTrigger("attack");
                }
                if (RangedAttack)
                {
                    FireTimer = 0.25f;
                    AttackCooldown = PlayerManager.Instance.GetPlayerStatus(photonView.owner).AttackSpeed;

                }
                else if (MagicAttack)
                {
                    FireTimer = 0.01f;
                    AttackCooldown = PlayerManager.Instance.GetPlayerStatus(photonView.owner).AttackSpeed;
                }
                else
                {
                    WeaponShow = true;
                    WeaponObject.SetActive(WeaponShow);
                    AttackCooldown = PlayerManager.Instance.GetPlayerStatus(photonView.owner).AttackSpeed;
                }
            }
        }
        if (FireTimer > 0)
        {
            FireTimer -= 1f * Time.deltaTime;
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                ModelTrans.LookAt(pointToLook);
            }



            Quaternion rot = ModelTrans.rotation;
            rot.z = 0;
            rot.x = 0;
            ModelTrans.rotation = rot;

            if (!MagicAttack)
            {
                // GameMaster.Instance.RangedWeaponObjectShoot(photonView.owner, ModelTrans.transform.position+(ModelTrans.transform.forward*1.5f)+new Vector3(0,1f,0), ModelTrans.transform.eulerAngles + new Vector3(0, 10, 0), true);
                GameMaster.Instance.RangedWeaponObjectShoot(PhotonNetwork.player, ModelTrans.transform.position + (ModelTrans.transform.forward * 1.5f) + new Vector3(0, 1f, 0), ModelTrans.transform.eulerAngles, true, false);
                //   GameMaster.Instance.RangedWeaponObjectShoot(photonView.owner, ModelTrans.transform.position + (ModelTrans.transform.forward * 1.5f) + new Vector3(0, 1f, 0), ModelTrans.transform.eulerAngles + new Vector3(0, -10, 0), true);
            }
            else
            {
                GameMaster.Instance.RangedWeaponObjectShoot(PhotonNetwork.player, ModelTrans.transform.position + new Vector3(0, 1f, 0), ModelTrans.transform.eulerAngles + new Vector3(0, 10, 0), true, true);
                GameMaster.Instance.RangedWeaponObjectShoot(PhotonNetwork.player, ModelTrans.transform.position + new Vector3(0, 1f, 0), ModelTrans.transform.eulerAngles + new Vector3(0, -10, 0), true, true);
                GameMaster.Instance.RangedWeaponObjectShoot(PhotonNetwork.player, ModelTrans.transform.position + new Vector3(0, 1f, 0), ModelTrans.transform.eulerAngles + new Vector3(0, 20, 0), true, true);
                GameMaster.Instance.RangedWeaponObjectShoot(PhotonNetwork.player, ModelTrans.transform.position + new Vector3(0, 1f, 0), ModelTrans.transform.eulerAngles + new Vector3(0, -20, 0), true, true);

                GameMaster.Instance.RangedWeaponObjectShoot(PhotonNetwork.player, ModelTrans.transform.position + new Vector3(0, 1f, 0), ModelTrans.transform.eulerAngles, true, true);
            }


        }
    }

    void Combat()
    {
        if (!photonView.isMine)
        {
            MonoMgr.GetInstance().RemoveUpdateListener(SmoothMovement);
            MonoMgr.GetInstance().RemoveUpdateListener(Movement);
            MonoMgr.GetInstance().RemoveUpdateListener(Combat);
            MonoMgr.GetInstance().RemoveUpdateListener(UpdateDisplay);

            MonoMgr.GetInstance().RemoveUpdateListener(Attack);
            return;
        }
        if (PlayerManager.Instance.PlayerStatus.Count<=0 || PlayerManager.Instance.GetPlayerStatus(photonView.owner) == null)
        {
            AttackCooldown = 0f;
            return;
        }
        if (PlayerManager.Instance.PlayerStatus.Count==PhotonNetwork.playerList.Length-1&&PlayerManager.Instance.GetPlayerStatus(photonView.owner).isDown)
        {
            AttackCooldown = 0f;
            return;
        }
        if (AttackCooldown > 0f)
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                ModelTrans.LookAt(pointToLook);
            }



            Quaternion rot = ModelTrans.rotation;
            rot.z = 0;
            rot.x = 0;
            ModelTrans.rotation = rot;
            AttackCooldown -= 1f * Time.deltaTime;
        }
        else
        {
            WeaponShow = false;

            WeaponObject.SetActive(WeaponShow);
        }
    }
    void Movement()
    {
        if (!photonView.isMine)

        {
            MonoMgr.GetInstance().RemoveUpdateListener(SmoothMovement);
            MonoMgr.GetInstance().RemoveUpdateListener(Movement);
            MonoMgr.GetInstance().RemoveUpdateListener(Combat);
            MonoMgr.GetInstance().RemoveUpdateListener(UpdateDisplay);

            MonoMgr.GetInstance().RemoveUpdateListener(Attack);
            return;
        }
        if (PlayerManager.Instance.PlayerStatus.Count <= 0|| PlayerManager.Instance.GetPlayerStatus(photonView.owner)==null)
        {
            AttackCooldown = 0f;
            return;
        }
       /* if (PlayerManager.Instance.PlayerStatus.Count == PhotonNetwork.playerList.Length - 1 && anim!=null&&
            PlayerManager.Instance.GetPlayerStatus(photonView.owner)!=null&& anim.gameObject.activeInHierarchy)
        {

            anim.SetBool("death",true);
            return;
        }
        else
        {
            anim.SetBool("death",false);
        }*/
       

        if (PlayerManager.Instance.GetPlayerStatus(photonView.owner) == null || PlayerManager.Instance == null || photonView == null)
            return;
        if (PlayerManager.Instance.GetPlayerStatus(photonView.owner).isDown && nav != null && anim != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (PlayerManager.Instance.GetPlayerStatus(photonView.owner).PublicUpgradePoint - 10 >= 0)
                {

                    PlayerManager.Instance.GetPlayerStatus(photonView.owner).Health = PlayerManager.Instance.GetPlayerStatus(photonView.owner).MaxHP;
                    UpgradeStatus u = new UpgradeStatus();
                    u.AttackSpeed = 0;
                    u.Cost = 10;
                    u.Damage = 0;
                    u.MaxHealth = 0;
                    u.MaxMagic = 0;
                    u.WalkSpeed = 0;
                    u.GetPublicPoint = true;
                    PlayerManager.Instance.UpdateStatusFunc(u, photonView.owner.ID-1);


                    PlayerManager.Instance.GetPlayerStatus(photonView.owner).isDown = false;
                    transform.position += transform.forward * Random.Range(-2, 2);
                }



            }
            nav.velocity = Vector3.zero;
            if (anim.gameObject.activeInHierarchy)
            {
                anim.SetFloat("walk", 0);
            }
            return;
        }
        if (PlayerManager.Instance.GetPlayerStatus(photonView.owner).isDown&& anim.gameObject.activeInHierarchy)
        {

            anim.SetBool("death", true);
            return;
        }
        else if(anim.gameObject.activeInHierarchy)
        {
            anim.SetBool("death", false);
        }

        Vector3 vec = new Vector3(-Input.GetAxis("Horizontal"), 0f, -Input.GetAxis("Vertical")).normalized * PlayerManager.Instance.GetPlayerStatus(photonView.owner).MoveSpeed;
        if (anim && anim.gameObject.activeInHierarchy)
        {
            anim.SetFloat("walk", vec.magnitude);
        }
        if (vec.magnitude != 0)
        {
            nav.velocity = new Vector3(vec.x, nav.velocity.y, vec.z);
        }
        else
        {
            nav.velocity = Vector3.zero;
        }
        nav.transform.rotation = Quaternion.identity;
        if (vec.magnitude >= 1f && AttackCooldown <= 0f)
        {
            if (nav.velocity != Vector3.zero && ModelTargetRot != null && ModelTrans != null)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(nav.velocity);
                ModelTargetRot = Quaternion.Slerp(ModelTrans.rotation, desiredRotation, 10 * Time.deltaTime);

                ModelTrans.rotation = ModelTargetRot;
            }
        }
    }
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            //stream.SendNext(transform.rotation);
            stream.SendNext(ModelTrans.rotation);
            stream.SendNext(WeaponObject.activeInHierarchy);
         
        }
        else
        {

            TargetPosition = (Vector3)stream.ReceiveNext();
            //TargetRotation = (Quaternion)stream.ReceiveNext();
            ModelTargetRot = (Quaternion)stream.ReceiveNext();
            WeaponShow = (bool)stream.ReceiveNext();
           
        }
        if (WeaponObject != null)
        {
            WeaponObject.SetActive(WeaponShow);
        }
       
    }
    public void SetIfDown(PhotonPlayer player, bool down)
    {

        playerStatus = PlayerManager.Instance.GetPlayerStatus(player);
        
    }
    void UpdateDisplay()
    {
        if (PlayerNetwork.Instance.GetCurrentPlayerController() != null)
        {
            PlayerNetwork.Instance.GetCurrentPlayerController().SetPlayerStatus(PlayerManager.Instance.PlayerStatus[PhotonNetwork.player.ID-1]);
        }
        //   
        if (PanelMgr.GetInstance() != null && PanelMgr.GetInstance().GetPanelFunction("CharacterSelectPanel") != null&& !isOpenCharacterPanel)
        {


      

            
            PanelMgr.GetInstance().GetPanelFunction("CharacterSelectPanel").ShowMe();
            if (PanelMgr.GetInstance().GetPanelFunction("CharacterSelectPanel").gameObject.activeInHierarchy)
            {
                isOpenCharacterPanel = true;
            }
        }
        else if(isOpenCharacterPanel)
        {
            PanelMgr.GetInstance().GetPanelFunction("GameTopPanel").ShowMe();
            PanelMgr.GetInstance().GetPanelFunction("GameBottomPanel").ShowMe();
            MonoMgr.GetInstance().RemoveUpdateListener(UpdateDisplay);

        }
    }

}


