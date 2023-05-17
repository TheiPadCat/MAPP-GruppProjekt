using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    private bool justStarted;
    private void Awake()
    {
        carButABoat = player.GetComponent<CarButABoat>();
        snakeScript = player.GetComponent<SnakeScript>();
    }

    private void Start()
    {
       SetBoatType(BoatType.Boat1);
        justStarted = true;
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
       if(justStarted)
        {
            GetComponent<CanvasScript>().ToggleCharacterSelect();
        }
      
        boatType = type;
        
        switch (type)
        {
            case BoatType.Boat1:
             
                player.GetComponent<SpriteRenderer>().sprite = boatSprite1;
                carButABoat.SetAcceleration(25);
                carButABoat.SetSteering(5);
                carButABoat.SetMaxVelocity(25);
                carButABoat.SetDriftThreshold(35);
                snakeScript.SetMaxBoats(3);
                carButABoat.SetDmg(1);
                player.transform.localScale = Vector3.one;
                break;

            case BoatType.Boat2:
                player.GetComponent<SpriteRenderer>().sprite = boatSprite2;
                carButABoat.SetAcceleration(18);
                carButABoat.SetSteering(5);
                carButABoat.SetMaxVelocity(15);
                carButABoat.SetDriftThreshold(35);
                snakeScript.SetMaxBoats(12);
                carButABoat.SetDmg(3);
                player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;

            case BoatType.Boat3:
                player.GetComponent<SpriteRenderer>().sprite = boatSprite3;
                carButABoat.SetAcceleration(30);
                carButABoat.SetSteering(2);
                carButABoat.SetMaxVelocity(30);
                carButABoat.SetDriftThreshold(35);
                snakeScript.SetMaxBoats(8);
                carButABoat.SetDmg(6);
                player.transform.localScale = Vector3.one;
                break;
        }
    }

}
