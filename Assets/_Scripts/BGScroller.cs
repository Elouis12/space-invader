using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public float speed = -0.3f;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float offset = speed * Time.time;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset,0));
    }
}
