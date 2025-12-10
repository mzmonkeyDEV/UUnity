using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillExecutor
{
    void Execute(Transform caster, Transform target);
    void OnCastStart(Transform caster);
    void OnCastEnd(Transform caster);
}
