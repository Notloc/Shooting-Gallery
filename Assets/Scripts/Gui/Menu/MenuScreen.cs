using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuScreen : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] protected MenuController menuController;
    [SerializeField] protected MenuScreen previousScreen;

    protected void ReturnToPrevious()
    {
        this.gameObject.SetActive(false);
        if (previousScreen)
            previousScreen.gameObject.SetActive(true);
    }

}
