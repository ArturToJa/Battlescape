using UnityEngine;

namespace BattlescapeLogic
{
    /// <summary>
    ///     TO DO!!!      Create Spawn function, assign position to tiles
    /// </summary>
    
    public class Obstacle : MonoBehaviour, IMouseTargetable
    {
        [SerializeField]
        private bool _isTall;
        [SerializeField]
        private int _size;

        public Tile[] position;

        private Animator animator;

        public bool isTall
        {
            get
            {
                return _isTall;
            }
            private set
            {
                _isTall = value;
            }
        }
        public int size
        {
            get
            {
                return _size;
            }
        }

        void Start()
        {
            position = new Tile[_size];
            animator = GetComponent<Animator>();
            
        }

        public void Destruct(Unit source)
        {
            animator.SetTrigger("Destruct");
        }

        public void Spawn()
        {
            Debug.Log("'Spawn' function for Obstacle is empty!");
        }
        

    }


}
