namespace Phone
{
    using System;
    using Stateless;

    public class PhoneMachine
    {
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
                    .Ignore(PhoneActions.TurnOn);


                machine.Configure(PhoneStates.Ringing)
                    .Permit(PhoneActions.PickUp, PhoneStates.Busy)
                    .Permit(PhoneActions.HangUp, PhoneStates.StandBy)
                    .Ignore(PhoneActions.GetCall);

                machine.Configure(PhoneStates.Busy)
                    .Permit(PhoneActions.HangUp, PhoneStates.StandBy)
                    .Ignore(PhoneActions.PickUp)
                    .Ignore(PhoneActions.SendCall);

                machine.Configure(PhoneStates.Off)
                    .Permit(PhoneActions.TurnOn, PhoneStates.StandBy)
                    .Ignore(PhoneActions.TurnOff);

                return machine;
            });
        }

        public PhoneStates Is() => _phone.Value.State;

        public bool Can(PhoneActions action) => _phone.Value.CanFire(action);

        public IEnumerable<PhoneActions> WhatCan() => _phone.Value.PermittedTriggers;

        public void Do(PhoneActions action) => _phone.Value.Fire(action);
    }
}
