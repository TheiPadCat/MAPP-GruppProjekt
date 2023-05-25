using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxLifeTime;
    private float currentLifeTime;
    private bool lifeTimeActive;
    public GameObject particles;

    public Slider hpSlider;
    void Start()
    {
        // hpSlider = transform.parent.GetComponentInChildren<Slider>();
        hpSlider.maxValue = maxLifeTime;
        hpSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTimeActive)
        {
            currentLifeTime -= Time.deltaTime;
            updateHpSlider();
            if (currentLifeTime <= 0f)
            {
                if (GetComponent<BombScript>() != null)
                {
                    GetComponent<BombScript>().Explode();
                    return;
                }
                particles.transform.parent = null;
                particles.GetOrAddComponent<ParticleSystem>().Play();
                GameObject.Find("AudioMan").GetComponent<AudioScript>().Bomb(transform.position);
                Destroy(transform.root.gameObject);
            }

        }
    }


    public void ToggleLifeTime(bool toggle)
    {
        if (toggle == true)
        {
            currentLifeTime = maxLifeTime;
            lifeTimeActive = true;
            hpSlider.gameObject.SetActive(true);
        }
        else
        {
            lifeTimeActive = false;
            hpSlider.gameObject.SetActive(false);
        }
    }
    public void updateHpSlider()
    {
        hpSlider.value = currentLifeTime;
       // hpSlider.transform.position = transform.position + 1 * Vector3.up;
       // hpSlider.gameObject.transform.right = Vector3.right;

    }
}
