using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataLoader : MonoBehaviour
{
    void Awake()
    {
        UserData.Load();
    }
}
