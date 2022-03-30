using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmatureReassign : MonoBehaviour
{
    [System.NonSerialized] public Transform newArmature;
    public string rootBoneName = "Stomach";
    [SerializeField] SkinnedMeshRenderer smr;
    [SerializeField] GameObject oldArmature;

    public void Reassign()
    {
        Transform[] bones = smr.bones;

        smr.rootBone = newArmature.Find(rootBoneName);

        Transform[] children = newArmature.GetComponentsInChildren<Transform>();

        for (int i = 0; i < bones.Length; i++)
            for (int a = 0; a < children.Length; a++)
                if (bones[i].name == children[a].name)
                {
                    bones[i] = children[a];
                    break;
                }

        smr.bones = bones;
        Destroy(oldArmature);
    }

}