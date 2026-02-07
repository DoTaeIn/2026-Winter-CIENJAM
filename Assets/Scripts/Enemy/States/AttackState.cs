using UnityEngine;

public class AttackState : IState
{
    private Enemy enemy;
    private float timer;
    private bool hasAttacked;

    public AttackState(Enemy _enemy) : base(_enemy) { }

    public override void Enter()
    {
        Debug.Log("상태 변경: ATTACK");
        enemy.SetVelocity(0);
        timer = 0;
        hasAttacked = false;

        // 공격 애니메이션 시작
        // enemy.animator.SetTrigger("Attack"); 
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= enemy.attackWindup && !hasAttacked)
        {
            enemy.PerformAttack();
            hasAttacked = true;
        }
        
        if (timer >= enemy.attackDuration)
        {
            enemy.lastAttackTime = Time.time;
            enemy.ChangeState(enemy.chaseState);
        }
    }

    public override void Exit()
    {
        // 필요하다면 공격 애니메이션 종료 처리 등
    }
}
