using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAction : BaseAction
{
    float waitTimer = 1f;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        waitTimer -= Time.deltaTime;
        if (waitTimer < 0f)
        {
            waitTimer = 1f;
            ActionComplete();
        }
    }

    public override string GetActionName()
    {
        return "Wait";
    }

    public override int GetUIPriority()
    {
        return -1;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        unit.IncreaseHeldActions();
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition> { unitGridPosition };
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 1, };
    }
}
