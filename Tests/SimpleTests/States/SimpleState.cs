using System;
using NPG.States;

namespace NPG.States.Test
{
	public class SimpleState : State
	{
		public SimpleState(StateMachine stateMachine) : base(stateMachine)
		{
		}

		protected override void OnEnter()
		{
			Console.WriteLine("SimpleState:OnEnter");
		}

		protected override void OnExit()
		{
			Console.WriteLine("SimpleState:OnExit");
		}
	}
}