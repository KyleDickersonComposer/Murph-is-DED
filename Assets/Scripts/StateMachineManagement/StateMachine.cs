using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StateMachineManagement
{
   public class StateMachine
   {
   
     private Dictionary<Type,List<Transition>> _transitions;
     private List<Transition> _currentTransitions;
     private IState _currentState;
   
     public StateMachine()
     {
       _transitions = new Dictionary<Type, List<Transition>>();
       _currentTransitions = new List<Transition>();
     }
   
   
     public void SetState(IState state)
     {
        if(state == _currentState)
        {
           return;
        }
   
        _currentState?.OnExit();
        _currentState = state;
   
        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        _currentState?.OnEnter();
     }
   
     public void AddTransition(IState from,IState to,Func<bool> checkCallback)
     {
       if(_transitions.TryGetValue(from.GetType(),out var transitions) == false)
       {
           transitions = new List<Transition>();
           _transitions[from.GetType()] = transitions;
       }
       
       transitions.Add(new Transition(to,checkCallback));
     }  
   
     public void OnUpdate()
     {
         Transition transition = GetTransition();
         if(transition != null)
         {
           SetState(transition.ToState);
         }
         _currentState?.OnUpdate();
     }
   
      public string GetCurrentStateName()
      {
         if(_currentState == null)
         {
            return "No state";
         }
   
         return _currentState.ToString();
      }
     private class Transition
      {
         public Func<bool> Condition;
         public IState ToState;
         public Transition(IState toState, Func<bool> condition)
         {
            ToState = toState;
            Condition = condition;
         }
      }
   
      private Transition GetTransition()
      {
        if(_currentTransitions == null)
        {
            return null;
        }
        foreach(Transition transition in _currentTransitions)
        {
           if(transition.Condition())
           {
               return transition;
           }
        }
        return null;
      }
   }

}
