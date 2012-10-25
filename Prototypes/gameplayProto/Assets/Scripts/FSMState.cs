using UnityEngine;

abstract public class FSMState<T> : ScriptableObject
{
	abstract public void Enter( T entity );

	public virtual void Execute( T entity )
	{
	}

	public virtual void ExecuteFixed( T entity )
	{
	}

	public virtual void ExecuteLate( T entity )
	{
	}

	abstract public void Exit( T entity );
}