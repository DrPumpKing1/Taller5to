using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FadeBlockingObjects : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private List<Transform> targets;
    [SerializeField] private Camera _camera;

    [Header("Fade Object Settings")]
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField, Range(1,15)] private int maxBlockingObjectsPerTarget = 10;
    [SerializeField] private bool retainShadows = true;
    [SerializeField] private Vector3 targetPositionOffset = Vector3.zero;

    [Header("Read Only Data")]
    [SerializeField] private List<FadingObject> objectsBlockingView = new List<FadingObject>();

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    private Dictionary<FadingObject, Coroutine> runningCoroutines = new Dictionary<FadingObject, Coroutine>();

    private List<RaycastHit> hits = new List<RaycastHit>();

    private void Start()
    {
        StartCoroutine(CheckForObjects());
    }

    private IEnumerator CheckForObjects()
    {
        while (true)
        {
            foreach(Transform target in targets)
            {
                RaycastHit[] targetHits = new RaycastHit[maxBlockingObjectsPerTarget];

                Vector3 rayStartingPos = GetRayStartingPos(target.position + targetPositionOffset);
                float rayLenght = Vector3.Distance(rayStartingPos, target.position + targetPositionOffset);

                Physics.RaycastNonAlloc(rayStartingPos, CameraLookDirection(), targetHits, rayLenght, blockingLayer);

                if(drawRaycasts) Debug.DrawRay(rayStartingPos, CameraLookDirection() * (rayLenght), Color.green); 

                AddArrayToHitList(targetHits);
            }

            if (hits.Count > 0)
            {
                for (int i = 0; i < hits.Count; i++)
                {
                    FadingObject fadingObject = GetFadingObjectFromHit(hits[i]);

                    if (fadingObject != null && !objectsBlockingView.Contains(fadingObject))
                    {
                        if (runningCoroutines.ContainsKey(fadingObject))
                        {
                            if (runningCoroutines[fadingObject] != null)
                            {
                                StopCoroutine(runningCoroutines[fadingObject]);
                            }

                            runningCoroutines.Remove(fadingObject);
                        }

                        runningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                        objectsBlockingView.Add(fadingObject);
                    }
                }
            }

            FadeObjectsNoLongerBeingHit();

            ClearHits();

            yield return null;
        }
    }

    private void FadeObjectsNoLongerBeingHit()
    {
        List<FadingObject> objectsToRemove = new List<FadingObject>(objectsBlockingView.Count);

        foreach (FadingObject fadingObject in objectsBlockingView)
        {
            bool objectIsBeingHit = false;
            for (int i = 0; i < hits.Count; i++)
            {
                FadingObject hitFadingObject = GetFadingObjectFromHit(hits[i]);
                if (hitFadingObject != null && fadingObject == hitFadingObject)
                {
                    objectIsBeingHit = true;
                    break;
                }
            }

            if (!objectIsBeingHit)
            {
                if (runningCoroutines.ContainsKey(fadingObject))
                {
                    if (runningCoroutines[fadingObject] != null)
                    {
                        StopCoroutine(runningCoroutines[fadingObject]);
                    }
                    runningCoroutines.Remove(fadingObject);
                }

                runningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectIn(fadingObject)));
                objectsToRemove.Add(fadingObject);
            }
        }

        foreach(FadingObject removeObject in objectsToRemove)
        {
            objectsBlockingView.Remove(removeObject);
        }
    }

    private IEnumerator FadeObjectOut(FadingObject fadingObject)
    {
        foreach (Material material in fadingObject.materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_Surface", 1);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", retainShadows);

            material.SetOverrideTag("RenderType", "Transparent");

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        float time = 0;
        float startingAlpha = fadingObject.materials[0].color.a;

        while (fadingObject.materials[0].color.a > fadingObject.FadeAlpha)
        {
            foreach (Material material in fadingObject.materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        Mathf.Lerp(startingAlpha, fadingObject.FadeAlpha, time * 1/fadingObject.FadeTime)
                    );
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        if (runningCoroutines.ContainsKey(fadingObject))
        {
            StopCoroutine(runningCoroutines[fadingObject]);
            runningCoroutines.Remove(fadingObject);
        }
    }

    private IEnumerator FadeObjectIn(FadingObject fadingObject)
    {
        float time = 0;
        float startingAlpha = fadingObject.materials[0].color.a;

        while (fadingObject.materials[0].color.a < fadingObject.InitialAlpha)
        {
            foreach (Material material in fadingObject.materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        Mathf.Lerp(startingAlpha, fadingObject.InitialAlpha, time * 1 / fadingObject.FadeTime)
                    );
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        foreach (Material material in fadingObject.materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.SetInt("_Surface", 0);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            material.SetShaderPassEnabled("DepthOnly", true);
            material.SetShaderPassEnabled("SHADOWCASTER", true);

            material.SetOverrideTag("RenderType", "Opaque");

            material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        if (runningCoroutines.ContainsKey(fadingObject))
        {
            StopCoroutine(runningCoroutines[fadingObject]);
            runningCoroutines.Remove(fadingObject);
        }
    }

    private void ClearHits() => hits.Clear();

    private FadingObject GetFadingObjectFromHit(RaycastHit Hit) => Hit.collider != null ? Hit.collider.GetComponent<FadingObject>() : null;

    private Vector3 CameraLookDirection() => _camera.transform.forward;

    private Vector3 GetRayStartingPos(Vector3 targetPosition)
    {
        Plane cameraPlane = new Plane(CameraLookDirection(), _camera.transform.position);
        float distanceToPlane = cameraPlane.GetDistanceToPoint(targetPosition);

        Vector3 rayStartingPos = targetPosition - CameraLookDirection() * distanceToPlane;

        return rayStartingPos;
    }

    private void AddArrayToHitList(RaycastHit[] array)
    {
        foreach (RaycastHit hit in array)
        {
            if (hit.collider == null) continue;
            if (IsRayCastHitInList(hit, hits)) continue;
            
            hits.Add(hit);           
        }
    }

    private bool IsRayCastHitInList(RaycastHit element, List<RaycastHit> list)
    {
        foreach (RaycastHit item in list)
        {
            if (item.collider == element.collider)
            {
                return true;
            }
        }
        return false;
    }
}
