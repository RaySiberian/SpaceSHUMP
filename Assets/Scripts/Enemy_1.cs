using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector: Enemy_1")]
    public float waveFrequency = 2;
    public float waveWidth = 4;

    private float x0;
    private float birthTime;

    // Start is called before the first frame update
    void Start()
    {
        x0 = Pos.x;
        birthTime = Time.time;
    }

    protected override void Move()
    {
        Vector3 tempPos = Pos;
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        Pos = tempPos;
        
        base.Move();
    }
}
