namespace NPG.States
{
	public interface IStateFactory
	{
		T GetState<T>() where T : IExitState;
	}
}