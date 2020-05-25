using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSwap : MonoBehaviour
{
    Animator animator;
    Transform pivotTransform;
    Transform faceTransform;

    private void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>(true);
        pivotTransform = transform;
        faceTransform = transform.GetChild(0).transform;
    }

    public void SwapFace(FaceSwap otherFace)
    {
        gameObject.transform.localPosition = otherFace.pivotTransform.localPosition;
        gameObject.transform.localRotation = otherFace.pivotTransform.localRotation;
        gameObject.transform.localScale = otherFace.pivotTransform.localScale;

        GameObject face = transform.GetChild(0).gameObject;
        face.transform.localPosition = otherFace.faceTransform.localPosition;
        face.transform.localRotation = otherFace.faceTransform.localRotation;
        face.transform.localScale = otherFace.faceTransform.localScale;
        animator.enabled = true;
    }
}
