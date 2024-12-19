// Credit to YuraSuper2048
using System;
using UnityEngine;

[ExecuteInEditMode]
public class PickupAnimationScript : MonoBehaviour
{
    // Change this to control frequency of animation
    // Less = more optimized, but will increase animation delays
    // More = less optimized, will reduce animation delays
    // (This actually is distance to player where update will take 1 second)
    private const float UpdateFrequency =
        400;
    
    // Time before last update
    private float time;

    // Time to wait before billboarding
    private float waitTime;
    
    // Amplitude of animation
    [SerializeField]
    private float amplitude = 0.5f;
    
    // Center Y
    [SerializeField]
    private float centerY = 1f;
    
    // Needed to reduce the built-in unity calls
    private new Transform transform;

    private void Start()
    {
        // Getting transform
        transform = base.transform;
    }

    private void OnWillRenderObject()
    {
        // Getting current camera for further use
        var cameraCurrent = Camera.current;
        
        // Counting time
        time += Time.deltaTime;
        
        // Waiting some time based on distance
        if (time < waitTime) return;
        time = 0;

        try
        {
            // If distance is too high, return
            var distance = Vector3.Distance(transform.position, cameraCurrent.transform.position);
            if (distance > 125)
            {
                return;
            }
            
            // Calcualting time to wait
            waitTime = distance / UpdateFrequency;
        }
        catch
        {
            // Ignore all errors
        }

        // Animate
        transform.localPosition = Vector3.up *
                                      (MathF.Sin(Time.time * 2) * amplitude + centerY);
    }
}
