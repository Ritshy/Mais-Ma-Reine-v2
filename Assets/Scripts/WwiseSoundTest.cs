using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseSoundTest : MonoBehaviour
{
    public AK.Wwise.Event testSound;
    // Start is called before the first frame update
    void Start()
    {
        testSound.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
