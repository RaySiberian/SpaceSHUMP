using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 10f; 
    public float heath = 10f; 
    public int score = 100; // Очков за уничтожения
    public float showDamageDuration = 0.1f;

    [Header("Set in Dynamically: Enemy")] 
    public Color[] originalColors;
    public Material[] materials;
    public bool showingDamage = false;
    public bool notifiedOfDestruction;
    public float damageDoneTime;
    
    protected BoundCheck bndCheck;

    protected Vector3 Pos { get => transform.position; set => transform.position = value; }

    private void Awake()
    {
        bndCheck = GetComponent<BoundCheck>();
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        
        for (var i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    private void Update()
    {
        Move();

        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowingDamage();
        }
        
        if(bndCheck != null && bndCheck.offDown) // Если корабли за границей экрана
        {
            if(Pos.y < bndCheck.camHeight - bndCheck.radius) // Если за нижней границей
            {
                Destroy(gameObject);
            }
        }
    }
    protected virtual void Move()
    {
        Vector3 tempPos = Pos;
        tempPos.y -= speed * Time.deltaTime;
        Pos = tempPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        switch (otherGO.gameObject.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                // if (!bndCheck.isOnScreen)
                // {
                //     Destroy(otherGO);
                //     break;
                // }
                
                heath -= Main.GetWeaponDefinition(p.Type).damageOnHit;
                ShowDamage();
                Destroy(otherGO);
                if (heath <= 0)
                {
                    Destroy(gameObject);
                }
                break;
            
            default:
                print("Enemy hit by non Proj: " + otherGO.name);
                break;
        }
    }

    private void ShowDamage()
    {
        foreach (var m in materials)
        {
            m.color = Color.red;
        }

        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    private void UnShowingDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }

        showingDamage = false;
    }
}
