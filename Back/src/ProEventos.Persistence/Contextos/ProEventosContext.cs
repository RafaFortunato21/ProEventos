using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Domain.Identity;

namespace ProEventos.Persistence.Contextos
{
    public class ProEventosContext : IdentityDbContext<User, Role, int, 
                                                        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, 
                                                        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options){}
        
        public DbSet<Evento> Eventos { get; set; }

        public DbSet<Lote> Lotes { get; set; }

        public DbSet<Palestrante> Palestrantes { get; set; }

        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }

        public DbSet<RedeSocial> RedesSociais { get; set; }

        protected override void OnModelCreating(ModelBuilder modeloBuilder)
        {
            base.OnModelCreating(modeloBuilder);
            
            modeloBuilder.Entity<UserRole>(userRole =>
                {
                    userRole.HasKey(ur => new {ur.UserId, ur.RoleId});


                    userRole.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                    userRole.HasOne(ur => ur.User)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
                }
            );


            modeloBuilder.Entity<PalestranteEvento> ()
                .HasKey(PE => new {PE.EventoId, PE.PalestranteId}) ;


            modeloBuilder.Entity<Evento>()
                .HasMany(e => e.RedesSociais)
                .WithOne(rs => rs.Evento)
                .OnDelete(DeleteBehavior.Cascade);

            modeloBuilder.Entity<Palestrante>()
                .HasMany(p => p.RedesSociais)
                .WithOne(rs => rs.Palestrante)
                .OnDelete(DeleteBehavior.Cascade);
        }



        
    }
}