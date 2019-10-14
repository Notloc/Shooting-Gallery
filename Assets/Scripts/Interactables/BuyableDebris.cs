using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableDebris : MonoBehaviour, IInteractable, IHaveInfo
{
    [SerializeField] float cost;

    public string GetInfoText()
    {
        return "Clear for $" + cost.ToString("#");
    }

    public void Interact(Player player)
    {
        if (player.SpendMoney(cost))
            StartCoroutine(RemoveDebris());
    }

    private IEnumerator RemoveDebris()
    {
        float counter = 1f;
        float original = this.transform.localScale.y;

        while (counter > 0)
        {
            var scale = this.transform.localScale;
            scale.y = original * counter;
            this.transform.localScale = scale;

            counter -= Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
