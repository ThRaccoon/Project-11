using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState
{
    protected Player _player = null;
    protected PlayerStateManager _stateMachine = null;
    protected bool _didPhysicsUpdateRan = false;

    // --- Inputs ---
    protected Vector2 _movementInput = Vector2.zero;
    protected bool _runInput = false;
    protected bool _crouchInput = false;

    // --- Movement ---
    protected Vector3 _movementVector = Vector3.zero;
    protected Vector3 _ladderJumpForceDirection = Vector3.zero;


    public PlayerBaseState(Player player, PlayerStateManager stateMachine, bool didPhysicsUpdateRan)
    {
        _player = player;
        _stateMachine = stateMachine;
        _didPhysicsUpdateRan = didPhysicsUpdateRan;
    }

    public virtual void OnEnter() 
    {
        _didPhysicsUpdateRan = false;
    }

    public virtual void LogicUpdate()
    {
        _movementInput = _player.PlayerInput.MovementInput;
        _crouchInput = _player.PlayerInput.CrouchInput;
    }

    public virtual void PhysicsUpdate() { }

    public virtual void OnExit() { }
}

