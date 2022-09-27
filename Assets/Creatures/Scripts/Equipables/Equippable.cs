using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creatures.Equippables
{
    [Serializable]
    public class Equippable
    {
        [SerializeField] public Transform armatureRoot;
        [SerializeField] public Transform animationRoot;
        [SerializeField] public Animator animator;

        protected GameObject Equip(GameObject prefab)
        {
            if (prefab == null)
            {
                return null;
            }

            var equippable = GameObject.Instantiate(prefab);
            equippable.transform.SetParent(animationRoot);

            if (equippable != null && equippable.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinnedMeshRenderer))
            {
                Transform[] bones = skinnedMeshRenderer.bones;
                skinnedMeshRenderer.rootBone = armatureRoot;

                Transform[] children = armatureRoot.GetComponentsInChildren<Transform>();
                for (int i = 0; i < bones.Length; i++)
                {
                    for (int a = 0; a < children.Length; a++)
                    {
                        if (bones[i].name == children[a].name)
                        {
                            bones[i] = children[a];
                            break;
                        }
                    }
                }

                skinnedMeshRenderer.bones = bones;
            }

            animator.Rebind();
            return equippable;
        }
    }
}
