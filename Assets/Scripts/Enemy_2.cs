using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{

    [Header("Set in Inspector")]
    public float sinEccentricity = 0.6f; // Определяет, насколько ярко выражен синусоид
    public float lifeTime = 10;

    [Header("Set Dynamically")]
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;

    private void Start()
    {
        // Выбор случайной точки в левой границе экрана
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        // Выбор случайной точки в Правой границе экрана
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        // Случайно поменять конечную и начальную точку местами
        if(Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }
        birthTime = Time.time;
    }

    protected override void Move()
    {
        // Определение длителости жизни врага
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        // Попытка в линейную интерполяцию
        // Работает, но хрен пойми почему
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));

        Pos = (1 - u) * p0 + u * p1;
    }
}
