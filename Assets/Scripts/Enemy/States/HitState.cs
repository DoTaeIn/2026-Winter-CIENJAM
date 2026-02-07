using UnityEngine;

public class HitState : IState
{
    private Enemy enemy;
    private float timer;
    private float stunDuration = 0.5f;

    public HitState(Enemy en) : base(en)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        enemy.SetVelocity(0); 
        timer = 0;
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= stunDuration)
        {
            // 플레이어가 아직 사거리 내에 있으면 공격, 아니면 추격 등
            // 보통은 그냥 추격(Chase)으로 보내면 알아서 판단함
            enemy.ChangeState(enemy.chaseState);
        }
    }

    public override void Exit(){}
}
