using UnityEngine;

public class JumpAnimatorBehaviour : StateMachineBehaviour
{
    public string boolToSetFalse = "Jump";
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolToSetFalse, false);
    }
}
