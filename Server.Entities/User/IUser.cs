namespace ProjectBank.Server.Entities
{
    internal interface IUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsSupervisor { get; set; }
        
        public string Email { get; set; }
    }
}
