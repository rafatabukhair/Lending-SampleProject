using System.Linq;
using BusinessEntities;
using Raven.Client.Documents.Indexes;

namespace Data.Indexes
{
    public class UsersListIndex : AbstractIndexCreationTask<User>
    {
        public UsersListIndex()
        {
            Map = users => from user in users
                           select new
                                  {
                                      user.Name,
                                      user.Email,
                                      user.Type
                                  };

            Index(x => x.Type, FieldIndexing.Default);
        }
    }
}