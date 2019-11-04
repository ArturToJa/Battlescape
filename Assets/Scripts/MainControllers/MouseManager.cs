using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance { get; private set; }
    //This is supposed to be ONLY a selection and highlighting tool. NOT a godobject nr.1

    public UnitScript SelectedUnit { get; private set; }
    public UnitScript MouseoveredUnit;
    public Tile mouseoveredTile;
    public GameObject mouseoveredDestructible;
    public Sound selectionSound;
    List<UnitScript> coloredUnits;
    bool isMouseOverUI;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        coloredUnits = new List<UnitScript>();
        AudioSource selectionAudioSource = gameObject.AddComponent<AudioSource>();
        selectionSound.oldSource = selectionAudioSource;
        selectionSound.oldSource.clip = selectionSound.clip;
        selectionSound.oldSource.volume = selectionSound.volume;
        selectionSound.oldSource.pitch = selectionSound.pitch;
        selectionSound.oldSource.loop = selectionSound.loop;
        TurnManager.Instance.NewTurnEvent += OnNewTurn;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && GameStateManager.Instance.IsCurrentPlayerAI() == false)
        {
            SelectNextAvailableUnit();
        }
        if (Helper.IsOverNonHealthBarUI())
        {
            isMouseOverUI = true;
            if (AbilityIconScript.IsAnyAbilityHovered == false && SelectedUnit != null && SelectedUnit.newMovement.isMoving == false && QCManager.Instance.PlayerChoosesWhetherToQC == false && GameStateManager.Instance.GameState != GameStates.TargettingState)
            {
                //PathCreator.Instance.ClearPath();
                ColouringTool.UncolourAllTiles();
            }
            return;
        }
        else if (GameStateManager.Instance.GameState != GameStates.TargettingState && (isMouseOverUI && SelectedUnit != null && SelectedUnit.newMovement.isMoving == false && QCManager.Instance.PlayerChoosesWhetherToQC == false && mouseoveredTile != null && TurnManager.Instance.CurrentPhase == TurnPhases.Movement && MovementQuestions.Instance.CanUnitMoveAtAll(SelectedUnit.GetComponent<UnitScript>())))
        {
            if (GameStateManager.Instance.MatchType == MatchTypes.Online && Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0].type == PlayerType.Local)
            {
                // we are in an Online match and its the other dudes turn, so no point in showing BFS
            }
            else
            {
                //So we just turned from UI back to the board - time to recolour stuff!
                isMouseOverUI = false;
                if ((SelectedUnit.EnemyList == null) || (SelectedUnit.EnemyList.Count == 0))
                {
                    ColouringTool.ColourLegalTilesFor(SelectedUnit);
                }

            }
        }
        TileRay();
        UnitRay();
        DestructibleRay();
        TestKiller();
    }

    public void OnNewTurn()
    {
        Deselect();
    }

    private void TestKiller()
    {
        if (Input.GetKeyDown(KeyCode.F) && Application.isEditor)
        {
            SelectedUnit.DealDamage(10, true, false, false);
        }
    }

    public bool IsMousoveredTile()
    {
        if (mouseoveredTile != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    private bool CanSelect(UnitScript selectableUnit)
    {
        if (GameStateManager.Instance.GameState == GameStates.AnimatingState || GameStateManager.Instance.GameState == GameStates.RetaliationState || GameStateManager.Instance.GameState == GameStates.TargettingState)
        {
            return false;
        }
        if (selectableUnit.IsAlive() == false)
        {
            return false;
        }
        switch (GameStateManager.Instance.MatchType)
        {
            case MatchTypes.Online:
                return Global.instance.playerTeams[selectableUnit.PlayerID].players[0].type == PlayerType.Local;
            case MatchTypes.HotSeat:
                return selectableUnit.PlayerID == Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0].team.index;
            case MatchTypes.Singleplayer:
                return Global.instance.playerTeams[selectableUnit.PlayerID].players[0].type == PlayerType.Local;
            default:
                Debug.Log("New type of MatchType exists and it isn't taken into consdieation here!");
                return false;
        }
    }

    private void Mouseover(GameObject mouseOveredObject)
    {
        MouseOverObjectResponse toolMouseOverer = new MouseOverObjectResponse();

        if (mouseOveredObject.tag == "Unit")
        {
            if (MouseoveredUnit != null)
            {
                toolMouseOverer.ClearMouseover(MouseoveredUnit.gameObject);
            }
            MouseoveredUnit = mouseOveredObject.GetComponent<UnitScript>();
            toolMouseOverer.Mouseover(MouseoveredUnit.gameObject);
        }
        if (mouseOveredObject.tag == "Tile")
        {
            //Tile oldTile = mouseoveredTile;
            if (mouseoveredTile != null)
            {
                UnMouseover(mouseoveredTile.gameObject);
            }
            if (mouseoveredTile != null)
            {
                toolMouseOverer.ClearMouseover(mouseoveredTile.gameObject);
            }
            mouseoveredTile = mouseOveredObject.GetComponent<Tile>();    
            toolMouseOverer.Mouseover(mouseoveredTile.gameObject);
            if (SelectedUnit != null && GameStateManager.Instance.GameState == GameStates.MoveState && MovementQuestions.Instance.CanUnitMoveAtAll(SelectedUnit.GetComponent<UnitScript>()))
            {
                if (mouseoveredTile.IsProtectedByEnemyOf(SelectedUnit))
                {
                    foreach (Tile neighbour in mouseoveredTile.neighbours)
                    {
                        if (neighbour.myUnit != null && neighbour.myUnit.PlayerID != SelectedUnit.PlayerID)
                        {
                            toolMouseOverer.PaintObject(neighbour.myUnit.gameObject, Color.red);
                            coloredUnits.Add(neighbour.myUnit);
                        }
                    }
                }
                //THIS one below is just poorly re-written for now. We need to maybe re-do it :D
                else if (Pathfinder.instance.IsLegalTileForUnit(mouseoveredTile, SelectedUnit) && SelectedUnit.isRanged && SelectedUnit.GetComponent<ShootingScript>().CanShoot)
                {
                    foreach (UnitScript enemy in FindObjectsOfType<UnitScript>())
                    {
                        if (ShootingScript.WouldItBePossibleToShoot(SelectedUnit, mouseoveredTile.transform.position, enemy.transform.position).Key && enemy.PlayerID != SelectedUnit.PlayerID)
                        {
                            toolMouseOverer.PaintObject(enemy.gameObject, Color.red);
                            coloredUnits.Add(enemy);
                        }

                    }
                }
                // THIS MIGHT NOT WORK! :<
                else if (mouseoveredTile != SelectedUnit.myTile && Pathfinder.instance.IsLegalTileForUnit(mouseoveredTile, SelectedUnit) == false)
                {

                    //PathCreator.Instance.ClearPath();
                }
            }

            //MovementSystem.Instance.CheckForAddingSteps(SelectedUnit, oldTile, mouseoveredTile);

        }
        if (mouseOveredObject.layer == 11)
        {
            toolMouseOverer.ClearMouseover(mouseoveredDestructible);
            mouseoveredDestructible = mouseOveredObject;
            toolMouseOverer.Mouseover(mouseoveredDestructible);
        }


    }

    private void UnMouseover(GameObject gameObject)
    {
        MouseOverObjectResponse toolMouseOverer = new MouseOverObjectResponse();
        if (gameObject.tag == "Unit")
        {
            toolMouseOverer.ClearMouseover(MouseoveredUnit.gameObject);
            MouseoveredUnit = null;
        }
        if (gameObject.tag == "Tile")
        {
            foreach (UnitScript unit in coloredUnits)
            {
                toolMouseOverer.PaintObject(unit.gameObject, Color.white);
            }
            coloredUnits.Clear();
            toolMouseOverer.ClearMouseover(mouseoveredTile.gameObject);
            mouseoveredTile = null;
        }
        if (gameObject.layer == 11)
        {
            toolMouseOverer.ClearMouseover(mouseoveredDestructible);
            mouseoveredDestructible = null;
        }

    }

    public void SelectAUnit(UnitScript unit, bool changeCamera)
    {
        if (changeCamera)
        {
            FindObjectOfType<CameraController>().SetCamToU(unit);
        }
        //PathCreator.Instance.ClearPath();
        UnitSelector toolUnitSelector = new UnitSelector();
        if (SelectedUnit != null)
        {
            toolUnitSelector.DeselectUnit();
        }
        else if (GameStateManager.Instance.IsCurrentPlayerAI() == false)
        {
            PlaySelectionSound();
        }
        toolUnitSelector.SelectUnit(unit);
        SelectedUnit = unit;
        UnitScript UnitScript = SelectedUnit.GetComponent<UnitScript>();
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement && MovementQuestions.Instance.CanUnitMoveAtAll(UnitScript))
        {
            ColourForSelectedUnit();
        }
        if (GameStateManager.Instance.IsCurrentPlayerAI() == false)
        {
            UIManager.UpdateAbilitiesPanel(UIManager.Instance.AbilitiesPanel, UIManager.Instance.AbilityPrefab, SelectedUnit);
        }
    }

    public void PlaySelectionSound()
    {
        selectionSound.oldSource.Play();
    }

    void ColourForSelectedUnit()
    {

        ColouringTool.UncolourAllTiles();
        if ((SelectedUnit.EnemyList == null) || (SelectedUnit.EnemyList.Count == 0))
        {
            ColouringTool.ColourLegalTilesFor(SelectedUnit);
        }
    }    

    public void Deselect()
    {
        ColouringTool.UncolourAllTiles();
        if (GameStateManager.Instance.IsCurrentPlayerAI() == false)
        {
            PlaySelectionSound();
        }
        //PathCreator.Instance.ClearPath();
        UnitSelector toolUnitSelector = new UnitSelector();
        toolUnitSelector.DeselectUnit();
        SelectedUnit = null;
    }

    private void TileRay()
    {
        Ray tileRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit tileInfo;
        int tileMask = 1 << 9;
        if (Physics.Raycast(tileRay, out tileInfo, Mathf.Infinity, tileMask))
        {
            Mouseover(tileInfo.transform.gameObject);
            
        }
        else if (mouseoveredTile != null)
        {
            UnMouseover(mouseoveredTile.gameObject);
            if (SelectedUnit != null && SelectedUnit.newMovement.isMoving == false)
            {
                //PathCreator.Instance.ClearPath();
            }
        }
    }

    private void UnitRay()
    {
        Ray unitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit unitInfo;
        int unitMask = 1 << 8;

        if (Physics.Raycast(unitRay, out unitInfo, Mathf.Infinity, unitMask))
        {
            //So we hit a UNIT with our UnitRay!
            Mouseover(unitInfo.transform.root.gameObject);

            if (Input.GetMouseButtonDown(0) && CanSelect(MouseoveredUnit))
            {
                if (SelectedUnit == MouseoveredUnit)
                {
                    Deselect();
                }
                else
                {
                    SelectAUnit(MouseoveredUnit, false);
                }
            }
        }
        else if (MouseoveredUnit != null)
        {
            // So we DIDN'T hit a unit with a UnitRay, and we had a mouseovered object a moment ago!
            UnMouseover(MouseoveredUnit.gameObject);
        }
    }
    private void DestructibleRay()
    {
        Ray destructibleRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit objectInfo;
        int tileMask = 1 << 11;
        if (Physics.Raycast(destructibleRay, out objectInfo, Mathf.Infinity, tileMask))
        {
            Mouseover(objectInfo.collider.transform.gameObject);
        }
        else if (mouseoveredDestructible != null)
        {
            UnMouseover(mouseoveredDestructible);
        }
    }



    bool IsCamManual()
    {
        return FindObjectOfType<CameraController>().manualCamera;
    }

    void SelectNextAvailableUnit()
    {

        List<UnitScript> AllUnits = new List<UnitScript>(VictoryLossChecker.GetMyUnitList());
        List<UnitScript> PossibleUnits = new List<UnitScript>();
        foreach (UnitScript unit in AllUnits)
        {
            if (GameStateManager.Instance.CanUnitActInThisPhase(unit))
            {
                PossibleUnits.Add(unit);
            }
        }

        if (PossibleUnits.Count > 0)
        {
            SelectAUnit(PossibleUnits[Random.Range(0, PossibleUnits.Count)], true);
        }
        else
        {
            PopupTextController.AddPopupText("No more units ot act!", PopupTypes.Info);
        }
    }
}
