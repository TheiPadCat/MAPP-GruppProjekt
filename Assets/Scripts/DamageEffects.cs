using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffects : MonoBehaviour
{

    [SerializeField] private Color flashColor;
    [SerializeField] private float lerpSpeed = 2f;
 

    private Material material;
    private Coroutine flashCoroutine;

    
   

    private void Start()
    {
       
        material = GetComponentInChildren<SpriteRenderer>().material;
    }




    public void PlayFlash()
    {
        
        if(flashCoroutine != null && material != null)
        {
           
        }
        if(material != null)
        {
            flashCoroutine = StartCoroutine(Flashing());
        }
        




        //FICK EN NULLREF EXCEPTION HÄR NÅGON GÅNG FIXA SÅ DET INTE SKER








    }
    


    private IEnumerator Flashing()
    {
        
        material.SetColor("_Color", flashColor);

        float lerp = 0f;

        
        float intensity = 1;

        while (lerp < 1)
        {
            lerp += Time.deltaTime * lerpSpeed;
            intensity = Mathf.Lerp(1, 0, lerp);



           if(material != null)
            {
                material.SetFloat("_Intensity", intensity);
            }
           

            //Debug.Log(intensity);
            yield return null;
        }
        
      
       
    }






}
