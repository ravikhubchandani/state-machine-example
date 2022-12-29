namespace Phone
{
    using System;
    using Stateless;

    public class PhoneMachine
    {
        public PhoneStates PreviousState { get; private set; }
        public int NumberOfCallsWhileBusy { get; private set; }
        public PhoneStates State { get { return Is(); } }
        public Action? OnActivateAction { get; set; }
        public Action? OnDeactivateAction { get; set; }
        public Action? OnEntryAction { get; set; }
        public Action? OnExitAction { get; set; }
        private readonly Lazy<StateMachine<PhoneStates, PhoneActions>> _phone;

        public PhoneMachine()
        {
            _phone = new Lazy<StateMachine<PhoneStates, PhoneActions>>( () =>
            {
                var machine = new StateMachine<PhoneStates, PhoneActions>(PhoneStates.Off);

                machine.Configure(PhoneStates.StandBy)
                    .Permit(PhoneActions.TurnOff, PhoneStates.Off)
                    .Permit(PhoneActions.PickUp, PhoneStates.Busy)
                    .Permit(PhoneActions.SendCall, PhoneStates.Busy)
                    .Permit(PhoneActions.GetCall, PhoneStates.Ringing)
                    .Ignore(PhoneActions.HangUp)
                    .Ignore(PhoneActions.TurnOn)
                    .OnActivate(() => OnActivateAction?.Invoke())
                    .OnDeactivate(() => OnDeactivateAction?.Invoke())
                    .OnEntry(() => OnEntryAction?.Invoke())
                    .OnExit(() => OnPhoneStateExit());

                machine.Configure(PhoneStates.Ringing)
                    .Permit(PhoneActions.PickUp, PhoneStates.Busy)
                    .Permit(PhoneActions.HangUp, PhoneStates.StandBy)
                    .Ignore(PhoneActions.GetCall)
                    .OnActivate(() => OnActivateAction?.Invoke())
                    .OnDeactivate(() => OnDeactivateAction?.Invoke())
                    .OnEntry(() => OnEntryAction?.Invoke())
                    .OnExit(() => OnPhoneStateExit())
                    .InternalTransition(PhoneActions.GetCall, () => NumberOfCallsWhileBusy++);

                machine.Configure(PhoneStates.Busy)
                    .Permit(PhoneActions.HangUp, PhoneStates.StandBy)
                    .Ignore(PhoneActions.PickUp)
                    .Ignore(PhoneActions.SendCall)
                    .OnActivate(() => OnActivateAction?.Invoke())
                    .OnDeactivate(() => OnDeactivateAction?.Invoke())
                    .OnEntry(() => OnEntryAction?.Invoke())
                    .OnExit(() => OnPhoneStateExit())
                    .InternalTransition(PhoneActions.GetCall, () => NumberOfCallsWhileBusy++);

                machine.Configure(PhoneStates.Off)
                    .Permit(PhoneActions.TurnOn, PhoneStates.StandBy)
                    .Ignore(PhoneActions.TurnOff)
                    .OnActivate(() => OnActivateAction?.Invoke())
                    .OnDeactivate(() => OnDeactivateAction?.Invoke())
                    .OnEntry(() => OnEntryAction?.Invoke())
                    .OnExit(() => OnPhoneStateExit());

                return machine;
            });
        }

        public PhoneStates Is() => _phone.Value.State;

        public bool Can(PhoneActions action) => _phone.Value.CanFire(action);

        public IEnumerable<PhoneActions> WhatCan() => _phone.Value.PermittedTriggers;

        public void Do(PhoneActions action) => _phone.Value.Fire(action);

        public void Activate() => _phone.Value.Activate();

        public void Deactivate() => _phone.Value.Deactivate();

        private void OnPhoneStateExit()
        {
            PreviousState = _phone.Value.State;
            OnExitAction?.Invoke();
        }
    }
}
