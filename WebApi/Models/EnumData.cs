using System;
using Common.Extensions;

namespace WebApi.Models
{
    public class EnumData
    {
        public EnumData(object entity)
        {
            var type = entity.GetType();
            Key = entity.ToString();
            var enumValue = Enum.Parse(type, Key);
            Id = Convert.ToInt32(enumValue);
            Name = enumValue.ToString().AddSpaceBeforeCapitalLetter();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
    }
}