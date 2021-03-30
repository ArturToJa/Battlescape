using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ModifierGroup : IEnumerable<AbstractAttackModifier>
    {
        List<AbstractAttackModifier> modifiers;
        Unit owner;

        public AbstractAttackModifier this[int key]
        {
            get
            {
                return modifiers[key];
            }
        }
        public ModifierGroup(Unit owner)
        {
            this.owner = owner;
            modifiers = new List<AbstractAttackModifier>();
        }

        public ModifierGroup(Unit owner, List<AbstractAttackModifier> modifiers)
        {
            this.owner = owner;
            this.modifiers = modifiers;
        }

        public void Add(AbstractAttackModifier modifier)
        {
            modifiers.Add(modifier);
        }

        public void Remove(AbstractAttackModifier modifier)
        {
            modifiers.Remove(modifier);
        }

        public IEnumerator<AbstractAttackModifier> GetEnumerator()
        {
            return modifiers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsEmpty()
        {
            return modifiers.Count == 0;
        }

        public void Clear()
        {
            foreach (AbstractAttackModifier modifier in modifiers)
            {
                modifier.RemoveInstantly();
            }
        }

        public ModifierGroup FindAll(System.Predicate<AbstractAttackModifier> match)
        {
            return new ModifierGroup(owner, modifiers.FindAll(match));
        }

        public ModifierGroup FindAllModifiersOfType(string modifierType)
        {
            return FindAll(
                delegate (AbstractAttackModifier modifier)
                {
                    if (modifier.abilityName.Equals(modifierType))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
        }
    }
}
