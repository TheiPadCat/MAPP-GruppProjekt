using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{

    public Shader test;
    //[SerializeField] Shader waterShader;
    // Start is called before the first frame update
    void Start()
    {
        Shader.WarmupAllShaders();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
