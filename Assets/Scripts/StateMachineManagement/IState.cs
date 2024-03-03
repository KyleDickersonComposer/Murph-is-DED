

namespace Game.StateMachineManagement
{
    public interface IState
    {
        void OnUpdate();
        void OnEnter();
        void OnExit();    
    }

}
