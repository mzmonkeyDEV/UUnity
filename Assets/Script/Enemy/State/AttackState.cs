
using UnityEngine;
using System.Collections;

public class AttackState : IEnemyState
{
    private readonly EnemyAI _ai;
    private readonly EnemyMovementService _movement;
    private readonly EnemyCombatService _combat;
    private readonly EnemyDetectionService _detection;
    private readonly EnemyConfig _config;

    public AttackState(EnemyAI ai, EnemyMovementService movement, EnemyCombatService combat, EnemyDetectionService detection, EnemyConfig config)
    {
        _ai = ai;
        _movement = movement;
        _combat = combat;
        _detection = detection;
        _config = config;

    }

    public void Enter() { }

    public void Execute()
    {
        if (_ai.Player == null) return;
        if (!_detection.HasLineOfSight(_ai.Player))
        {
            _ai.ChangeState(_ai.ChaseState);
            return;
        }

        _movement.Stop();
        _movement.LookAt(_ai.Player);

        if (_config.enemyType == EnemyConfig.EnemyType.Minion)
        {
            // MINION USES ONLY MELEE
           _combat.MeleeAttack();
            return;
        }

        //BOSS SKILLS
        if (_combat.CanUseSkill())
        {
            int randomSkill = Random.Range(0, 4);

            switch (randomSkill)
            {
                case 0: _combat.SkillBigBone_Start(); break;
                case 1: _combat.SkillRapidFire_Start(); break;
                case 2: _combat.SkillAOE_Start(); break;
                case 3: _combat.SkillSummon_Start(); break;
            }

        }
    }


    public void Exit() { }
}