using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in inspector")]
    public float rotationsPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    Material mat;


    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Получаем текущий уровень поля из класса Хиро
        int currLevel = Mathf.FloorToInt(Hero.S.ShieldLevel);

        if(levelShown != currLevel)
        {
            levelShown = currLevel;
            // Корректировка смены текстуры, для отображения поля другого уровня
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        // Невменяемая формула для вращения щита за единицу времени вокруг своей оси
        float rZ = -(rotationsPerSecond * Time.deltaTime * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }

    
}
