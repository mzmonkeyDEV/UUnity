using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillState : IEnemyState
{
    private readonly EnemyAI _ai;
    private readonly EnemyMovementService _movement;
    private readonly EnemyBehaviorProfile _profile;

    private EnemySkill _currentSkill;
    private ISkillExecutor _currentExecutor;
    private float _castStartTime;
    private bool _isCasting;
    private bool _hasExecuted;

    public SkillState(EnemyAI ai, EnemyMovementService movement, EnemyBehaviorProfile profile)
    {
        _ai = ai;
        _movement = movement;
        _profile = profile;
    }

    public void Enter()
    {
        _currentSkill = SelectRandomSkill();
        if (_currentSkill == null)
        {
            // No skills available, switch to different state
            _ai.ChangeState(_ai.ChaseState);
            return;
        }

        _currentExecutor = _ai.GetSkillExecutor(_currentSkill.skillName);
        _castStartTime = Time.time;
        _isCasting = true;
        _hasExecuted = false;

        _movement.Stop();
        if (_profile.shouldFacePlayer && _ai.Player != null)
        {
            _movement.LookAt(_ai.Player);
        }

        _currentExecutor?.OnCastStart(_ai.transform);
    }

    public void Execute()
    {
        if (_isCasting && Time.time >= _castStartTime + _currentSkill.castTime && !_hasExecuted)
        {
            _currentExecutor?.Execute(_ai.transform, _ai.Player);
            _currentSkill.MarkUsed();
            _hasExecuted = true;
            _isCasting = false;
            _currentExecutor?.OnCastEnd(_ai.transform);
        }
    }

    public void Exit() { }

    public bool IsComplete()
    {
        return _hasExecuted;
    }

    private EnemySkill SelectRandomSkill()
    {
        var availableSkills = _profile.availableSkills.Where(s => s.CanUse()).ToList();
        if (availableSkills.Count == 0) return null;

        // Weighted random selection based on priority
        int totalPriority = availableSkills.Sum(s => s.priority);
        int randomValue = Random.Range(0, totalPriority);

        int currentSum = 0;
        foreach (var skill in availableSkills)
        {
            currentSum += skill.priority;
            if (randomValue < currentSum)
            {
                return skill;
            }
        }

        return availableSkills[0];
    }
}