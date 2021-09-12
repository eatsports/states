using System;

namespace NPG.States
{
	public abstract class StateMachine<TBaseState> : IUpdatable, IFixedUpdatable, IDisposable
	{
		public Type ActiveStateType => _currentStateInfo?.StateType;

		private readonly IStateFactory _stateFactory;

		private IStateInfo _currentStateInfo;
		private IStateInfo _lastStateInfo;

		protected StateMachine(IStateFactory stateFactory)
		{
			_stateFactory = stateFactory;
		}

		public TState Enter<TState>() where TState : class, TBaseState, IState
		{
			ChangeState(out TState state);
			
			_lastStateInfo = _currentStateInfo;
			_currentStateInfo = new StateInfo<TState, TBaseState>(this, state);

			state.Enter();
			return state;
		}

		public TState Enter<TState, TPayload>(TPayload payload) where TState : class, TBaseState, IPayloadedState<TPayload>
		{
			ChangeState(out TState state);
			
			_lastStateInfo = _currentStateInfo;
			_currentStateInfo = new PayloadedStateInfo<TState, TBaseState, TPayload>(this, state, payload);
			
			state.Enter(payload);
			return state;
		}

		public bool Back()
		{
			if (_lastStateInfo == null)
			{
				return false;
			}
			
			_lastStateInfo.Enter();
			return true;
		}
		
		public void Update()
		{
			_currentStateInfo?.Update();
		}
		
		public void FixedUpdate()
		{
			_currentStateInfo?.FixedUpdate();
		}
		
		public void Dispose()
		{
			_currentStateInfo?.Exit();
			_currentStateInfo = null;
			_lastStateInfo = null;
		}
		
		protected virtual void StateChanged(Type oldStateType, Type newStateType)
		{
		}

		private void ChangeState<TState>(out TState state) where TState : class, IExitable
		{
			_currentStateInfo?.Exit();
			
			state = _stateFactory.GetState<TState>();
			
			StateChanged(ActiveStateType, typeof(TState));
		}
	}
}