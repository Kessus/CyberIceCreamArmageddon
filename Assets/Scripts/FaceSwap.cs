using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Allows to swap an enemy's face for player's goggles
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

        GameObject face = transform.GetChild(0).gameObject; //Getting the actual face sprite GameObject from the pivot GameObject
        face.transform.localPosition = otherFace.faceTransform.localPosition;
        face.transform.localRotation = otherFace.faceTransform.localRotation;
        face.transform.localScale = otherFace.faceTransform.localScale;
        animator.enabled = true;
    }
}
