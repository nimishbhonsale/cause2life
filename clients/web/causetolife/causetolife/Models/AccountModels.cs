using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Runtime.Serialization;

namespace causetolife.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    [DataContract]
    public class Cause
    {
        [DataMember(EmitDefaultValue = false)]
        public string _id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Status { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<string> Geographies { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Country { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<string> Assets { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public long Votes { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public DateTime StartDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public DateTime EndDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<string> Owners { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Donator> Sponsers { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<string> Testimonials { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public double Budget { get; set; }
    }

    [DataContract]
    public class Donator
    {
        [DataMember(EmitDefaultValue = false)]
        public string Username { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public double Amount { get; set; }
    }


    [DataContract]
    public class User
    {
        
        [DataMember(EmitDefaultValue = false)]
        public string _id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Username { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Password { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FirstName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string LastName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ProfileType { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ProfileDetails { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Address { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Phone { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Email { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Seller SellerDetails { get; set; }
    }

    public class Seller
    {

    }
}
