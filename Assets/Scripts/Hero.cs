using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{

    public static Hero S; // Singleton

    [Header("Set in Inspector")]
    // Поля управляющие караблем 
    public float speed = 30;
    public float rollMuilt = -45;
    public float pitchMult = 30;

    [Header("Set Dynamically")] [SerializeField]
    private float _shieldLevel = 1;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    private GameObject lastTriggerGO = null;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;
    public Weapon[] weapons;

    private AudioSource _shotSound;
    
    public float ShieldLevel
    {
        get => _shieldLevel;

        private set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayRestart();
            }
        }
    }

    private void Start()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake");
        }

        _shotSound = GetComponent<AudioSource>();
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
        //fireDelegate += TempFire;
    }

    void Update()
    {

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMuilt, 0);
    }

    private void OnTriggerEnter(Collider other) // Столкновения
    {
        if (other.gameObject == lastTriggerGO)
        {
            return;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            ShieldLevel--;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            AbsorbPowerUp(other.gameObject);
        }
        else
        {
            Debug.Log("Triggered by non-Enemy" + other.gameObject.transform.root);
        }
    }


    private void AbsorbPowerUp(GameObject go)
    {
        // Нечеловеческий костыль 
        CubeHelper cubeHelper = go.GetComponent<CubeHelper>();
        switch (cubeHelper.pu.type)
        {
            case WeaponType.shield:
                _shieldLevel++;
                break;
            
            default:
                print($"Default");
                if (cubeHelper.pu.type == weapons[0].Type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(cubeHelper.pu.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(cubeHelper.pu.type);
                }
                break;
        }

        cubeHelper.pu.AbsorbedBy(gameObject);
    }

    private void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.up * projectileSpeed;

        Projectile proj = projGO.GetComponent<Projectile>();
        proj.Type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.Type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }

    private Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].Type == WeaponType.none)
            {
                print(weapons[i].gameObject.name);
                return weapons[i];
            }
        }

        return null;
    }

    private void ClearWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.SetType(WeaponType.none);
        }
    }

    private void PlaySoundShot()
    {
        _shotSound.Play();
    }
    
    private void OnEnable()
    {
        Weapon.Fired += PlaySoundShot;
    }
    
    private void OnDisable()
    {
        Weapon.Fired -= PlaySoundShot;
    }
}
