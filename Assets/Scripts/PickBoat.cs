using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickBoat : MonoBehaviour
{
    public enum BoatType
    {
        Boat1, // seg stor många turrets
        Boat2, // snabbare färre turrets
        Boat3  // seg rotation snabb
    }

    public BoatType boatType;

    private CarButABoat carButABoat;
    private SnakeScript snakeScript;
    [SerializeField] Sprite boatSprite1;
    [SerializeField] Sprite boatSprite2;
    [SerializeField] Sprite boatSprite3;
    [SerializeField] SpriteRenderer playerSprite;

    private void Awake()
    {
        carButABoat = GetComponent<CarButABoat>();
        snakeScript = GetComponent<SnakeScript>();
    }

    private void Start()
    {
        SetBoatType(boatType);
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
        boatType = type;

        switch (type)
        {
            case BoatType.Boat1:
                playerSprite.sprite = boatSprite1;
                carButABoat.SetAcceleration(2);
                carButABoat.SetSteering(2);
                carButABoat.SetMaxVelocity(5);
                carButABoat.SetDriftThreshold(3);
                snakeScript.SetMaxBoats(6);
                break;

            case BoatType.Boat2:
                playerSprite.sprite = boatSprite2;
                carButABoat.SetAcceleration(5);
                carButABoat.SetSteering(5);
                carButABoat.SetMaxVelocity(7);
                carButABoat.SetDriftThreshold(4);
                snakeScript.SetMaxBoats(4);
                break;

            case BoatType.Boat3:
                playerSprite.sprite = boatSprite2;
                carButABoat.SetAcceleration(4);
                carButABoat.SetSteering(8);
                carButABoat.SetMaxVelocity(7);
                carButABoat.SetDriftThreshold(4);
                snakeScript.SetMaxBoats(5);
                break;
        }
    }

}
