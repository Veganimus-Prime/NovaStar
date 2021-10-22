using UnityEngine;

namespace Veganimus.NovaStar
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///@info: Can be added to any class with a MovementSO field. Allows
    ///the game object to mirror the Player's movement input.
    ///</summary>
    [CreateAssetMenu(fileName = "newMovement.asset", menuName = "Movement/ Mirror Movement")]
    public class MirrorMoveSO : MovementSO
    {
        [SerializeField] private InputReaderSO inputReader;

        private void OnEnable() => inputReader.moveEvent += OnMoveInput;

        private void OnDisable() => inputReader.moveEvent -= OnMoveInput;

        public void MirrorMovement(Transform mover, Rigidbody rigidbody, float deltaTime)
        {
            moveDirection = new Vector3(-horizontal, -vertical, 0);
            rigidbody.MovePosition(mover.localPosition + (moveDirection * speed * deltaTime));
        }

        public void OnMoveInput(float h, float v)
        {
            horizontal = h;
            vertical = v;
        }

        public void Pitch(Transform mover, float deltaTime)
        {
            float pitch = vertical * tiltAngle;
            Quaternion target = Quaternion.Euler(0, mover.transform.rotation.y , pitch);
            mover.rotation = Quaternion.Slerp(mover.rotation, target, deltaTime * smooth);
        }
        
    }
}