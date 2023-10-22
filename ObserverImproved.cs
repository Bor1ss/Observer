using System;
using System.Collections.Generic;
using System.Threading;

namespace VTech.Observable.Improved
{
    public interface IObserver
    {
        void Update(IState state);
    }

    public interface IState
    {
        int State { get; }
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }


    public class Subject : ISubject
    {
        public MyState State { get; } = new();
        private List<IObserver> _observers = new(); //list of all subscribers to be notified

        public void Attach(IObserver observer)
        {
            this._observers.Add(observer);//subscriber/observer attached/subscribed
        }

        public void Detach(IObserver observer)
        {
            this._observers.Remove(observer); //subscriber/observer unattached/unsubscribed        
        }

        public void Notify()
        {
            foreach (var observer in _observers) //all subscribers/observers are going to be notified about sate change
            {
                observer.Update(State);
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

    class ConcreteObserverA : IObserver
    {
        public void Update(IState state)
        {
            if (state.State < 3)
            {
                Console.WriteLine("ConcreteObserverA: Reacted to the event.");
            }
        }
    }

    class ConcreteObserverB : IObserver
    {
        public void Update(IState state)
        {
            if (state.State == 0 || state.State >= 2)
            {
                Console.WriteLine("ConcreteObserverB: Reacted to the event.");
            }
        }
    }

    public class ProgramImproved
    {
        public static void Main(string[] args)
        {
            // The client code.
            var subject = new Subject();
            var observerA = new ConcreteObserverA();
            subject.Attach(observerA);

            var observerB = new ConcreteObserverB();
            subject.Attach(observerB);

            subject.SomeBusinessLogic();
            subject.SomeBusinessLogic();

            subject.Detach(observerB);

            subject.SomeBusinessLogic();
        }
    }
}