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
                    .Permit(PhoneActions.GetCall, PhoneStates.Ringing);

                machine.Configure(PhoneStates.Ringing)
                    .Permit(PhoneActions.PickUp, PhoneStates.Busy)
                    .Permit(PhoneActions.HangUp, PhoneStates.StandBy);

                machine.Configure(PhoneStates.Busy)
                    .Permit(PhoneActions.HangUp, PhoneStates.StandBy);

                machine.Configure(PhoneStates.Off)
                    .Permit(PhoneActions.TurnOn, PhoneStates.StandBy);

                return machine;
            });
        }
    }
}
