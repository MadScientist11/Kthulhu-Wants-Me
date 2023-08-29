using FSM;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAIBrain : MonoBehaviour
    {
        [SerializeField] private TentacleFacade _tentacleFacade;

        private PlayerFacade _player;

        private TentacleState _currentState;
        private StateMachine _tentacleFsm;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

        private void Start()
        {
            _tentacleFsm = new StateMachine();
            InitializeStates(_tentacleFsm, new TentacleStatesFactory(_tentacleFacade, _player));
            SetUpTransitions(_tentacleFsm);
            _tentacleFsm.Init();
        }

        private void Update()
        {
            Debug.Log(_tentacleFsm.ActiveStateName);
            _tentacleFsm.OnLogic();
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position);
        }

        private void InitializeStates(StateMachine fsm, TentacleStatesFactory statesFactory)
        {
            fsm.AddState(TentacleState.Idle.ToString(), statesFactory.Create(TentacleState.Idle));
            fsm.AddState(TentacleState.GrabPlayer.ToString(), statesFactory.Create(TentacleState.GrabPlayer));
            fsm.AddState(TentacleState.Stunned.ToString(), statesFactory.Create(TentacleState.Stunned));

            fsm.SetStartState(TentacleState.Idle.ToString());
        }

        private void SetUpTransitions(StateMachine fsm)
        {
            fsm.AddTransition(new Transition(
                TentacleState.Idle.ToString(),
                TentacleState.GrabPlayer.ToString(),
                (transition) => DistanceToPlayer() < 5f
            ));

            fsm.AddTransition(new Transition(
                TentacleState.GrabPlayer.ToString(),
                TentacleState.Stunned.ToString(),
                (transition) => Input.GetKeyDown(KeyCode.Space)
            ));

            fsm.AddTransition(new Transition(
                TentacleState.Stunned.ToString(),
                TentacleState.Idle.ToString()
            ));
        }
    }
}