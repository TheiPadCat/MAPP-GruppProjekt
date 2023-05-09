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
    }
}
