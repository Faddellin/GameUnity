using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrailController : MonoBehaviour
{
    private Animator _animator;
    public void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void stopAttack2Right_Trail()
    {
        _animator.SetBool("Attack2Right_Trail", false);
    }
    public void stopAttack2Left_Trail()
    {
        _animator.SetBool("Attack2Left_Trail", false);
    }

    public void stopAttack1Right_Trail()
    {
        _animator.SetBool("Attack1Right_Trail", false);
    }

    public void stopAttack1Left_Trail()
    {
        _animator.SetBool("Attack1Left_Trail", false);
    }
}
