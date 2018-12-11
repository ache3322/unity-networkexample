using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    void Awake()
    {
        // Don't destroy the GameObject when loading
        // another scene or place...
        DontDestroyOnLoad(gameObject);
    }
}
