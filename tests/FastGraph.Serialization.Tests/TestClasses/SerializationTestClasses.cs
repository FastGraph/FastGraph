#nullable enable

using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace FastGraph.Serialization.Tests
{
    #region Test enumerations

    /// <summary>
    /// Enumeration of the <see cref="Person"/>'s gender.
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Male gender.
        /// </summary>
        Male,

        /// <summary>
        /// Female gender.
        /// </summary>
        Female
    }

    /// <summary>
    /// Enumeration of the <see cref="Person"/>'s age group.
    /// </summary>
    public enum AgeGroup
    {
        /// <summary>
        /// Unknown age group.
        /// </summary>
        Unknown,

        /// <summary>
        /// 0 to 20 age group.
        /// </summary>
        Youth,

        /// <summary>
        /// 20 to 40 age group.
        /// </summary>
        Adult,

        /// <summary>
        /// 40 to 65 age group.
        /// </summary>
        MiddleAge,

        /// <summary>
        /// Over 65 age group.
        /// </summary>
        Senior
    }

    #endregion

    #region Test classes

    /// <summary>
    /// Representation for a single serializable <see cref="Person"/>.
    /// <see cref="INotifyPropertyChanged"/> allows properties of the <see cref="Person"/> class to
    /// participate as source in data bindings.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class Person : INotifyPropertyChanged, IEquatable<Person>, IDataErrorInfo
    {
        #region Fields and Constants

        [NotNull]
        private const string DefaultFirstName = "Unknown";

        private string _id;

        private string _firstName;

        private string? _lastName;

        private string? _middleName;

        private string? _suffix;

        private string? _nickName;

        private string? _maidenName;

        private Gender _gender;

        private DateTime? _birthDate;

        private string? _birthPlace;

        private DateTime? _deathDate;

        private string? _deathPlace;

        private bool _isLiving;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// Each new instance will be given a unique identifier.
        /// This parameterless constructor is also required for serialization.
        /// </summary>
        public Person()
        {
            _id = Guid.NewGuid().ToString();
            _firstName = DefaultFirstName;
            _isLiving = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class with
        /// the <paramref name="firstName"/> and the <paramref name="lastName"/>.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        public Person(string? firstName, string? lastName)
            : this()
        {
            // Use the first name if specified, if not, the default first name is used.
            if (!string.IsNullOrEmpty(firstName))
            {
                _firstName = firstName;
            }

            _lastName = lastName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class with
        /// the <paramref name="firstName"/>, the <paramref name="lastName"/>
        /// and the <paramref name="gender"/>.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="gender">Gender of the person.</param>
        public Person(string? firstName, string? lastName, Gender gender)
            : this(firstName, lastName)
        {
            _gender = gender;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for each person.
        /// </summary>
        [XmlAttribute]
        public string Id
        {
            get => _id;
            set
            {
                if (_id == value)
                    return;

                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// Gets or sets the name that occurs first in a given name.
        /// </summary>
        [XmlElement]
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName == value)
                    return;

                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(FullName));
            }
        }

        /// <summary>
        /// Gets or sets the part of a given name that indicates what family the person belongs to.
        /// </summary>
        [XmlElement]
        public string? LastName
        {
            get => _lastName;
            set
            {
                if (_lastName == value)
                    return;

                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(FullName));
            }
        }

        /// <summary>
        /// Gets or sets the name that occurs between the first and last name.
        /// </summary>
        [XmlElement]
        public string? MiddleName
        {
            get => _middleName;
            set
            {
                if (_middleName == value)
                    return;

                _middleName = value;
                OnPropertyChanged(nameof(MiddleName));
                OnPropertyChanged(nameof(FullName));
            }
        }

        /// <summary>
        /// Gets the person's name in the format <see cref="FirstName"/> <see cref="LastName"/>.
        /// </summary>
        [XmlIgnore]
        public string Name
        {
            get
            {
                string name = string.Empty;
                if (!string.IsNullOrEmpty(_firstName))
                {
                    name += _firstName;
                }

                if (!string.IsNullOrEmpty(_lastName))
                {
                    name += $" {_lastName}";
                }

                return name;
            }
        }

        /// <summary>
        /// Gets the person's fully qualified name: <see cref="FirstName"/> <see cref="MiddleName"/> <see cref="LastName"/> <see cref="Suffix"/>.
        /// </summary>
        [XmlIgnore]
        public string FullName
        {
            get
            {
                string fullName = string.Empty;
                if (!string.IsNullOrEmpty(_firstName))
                {
                    fullName += _firstName;
                }

                if (!string.IsNullOrEmpty(_middleName))
                {
                    fullName += $" {_middleName}";
                }

                if (!string.IsNullOrEmpty(_lastName))
                {
                    fullName += $" {_lastName}";
                }

                if (!string.IsNullOrEmpty(_suffix))
                {
                    fullName += $" {_suffix}";
                }

                return fullName;
            }
        }

        /// <summary>
        /// Gets or sets the text that appear behind the last name providing additional information about the person.
        /// </summary>
        [XmlElement]
        public string? Suffix
        {
            get => _suffix;
            set
            {
                if (_suffix == value)
                    return;

                _suffix = value;
                OnPropertyChanged(nameof(Suffix));
                OnPropertyChanged(nameof(FullName));
            }
        }

        /// <summary>
        /// Gets or sets the person's familiar or shortened name.
        /// </summary>
        [XmlElement]
        public string? NickName
        {
            get => _nickName;
            set
            {
                if (_nickName == value)
                    return;

                _nickName = value;
                OnPropertyChanged(nameof(NickName));
            }
        }

        /// <summary>
        /// Gets or sets the person's name carried before marriage.
        /// </summary>
        [XmlElement]
        public string? MaidenName
        {
            get => _maidenName;
            set
            {
                if (_maidenName == value)
                    return;

                _maidenName = value;
                OnPropertyChanged(nameof(MaidenName));
            }
        }

        /// <summary>
        /// Gets or sets the person's gender.
        /// </summary>
        [XmlElement]
        public Gender Gender
        {
            get => _gender;
            set
            {
                if (_gender == value)
                    return;

                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        /// <summary>
        /// Gets the age of the person.
        /// </summary>
        [XmlIgnore]
        public int? Age
        {
            get
            {
                if (BirthDate is null)
                    return default;

                // Determine the age of the person based on just the year.
                DateTime startDate = BirthDate.Value;
                DateTime endDate = IsLiving || DeathDate is null ? DateTime.Now : DeathDate.Value;
                int age = endDate.Year - startDate.Year;

                // Compensate for the month and day of month (if they have not had a birthday this year).
                if (endDate.Month < startDate.Month || (endDate.Month == startDate.Month && endDate.Day < startDate.Day))
                {
                    age--;
                }

                return Math.Max(0, age);
            }
        }

        /// <summary>
        /// Gets the age of the person.
        /// </summary>
        [XmlIgnore]
        public AgeGroup AgeGroup
        {
            get
            {
                AgeGroup ageGroup = AgeGroup.Unknown;

                if (Age.HasValue)
                {
                    // The AgeGroup enumeration is defined later in this file. It is up to the Person
                    // class to define the ages that fall into the particular age groups
                    if (Age >= 0 && Age < 20)
                    {
                        ageGroup = AgeGroup.Youth;
                    }
                    else if (Age >= 20 && Age < 40)
                    {
                        ageGroup = AgeGroup.Adult;
                    }
                    else if (Age >= 40 && Age < 65)
                    {
                        ageGroup = AgeGroup.MiddleAge;
                    }
                    else
                    {
                        ageGroup = AgeGroup.Senior;
                    }
                }

                return ageGroup;
            }
        }

        /// <summary>
        /// Gets the year the person was born.
        /// </summary>
        [XmlIgnore]
        public string YearOfBirth
        {
            get
            {
                if (_birthDate.HasValue)
                {
                    return _birthDate.Value.Year.ToString(CultureInfo.CurrentCulture);
                }

                return "-";
            }
        }

        /// <summary>
        /// Gets the year the person died.
        /// </summary>
        [XmlIgnore]
        public string YearOfDeath
        {
            get
            {
                if (_deathDate.HasValue && !_isLiving)
                {
                    return _deathDate.Value.Year.ToString(CultureInfo.CurrentCulture);
                }

                return "-";
            }
        }

        /// <summary>
        /// Gets or sets the person's birth date.
        /// </summary>
        [XmlElement]
        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate is null || _birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged(nameof(BirthDate));
                    OnPropertyChanged(nameof(Age));
                    OnPropertyChanged(nameof(AgeGroup));
                    OnPropertyChanged(nameof(YearOfBirth));
                    OnPropertyChanged(nameof(BirthMonthAndDay));
                    OnPropertyChanged(nameof(BirthDateAndPlace));
                }
            }
        }

        /// <summary>
        /// Gets or sets the person's place of birth.
        /// </summary>
        [XmlElement]
        public string? BirthPlace
        {
            get => _birthPlace;
            set
            {
                if (_birthPlace == value)
                    return;

                _birthPlace = value;
                OnPropertyChanged(nameof(BirthPlace));
                OnPropertyChanged(nameof(BirthDateAndPlace));
            }
        }

        /// <summary>
        /// Gets the month and day the person was born in.
        /// </summary>
        [XmlIgnore]
        public string? BirthMonthAndDay =>
            _birthDate?.ToString(
                DateTimeFormatInfo.CurrentInfo?.MonthDayPattern,
                CultureInfo.CurrentCulture);

        /// <summary>
        /// Gets a friendly string for BirthDate and Place.
        /// </summary>
        [XmlIgnore]
        public string? BirthDateAndPlace
        {
            get
            {
                if (_birthDate is null)
                    return default;

                var returnValue = new StringBuilder();
                returnValue.Append("Born ");
                returnValue.Append(
                    _birthDate.Value.ToString(
                        DateTimeFormatInfo.CurrentInfo?.ShortDatePattern,
                        CultureInfo.CurrentCulture));

                if (!string.IsNullOrEmpty(_birthPlace))
                {
                    returnValue.Append(", ");
                    returnValue.Append(_birthPlace);
                }

                return returnValue.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the person's death of death.
        /// </summary>
        [XmlElement]
        public DateTime? DeathDate
        {
            get => _deathDate;
            set
            {
                if (_deathDate == default || _deathDate != value)
                {
                    IsLiving = false;
                    _deathDate = value;
                    OnPropertyChanged(nameof(DeathDate));
                    OnPropertyChanged(nameof(Age));
                    OnPropertyChanged(nameof(YearOfDeath));
                }
            }
        }

        /// <summary>
        /// Gets or sets the person's place of death.
        /// </summary>
        [XmlElement]
        public string? DeathPlace
        {
            get => _deathPlace;
            set
            {
                if (_deathPlace == value)
                    return;

                IsLiving = false;
                _deathPlace = value;
                OnPropertyChanged(nameof(DeathPlace));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the person is still alive or deceased.
        /// </summary>
        [XmlElement]
        public bool IsLiving
        {
            get => _isLiving;
            set
            {
                if (_isLiving == value)
                    return;

                _isLiving = value;
                OnPropertyChanged(nameof(IsLiving));
            }
        }

        /// <summary>
        /// Gets a string that describes this person to their parents.
        /// </summary>
        [XmlIgnore]
        public string ParentRelationshipText => _gender == Gender.Male ? "Son" : "Daughter";

        /// <summary>
        /// Gets a string that describes this person to their siblings.
        /// </summary>
        [XmlIgnore]
        public string SiblingRelationshipText => _gender == Gender.Male ? "Brother" : "Sister";

        /// <summary>
        /// Gets a string that describes this person to their spouses.
        /// </summary>
        [XmlIgnore]
        public string SpouseRelationshipText => _gender == Gender.Male ? "Husband" : "Wife";

        /// <summary>
        /// Gets a string that describes this person to their children.
        /// </summary>
        [XmlIgnore]
        public string ChildRelationshipText => _gender == Gender.Male ? "Father" : "Mother";

        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// <see cref="INotifyPropertyChanged"/> requires an event called <see cref="PropertyChanged"/>.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Fires the event for the property when it changes.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IEquatable

        /// <inheritdoc />
        public bool Equals(Person? other)
        {
            if (other is null)
                return false;
            return Id == other.Id;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region IDataErrorInfo

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        public string Error => default!;

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">>The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;

                if (columnName == nameof(BirthDate))
                {
                    if (BirthDate == DateTime.MinValue)
                    {
                        result = "This does not appear to be a valid date.";
                    }
                }

                if (columnName == nameof(DeathDate))
                {
                    if (DeathDate == DateTime.MinValue)
                    {
                        result = "This does not appear to be a valid date.";
                    }
                }

                return result;
            }
        }

        #endregion
    }

    #endregion
}
