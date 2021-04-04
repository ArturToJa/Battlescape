using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

namespace BattlescapeUI
{
    public class AbilitiesInfoInManagement : MonoBehaviour
    {
        [SerializeField] GameObject abilityIconPrefab;
        [SerializeField] Transform unitActiveAbilities;
        [SerializeField] Transform unitPassiveAbilities;
        [SerializeField] Transform heroActiveAbilities;
        [SerializeField] Transform heroPassiveAbilities;
        void Awake()
        {
            MouseoveredButtonUnitScript.OnUnitHovered += ShowAbilities;
        }

        void ShowAbilities(UnitCreator unitCreator)
        {
            Unit unit = unitCreator.prefab.GetComponent<Unit>();

            ClearGrid(unitActiveAbilities);
            ClearGrid(unitPassiveAbilities);
            ClearGrid(heroActiveAbilities);
            ClearGrid(heroPassiveAbilities);

            if (unit is Hero)
            {
                AddAbilitiesForTo(unit, heroActiveAbilities, heroPassiveAbilities);
            }
            else
            {
                AddAbilitiesForTo(unit, unitActiveAbilities, unitPassiveAbilities);
            }
            
        }

        private void AddAbilitiesForTo(Unit unit, Transform actives, Transform passives)
        {
            foreach (AbstractAbility ability in unit.abilities)
            {
                if (ability is AbstractActiveAbility)
                {
                    AddAbility(ability, actives);
                }
                else
                {
                    AddAbility(ability, passives);
                }
            }
        }

        void AddAbility(AbstractAbility ability, Transform grid)
        {
            GameObject abilityIcon = Instantiate(abilityIconPrefab, grid);
            abilityIcon.transform.GetChild(0).GetComponent<Image>().sprite = ability.icon;
            abilityIcon.name = ability.abilityName;
            MouseHoverInfoCursor iconInfo = abilityIcon.GetComponentInChildren<MouseHoverInfoCursor>();

            iconInfo.tooltipName = ability.abilityName;
            iconInfo.tooltipText = ability.description;
        }

        void ClearGrid(Transform grid)
        {
            foreach (Transform child in grid)
            {
                child.name = "RemovedAbility";
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }
    }
}