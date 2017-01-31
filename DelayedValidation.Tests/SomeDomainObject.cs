namespace DelayedValidation
{
    /// <summary>
    ///     Some domain object that requires to always be valid but has properties that are dependant on one another or should
    ///     run delayed validation
    /// </summary>
    public class SomeDomainObject : DelayedValidation, IDraftable
    {
        #region Fields

        private int age;

        private string firstName;

        private string lastName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Constructor to create a valid class
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="age"></param>
        public SomeDomainObject(string firstName, string lastName, int age)
            : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        private SomeDomainObject()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     HOLDS THE AGE OF THE DOMAIN OBJECT
        /// </summary>
        public int Age
        {
            get
            {
                Validate();
                return age;
            }

            set
            {
                age = value;

                //ADD DELAYED VALIDATION RULE TO MAKE SURE THE AGE IS < 100
                AddDelayedValidationRule(
                    delegate { return age < 100; },
                    null,
                    new ValidationException("Age Cannot Be Greater Than 100"));

                //ADD DELAYED VALIDATION RULE TO MAKE SURE THE AGE IS > 0
                AddDelayedValidationRule(
                    ageGreaterThanZero,
                    null,
                    new ValidationException("Age Must Be Greater Than Zero"));
            }
        }

        /// <summary>
        ///     HOLDS THE FIRST NAME OF THE DOMAIN OBJECT
        /// </summary>
        public string FirstName
        {
            get
            {
                Validate();
                return firstName;
            }
            set
            {
                firstName = value;

                //ADD A RULE TO MAKE SURE THE FIRST NAME IS NOT EMPTY USING A DELEGATE
                AddDelayedValidationRule(
                    delegate { return !string.IsNullOrEmpty(firstName); },
                    new object[] { },
                    new ValidationException($"{nameof(firstName)} cannot be empty"));

                //ADD A RULE TO MAKE SURE THE FIRST NAME IS 3 OR MORE CHARACTERS USING LAMBDA
                AddDelayedValidationRule(
                    args => firstName.Length >= 3,
                    null,
                    new ValidationException($"{nameof(firstName)} must be at least 3 characters long"));

                //ADD A RULE TO MAKE SURE THE FIRST NAME IS NOT THE SAME AS THE LAST NAME USING A METHOD
                AddDelayedValidationRule(
                    firstAndLastCannotBeEqual,
                    null,
                    new ValidationException($"{nameof(firstName)} and {nameof(lastName)} cannot be the same"));
            }
        }

        /// <summary>
        ///     IMPLEMENTED FROM IDRAFTABLE TO ALLOW READS OF AN INVALID OBJECT
        /// </summary>
        public bool IsDraft { get; set; } = false;

        /// <summary>
        ///     HOLDS LAST NAME OF THE DOMAIN OBJECT
        /// </summary>
        public string LastName
        {
            get
            {
                Validate();
                return lastName;
            }
            set
            {
                lastName = value;

                //ADD A RULE TO MAKE SURE THE FIRST NAME AND LAST NAME ARE NOT EQUAL
                AddDelayedValidationRule(
                    firstAndLastCannotBeEqual,
                    new object[] { },
                    new ValidationException("First and Last cannot be the same"));
            }
        }

        #endregion

        #region Methods

        private bool ageGreaterThanZero(object[] o)
        {
            return age > 0;
        }

        private bool firstAndLastCannotBeEqual(object[] arg)
        {
            return firstName != lastName;
        }

        #endregion
    }
}