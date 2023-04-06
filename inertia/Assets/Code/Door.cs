using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;
    public List<int> requiredKeys;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            if (requiredKeys != null)
            {//case where we have keyIDs that are required
                var keycheck_successes = 0;
                foreach (var key in requiredKeys)
                {
                    if (PlayerInventory.instance.CheckKey(key))
                    {
                        keycheck_successes += 1;
                    }
                }

                if (keycheck_successes == requiredKeys.Count)
                {
                    _animator.SetTrigger("Open");
                }
            }
            else
            {
                _animator.SetTrigger("Open");
            }
        }
    }
}
