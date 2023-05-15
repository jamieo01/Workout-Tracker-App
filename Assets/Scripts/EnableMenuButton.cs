using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMenuButton : MonoBehaviour
{
    public void EnableMenu(GameObject menuObject) 
    {
        menuObject.SetActive(!menuObject.activeSelf);
    }
}
