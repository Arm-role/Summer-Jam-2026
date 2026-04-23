using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemenager : MonoBehaviour
{
    public GameObject Setting;
    void Start()
    {
        if (Setting != null)
        {
            Setting.SetActive(false);
        }
    }

  
}
