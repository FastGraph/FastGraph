#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;

namespace FastGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="PetriNetSimulator{TToken}"/>.
    /// </summary>
    internal sealed class PetriNetSimulatorTests
    {
        #region Test classes

        #region Conditions

        private class CustomerWaitingAndBarberReady : IConditionExpression<Person>
        {
            public bool IsEnabled(IList<Person> tokens)
            {
                // A customer waiting and barber ready
                return tokens.OfType<Customer>().Any()
                       && tokens.OfType<Barber>().Any();
            }
        }

        private class CustomerHairCutAndBarberFinished : IConditionExpression<Person>
        {
            public bool IsEnabled(IList<Person> tokens)
            {
                // A customer with hair cut and barber barber finished to cut hair
                return tokens.OfType<Customer>().Any()
                       && tokens.OfType<Barber>().Any();
            }
        }

        private class CustomerPaid : IConditionExpression<Person>
        {
            public bool IsEnabled(IList<Person> tokens)
            {
                // A customer has paid
                return tokens.OfType<Customer>().Any();
            }
        }

        #endregion

        #region Expressions

        private class GoToChair : IExpression<Person>
        {
            /// <inheritdoc />
            public IList<Person> Evaluate(IList<Person> markings)
            {
                var persons = new List<Person>();
                Person? customer = markings.FirstOrDefault(p => p is Customer);
                if (customer != default)
                    persons.Add(customer);
                Person? barber = markings.FirstOrDefault(p => p is Barber);
                if (barber != default)
                    persons.Add(barber);
                return persons;
            }
        }

        private class GoToPay : IExpression<Person>
        {
            /// <inheritdoc />
            public IList<Person> Evaluate(IList<Person> markings)
            {
                var persons = new List<Person>();
                Person? customer = markings.FirstOrDefault(p => p is Customer);
                if (customer != default)
                    persons.Add(customer);
                return persons;
            }
        }

        private class GoToIdle : IExpression<Person>
        {
            /// <inheritdoc />
            public IList<Person> Evaluate(IList<Person> markings)
            {
                var persons = new List<Person>();
                Person? barber = markings.FirstOrDefault(p => p is Barber);
                if (barber != default)
                    persons.Add(barber);
                return persons;
            }
        }

        private class NoRobber : IExpression<Person>
        {
            public IList<Person> Evaluate(IList<Person> markings)
            {
                return new List<Person>();
            }
        }

        #endregion

        #region Persons

        private abstract class Person
        {
            public string Name { [UsedImplicitly] get; }

            public Person(string name)
            {
                Name = name;
            }
        }

        private class Barber : Person
        {
            public Barber(string name)
                : base(name)
            {
            }
        }

        private class Customer : Person
        {
            public Customer(string name)
                : base(name)
            {
            }
        }

        #endregion

        #endregion

        [Test]
        public void Constructor()
        {
            var net = new PetriNet<int>();
            var simulator = new PetriNetSimulator<int>(net);
            simulator.Net.Should().BeSameAs(net);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new PetriNetSimulator<int>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void Simulate()
        {
            #region Petri Net setup

            var net = new PetriNet<Person>();

            // Places
            var customerAtEntrance = net.AddPlace("Customer in front of barber");
            var customerWaiting = net.AddPlace("Customer waiting");
            var cutting = net.AddPlace("Cutting");
            var idle = net.AddPlace("Idle barber");
            var customerPaying = net.AddPlace("Customer paying");
            var customerNotPaying = net.AddPlace("Customer not paying");
            var customerOut = net.AddPlace("Customer out");

            // Transitions
            var enter = net.AddTransition("Enter");

            var startCutting = net.AddTransition("Start cutting");
            startCutting.Condition = new CustomerWaitingAndBarberReady();

            var finishCutting = net.AddTransition("Finish cutting");
            finishCutting.Condition = new CustomerHairCutAndBarberFinished();

            var exit = net.AddTransition("Exit");
            exit.Condition = new CustomerPaid();

            // Arcs
            net.AddArc(customerAtEntrance, enter);

            net.AddArc(enter, customerWaiting);

            var goToChair = net.AddArc(customerWaiting, startCutting);
            goToChair.Annotation = new GoToChair();

            net.AddArc(startCutting, cutting);
            net.AddArc(cutting, finishCutting);

            var goToIdle = net.AddArc(finishCutting, idle);
            goToIdle.Annotation = new GoToIdle();

            net.AddArc(idle, startCutting);

            var goToPay = net.AddArc(finishCutting, customerPaying);
            goToPay.Annotation = new GoToPay();

            var notPay = net.AddArc(finishCutting, customerNotPaying);
            notPay.Annotation = new NoRobber();

            net.AddArc(customerPaying, exit);
            net.AddArc(customerNotPaying, exit);
            net.AddArc(exit, customerOut);

            var jean = new Customer("Jean");
            var daniel = new Customer("Daniel");
            customerAtEntrance.Marking.Add(jean);
            customerAtEntrance.Marking.Add(daniel);

            var joe = new Barber("Joe");
            idle.Marking.Add(joe);

            #endregion

            // Run simulation
            var simulator = new PetriNetSimulator<Person>(net);
            simulator.Initialize();
            customerAtEntrance.Marking.Should().BeEquivalentTo(new[] { jean, daniel });
            customerWaiting.Marking.Should().BeEmpty();
            cutting.Marking.Should().BeEmpty();
            idle.Marking.Should().BeEquivalentTo(new[] { joe });
            customerPaying.Marking.Should().BeEmpty();
            customerNotPaying.Marking.Should().BeEmpty();
            customerOut.Marking.Should().BeEmpty();

            simulator.SimulateStep();
            customerAtEntrance.Marking.Should().BeEmpty();
            customerWaiting.Marking.Should().BeEquivalentTo(new[] { jean, daniel });
            cutting.Marking.Should().BeEmpty();
            idle.Marking.Should().BeEquivalentTo(new[] { joe });
            customerPaying.Marking.Should().BeEmpty();
            customerNotPaying.Marking.Should().BeEmpty();
            customerOut.Marking.Should().BeEmpty();

            simulator.SimulateStep();
            customerAtEntrance.Marking.Should().BeEmpty();
            customerWaiting.Marking.Should().BeEquivalentTo(new[] { daniel });
            cutting.Marking.Should().BeEquivalentTo(new Person[] { joe, jean });
            idle.Marking.Should().BeEmpty();
            customerPaying.Marking.Should().BeEmpty();
            customerNotPaying.Marking.Should().BeEmpty();
            customerOut.Marking.Should().BeEmpty();

            simulator.SimulateStep();
            customerAtEntrance.Marking.Should().BeEmpty();
            customerWaiting.Marking.Should().BeEquivalentTo(new[] { daniel });
            cutting.Marking.Should().BeEmpty();
            idle.Marking.Should().BeEquivalentTo(new[] { joe });
            customerPaying.Marking.Should().BeEquivalentTo(new[] { jean });
            customerNotPaying.Marking.Should().BeEmpty();
            customerOut.Marking.Should().BeEmpty();

            simulator.SimulateStep();
            customerAtEntrance.Marking.Should().BeEmpty();
            customerWaiting.Marking.Should().BeEmpty();
            cutting.Marking.Should().BeEquivalentTo(new Person[] { joe, daniel });
            idle.Marking.Should().BeEmpty();
            customerPaying.Marking.Should().BeEmpty();
            customerNotPaying.Marking.Should().BeEmpty();
            customerOut.Marking.Should().BeEquivalentTo(new[] { jean });

            simulator.SimulateStep();
            customerAtEntrance.Marking.Should().BeEmpty();
            customerWaiting.Marking.Should().BeEmpty();
            cutting.Marking.Should().BeEmpty();
            idle.Marking.Should().BeEquivalentTo(new Person[] { joe });
            customerPaying.Marking.Should().BeEquivalentTo(new Person[] { daniel });
            customerNotPaying.Marking.Should().BeEmpty();
            customerOut.Marking.Should().BeEquivalentTo(new[] { jean });

            simulator.SimulateStep();
            customerAtEntrance.Marking.Should().BeEmpty();
            customerWaiting.Marking.Should().BeEmpty();
            cutting.Marking.Should().BeEmpty();
            idle.Marking.Should().BeEquivalentTo(new Person[] { joe });
            customerPaying.Marking.Should().BeEmpty();
            customerNotPaying.Marking.Should().BeEmpty();
            customerOut.Marking.Should().BeEquivalentTo(new[] { jean, daniel });

            // No more move
            simulator.SimulateStep();
            customerAtEntrance.Marking.Should().BeEmpty();
            customerWaiting.Marking.Should().BeEmpty();
            cutting.Marking.Should().BeEmpty();
            idle.Marking.Should().BeEquivalentTo(new Person[] { joe });
            customerPaying.Marking.Should().BeEmpty();
            customerNotPaying.Marking.Should().BeEmpty();
            customerOut.Marking.Should().BeEquivalentTo(new[] { jean, daniel });
        }
    }
}
