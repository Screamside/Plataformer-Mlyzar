using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBaseState
{
    public abstract void EnterState(CharacterManager characterManager);

    public abstract void UpdateState(CharacterManager characterManager);



}
