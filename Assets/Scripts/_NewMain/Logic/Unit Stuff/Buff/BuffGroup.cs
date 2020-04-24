using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class BuffGroup : IEnumerable<AbstractBuff>
    {
        private List<AbstractBuff> buffs;
        public IDamageable owner { get; private set; }

        public AbstractBuff this[int key]
        {
            get
            {
                return buffs[key];
            }
        }

        public BuffGroup(IDamageable owner)
        { 
            this.owner = owner;
            buffs = new List<AbstractBuff>();
        }

        public BuffGroup(IDamageable owner, List<AbstractBuff> buffs)
        {
            this.owner = owner;
            this.buffs = buffs;
        }

        public void AddBuff(AbstractBuff buff)
        {
            buffs.Add(buff);
        }

        public void RemoveBuff(AbstractBuff buff)
        {
            buffs.Remove(buff);
        }

        public IEnumerator<AbstractBuff> GetEnumerator()
        {
            return buffs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsEmpty()
        {
            return buffs.Count == 0;
        }

        public void Clear()
        {
            foreach(AbstractBuff buff in buffs)
            {
                buff.RemoveFromTargetInstantly();
            }
        }

        public BuffGroup FindAll(System.Predicate<AbstractBuff> match)
        {
            return new BuffGroup(owner, buffs.FindAll(match));
        }

        public BuffGroup FindAllBuffsOfType(string buffType)
        {
            return FindAll
            (
                delegate(AbstractBuff buff)
                {
                    if(buff.buffName.Equals(buffType))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            );
        }
    }
}
