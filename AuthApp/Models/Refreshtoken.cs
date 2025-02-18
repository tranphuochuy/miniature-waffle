using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Models;

[Table("refreshtokens")]
public partial class Refreshtoken
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userid")]
    public int? Userid { get; set; }

    [Column("token")]
    public string Token { get; set; } = null!;

    [Column("expirydate", TypeName = "timestamp without time zone")]
    public DateTime Expirydate { get; set; }

    [ForeignKey("Userid")]
    [InverseProperty("Refreshtokens")]
    public virtual User? User { get; set; }
}