using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories;

namespace Core.Services.Users
{
    [AutoRegister]
    public class CreateUserService : ICreateUserService
    {
        private readonly IUpdateUserService _updateUserService;
        private readonly IIdObjectFactory<User> _userFactory;
        private readonly IUserRepository _userRepository;

        public CreateUserService(IIdObjectFactory<User> userFactory, IUserRepository userRepository, IUpdateUserService updateUserService)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _updateUserService = updateUserService;
        }

        public User Create(Guid id, string name, string email, UserTypes type, decimal? annualSalary, IEnumerable<string> tags)
        {
            var dedupFields = ConfigurationManager.AppSettings["UserDeduplicationFields"]?.Split(',').Select(x => x.Trim().ToLower()).ToList();
            
            if (dedupFields != null || dedupFields.Count > 0)
            {
                var existingUsers = _userRepository.Get().ToList();

                var isDuplicate = existingUsers.Any(u => 
                    dedupFields.All(field =>
                    {
                        var existingValue = GetPropertyValue(u, field);
                        var newValue = GetInputFieldValue(field, name, email, type, annualSalary);
                        return existingValue == newValue;
                    }));

                if (isDuplicate)
                    throw new InvalidOperationException("User already exists based on deduplication fields.");
            }               

            var user = _userFactory.Create(id);
            _updateUserService.Update(user, name, email, type, annualSalary, tags);
            _userRepository.Save(user);
            return user;
        }

        private string GetPropertyValue(User user, string fieldName)
        {
            var prop = typeof(User).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return prop?.GetValue(user)?.ToString()?.ToLowerInvariant();
        }

        private string GetInputFieldValue(string field, string name, string email, UserTypes type, decimal? annualSalary)
        {
            switch (field)
            {
                case "name": return name?.ToLowerInvariant();
                case "email": return email?.ToLowerInvariant();
                case "type": return type.ToString().ToLowerInvariant();
                case "annualSalary": return annualSalary?.ToString();
                default: return null;
            }
        }
    }
}