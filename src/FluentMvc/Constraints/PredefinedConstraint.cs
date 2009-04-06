namespace FluentMvc.Constraints
{
    using System.Web.Mvc;

    public sealed class PredefinedConstraint : IConstraint
    {
        private readonly bool value;

        public PredefinedConstraint(bool value)
        {
            this.value = value;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return value;
        }
    }
}