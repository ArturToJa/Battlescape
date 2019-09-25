using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Unit : MonoBehaviour
    {
        public readonly int index;
        public Player owner { get; private set; }
        public string unitName { get; private set; }
        public Tile currentPosition { get; private set; }
        public Statistics statistics { get; private set; }
        public List<Ability> abilities { get; private set; }
        public List<Buff> buffs { get; private set; }
        [SerializeField]GameObject visuals;

        public void Start()
        {
            
        }

        public void Move(Tile newPosition)
        {
            Queue<Tile> path = Pathfinder.instance.GetPathFromTo(this, newPosition);
            PlayMovementAnimation();
            for (int i = 0; i < path.Count; ++i)
            {
                Tile temporaryGoal = path.Dequeue();
                StartCoroutine(MoveUnitSlowlyTowardsTile(temporaryGoal));
                currentPosition = temporaryGoal;
            }
            StopMovementAnimation();
            statistics.movementPoints = 0;
        }

        void PlayMovementAnimation()
        {

        }

        void StopMovementAnimation()
        {

        }

        IEnumerator MoveUnitSlowlyTowardsTile(Tile tile)
        {
            while (IsFacing(tile.transform.position) == false)
            {
                TurnTowards(tile.transform.position);
                yield return null;
            }
        }

        bool IsFacing(Vector3 target)
        {
            return true;
            //visuals.transform.forward and so on... raycasts? or stuff?
        }

        void TurnTowards(Vector3 target)
        {
            //the following line does it in, i believe, one frame. If we want to do it 'slowly' (with some turn rate and not in one frame), then we just have to do it ;D Time.deltaTime for the win!
            //visuals.transform.LookAt(new Vector3(target.x, visuals.transform.position.y, target.z));
        }
        
    }
}

