using System;

namespace UserApi.Models
{
    public class UserItem
    {
        public long id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fathersName { get; set; }
        public DateTime birthDay { get; set; }
    }
}