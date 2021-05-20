using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundCheck _bndCheck;
    private Renderer _renderer;

    [Header("Set Dynamically")] 
    public Rigidbody rb;
    [SerializeField] private WeaponType type;

    public WeaponType Type
    {
        get => type;
        set => SetType(value);
    }
    
    private void Awake()
    {
        _bndCheck = GetComponent<BoundCheck>();
        _renderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }

    private void SetType(WeaponType eType)
    {
        type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(type);
        _renderer.material.color = def.projectileeColor;
    }
    
}
