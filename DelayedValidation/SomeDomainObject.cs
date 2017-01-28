namespace DelayedValidation
{
    using System;

    public class SomeDomainObject : DelayedValidation
    {
        #region Fields

        private int age = -1;

        private string firstName;

        private string lastName;

        #endregion

        #region Constructors and Destructors

        public SomeDomainObject(string firstName, string lastName)
            : this()
        {
            firstName = firstName;
            lastName = lastName;
        }

        private SomeDomainObject()
        {
            AddValidationRule(delegate { return age > 100; }, null, new Exception("Age Cannot Be Greater Than 100"));
            AddValidationRule(ageGreaterThanZero, null, new Exception("Age Must Be Greater Than Zero"));
        }

        #endregion

        #region Public Properties

        public int Age
        {
            get
            {
                Validate(true);
                return age;
            }

            set
            {
                age = value;
            }
        }

        public string FirstName
        {
            get
            {
                Validate(true);
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                Validate(true);
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }

        #endregion

        #region Methods

        private bool ageGreaterThanZero(object[] o)
        {
            return age > 0;
        }

        #endregion
    }
}