using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    //Instanced because there should only be one
    public static TurnSystem Instance { get; private set; }

    [SerializeField]
    private AudioClip turnButtonPressedSFX;

    public event EventHandler OnTurnChanged;
    public event Action OnNextUnitInitiative;
    public event EventHandler<Queue<Initiative>> OnNewInitiative;

    //Keeps track of what turn it is
    private int turnNumber = 1;
    private bool isPlayerTurn;

    //private Initiative initiativeUnit;
    private Queue<Initiative> initiativeOrder = new Queue<Initiative>();

    //Singleton-ed
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one TurnSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        EnemyAI.Instance.OnEnemyTurnFinished += EnemyAI_OnEnemyTurnFinished;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;

        GetNewInitiativeRound();
    }

    private void OnDisable()
    {
        EnemyAI.Instance.OnEnemyTurnFinished -= EnemyAI_OnEnemyTurnFinished;
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }

    public void NextInitiative()
    {
        if (initiativeOrder.TryDequeue(out Initiative initiative))
        {
            Initiative currentInitiative = initiative;
            isPlayerTurn = !currentInitiative.unit.IsEnemy();
            OnTurnChanged?.Invoke(this, EventArgs.Empty);
            currentInitiative.unit.SetMovementCompleted(false);
            currentInitiative.unit.SetActionCompleted(false);
            if (!isPlayerTurn)
            {
                EnemyAI.Instance.TakeEnemyTurn(currentInitiative.unit);
            }
            else
            {
                UnitActionSystem.Instance.SetSelectedUnit(currentInitiative.unit);
            }
            OnNextUnitInitiative?.Invoke();
        }
        else
        {
            NextTurn();
        }
    }

    //Advances turn, fires off OnTurnChanged event
    public void NextTurn()
    {
        turnNumber++;

        AudioSource.PlayClipAtPoint(
            turnButtonPressedSFX,
            Camera.main.transform.position,
            SoundManager.Instance.GetSoundEffectVolume()
        );
        GetNewInitiativeRound();
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    private void GetNewInitiativeRound()
    {
        //initiativeUnit = null;
        initiativeOrder.Clear();
        List<Unit> unitList = UnitManager.Instance.GetUnitList();
        List<Initiative> tempInitiativeList = new List<Initiative>();
        foreach (Unit unit in unitList)
        {
            Initiative newInitiative = new Initiative(unit, unit.GetUnitStats().GetInitiative());
            tempInitiativeList.Add(newInitiative);
        }
        tempInitiativeList.Sort(
            (Initiative a, Initiative b) => b.unitInitiative - a.unitInitiative
        );

        foreach (Initiative initiative in tempInitiativeList)
        {
            initiativeOrder.Enqueue(initiative);
        }

        OnNewInitiative?.Invoke(this, initiativeOrder);
        NextInitiative();
    }

    private void RemoveUnitFromInitiative(Unit deadUnit)
    {
        List<Initiative> tempInitiativeList = new List<Initiative>();

        for (int i = 0; i < initiativeOrder.Count; i++)
        {
            tempInitiativeList.Add(initiativeOrder.Dequeue());
        }

        Initiative initiativeToRemove = tempInitiativeList.Find(
            (Initiative a) => a.unit == deadUnit
        );
        if (initiativeToRemove != null)
        {
            if (!tempInitiativeList.Remove(initiativeToRemove))
            {
                Debug.Log("Unit not removed from initiative");
            }
        }

        initiativeOrder.Clear();
        foreach (Initiative initiative in tempInitiativeList)
        {
            initiativeOrder.Enqueue(initiative);
        }

        OnNewInitiative?.Invoke(this, initiativeOrder);
    }

    private void EnemyAI_OnEnemyTurnFinished(object sender, EventArgs e)
    {
        NextInitiative();
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        RemoveUnitFromInitiative(unit);
    }
}
