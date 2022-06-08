using System.Collections.Generic;
using System.Threading.Tasks;
using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditValidator : ICreditValidator
    {
        private readonly IEnumerable<IValidationChecker> validations;

        public CreditValidator(IEnumerable<IValidationChecker> validations)
        {
            this.validations = validations;
        }

        public async Task<(bool, IEnumerable<string>)> PassAllValidations(CreditApplication model)
        {
            var valid = true;

            var errors = new List<string>();

            foreach (var validation in validations)
            {
                if (!validation.ValidatorLogic(model))
                {
                    errors.Add(validation.ErrorMessage);
                    valid = false;
                }
            }

            return (valid, errors);

        }
    }
}
