using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickBoat : MonoBehaviour
{
    public enum BoatType
    {
        Boat1,  // snabbare färre turrets 
        Boat2,  // seg stor många turrets
        Boat3  // seg rotation snabb
    }
       
    public BoatType boatType;

    private CarButABoat carButABoat;
    private SnakeScript snakeScript;
    [SerializeField] Sprite boatSprite1;
    [SerializeField] Sprite boatSprite2;
    [SerializeField] Sprite boatSprite3;
    [SerializeField] GameObject player;

    private void Awake()
    {
        carButABoat = player.GetComponent<CarButABoat>();
        snakeScript = player.GetComponent<SnakeScript>();
    }

    private void Start()
    {
        //SetBoatType(boatType);
    }

    public void SetBoat1()
    {
        SetBoatType(BoatType.Boat1);
    }
    public void SetBoat2()
    {
        SetBoatType(BoatType.Boat2);
    }

    public void SetBoat3()
    {
        SetBoatType(BoatType.Boat3);
       
    }

    public void SetBoatType(BoatType type)
    {

        GetComponent<CanvasScript>().ToggleCharacterSelect();
        boatType = type;
        Debug.Log("SWAP");
        switch (type)
        {
            case BoatType.Boat1:
                Debug.Log("BOAT1");
                player.GetComponent<SpriteRenderer>().sprite = boatSprite1;
                carButABoat.SetAcceleration(25);
                carButABoat.SetSteering(5);
                carButABoat.SetMaxVelocity(20);
                carButABoat.SetDriftThreshold(35);
                snakeScript.SetMaxBoats(6);
                break;

            case BoatType.Boat2:
                player.GetComponent<SpriteRenderer>().sprite = boatSprite2;
                carButABoat.SetAcceleration(18);
                carButABoat.SetSteering(5);
                carButABoat.SetMaxVelocity(15);
                carButABoat.SetDriftThreshold(35);
                snakeScript.SetMaxBoats(12);
                break;

            case BoatType.Boat3:
                player.GetComponent<SpriteRenderer>().sprite = boatSprite3;
                carButABoat.SetAcceleration(30);
                carButABoat.SetSteering(2);
                carButABoat.SetMaxVelocity(30);
                carButABoat.SetDriftThreshold(35);
                snakeScript.SetMaxBoats(8);
                break;
        }
    }

}
