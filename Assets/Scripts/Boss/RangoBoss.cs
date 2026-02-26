using UnityEngine;

public class RangoBoss : MonoBehaviour
{
    public Animator animator;
    public GameObject boss;
    public int melee = 0;

    private void OnTriggerEnter(Collider other)
    {
        int hitSelect = 0;
        if (other.CompareTag("Player"))
        {
            switch(melee)
            {
                case 0:
                    //golpe 1
                    animator.SetFloat("skills", 0);
                    hitSelect = 0;
                    break;

                //case 1:
                //    //golpe 2
                //    animator.SetFloat("skills", 0);
                //    hitSelect = 1;
                //    break;
                //case 2:
                //    //golpe 3
                //    animator.SetFloat("skills", 0);
                //    hitSelect = 2;
                //    break;
                //case 3:
                //    //fireball
                //    if(boss.fase == 2)
                //    {
                //        animator.SetFloat("skills", 0);
                //    } else
                //    {
                //        melee = 0;
                //    }
                //    break;
            }
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("attack", true);
            if(boss.GetComponent<BossPaquirrin>() != null)
            {
                boss.GetComponent<BossPaquirrin>().atacando = true;
                boss.GetComponent<BossPaquirrin>().hit_select = hitSelect;
            } else if( boss.GetComponent<BossFary>() != null)
            {
                boss.GetComponent<BossFary>().atacando = true;
                boss.GetComponent<BossFary>().hit_select = hitSelect;
            } else if (boss.GetComponent<BossLobo>() != null)
            {
                boss.GetComponent<BossLobo>().atacando = true;
                boss.GetComponent<BossLobo>().hit_select = hitSelect;
            }

                GetComponent<CapsuleCollider>().enabled = false;

        }
    }
}
