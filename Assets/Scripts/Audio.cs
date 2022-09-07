using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour

{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip landingSound;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            soundSource.PlayOneShot(landingSound, 0.5f);
        }
    }
}
