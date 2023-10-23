using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weapon : MonoBehaviour
{

    [Header("Weapon Stats")]
    public float damage;
    public float rang;
    public float FireRate;
    public int AmmoInMag;
    public int MaxInMag;
    public int AmmoInTotal;
    public int MaxAmmoInTotal;
    public int remove;
    public Text AmmoInMagCountText;
    public Text Total;
    public float realoadTime;
    public bool IsRealoading = false;

    [Header("Weapon Type")]
    public bool SingleFire = false;
    public bool AutoFire = false;

    [Header("Weapon Animation")]
    public string RealoadAnimationString;
    public Animator Guns;


    [Header("Inputs")]
    public Camera CAM;
    public AudioSource shot;
    public Vector3 upRecoil;
    Vector3 orignalRotation;
    public GameObject midRightArm;
    public int GunSloat;
    public GameObject Parent;
    private float NextTimeToFire;

    public GameManager Manager;

    private void Start()
    {
        orignalRotation = midRightArm.transform.localEulerAngles;
        
    }
    private void Awake()
    {
        Manager.MaxAmmoInWeapond = MaxAmmoInTotal;
    }


    void Update()
    {
        
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= NextTimeToFire && AutoFire == true && AmmoInMag != 0 && IsRealoading == false || Input.GetKey(KeyCode.Mouse0) && Time.time >= NextTimeToFire && SingleFire == false && AmmoInMag != 0 && IsRealoading == false)
        {
            NextTimeToFire = Time.time + 1f / FireRate;
            Shoot();
            StartCoroutine(Recoil());
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= NextTimeToFire && SingleFire == true || Input.GetKey(KeyCode.Mouse0) && Time.time >= NextTimeToFire && AutoFire == false )
        {
            NextTimeToFire = Time.time + 1f / FireRate;
            Shoot();

        }
        
        AmmoInMagCountText.text = AmmoInMag.ToString();
        Total.text = AmmoInTotal.ToString();
        if (Input.GetKeyDown(KeyCode.R) && AmmoInMag != MaxInMag) 
        {
            StartCoroutine(Reload());
        }


        RaycastHit Hit;
        if (Physics.Raycast(CAM.transform.position, CAM.transform.forward, out Hit, rang))
        {

            if (Hit.transform.tag == "PickUpGun")
            {
                GameObject gun = Hit.transform.gameObject;

                Weapond PickUpGun = gun.GetComponent<Weapond>();
                gun.GetComponent<Weapond>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F)) // Does not work atm 
                {
                    
                    gun.transform.SetParent(PickUpGun.Parent.transform);
                    gun.transform.SetSiblingIndex(GunSloat);
                    PickUpGun.GunSloat = GunSloat;
                    gun.transform.localPosition = new Vector3(0, 0, 0);
                   gun.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    //Destroy(gameObject);
                }

            }

        }


    



}


    IEnumerator Reload()
    {
        IsRealoading = true;
        Guns.SetBool(RealoadAnimationString, true);
        yield return new WaitForSeconds(realoadTime);
        if (AmmoInMag < MaxInMag && AmmoInTotal >= MaxInMag)
        {

            
            remove = MaxInMag - AmmoInMag;
            AmmoInMag += remove;
            AmmoInTotal -= remove;
            IsRealoading = false;
            Guns.SetBool(RealoadAnimationString, false);

        }
        else if (AmmoInMag < MaxInMag && AmmoInTotal <= MaxInMag)
        {
            remove = MaxInMag - AmmoInMag;
            if (AmmoInTotal == 0)
            {
                IsRealoading = false;
                Guns.SetBool(RealoadAnimationString, false);
            }
            else if (AmmoInTotal < remove)
            {
                AmmoInMag += AmmoInTotal;
                AmmoInTotal -= AmmoInTotal;
            }
            else
            {
                AmmoInMag += remove;
                AmmoInTotal -= remove;
                IsRealoading = false;
                Guns.SetBool(RealoadAnimationString, false);

            }
            


        }
        else
        {
            IsRealoading = false;
            Guns.SetBool(RealoadAnimationString, false);
        }
    }




//--------------------------------------------------------------------------------------SHOOTING-------------------------------------------------------------------------------------


    void Shoot()
    {
        RaycastHit hit;
       if ( Physics.Raycast(CAM.transform.position, CAM.transform.forward, out hit, rang)  )
        {
            shot.Stop();
            shot.Play();
            
            AmmoInMag -= 1;
            placeholder placeholder = hit.transform.GetComponent <placeholder>();
            if (placeholder != null)
            {
                placeholder.TakeDamage(damage);
            }


        

                Debug.Log("Shot");
        }
    }


    private void AddRecoil()
    {
        transform.localEulerAngles += upRecoil;
    }

    private void StopRecoil()
    {
        transform.localEulerAngles = orignalRotation;
    }

    IEnumerator Recoil()
    {
        Guns.SetBool("placeholder", true);
        yield return new WaitForSeconds(.16f);
        Guns.SetBool("placeholder", false);
    }

}
