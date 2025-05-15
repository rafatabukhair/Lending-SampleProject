using System.Collections.Generic;
using BusinessEntities;
using Common;

namespace Core.Services.Users
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateUserService : IUpdateUserService
    {
        public void Update(User user, string name, string email, UserTypes type, decimal? annualSalary, IEnumerable<string> tags, List<string> dedupFields)
        {
            if (ShouldUpdate("name", name, dedupFields))
                user.SetName(name ?? string.Empty);

            if (ShouldUpdate("email", email, dedupFields))
                user.SetEmail(email ?? string.Empty);

            if (ShouldUpdate("type", type.ToString(), dedupFields))
                user.SetType(type);

            if (ShouldUpdate("annualSalary", annualSalary?.ToString(), dedupFields))
                user.SetMonthlySalary(annualSalary.HasValue ? (decimal?)(annualSalary.Value / 12) : null);

            if (ShouldUpdate("tags", tags != null ? string.Join(",", tags) : null, dedupFields))
                user.SetTags(tags ?? new List<string>());
        }

        private bool ShouldUpdate<T>(string fieldName, T newValue, List<string> dedupFields)
        {
            if (dedupFields == null || !dedupFields.Contains(fieldName)) return true;

            if (newValue == null || newValue?.ToString() == "") return false;

            return true;
        }
    }
}