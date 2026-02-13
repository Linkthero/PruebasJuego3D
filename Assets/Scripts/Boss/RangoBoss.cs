using UnityEngine;

public class RangoBoss : MonoBehaviour
{
    public Animator animator;
    public BossPaquirrin boss;
    public int melee;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            melee = Random.Range(0, 4);
            switch(melee)
            {
                case 0:
                    //golpe 1
                    animator.SetFloat("skills", 0);
                    boss.hit_select = 0;
                    break;

                case 1:
                    //golpe 2
                    animator.SetFloat("skills", 0);
                    boss.hit_select = 1;
                    break;
                case 2:
                    //golpe 3
                    animator.SetFloat("skills", 0);
                    boss.hit_select = 2;
                    break;
                case 3:
                    //fireball
                    if(boss.fase == 2)
                    {
                        animator.SetFloat("skills", 0);
                    } else
                    {
                        melee = 0;
                    }
                    break;
            }
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("attack", true);
            boss.atacando = true;
            GetComponent<CapsuleCollider>().enabled = false;

            /////////////////////////////////
            /////////////////////////////////////
            ///
            //TE HAS QUEDAO POR EL MIN 8.57 DEL VIDEO
            ////////////////////////////////////
            ///
            /////////////////////////////////////
        }
    }
}
