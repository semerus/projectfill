﻿using System.Collections.Generic;
using System.Reflection;
using System;

namespace GiraffeStar
{
    public abstract class StateMachine : Module
    {
        Dictionary<string, State> states;
        State currentState;

        public override void OnRegister()
        {
            base.OnRegister();
            Init();
        }

        void Init()
        {
            states = new Dictionary<string, State>();

            var definedClasses = this.GetType().GetNestedTypes(BindingFlags.Instance|BindingFlags.NonPublic);
            foreach (var defined in definedClasses)
            {
                if(defined.IsSubclassOf(typeof(State)))
                {
                    var instance = Activator.CreateInstance(defined);
                    states.Add(instance.GetType().Name, (State)instance);
                }
            }
        }

        public virtual void SwitchTo(Type nextState, bool reoccur = false)
        {
            if(!nextState.IsSubclassOf(typeof(State))) { return; }

            if(currentState == null)
            {
                currentState = states[nextState.Name];
                currentState.OnEnter();
                return;
            }

            if(currentState.Equals(nextState))
            {
                if (!reoccur) { return; }                
            }

            currentState.OnExit();
            currentState = states[nextState.Name];
            currentState.OnEnter();
        }
    }
}


