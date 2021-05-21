using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main S; // ЫЫЫЫ
    private static Dictionary<WeaponType, WeaponDefinition> weaponDict;
    
    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;// Отступ для корректировки позиционирования
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
        {WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield};
    
    private BoundCheck bndCheck;

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundCheck>();
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
        weaponDict = new Dictionary<WeaponType, WeaponDefinition>();
        
        foreach (WeaponDefinition definition in weaponDefinitions)
        {
            weaponDict[definition.type] = definition;
        }
    }
    
    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);// Случайный индекс для префаба
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);// Создание объекта по случайному индексу

        float enemyPadding = enemyDefaultPadding; // Создаю буферную переменную
        if(go.GetComponent<BoundCheck>() != null) // Проверка на наличие скрипта на объекте
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundCheck>().radius);
        }

        Vector3 pos = Vector3.zero; // Настройка позиции спавна
        float xMin = -bndCheck.camWidth + enemyPadding - 15; // Число(15) нужно для коректировки спавна объектов, а то она через ....
        float xMax = bndCheck.camWidth - enemyPadding - 25;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding + 20;
        go.transform.position = pos;

        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }

    public void ShipDestroyed(Enemy e)
    {
        if (Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUP pu = go.GetComponent<PowerUP>();
            pu.SetType(puType);

            pu.transform.position = e.transform.position;
        }
    }
    
    public void DelayRestart()
    {
        Invoke(nameof(Restart), 2);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    public static WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (weaponDict.ContainsKey(wt))
        {
            return weaponDict[wt];
        }

        return new WeaponDefinition();
    }
}
