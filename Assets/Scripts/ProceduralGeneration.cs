using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] GameObject dirt, grass;


    // Start is called before the first frame update
    void Start()
    {
        Generation();
    }

    // Update is called once per frame
    void Generation()
    {
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Instantiate(dirt, new Vector2(x, y), Quaternion.identity);
            }
            Instantiate(grass, new Vector2(x, height), Quaternion.identity);
        }
    }
}
