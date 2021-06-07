using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public enum WeaponType
{
    none, // Дефолтная
    blaster, // Простой блатер
    spread, // Веерная пушка
    phaser, // Волновой лазер
    missile, // Самонодящиеся ракеры
    laser, // Нагревочный лазер
    shield
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;

    public Color color = Color.white;
    public GameObject projectillePrefab;
    public Color projectileeColor = Color.white;
    public float damageOnHit = 0;
    public float continiousDamage = 0; // Урон от лазера 

    public float delayBetweenShots = 0;
    public float velocity = 20; // Скорость снарядов
}

public class Weapon : MonoBehaviour
{
    public static Transform projectileAnchor;

    [Header("Set Dynamically")] 
    [SerializeField] private WeaponType type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer _collarRend;

    public static event Action Fired;
    
    public WeaponType Type
    {
        get => type;
        set => SetType(value);
    }
    
    private void Start()
    {
        collar = transform.Find("Collar").gameObject;
        _collarRend = collar.GetComponent<Renderer>();
        

        SetType(type);

        if (projectileAnchor == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            projectileAnchor = go.transform;
        }

        GameObject rootGO = transform.root.gameObject;
        
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public void SetType(WeaponType wt)
    {
        //print("SetType");
        type = wt;
        if (Type != WeaponType.none)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            return;
        }

        def = Main.GetWeaponDefinition(type);
        _collarRend.material.color = def.color;
        lastShotTime = 0;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Fire()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }

        Projectile projectile;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (Type)
        {
            case WeaponType.blaster:
                projectile = MakeProjectile();
                projectile.rb.velocity = vel;
                Fired?.Invoke();
                break;
            
            case WeaponType.spread:
                projectile = MakeProjectile();
                projectile.rb.velocity = vel;
                
                projectile = MakeProjectile();
                projectile.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                projectile.rb.velocity = projectile.transform.rotation * vel;
                
                projectile = MakeProjectile();
                projectile.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                projectile.rb.velocity = projectile.transform.rotation * vel;
                Fired?.Invoke();
                break;
        }
    }

    private Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectillePrefab);
        if (transform.parent.gameObject.CompareTag("Hero"))
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.CompareTag("ProjectileEnemy");
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(projectileAnchor,true);
        Projectile p = go.GetComponent<Projectile>();
        p.Type = Type;
        lastShotTime = Time.time;
        return p;
    }
}
