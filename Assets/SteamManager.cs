using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Unity.VisualScripting;

public class SteamManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            SteamClient.Init(480, true);
        }
        catch (System.Exception e)
        {

        }

        if (SteamClient.IsValid)
        {
            Debug.Log(SteamClient.Name + " : " + SteamClient.SteamId);
        }
    }

    void OnDisable()
    {
        SteamClient.Shutdown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
