using System;

namespace UserApi.Models
{
    public class UserItem
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FathersName { get; set; }
        public DateTime BirthDay { get; set; }
    }
}