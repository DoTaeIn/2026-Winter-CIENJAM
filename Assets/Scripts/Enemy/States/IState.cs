using UnityEngine;

public abstract class IState
{
    protected Enemy _enemy;

    public IState(Enemy en)
    {
        this._enemy = en;
        
    }
    
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
