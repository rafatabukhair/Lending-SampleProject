using System;

namespace BusinessEntities
{
    public class IdNameObject : IdObject
    {
        private string _name;

        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name was not provided.");
            }
            _name = name;
        }
    }
}