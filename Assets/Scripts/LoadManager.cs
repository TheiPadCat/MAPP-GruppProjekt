using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{

   


    public void SaveGame()
    {
        PlayerPrefs.SetInt("BaseHealth", Island.Instance.health);

        //spara position av varje turret i scenen
        SnakeScript snakeScript = FindObjectOfType<SnakeScript>();
        for (int i = 0; i < snakeScript.trailList.Count; i++)
        {
            GameObject turret = snakeScript.trailList[i];
            string turretNyckel = "Turret" + i.ToString();
            PlayerPrefs.SetFloat(turretNyckel + "PosX", turret.transform.position.x);
            PlayerPrefs.SetFloat(turretNyckel + "PosY", turret.transform.position.y);
        }
        //spara position av varje realsed turret i scenen
        for(int j = 0; j < snakeScript.releasedBoats.Count; j++)
        {
            GameObject releasedTurret = snakeScript.releasedBoats[j];
            string releasedTurretNyckel = "ReleasedTurret" + j.ToString();
            PlayerPrefs.SetFloat(releasedTurretNyckel + "PosX", releasedTurret.transform.position.x);
            PlayerPrefs.SetFloat(releasedTurretNyckel + "PosY", releasedTurret.transform.position.y);
        }
        //spara player boat position
        CarButABoat carButABoat = FindObjectOfType<CarButABoat>();
        PlayerPrefs.SetFloat("CarButABoatPosX", carButABoat.transform.position.x);
        PlayerPrefs.SetFloat("CarButABoatPosY", carButABoat.transform.position.y);

        //spara alla object med enemyscript
        EnemyScript[] objectsWithEnemyScript = FindObjectsOfType<EnemyScript>();
        for(int k = 0; k < objectsWithEnemyScript.Length; k++)
        {

            EnemyScript objectWithEnemyScript = objectsWithEnemyScript[k];
            string enemyKey = "Enemy" + k.ToString();
            PlayerPrefs.SetFloat(enemyKey + "PosX", objectWithEnemyScript.transform.position.x);
            PlayerPrefs.SetFloat(enemyKey + "PosY", objectWithEnemyScript.transform.position.y);

        }

        //spara antal rundor
        PlayerPrefs.SetInt("CurrentRound", RoundManager.Instance.RoundNumber);
       

    }

    
    public static void LoadGame()
    {
        //ladda hälsan för basen
        Island.Instance.health = PlayerPrefs.GetInt("BaseHealth");

        //ladda position av varje turret i "ormen"
        SnakeScript snakeScript = FindObjectOfType<SnakeScript>();
        for(int i = 0; i < snakeScript.trailList.Count; i++)
        {
            GameObject turret = snakeScript.trailList[i];
            string turretNyckel = "Turret" + i.ToString();
            float posX = PlayerPrefs.GetFloat(turretNyckel + "PosX");
            float posY = PlayerPrefs.GetFloat(turretNyckel + "PosY");
            turret.transform.position = new Vector2(posX, posY);
        }

        //ladda position av vajre släppt turret
        for(int j = 0; j < snakeScript.releasedBoats.Count; j++)
        {
            string releasedTurretNyckel = "ReleasedTurret" + j.ToString();
            float posX = PlayerPrefs.GetFloat(releasedTurretNyckel + "PosX");
            float posY = PlayerPrefs.GetFloat(releasedTurretNyckel + "PosY");
            GameObject releasedTurret = Instantiate(Resources.Load<GameObject>("TurretPrefab"), new Vector2(posX, posY), Quaternion.identity);
        }

        //ladda main båtens position
        CarButABoat carButABoat = FindObjectOfType<CarButABoat>();
        float carButABoatPosX = PlayerPrefs.GetFloat("CarButABoatPosX");
        float carButABoatPosY = PlayerPrefs.GetFloat("CarButABoatPosY");
        carButABoat.transform.position = new Vector2(carButABoatPosX, carButABoatPosY);

        //ladda positionen av enemies i scenen
        EnemyScript[] objectsWithEnemyScripts = FindObjectsOfType<EnemyScript>();
        for(int k = 0; k < objectsWithEnemyScripts.Length; k++)
        {
            EnemyScript enemyScript = objectsWithEnemyScripts[k];
            string enemyKey = "Enemy" + k.ToString();
            float posX = PlayerPrefs.GetFloat(enemyKey + "PosX");
            float posY = PlayerPrefs.GetFloat(enemyKey + "PosY");
            enemyScript.transform.position = new Vector2(posX, posY);
        }

        //ladda antal rundor
        RoundManager.Instance.RoundNumber = PlayerPrefs.GetInt("CurrentRound");
    }
}
