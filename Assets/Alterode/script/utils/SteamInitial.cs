using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamInitial : MonoBehaviour
{
    void Start()
    {
		#if UNITY_STANDALONE

        try 
		{
		    SteamClient.Init( 2713650 );
		}
		catch ( System.Exception e )
		{
			Debug.Log(e);
		    // Couldn't init for some reason (steam is closed etc)
		}

		#endif
    }
}
