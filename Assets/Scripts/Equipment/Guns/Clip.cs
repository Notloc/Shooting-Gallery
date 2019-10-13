using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clip : MonoBehaviour
{
    [Header("Prefab Only References")]
    [SerializeField] new Rigidbody rigidbody;
    [Header("Required References")]
    [SerializeField] Clip clipPrefab;

    [Header("Options")]
    [SerializeField] float clipLifeTime = 40f;
    [SerializeField] Vector3 launchVector;

    public void DoClipEffect()
    {
        Clip newClip = Instantiate(clipPrefab);
        newClip.Initialize(this, launchVector, clipLifeTime);
    }
    
    private void Initialize(Clip clip, Vector3 launchVector, float lifeTime)
    {
        this.transform.position = clip.transform.position;
        this.transform.rotation = clip.transform.rotation;
        GameObject effectClipVisuals = Instantiate(clip.gameObject, clip.transform.position, clip.transform.rotation, this.transform);

        Launch(launchVector);
        Destroy(this.gameObject, lifeTime);
    }

    private void Launch(Vector3 launchVector)
    {
        this.rigidbody.AddRelativeForce(launchVector);
    }
}
