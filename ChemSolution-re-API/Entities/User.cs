using ChemSolution_re_API.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [EmailAddress]
        public string UserEmail { set; get; } = string.Empty;
        [StringLength(50, MinimumLength = 3)]
        public string UserName { set; get; } = string.Empty;
        public DateTime DateOfBirth { set; get; }
        public string PasswordHash { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int Balance { set; get; }
        [Range(0, int.MaxValue)]
        public int Rating { set; get; }
        [Range(0, int.MaxValue)]
        public int Honesty { set; get; } = 100;
        public Role Role { set; get; } = Role.User;

        public List<BlogPost> BlogPosts { set; get; } = new List<BlogPost>();
        public List<Request> Requests { set; get; } = new List<Request>();
        public List<Achievement> Achievements { set; get; } = new List<Achievement>();
        public List<Order> Orders { set; get; } = new List<Order>();
        public List<Element> Elements { set; get; } = new List<Element>();

        public List<ResearchHistory> ResearchHistorys { set; get; } = new List<ResearchHistory>();
        public List<Material> Materials { set; get; } = new List<Material>();
    }
}
