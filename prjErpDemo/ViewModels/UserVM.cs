using prjErpDemo.Models;

namespace prjErpDemo.ViewModels
{
    public class UserVM
    {
        public UserVM() { User = new User(); oldPassword = string.Empty; }
        public User User { get; set; }
        public int userID
        {
            get { return this.User.UserID; }
            set { this.User.UserID = value; }
        }

        public string oldPassword { get; set; }

        public string password
        {
            get { return this.User.Password; }
            set { this.User.Password = value; }
        }
    }
}
