using UI;
using UnityEngine;

namespace Game.Level
{
    public class Builder : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _inputPlane;
        [SerializeField] private CompleteHandler _completeHandler;
        [SerializeField] private DeadHandler _deadHandler;

        private Camera _camera;
        private Player.Main _player;

        public Player.Main Player => _player;

        public void Init(Data.SO.Level level, uint levelIndex, bool endless, Camera camera, ScreenHandler screenHandler)
        {
            _camera = camera;

            SpawnPlayer();

            level.Instantiate(transform, _camera, endless);

            _completeHandler.Init(screenHandler, levelIndex, level);
            _deadHandler.Init(screenHandler, endless, level);
        }

        public void SpawnPlayer(uint? health = null, uint? points = null)
        {
            _player = Instantiate(_playerPrefab, transform).GetComponentInChildren<Player.Main>();

            _player.Init(_camera, _inputPlane);

            if (health != null)
                _player.Health.SetHealth((uint)health);

            if (points != null)
                _player.Points.SetPoints((uint)points);
        }
    }
}
