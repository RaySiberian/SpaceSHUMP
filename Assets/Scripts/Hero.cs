using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{

    public static Hero S;// Singleton

    [Header("Set in Inspector")]
    // Поля управляющие караблем 
    public float speed = 30;
    public float rollMuilt = -45;
    public float pitchMult = 30;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject lastTriggerGO = null;

    public delegate void WeaponFireDelegate();

    public WeaponFireDelegate fireDelegate;
    
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;

    // Свойство, проверяющее входные значения поля.
    public float ShieldLevel 
    {
        get => _shieldLevel;
        
        private set { _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);Main.S.DelayRestart();
            } 
        } 
    }

    void Awake()
    {
        if(S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake");
        }

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
        print("Triggered: " + other.gameObject.transform.root);
        if (other.gameObject == lastTriggerGO)
        {
            return;
        }

        if(other.gameObject.tag == "Enemy")
        {
            ShieldLevel--;
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("Triggered by non-Enemy" + other.gameObject.transform.root);
        }
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
}
