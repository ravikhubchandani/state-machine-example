namespace Phone
{
    public class Program
    {
        private static PhoneMachine _phone;

        public static void Main(string[] args)
        {
            _phone = new PhoneMachine
            {
                OnActivateAction = OnActivate,
                OnDeactivateAction = OnDeactivate,
                OnEntryAction = OnEntry,
                OnExitAction = OnExit
            };

            // Initial state is Off

            Console.WriteLine("Turning phone on ..");
            _phone.Do(PhoneActions.TurnOn);
            Console.WriteLine($"Transitioned from {_phone.PreviousState} to {_phone.State}");

            Console.WriteLine("Calling ..");
            _phone.Do(PhoneActions.SendCall);
            Console.WriteLine($"Transitioned from {_phone.PreviousState} to {_phone.State}");

            Console.WriteLine("Ending call ..");
            _phone.Do(PhoneActions.HangUp);
            Console.WriteLine($"Transitioned from {_phone.PreviousState} to {_phone.State}");

            Console.WriteLine("Picking up ..");
            _phone.Do(PhoneActions.PickUp);
            Console.WriteLine($"Transitioned from {_phone.PreviousState} to {_phone.State}");

            Console.WriteLine($"Now state is {_phone.State}");
            Console.WriteLine("Calling ..");
            _phone.Do(PhoneActions.GetCall);

            Console.WriteLine("Calling ..");
            _phone.Do(PhoneActions.GetCall);

            Console.WriteLine("Calling ..");
            _phone.Do(PhoneActions.GetCall);
            Console.WriteLine($"Number of calls received while Busy: {_phone.NumberOfCallsWhileBusy}");

            Console.WriteLine("Activating ..");
            _phone.Activate();
            Console.WriteLine($"Previous was {_phone.PreviousState}. Now is {_phone.State} (No transition takes place on Activate)");

            Console.WriteLine("Calling again ..");
            _phone.Do(PhoneActions.SendCall);
            Console.WriteLine($"Transitioned from {_phone.PreviousState} to {_phone.State}");

            Console.WriteLine("Deactivating ..");
            _phone.Deactivate();
            Console.WriteLine($"Previous was {_phone.PreviousState}. Now is {_phone.State} (No transition takes place on Dectivate)");            

            // Output
            // Exiting. Current state: StandBy
            // Entering. Current state: Busy
            // Transitioned from StandBy to Busy
            // Ending call ..
            // Exiting. Current state: Busy
            // Entering. Current state: StandBy
            // Transitioned from Busy to StandBy
            // Picking up ..
            // Exiting. Current state: StandBy
            // Entering. Current state: Busy
            // Transitioned from StandBy to Busy
            // Now state is Busy
            // Calling ..
            // Calling ..
            // Calling ..
            // Number of calls received while Busy: 3
            // Activating ..
            // Activating. Current state: Busy
            // Previous was StandBy. Now is Busy (No transition takes place on Activate)
            // Calling again ..
            // Transitioned from StandBy to Busy
            // Deactivating ..
            // Deactivating. Current state: Busy
            // Previous was StandBy. Now is Busy (No transition takes place on Dectivate)
        }

        private static void OnActivate() => Console.WriteLine($"Activating. Current state: {_phone.Is()}");
        private static void OnDeactivate() => Console.WriteLine($"Deactivating. Current state: {_phone.Is()}");
        private static void OnEntry() => Console.WriteLine($"Entering. Current state: {_phone.Is()}");
        private static void OnExit() => Console.WriteLine($"Exiting. Current state: {_phone.Is()}");
    }
}