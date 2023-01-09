using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ImageUpdate : MonoBehaviour
{
    
    public RawImage mapImageMult;
    public Texture[] ListMapImage;
    
    [PunRPC]
    public void UpdateMultMapImage(int map)
    {
        mapImageMult.texture = ListMapImage[map];
    }
}
