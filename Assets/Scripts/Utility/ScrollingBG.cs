using UnityEngine;

namespace Veganimus.NovaStar
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    public class ScrollingBG : MonoBehaviour
    {
        public enum ScrollDirection
        {
            Left, Right, Up, Down
        }
        [SerializeField] private ScrollDirection _scrollDirection;
        [SerializeField] private float _speed;
        private Renderer _renderer;
       
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            if(_renderer == null)
            {
                Debug.LogError(message:"Renderer: ScrollingBG is NULL!");
            }
        }
        void Update()
        {
            Scroll();
        }
        void Scroll()
        {
            switch(_scrollDirection)
            {
                case ScrollDirection.Left:
                    _renderer.material.mainTextureOffset = new Vector2(Time.time * _speed, 0);
                    break;
                case ScrollDirection.Right:
                    _renderer.material.mainTextureOffset = new Vector2(Time.time * -_speed, 0);
                    break;
                case ScrollDirection.Up:
                    _renderer.material.mainTextureOffset = new Vector2(0,Time.time * _speed);
                    break;
                case ScrollDirection.Down:
                    _renderer.material.mainTextureOffset = new Vector2(0, Time.time * -_speed);
                    break;
            }
        }
    }
  }