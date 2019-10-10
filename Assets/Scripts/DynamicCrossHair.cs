using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class DynamicCrossHair : MonoBehaviour
{
    [SerializeField] RectTransform crosshair;
    [SerializeField] GunManager gunManager;

    private List<RectTransform> crosshairs;
    private void Awake()
    {
        crosshairs = new List<RectTransform>();
        crosshairs.Add(crosshair);
    }

    private void Update()
    {
        UpdateCrosshair();
    }


    private void UpdateCrosshair()
    {
        EnsureCrosshairs();

        int i = 0;
        foreach (var gun in gunManager.ActiveGuns)
        {
            Vector3 aimPos = gun.GetAim();


            Vector3 screenPos = Camera.main.WorldToViewportPoint(aimPos);

            screenPos -= (Vector3.one / 2f);
            Vector2 size = (this.transform as RectTransform).sizeDelta;
            Vector3 size3 = new Vector3(size.x, size.y, 0f);

            crosshairs[i].localPosition = Vector3.Scale(screenPos, size3);
            i++;
        }
    }

    private void EnsureCrosshairs()
    {
        int needed = gunManager.ActiveGuns.Count;
        for (int i = crosshairs.Count; i < needed; i++)
            crosshairs.Add(Instantiate(crosshair, crosshair.transform.parent));
    }
    
}
