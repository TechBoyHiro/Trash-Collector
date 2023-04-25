using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Trash.Infra.User.Infra;
using Trash.Infra.User.Models;

namespace Trash.Infra.Service.Models
{
    // Selected Services
    [Display(Name = "سرویس هایی که کاربر دارد")]
    public class SService : IEntity
    {
        [Required]
        public Guid Service_Id { get; set; }
        [Required]
        public Guid User_Id { get; set; }
        public bool Is_CurrentlyUse { get; set; }
        [Required]
        [Display(Name = "زمان شروع استفاده کاربر از سرویس")]
        public DateTimeOffset StartsFrom { get; set; }
        [Display(Name = "زمان اتمام استفاده کاربر از سرویس")]
        public DateTimeOffset? Ends { get; set; }
        public bool Is_Deleted { get; set; }
    }

    //public class SServiceConfiguration : IEntityTypeConfiguration<SService>
    //{
    //    public void Configure(EntityTypeBuilder<SService> builder)
    //    {
    //        builder.HasOne(x => x.Service).WithMany(x => x.Users).HasForeignKey(x => x.Service_Id);
    //        builder.HasOne(x => x.user).WithMany(x.)
    //    }
    //}
}
