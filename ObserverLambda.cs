using System;
using System.Collections.Generic;
using System.Threading;

namespace VTech.Observable.Lambda
{
    public interface IState
    {
        int State { get; }
    }

    public interface ISubject
    {
        void Attach(Guid subscriberId, Action<IState> updateAction);
        void Detach(Guid subscriberId);
        void Notify();
    }


    public class Subject : ISubject
    {
        public MyState State { get; } = new();
        private readonly Dictionary<Guid, Action<IState>> _observers = new(); //list of all subscribers to be notified

        public void Attach(Guid subscriberId, Action<IState> updateAction)
        {
            this._observers.Add(subscriberId, updateAction);//subscriber/observer attached/subscribed
        }

        public void Detach(Guid subscriberId)
        {
            this._observers.Remove(subscriberId); //subscriber/observer unattached/unsubscribed        
        }

        public void Notify()
        {
            foreach (var observer in _observers) //all subscribers/observers are going to be notified about sate change
            {
                observer.Value(State);
            }
        }

        public void SomeBusinessLogic()
        {
            this.State.UpdateState();
            this.Notify(); //after State changed all observers/subscribers must be notified
        }
    }

    public class MyState : IState
    {
        public int State { get; private set; }

        public void UpdateState()
        {
            this.State = new Random().Next(0, 10);
        }
    }

    class ConcreteObserverA
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public void Update(IState state)
        {
            if (state.State < 3)
            {
                Console.WriteLine("ConcreteObserverA: Reacted to the event.");
            }
        }
    }

    class ConcreteObserverB
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public void Update(IState state)
        {
            if (state.State == 0 || state.State >= 2)
            {
                Console.WriteLine("ConcreteObserverB: Reacted to the event.");
            }
        }
    }

    public class ProgramLambda
    {
        public static void Main(string[] args)
        {
            // The client code.
            var subject = new Subject();
            var observerA = new ConcreteObserverA();
            subject.Attach(observerA.Id, observerA.Update);

            var observerB = new ConcreteObserverB();
            subject.Attach(observerB.Id, observerB.Update);

            subject.SomeBusinessLogic();
            subject.SomeBusinessLogic();

            subject.Detach(observerB.Id);

            subject.SomeBusinessLogic();
        }
    }
}