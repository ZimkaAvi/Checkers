using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerNetwork : Player
{
    [SyncVar] string displayName;

    public string DisplayName
    {
        get { return displayName; }

        [Server]
        set { displayName = value; }
    }
}
