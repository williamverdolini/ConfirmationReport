using System.Collections.Generic;

namespace ConfirmRep.Models.View
{
    public class UserViewModel
    { 
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IList<string> Roles { get; protected set; }
    }
}