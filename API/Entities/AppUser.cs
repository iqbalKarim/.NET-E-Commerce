namespace API.Entities
{
    public class AppUser
    {
        private int id;
        public int Id { 
            get{
                return id;
            } 
            set{
                id = value;
            } 
        }
        
        private string username;
        public string UserName { 
            get{
                return username;
            } set{
                username = value;
            } 
        }
        
        private byte[] passwordHash;
        public byte[] PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = value; }
        }        
        public AppUser(){}
        public AppUser(int Id, string UserName){
            this.Id = Id;
            this.UserName = UserName;
        }

    }
}